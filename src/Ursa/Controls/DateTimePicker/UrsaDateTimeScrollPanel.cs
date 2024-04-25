using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.VisualTree;

namespace Ursa.Controls.Panels;

public enum UrsaDateTimeScrollPanelType
{
    Year,
    Month,
    Day,
    Hour,
    Minute,
    Second,
    TimePeriod // AM/PM
}

/// <summary>
///     The panel to display items for time selection
/// </summary>
public class UrsaDateTimeScrollPanel : Panel, ILogicalScrollable
{
    public static readonly StyledProperty<double> ItemHeightProperty =
        AvaloniaProperty.Register<UrsaDateTimeScrollPanel, double>(
            nameof(ItemHeight), 32);

    public static readonly StyledProperty<bool> ShouldLoopProperty =
        AvaloniaProperty.Register<UrsaDateTimeScrollPanel, bool>(
            nameof(ShouldLoop));

    public static readonly StyledProperty<UrsaDateTimeScrollPanelType> PanelTypeProperty = AvaloniaProperty.Register<UrsaDateTimeScrollPanel, UrsaDateTimeScrollPanelType>(
        nameof(PanelType));

    public UrsaDateTimeScrollPanelType PanelType
    {
        get => GetValue(PanelTypeProperty);
        set => SetValue(PanelTypeProperty, value);
    }

    public static readonly StyledProperty<string> ItemFormatProperty = AvaloniaProperty.Register<UrsaDateTimeScrollPanel, string>(
        nameof(ItemFormat));

    public string ItemFormat
    {
        get => GetValue(ItemFormatProperty);
        set => SetValue(ItemFormatProperty, value);
    }
    

    private int _countItemAboveBelowSelected;
    private double _extendOne;

    private bool _initialized;
    private int _maximumValue;
    private int _minimumValue;
    private Vector _offset;
    private ScrollContentPresenter? _parentScroller;

    private int _range;

    private int _totalItems;

    static UrsaDateTimeScrollPanel()
    {
        ItemHeightProperty.Changed.AddClassHandler<UrsaDateTimeScrollPanel, double>((panel, args) =>
            panel.OnItemHeightChanged(args));
        AffectsArrange<UrsaDateTimeScrollPanel>(ItemHeightProperty);
        AffectsMeasure<UrsaDateTimeScrollPanel>(ItemHeightProperty);
    }

    public int Increment { get; set; }

    public int SelectedIndex { get; set; }

    public int SelectedValue { get; set; }

    public double ItemHeight
    {
        get => GetValue(ItemHeightProperty);
        set => SetValue(ItemHeightProperty, value);
    }

    public bool ShouldLoop
    {
        get => GetValue(ShouldLoopProperty);
        set => SetValue(ShouldLoopProperty, value);
    }

    public Vector Offset
    {
        get => _offset;
        set => SetOffset(value);
    }

    public Size Extent { get; private set; }

    public Size Viewport => Bounds.Size;

    public bool BringIntoView(Control target, Rect targetRect)
    {
        return false;
    }

    public Control? GetControlInDirection(NavigationDirection direction, Control? from)
    {
        return null;
    }

    public void RaiseScrollInvalidated(System.EventArgs e)
    {
        ScrollInvalidated?.Invoke(this, e);
    }

    public bool CanHorizontallyScroll
    {
        get => false;
        set { }
    }

    public bool CanVerticallyScroll
    {
        get => false;
        set { }
    }

    public bool IsLogicalScrollEnabled => true;
    public Size ScrollSize { get; private set; }

    public Size PageScrollSize { get; private set; }

    public event EventHandler? ScrollInvalidated;

    private void OnItemHeightChanged(AvaloniaPropertyChangedEventArgs<double> args)
    {
        var newValue = args.NewValue.Value;
        ScrollSize = new Size(0, newValue);
        PageScrollSize = new Size(0, newValue * 3);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _parentScroller?.RemoveHandler(Gestures.ScrollGestureEndedEvent, OnScrollGestureEnded);
        _parentScroller = this.GetVisualParent() as ScrollContentPresenter;
        _parentScroller?.AddHandler(Gestures.ScrollGestureEndedEvent, OnScrollGestureEnded);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _parentScroller?.RemoveHandler(Gestures.ScrollGestureEndedEvent, OnScrollGestureEnded);
        _parentScroller = null;
    }

    public override void ApplyTemplate()
    {
        base.ApplyTemplate();
        AddHandler(TappedEvent, OnItemTapped, RoutingStrategies.Bubble);
    }

    private void OnItemTapped(object sender, TappedEventArgs e)
    {
        if (e.Source is Visual source && GetItemFromSource(source) is { Tag: int tag })
        {
            SelectedValue = tag;
            e.Handled = true;
        }
    }

