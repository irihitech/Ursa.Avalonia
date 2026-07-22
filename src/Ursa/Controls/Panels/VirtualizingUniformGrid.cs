using System.Collections.Specialized;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.VisualTree;

namespace Ursa.Controls;


/// <summary>
/// A virtualizing panel that arranges items in a uniform grid with a fixed number
/// of columns. Only items that are currently visible (plus a buffer area) are
/// realized; off-screen containers are recycled.
/// </summary>
/// <remarks>
/// Designed to be used as an <see cref="ItemsControl.ItemsPanel"/> inside a
/// <see cref="ScrollViewer"/>.  Works together with the owning
/// <see cref="ItemsControl"/> to generate, recycle, and prepare item containers.
/// </remarks>
public class VirtualizingUniformGrid : VirtualizingPanel, IScrollSnapPointsInfo
{
    // ─────────────────────────────────────────────────────────────
    //  Styled properties
    // ─────────────────────────────────────────────────────────────

    /// <summary>Defines the <see cref="Columns"/> property.</summary>
    public static readonly StyledProperty<int> ColumnsProperty =
        AvaloniaProperty.Register<VirtualizingUniformGrid, int>(
            nameof(Columns), defaultValue: 4,
            validate: static v => v > 0);

    /// <summary>Defines the <see cref="ItemWidth"/> property.</summary>
    public static readonly StyledProperty<double> ItemWidthProperty =
        AvaloniaProperty.Register<VirtualizingUniformGrid, double>(
            nameof(ItemWidth), defaultValue: double.NaN);

    /// <summary>Defines the <see cref="ItemHeight"/> property.</summary>
    public static readonly StyledProperty<double> ItemHeightProperty =
        AvaloniaProperty.Register<VirtualizingUniformGrid, double>(
            nameof(ItemHeight), defaultValue: double.NaN);

    /// <summary>Defines the <see cref="CacheLength"/> property.</summary>
    /// <remarks>
    /// Buffer factor applied to the viewport size on each side.  A value of 0.5
    /// means the panel keeps half a viewport's worth of extra rows above and
    /// below the visible area.  Range: 0 to 2.
    /// </remarks>
    public static readonly StyledProperty<double> CacheLengthProperty =
        AvaloniaProperty.Register<VirtualizingUniformGrid, double>(
            nameof(CacheLength), 0.5,
            validate: static v => v is >= 0 and <= 2);

    /// <summary>Defines the <see cref="UniformItemHeight"/> property.</summary>
    /// <remarks>
    /// When <see langword="true"/> (default), all items share the same cell height
    /// and the grid extent is <c>Rows * CellHeight</c>.  When <see langword="false"/>,
    /// each row takes the height of its tallest item — useful for text blocks or
    /// cards whose content varies in length.
    /// </remarks>
    public static readonly StyledProperty<bool> UniformItemHeightProperty =
        AvaloniaProperty.Register<VirtualizingUniformGrid, bool>(
            nameof(UniformItemHeight), defaultValue: true);

    // ─────────────────────────────────────────────────────────────
    //  Snap-point routed events
    // ─────────────────────────────────────────────────────────────

    public static readonly RoutedEvent<RoutedEventArgs> HorizontalSnapPointsChangedEvent =
        RoutedEvent.Register<VirtualizingUniformGrid, RoutedEventArgs>(
            nameof(HorizontalSnapPointsChanged), RoutingStrategies.Bubble);

    public static readonly RoutedEvent<RoutedEventArgs> VerticalSnapPointsChangedEvent =
        RoutedEvent.Register<VirtualizingUniformGrid, RoutedEventArgs>(
            nameof(VerticalSnapPointsChanged), RoutingStrategies.Bubble);

    // ─────────────────────────────────────────────────────────────
    //  Recycle-key attached property (marks container reuse key)
    // ─────────────────────────────────────────────────────────────

    private static readonly AttachedProperty<object?> RecycleKeyProperty =
        AvaloniaProperty.RegisterAttached<VirtualizingUniformGrid, Control, object?>("RecycleKey");

    private static readonly object s_itemIsItsOwnContainer = new();

    // ─────────────────────────────────────────────────────────────
    //  Fields
    // ─────────────────────────────────────────────────────────────

    private readonly Action<Control, int> _recycleElement;
    private readonly Action<Control> _recycleElementOnItemRemoved;
    private readonly Action<Control, int, int> _updateElementIndex;

    private RealizedGridElements? _measureElements;
    private RealizedGridElements? _realizedElements;
    private IScrollAnchorProvider? _scrollAnchorProvider;
    private Rect _viewport;
    private Dictionary<object, Stack<Control>>? _recyclePool;
    private bool _isInLayout;
    private bool _isWaitingForViewportUpdate;
    private double _bufferFactor;