    private ListBoxItem? GetItemFromSource(Visual source)
    {
        var item = source;
        while (item != null && !(item is ListBoxItem)) item = item.GetVisualParent();
        return item as ListBoxItem;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        if (double.IsInfinity(availableSize.Width) || double.IsInfinity(availableSize.Height))
            throw new InvalidOperationException("Panel must have finite height");
        if (_initialized) UpdateHelperInfo();
        var initY = availableSize.Height / 2.0 - ItemHeight / 2.0;
        _countItemAboveBelowSelected = (int)Math.Ceiling(initY / ItemHeight);

        var children = Children;

        CreateOrDestroyItems(children);

        for (var i = 0; i < children.Count; i++) children[i].Measure(availableSize);

        if (!_initialized)
        {
            UpdateItems();
            RaiseScrollInvalidated(System.EventArgs.Empty);
            _initialized = true;
        }

        return availableSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (Children.Count == 0) return base.ArrangeOverride(finalSize);

        var itemHeight = ItemHeight;
        var children = Children;
        Rect rc;
        var initY = finalSize.Height / 2.0 - itemHeight / 2.0;

        if (ShouldLoop)
        {
            var currentSet = Math.Truncate(Offset.Y / _extendOne);
            initY += _extendOne * currentSet + (SelectedIndex - _countItemAboveBelowSelected) * ItemHeight;
            foreach (var child in children)
            {
                rc = new Rect(0, initY - Offset.Y, finalSize.Width, itemHeight);
                child.Arrange(rc);
                initY += itemHeight;
            }
        }
        else
        {
            var first = Math.Max(0, SelectedIndex - _countItemAboveBelowSelected);
            foreach (var child in children)
            {
                rc = new Rect(0, initY + first + itemHeight - Offset.Y, finalSize.Width, itemHeight);
                child.Arrange(rc);
                initY += itemHeight;
            }
        }

        return finalSize;
    }

    private void OnScrollGestureEnded(object sender, ScrollGestureEndedEventArgs e)
    {
        var snapY = Math.Round(Offset.Y / ItemHeight) * ItemHeight;
        if (!snapY.Equals(Offset.Y)) Offset = Offset.WithY(snapY);
    }

    private void SetOffset(Vector value)
    {
        var oldValue = _offset;
        _offset = value;
        var dy = _offset.Y - oldValue.Y;
        var children = Children;
        // TODO
    }

    private void UpdateHelperInfo()
    {
        _range = _maximumValue - _minimumValue + 1;
        _totalItems = (int)Math.Ceiling((double)_range / Increment);
        var itemHeight = ItemHeight;
        Extent = new Size(0, ShouldLoop ? _totalItems * itemHeight * 100 : _totalItems * itemHeight);

        _extendOne = _totalItems * itemHeight;
        _offset = new Vector(0,
            ShouldLoop ? _extendOne * 50 + SelectedIndex * itemHeight : SelectedIndex * itemHeight);
    }

    private void UpdateItems()
    {
        var children = Children;
        var min = _minimumValue;
        var max = _maximumValue;
        var selected = SelectedValue;

        int first;
        if (ShouldLoop)
        {
            first = (SelectedIndex - _countItemAboveBelowSelected) % _totalItems;
            first = first < 0 ? min + (first + _totalItems) * Increment : min + first * Increment;
        }
        else
        {
            first = min + Math.Max(0, SelectedIndex - _countItemAboveBelowSelected) * Increment;
        }

        for (var i = 0; i < children.Count; i++)
        {
            var item = (ListBoxItem)children[i];
            item.Content = first + i * Increment; // TODO
            item.Tag = first;
            item.IsSelected = first == selected;
            first += Increment;
            if (first > max) first = min;
        }
    }

    private void CreateOrDestroyItems(Avalonia.Controls.Controls children)
    {
        var totalItemsInViewport = _countItemAboveBelowSelected * 2 + 1;
        if (!ShouldLoop)
        {
            var numItemAboveSelect = _countItemAboveBelowSelected;
            if (SelectedIndex - _countItemAboveBelowSelected < 0) numItemAboveSelect = SelectedIndex;
            var numItemBelowSelect = _countItemAboveBelowSelected;
            if (SelectedIndex + _countItemAboveBelowSelected >= _totalItems)
                numItemBelowSelect = _totalItems - SelectedIndex - 1;
            totalItemsInViewport = numItemAboveSelect + numItemBelowSelect + 1;
        }

        while (children.Count < totalItemsInViewport)
            children.Add(new ListBoxItem
            {
                Height = ItemHeight,
                VerticalContentAlignment = VerticalAlignment.Center,
                Focusable = false
            });

        if (children.Count > totalItemsInViewport)
        {
            var countToRemove = children.Count - totalItemsInViewport;
            children.RemoveRange(children.Count - countToRemove, countToRemove);
        }
    }

    private int CoerceSelected(int newValue)
    {
        if (newValue < _minimumValue) return _minimumValue;
        if (newValue > _maximumValue) return _maximumValue;
        if (newValue % Increment == 0) return newValue;
        var items = Enumerable.Range(_minimumValue, _range).Where(x => x % Increment == 0).ToList();
        var nearest = items.Aggregate((x, y) => Math.Abs(x - newValue) > Math.Abs(y - newValue) ? y : x);
        return items.IndexOf(nearest) * Increment;
    }

    public event EventHandler? OnSelectionChanged;
    public event EventHandler? SelectionChanged;
}