    // Scroll-into-view tracking.
    private int _scrollToIndex = -1;
    private Control? _scrollToElement;

    // Row-height tracking for non-uniform mode.
    private readonly Dictionary<int, double> _rowHeights = new();
    private double _estRowHeight;

    // Computed layout metrics (updated each measure pass).
    private double _cellWidth;
    private double _cellHeight;
    private int _totalRows;

    // ─────────────────────────────────────────────────────────────
    //  Static constructor
    // ─────────────────────────────────────────────────────────────

    static VirtualizingUniformGrid()
    {
        CacheLengthProperty.Changed.AddClassHandler<VirtualizingUniformGrid>(
            (x, _) => x.OnCacheLengthChanged());
        ColumnsProperty.Changed.AddClassHandler<VirtualizingUniformGrid>(
            (x, _) => x.OnColumnsChanged());
        ItemWidthProperty.Changed.AddClassHandler<VirtualizingUniformGrid>(
            (x, _) => x.InvalidateMeasure());
        ItemHeightProperty.Changed.AddClassHandler<VirtualizingUniformGrid>(
            (x, _) => x.InvalidateMeasure());
        UniformItemHeightProperty.Changed.AddClassHandler<VirtualizingUniformGrid>(
            (x, _) => x.InvalidateMeasure());
    }

    public VirtualizingUniformGrid()
    {
        _recycleElement = RecycleElement;
        _recycleElementOnItemRemoved = RecycleElementOnItemRemoved;
        _updateElementIndex = UpdateElementIndex;
        _bufferFactor = Math.Max(0, CacheLength);
        EffectiveViewportChanged += OnEffectiveViewportChanged;
    }

    // ─────────────────────────────────────────────────────────────
    //  CLR properties
    // ─────────────────────────────────────────────────────────────

    public int Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public double ItemWidth
    {
        get => GetValue(ItemWidthProperty);
        set => SetValue(ItemWidthProperty, value);
    }

    public double ItemHeight
    {
        get => GetValue(ItemHeightProperty);
        set => SetValue(ItemHeightProperty, value);
    }

    public double CacheLength
    {
        get => GetValue(CacheLengthProperty);
        set => SetValue(CacheLengthProperty, value);
    }

    /// <summary>
    /// Gets or sets whether all rows share the same height (default <see langword="true"/>).
    /// When <see langword="false"/>, each row height is determined by its tallest item.
    /// </summary>
    public bool UniformItemHeight
    {
        get => GetValue(UniformItemHeightProperty);
        set => SetValue(UniformItemHeightProperty, value);
    }

    public int FirstRealizedIndex => _realizedElements?.FirstIndex ?? -1;
    public int LastRealizedIndex => _realizedElements?.LastIndex ?? -1;

    // ── Snap-point events ────────────────────────────────────────

    public event EventHandler<RoutedEventArgs>? HorizontalSnapPointsChanged
    {
        add => AddHandler(HorizontalSnapPointsChangedEvent, value);
        remove => RemoveHandler(HorizontalSnapPointsChangedEvent, value);
    }

    public event EventHandler<RoutedEventArgs>? VerticalSnapPointsChanged
    {
        add => AddHandler(VerticalSnapPointsChangedEvent, value);
        remove => RemoveHandler(VerticalSnapPointsChangedEvent, value);
    }

    // ─────────────────────────────────────────────────────────────
    //  VirtualizingPanel overrides
    // ─────────────────────────────────────────────────────────────

    protected override IEnumerable<Control> GetRealizedContainers()
    {
        return _realizedElements?.Elements.Where(x => x is not null)!;
    }

    protected override Control? ContainerFromIndex(int index)
    {
        var items = Items;
        if (index < 0 || index >= items.Count)
            return null;
        if (_scrollToIndex == index && _scrollToElement is not null)
            return _scrollToElement;
        if (GetRealizedElement(index) is { } realized)
            return realized;
        if (items[index] is Control c && c.GetValue(RecycleKeyProperty) == s_itemIsItsOwnContainer)
            return c;
        return null;
    }

    protected override int IndexFromContainer(Control container)
    {
        if (container == _scrollToElement)
            return _scrollToIndex;
        return _realizedElements?.GetIndex(container) ?? -1;
    }

    protected override Control? ScrollIntoView(int index)
    {
        var items = Items;
        if (_isInLayout || index < 0 || index >= items.Count || _realizedElements is null || !IsEffectivelyVisible)
            return null;

        if (GetRealizedElement(index) is { } element)
        {
            element.BringIntoView();
            return element;
        }

        // Create and measure the element to be brought into view.
        var scrollToElement = GetOrCreateElement(items, index);
        scrollToElement.Measure(Size.Infinity);

        // Arrange it at its estimated position.
        var row  = index / Columns;
        var rect = new Rect(0, row * _cellHeight, _cellWidth, _cellHeight);
        scrollToElement.Arrange(rect);

        _scrollToElement = scrollToElement;
        _scrollToIndex   = index;

        // Ensure the panel extent reflects the new item, then bring it into view.
        if (!_viewport.Contains(rect))
        {
            _isWaitingForViewportUpdate = true;
            InvalidateMeasure();
        }

        scrollToElement.BringIntoView();

        _scrollToElement = null;
        _scrollToIndex   = -1;
        return scrollToElement;
    }

    protected override IInputElement? GetControl(
        NavigationDirection direction, IInputElement? from, bool wrap)
    {
        var items = Items;
        var count = items.Count;
        var fromControl = from as Control;

        if (count == 0 || (fromControl is null && direction is not NavigationDirection.First and not NavigationDirection.Last))
            return null;

        var cols = Columns;
        var fromIndex = fromControl is not null ? IndexFromContainer(fromControl) : -1;
        var toIndex = fromIndex;

        switch (direction)
        {
            case NavigationDirection.First:
                toIndex = 0;
                break;
            case NavigationDirection.Last:
                toIndex = count - 1;
                break;
            case NavigationDirection.Next:
                ++toIndex;
                break;
            case NavigationDirection.Previous:
                --toIndex;
                break;
            case NavigationDirection.Left:
                if (toIndex % cols > 0) --toIndex; else return null;
                break;
            case NavigationDirection.Right:
                if (toIndex % cols < cols - 1) ++toIndex; else return null;
                break;
            case NavigationDirection.Up:
                if (toIndex >= cols) toIndex -= cols; else return null;
                break;
            case NavigationDirection.Down:
                if (toIndex + cols < count) toIndex += cols; else return null;
                break;
            default:
                return null;
        }

        if (fromIndex == toIndex)
            return from;

        if (wrap)
        {
            if (toIndex < 0) toIndex = count - 1;
            else if (toIndex >= count) toIndex = 0;
        }

        return ScrollIntoView(toIndex);
    }

    // ─────────────────────────────────────────────────────────────
    //  Items changed
    // ─────────────────────────────────────────────────────────────

    protected override void OnItemsChanged(
        IReadOnlyList<object?> items, NotifyCollectionChangedEventArgs e)
    {
        InvalidateMeasure();

        // Update special-element indices before we touch realized elements.
        UpdateScrollToIndexOnItemsChanged(e);

        if (_realizedElements is null) return;

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                _realizedElements.ItemsInserted(e.NewStartingIndex, e.NewItems!.Count, _updateElementIndex);
                break;
            case NotifyCollectionChangedAction.Remove:
                _realizedElements.ItemsRemoved(e.OldStartingIndex, e.OldItems!.Count, _updateElementIndex, _recycleElementOnItemRemoved);
                break;
            case NotifyCollectionChangedAction.Replace:
                _realizedElements.ItemsReplaced(e.OldStartingIndex, e.OldItems!.Count, _recycleElementOnItemRemoved);
                break;
            case NotifyCollectionChangedAction.Move:
                if (e.OldStartingIndex < 0) goto case NotifyCollectionChangedAction.Reset;
                _realizedElements.ItemsRemoved(e.OldStartingIndex, e.OldItems!.Count, _updateElementIndex, _recycleElementOnItemRemoved);
                var insertIdx = e.NewStartingIndex;
                if (e.NewStartingIndex > e.OldStartingIndex)
                    insertIdx -= e.OldItems!.Count - 1;
                _realizedElements.ItemsInserted(insertIdx, e.NewItems!.Count, _updateElementIndex);
                break;
            case NotifyCollectionChangedAction.Reset:
                _realizedElements.ItemsReset(_recycleElementOnItemRemoved);
                break;
        }
    }

    private void UpdateScrollToIndexOnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
        if (_scrollToElement is null) return;

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewStartingIndex <= _scrollToIndex)
                    _scrollToIndex += e.NewItems!.Count;
                break;
            case NotifyCollectionChangedAction.Remove:
                if (e.OldStartingIndex <= _scrollToIndex && _scrollToIndex < e.OldStartingIndex + e.OldItems!.Count)
                {
                    RecycleScrollToElement();
                }
                else if (e.OldStartingIndex < _scrollToIndex)
                {
                    _scrollToIndex -= e.OldItems!.Count;
                }
                break;
            case NotifyCollectionChangedAction.Replace:
                if (e.OldStartingIndex <= _scrollToIndex && _scrollToIndex < e.OldStartingIndex + e.OldItems!.Count)
                    RecycleScrollToElement();
                break;
            case NotifyCollectionChangedAction.Move:
                if (e.OldStartingIndex < 0) goto case NotifyCollectionChangedAction.Reset;
                if (e.OldStartingIndex <= _scrollToIndex && _scrollToIndex < e.OldStartingIndex + e.OldItems!.Count)
                {
                    _scrollToIndex = e.NewStartingIndex + (_scrollToIndex - e.OldStartingIndex);
                }
                else
                {
                    if (e.OldStartingIndex < _scrollToIndex)
                        _scrollToIndex -= e.OldItems!.Count;
                    if (e.NewStartingIndex <= _scrollToIndex)
                        _scrollToIndex += e.NewItems!.Count;
                }
                break;
            case NotifyCollectionChangedAction.Reset:
                RecycleScrollToElement();
                break;
        }
    }

    private void RecycleScrollToElement()
    {
        if (_scrollToElement is not null)
            RecycleElementOnItemRemoved(_scrollToElement);
        _scrollToElement = null;
        _scrollToIndex   = -1;
    }

    // ─────────────────────────────────────────────────────────────
    //  Visual-tree attach / detach
    // ─────────────────────────────────────────────────────────────

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _scrollAnchorProvider = this.FindAncestorOfType<IScrollAnchorProvider>();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _scrollAnchorProvider = null;
    }

    // ─────────────────────────────────────────────────────────────
    //  MeasureOverride
    // ─────────────────────────────────────────────────────────────

    protected override Size MeasureOverride(Size availableSize)
    {
        if (_isWaitingForViewportUpdate)
            return DesiredSize;

        var items = Items;
        var isVirtualizing = items is { Count: > 0 } && ItemContainerGenerator is not null;
        var itemCount = isVirtualizing ? items.Count : Children.Count;
        if (itemCount == 0) return default;

        _isInLayout = true;
        try
        {
            ComputeCellSizes(availableSize);
            var cols = Columns;
            _totalRows = (itemCount + cols - 1) / cols;
            var uniform = UniformItemHeight;

            // ── Virtualized path (ItemsControl attached) ──────────
            if (isVirtualizing)
            {
                _realizedElements ??= new();
                _measureElements ??= new();

                var (firstRow, lastRow) = uniform
                    ? GetVisibleRowRangeUniform()
                    : GetVisibleRowRangeNonUniform();
                var desiredFirst = firstRow * cols;
                var desiredLast  = Math.Min((lastRow + 1) * cols - 1, itemCount - 1);

                if (_realizedElements.Count > 0 &&
                    (desiredFirst > _realizedElements.LastIndex || desiredLast < _realizedElements.FirstIndex))
                    _realizedElements.RecycleAllElements(_recycleElement);
                else
                {
                    _realizedElements.RecycleElementsBefore(desiredFirst, _recycleElement);
                    _realizedElements.RecycleElementsAfter(desiredLast, _recycleElement);
                }

                RealizeRange(items, desiredFirst, desiredLast);

                // Measure children — uniform uses fixed size, non-uniform
                // lets them take their natural height.
                var measureSize = uniform
                    ? new Size(_cellWidth, _cellHeight)
                    : new Size(_cellWidth, double.PositiveInfinity);
                foreach (var e in _measureElements!.Elements)
                    e?.Measure(measureSize);

                // In non-uniform mode, record per-row max heights after measure.
                if (!uniform)
                    UpdateRowHeightsFromMeasure();

                (_measureElements, _realizedElements) = (_realizedElements, _measureElements);
                _measureElements.ResetForReuse();
            }

            // ── Standalone (no ItemsControl): optionally measure for non-uniform ──
            if (!isVirtualizing && !uniform)
            {
                var measureSize = new Size(_cellWidth, double.PositiveInfinity);
                for (int i = 0; i < Children.Count; i++)
                {
                    Children[i].Measure(measureSize);
                    int row = i / cols;
                    double h = Children[i].DesiredSize.Height;
                    if (!_rowHeights.TryGetValue(row, out var cur) || h > cur)
                        _rowHeights[row] = h;
                }
                if (_rowHeights.Count > 0)
                {
                    double sum = 0;
                    foreach (var v in _rowHeights.Values) sum += v;
                    _estRowHeight = sum / _rowHeights.Count;
                }
            }

            double totalHeight = uniform
                ? _totalRows * _cellHeight
                : ComputeTotalExtent(itemCount, cols);
            return new Size(
                !double.IsInfinity(availableSize.Width) ? availableSize.Width : _cellWidth * cols,
                totalHeight);
        }
        finally
        {
            _isInLayout = false;
        }
    }

    // ─────────────────────────────────────────────────────────────
    //  ArrangeOverride
    // ─────────────────────────────────────────────────────────────

    protected override Size ArrangeOverride(Size finalSize)
    {
        // When used standalone, arrange all children directly.
        if (_realizedElements is null || _realizedElements.Count == 0)
        {
            ArrangeChildrenStandalone(finalSize);
            return finalSize;
        }

        _isInLayout = true;
        try
        {
            var cols    = Columns;
            var uniform = UniformItemHeight;
            double y    = 0;
            int prevRow = -1;

            for (int i = 0; i < _realizedElements.Count; i++)
            {
                var e = _realizedElements.Elements[i];
                if (e is null) continue;

                var idx = _realizedElements.FirstIndex + i;
                var row = idx / cols;
                var col = idx % cols;

                if (row != prevRow)
                {
                    // Compute Y for this row by accumulating previous row heights.
                    y = 0;
                    for (int r = 0; r < row; r++)
                        y += uniform ? _cellHeight : RowHeight(r);
                    prevRow = row;
                }

                var x    = col * _cellWidth;
                var cellH = uniform ? _cellHeight : RowHeight(row);
                var rect = new Rect(x, y, _cellWidth, cellH);

                e.Arrange(rect);

                if (e.IsVisible && _viewport.Intersects(rect))
                {
                    try { _scrollAnchorProvider?.RegisterAnchorCandidate(e); }
                    catch (InvalidOperationException) { /* element not descendant */ }
                }
            }

            double totalH = uniform
                ? _totalRows * _cellHeight
                : ComputeTotalExtent(_totalRows * cols, cols);
            return new Size(_cellWidth * cols, totalH);
        }
        finally
        {
            _isInLayout = false;
            RaiseEvent(new RoutedEventArgs(VerticalSnapPointsChangedEvent));
        }
    }

    /// <summary>Standalone arrange (no ItemsControl) — arrange all Children.</summary>
    private void ArrangeChildrenStandalone(Size finalSize)
    {
        var cols = Columns;
        var itemCount = Children.Count;
        if (itemCount == 0) return;
        var uniform = UniformItemHeight;

        double cellW;
        if (!double.IsNaN(ItemWidth))
            cellW = ItemWidth;
        else if (!double.IsInfinity(finalSize.Width))
            cellW = Math.Max(1.0, finalSize.Width / cols);
        else
            cellW = 100.0;

        double cellH = !double.IsNaN(ItemHeight) ? ItemHeight : cellW;
        double y     = 0;
        int prevRow  = -1;

        for (int i = 0; i < itemCount; i++)
        {
            var child = Children[i];
            int row = i / cols;
            int col = i % cols;

            if (row != prevRow)
            {
                y = uniform ? row * cellH : CumulativeYUpTo(row);
                prevRow = row;
            }

            double h = uniform ? cellH : RowHeight(row);
            child.Arrange(new Rect(col * cellW, y, cellW, h));
        }
    }

    private double CumulativeYUpTo(int row)
    {
        double y = 0;
        for (int r = 0; r < row; r++)
            y += RowHeight(r);
        return y;
    }

    // ─────────────────────────────────────────────────────────────
    //  EffectiveViewportChanged
    // ─────────────────────────────────────────────────────────────

    private void OnEffectiveViewportChanged(object? sender, EffectiveViewportChangedEventArgs e)
    {
        var oldTop  = _viewport.Top;
        var oldBot  = _viewport.Bottom;

        _viewport = e.EffectiveViewport.Intersect(new(Bounds.Size));

        var newTop  = _viewport.Top;
        var newBot  = _viewport.Bottom;

        if (Math.Abs(oldTop - newTop) > 0.001 ||
            Math.Abs(oldBot - newBot) > 0.001)
        {
            InvalidateMeasure();
        }
    }

    // ─────────────────────────────────────────────────────────────
    //  Private helpers: cell size
    // ─────────────────────────────────────────────────────────────

    private void ComputeCellSizes(Size availableSize)
    {
        if (!double.IsNaN(ItemWidth))
            _cellWidth = ItemWidth;
        else if (!double.IsInfinity(availableSize.Width))
            _cellWidth = Math.Max(1.0, availableSize.Width / Columns);
        else
            _cellWidth = 100.0;

        _cellHeight = !double.IsNaN(ItemHeight) ? ItemHeight : _cellWidth;
    }

    // ─────────────────────────────────────────────────────────────
    //  Private helpers: visible row range
    // ─────────────────────────────────────────────────────────────

    private (int firstRow, int lastRow) GetVisibleRowRangeUniform()
    {
        if (_cellHeight <= 0 || _totalRows == 0)
            return (0, 0);

        double viewportTop    = _viewport.Top;
        double viewportHeight = _viewport.Height;
        double bufferPx       = viewportHeight * _bufferFactor;

        int first = Math.Max(0,               (int)((viewportTop - bufferPx) / _cellHeight));
        int last  = Math.Min(_totalRows - 1,  (int)((viewportTop + viewportHeight + bufferPx) / _cellHeight));

        if (first > last) last = first;
        return (first, last);
    }

    private (int firstRow, int lastRow) GetVisibleRowRangeNonUniform()
    {
        if (_totalRows == 0) return (0, 0);
        EnsureEstRowHeight();

        double viewportTop    = _viewport.Top;
        double viewportHeight = _viewport.Height;
        double bufferPx       = viewportHeight * _bufferFactor;

        double y = 0;
        int first = 0, last = _totalRows - 1;
        bool foundFirst = false;

        for (int r = 0; r < _totalRows; r++)
        {
            double rh = RowHeight(r);

            if (!foundFirst && y + rh > viewportTop - bufferPx)
            {
                first = r;
                foundFirst = true;
            }

            if (y >= viewportTop + viewportHeight + bufferPx)
            {
                last = Math.Max(0, r - 1);
                break;
            }

            y += rh;
        }

        if (first > last) last = first;
        return (Math.Max(0, first - 1), last);
    }

    private void UpdateRowHeightsFromMeasure()
    {
        _rowHeights.Clear();
        if (_measureElements is null) return;

        var cols = Columns;
        for (int i = 0; i < _measureElements.Count; i++)
        {
            var e = _measureElements.Elements[i];
            if (e is null) continue;
            int idx = _measureElements.FirstIndex + i;
            int row = idx / cols;
            double h = e.DesiredSize.Height;
            if (!_rowHeights.TryGetValue(row, out var cur) || h > cur)
                _rowHeights[row] = h;
        }

        // Update estimate from measured rows.
        if (_rowHeights.Count > 0)
        {
            double sum = 0;
            foreach (var h in _rowHeights.Values) sum += h;
            _estRowHeight = sum / _rowHeights.Count;
        }
        else
        {
            _estRowHeight = _cellHeight > 0 ? _cellHeight : _cellWidth;
        }
    }

    private double RowHeight(int row)
    {
        return _rowHeights.TryGetValue(row, out var h) ? h : _estRowHeight;
    }

    private void EnsureEstRowHeight()
    {
        if (_estRowHeight <= 0)
            _estRowHeight = _cellHeight > 0 ? _cellHeight : _cellWidth;
    }

    private double ComputeTotalExtent(int itemCount, int cols)
    {
        EnsureEstRowHeight();
        double y = 0;
        int rows = (itemCount + cols - 1) / cols;
        for (int r = 0; r < rows; r++)
            y += RowHeight(r);
        return y;
    }

    // ─────────────────────────────────────────────────────────────
    //  Private helpers: realize range
    // ─────────────────────────────────────────────────────────────

    private void RealizeRange(IReadOnlyList<object?> items, int first, int last)
    {
        // Walk the target index range.  GetOrCreateElement checks whether the
        // container already exists in _realizedElements and reuses it without
        // calling AddInternalChild again.  Only genuinely missing containers
        // are created or pulled from the recycle pool.
        for (int idx = first; idx <= last; idx++)
        {
            if (_measureElements!.GetElement(idx) is not null)
                continue;

            var row = idx / Columns;
            var e   = GetOrCreateElement(items, idx);
            var u   = row * _cellHeight;

            _measureElements.Add(idx, e, u, _cellHeight);
        }
    }

    private Control? GetRealizedElement(int index)
    {
        return _realizedElements?.GetElement(index) ?? _measureElements?.GetElement(index);
    }

    // ─────────────────────────────────────────────────────────────
    //  Private helpers: get / create / recycle elements
    // ─────────────────────────────────────────────────────────────

    private Control GetOrCreateElement(IReadOnlyList<object?> items, int index)
    {
        Debug.Assert(ItemContainerGenerator is not null);

        if (GetRealizedElement(index) is { } realized)
            return realized;

        var item    = items[index];
        var gen     = ItemContainerGenerator!;

        if (gen.NeedsContainer(item, index, out var recycleKey))
        {
            return GetRecycledElement(item, index, recycleKey) ??
                   CreateElement(item, index, recycleKey);
        }
        else
        {
            return GetItemAsOwnContainer(item, index);
        }
    }

    private Control GetItemAsOwnContainer(object? item, int index)
    {
        Debug.Assert(ItemContainerGenerator is not null);
        var controlItem = (Control)item!;
        var gen         = ItemContainerGenerator!;

        if (!controlItem.IsSet(RecycleKeyProperty))
        {
            gen.PrepareItemContainer(controlItem, controlItem, index);
            AddInternalChild(controlItem);
            controlItem.SetValue(RecycleKeyProperty, s_itemIsItsOwnContainer);
            gen.ItemContainerPrepared(controlItem, item, index);
        }

        controlItem.SetCurrentValue(Visual.IsVisibleProperty, true);
        return controlItem;
    }

    private Control? GetRecycledElement(object? item, int index, object? recycleKey)
    {
        Debug.Assert(ItemContainerGenerator is not null);
        if (recycleKey is null) return null;

        var gen = ItemContainerGenerator!;

        if (_recyclePool?.TryGetValue(recycleKey, out var pool) == true && pool.Count > 0)
        {
            var recycled = pool.Pop();
            recycled.SetCurrentValue(Visual.IsVisibleProperty, true);
            gen.PrepareItemContainer(recycled, item, index);
            AddInternalChild(recycled);
            gen.ItemContainerPrepared(recycled, item, index);
            return recycled;
        }

        return null;
    }

    private Control CreateElement(object? item, int index, object? recycleKey)
    {
        Debug.Assert(ItemContainerGenerator is not null);
        var gen = ItemContainerGenerator!;

        var container = gen.CreateContainer(item, index, recycleKey);
        container.SetValue(RecycleKeyProperty, recycleKey);
        gen.PrepareItemContainer(container, item, index);
        AddInternalChild(container);
        gen.ItemContainerPrepared(container, item, index);
        return container;
    }

    private void RecycleElement(Control element, int index)
    {
        Debug.Assert(ItemsControl is not null);
        Debug.Assert(ItemContainerGenerator is not null);

        _scrollAnchorProvider?.UnregisterAnchorCandidate(element);

        var recycleKey = element.GetValue(RecycleKeyProperty);

        if (recycleKey is null)
        {
            ItemContainerGenerator!.ClearItemContainer(element);
            RemoveInternalChild(element);
        }
        else if (recycleKey == s_itemIsItsOwnContainer)
        {
            element.SetCurrentValue(Visual.IsVisibleProperty, false);
        }
        else
        {
            ItemContainerGenerator!.ClearItemContainer(element);
            PushToRecyclePool(recycleKey, element);
            element.SetCurrentValue(Visual.IsVisibleProperty, false);
            RemoveInternalChild(element);
        }
    }

    private void RecycleElementOnItemRemoved(Control element)
    {
        Debug.Assert(ItemContainerGenerator is not null);
        _scrollAnchorProvider?.UnregisterAnchorCandidate(element);

        var recycleKey = element.GetValue(RecycleKeyProperty);

        if (recycleKey is null)
        {
            ItemContainerGenerator!.ClearItemContainer(element);
            RemoveInternalChild(element);
        }
        else if (recycleKey == s_itemIsItsOwnContainer)
        {
            RemoveInternalChild(element);
        }
        else
        {
            ItemContainerGenerator!.ClearItemContainer(element);
            PushToRecyclePool(recycleKey, element);
            element.SetCurrentValue(Visual.IsVisibleProperty, false);
            RemoveInternalChild(element);
        }
    }

    private void PushToRecyclePool(object recycleKey, Control element)
    {
        _recyclePool ??= new();
        if (!_recyclePool.TryGetValue(recycleKey, out var pool))
        {
            pool = new();
            _recyclePool.Add(recycleKey, pool);
        }
        pool.Push(element);
    }

    private void UpdateElementIndex(Control element, int oldIndex, int newIndex)
    {
        Debug.Assert(ItemContainerGenerator is not null);
        ItemContainerGenerator.ItemContainerIndexChanged(element, oldIndex, newIndex);
    }

    private void OnCacheLengthChanged()
    {
        _bufferFactor = Math.Max(0, CacheLength);
        InvalidateMeasure();
    }

    private void OnColumnsChanged()
    {
        _realizedElements?.RecycleAllElements(_recycleElement);
        _rowHeights.Clear();
        InvalidateMeasure();
    }

    // ─────────────────────────────────────────────────────────────
    //  IScrollSnapPointsInfo
    // ─────────────────────────────────────────────────────────────

    public bool AreHorizontalSnapPointsRegular { get; set; } = true;
    public bool AreVerticalSnapPointsRegular { get; set; } = true;

    public IReadOnlyList<double> GetIrregularSnapPoints(
        Orientation orientation, SnapPointsAlignment snapPointsAlignment)
    {
        return Array.Empty<double>();
    }

    public double GetRegularSnapPoints(
        Orientation orientation, SnapPointsAlignment snapPointsAlignment, out double offset)
    {
        offset = 0;
        if (orientation == Orientation.Vertical)
            return _cellHeight > 0 ? _cellHeight : 100;
        return _cellWidth > 0 ? _cellWidth : 100;
    }

    // ═════════════════════════════════════════════════════════════
    //  Internal: RealizedGridElements
    // ═════════════════════════════════════════════════════════════

    private sealed class RealizedGridElements
    {
        private readonly List<Control?> _elements = new();
        private readonly List<double>  _sizeU    = new();
        private readonly List<double>  _startU   = new();
        private int _firstIndex;

        public List<Control?> Elements => _elements;
        public List<double> SizeU     => _sizeU;
        public int FirstIndex         => _firstIndex;
        public int LastIndex          => _firstIndex + _elements.Count - 1;
        public int Count              => _elements.Count;
        public double StartU          => _elements.Count > 0 ? _startU[0] : 0;

        public void Add(int index, Control? element, double startU, double sizeU)
        {
            if (_elements.Count == 0)
            {
                _firstIndex = index;
                _elements.Add(element);
                _sizeU.Add(sizeU);
                _startU.Add(startU);
                return;
            }

            if (index < _firstIndex)
            {
                _firstIndex = index;
                _elements.Insert(0, element);
                _sizeU.Insert(0, sizeU);
                _startU.Insert(0, startU);
            }
            else if (index > LastIndex)
            {
                _elements.Add(element);
                _sizeU.Add(sizeU);
                _startU.Add(startU);
            }
            else
            {
                int local = index - _firstIndex;
                _elements[local] = element;
                _sizeU[local]    = sizeU;
                _startU[local]   = startU;
            }
        }

        public Control? GetElement(int index)
        {
            int local = index - _firstIndex;
            if (local < 0 || local >= _elements.Count) return null;
            return _elements[local];
        }

        public int GetIndex(Control element)
        {
            int local = _elements.IndexOf(element);
            return local < 0 ? -1 : _firstIndex + local;
        }

        public void RecycleAllElements(Action<Control, int> recycle)
        {
            for (int i = _elements.Count - 1; i >= 0; i--)
            {
                if (_elements[i] is { } e)
                    recycle(e, _firstIndex + i);
            }
            ResetForReuse();
        }

        public void RecycleElementsBefore(int beforeIndex, Action<Control, int> recycle)
        {
            while (_elements.Count > 0 && _firstIndex < beforeIndex)
            {
                if (_elements[0] is { } e)
                    recycle(e, _firstIndex);
                _elements.RemoveAt(0);
                _sizeU.RemoveAt(0);
                _startU.RemoveAt(0);
                _firstIndex++;
            }
        }

        public void RecycleElementsAfter(int afterIndex, Action<Control, int> recycle)
        {
            while (_elements.Count > 0 && LastIndex > afterIndex)
            {
                int last = _elements.Count - 1;
                if (_elements[last] is { } e)
                    recycle(e, LastIndex);
                _elements.RemoveAt(last);
                _sizeU.RemoveAt(last);
                _startU.RemoveAt(last);
            }
        }

        public void ItemsInserted(int index, int count, Action<Control, int, int> updateIndex)
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                int global = _firstIndex + i;
                if (global >= index && _elements[i] is { } e)
                    updateIndex(e, global, global + count);
            }
        }

        public void ItemsRemoved(int index, int count,
            Action<Control, int, int> updateIndex,
            Action<Control> recycle)
        {
            int removeStart = index;
            int removeEnd   = index + count - 1;

            for (int i = _elements.Count - 1; i >= 0; i--)
            {
                int global = _firstIndex + i;
                if (global > removeEnd && _elements[i] is { } e)
                {
                    updateIndex(e, global, global - count);
                }
                else if (global >= removeStart && global <= removeEnd)
                {
                    if (_elements[i] is { } removed) recycle(removed);
                    _elements.RemoveAt(i);
                    _sizeU.RemoveAt(i);
                    _startU.RemoveAt(i);
                }
            }

            if (_elements.Count == 0)
                _firstIndex = 0;
        }

        public void ItemsReplaced(int index, int count, Action<Control> recycle)
        {
            int start = index;
            int end   = index + count - 1;

            for (int i = _elements.Count - 1; i >= 0; i--)
            {
                int global = _firstIndex + i;
                if (global >= start && global <= end)
                {
                    if (_elements[i] is { } e) recycle(e);
                    _elements[i] = null;
                }
            }
        }

        public void ItemsReset(Action<Control> recycle)
        {
            foreach (var e in _elements)
                if (e is not null) recycle(e);
            ResetForReuse();
        }

        public void ResetForReuse()
        {
            _elements.Clear();
            _sizeU.Clear();
            _startU.Clear();
            _firstIndex = 0;
        }
    }
}
