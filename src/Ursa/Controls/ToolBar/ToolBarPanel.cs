using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

public class ToolBarPanel : StackPanel
{
    private ToolBar? _parent;
    private Panel? _overflowPanel;

    private Panel? OverflowPanel => _overflowPanel ??= ParentToolBar?.OverflowPanel;
    internal ToolBar? ParentToolBar => _parent ??= this.TemplatedParent as ToolBar;

    static ToolBarPanel()
    {
        OrientationProperty.OverrideDefaultValue<ToolBarPanel>(Orientation.Horizontal);
        // Overflow items are arranged outside the panel: clipping keeps them from painting over
        // the controls that follow the toolbar.
        ClipToBoundsProperty.OverrideDefaultValue<ToolBarPanel>(true);
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);
        _parent = this.TemplatedParent as ToolBar;
        if (_parent is null) return;
        this[!OrientationProperty] = _parent[!OrientationProperty];

        // The overflow popup changes which items the panel hosts; the toolbar raises this after the
        // popup opens or closes, and the panel must re-measure itself.
        _parent.OverflowPopupStateChanged -= OnOverflowPopupStateChanged;
        _parent.OverflowPopupStateChanged += OnOverflowPopupStateChanged;
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        if (_parent is not null)
        {
            _parent.OverflowPopupStateChanged -= OnOverflowPopupStateChanged;
        }

        base.OnDetachedFromLogicalTree(e);
    }

    private void OnOverflowPopupStateChanged(object? sender, System.EventArgs e)
    {
        InvalidateMeasure();
        InvalidateArrange();
    }

    /// <summary>
    /// The toolbar items in their declared order. The toolbar's logical children are the source of
    /// truth: they stay stable while items are displayed in the overflow popup.
    /// </summary>
    private List<Control> GetItems()
    {
        return ParentToolBar?.GetLogicalChildren().OfType<Control>().ToList() ?? [];
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var parent = ParentToolBar;
        var items = GetItems();
        if (parent is null) return default;

        bool horizontal = Orientation == Orientation.Horizontal;

        // StackPanel semantics: infinite space on the main axis and the full cross axis. Measuring
        // children with the toolbar's own width clamps their desired size (text is trimmed and
        // children that measure against the parent report a smaller size), which made the overflow
        // decision wrong.
        Size childConstraint = horizontal
            ? new Size(double.PositiveInfinity, availableSize.Height)
            : new Size(availableSize.Width, double.PositiveInfinity);

        // Items staying in the open overflow popup are still measured here, because the panel walks
        // the toolbar's logical children rather than its own.
        var desired = new Size[items.Count];
        for (var i = 0; i < items.Count; i++)
        {
            items[i].Measure(childConstraint);
            desired[i] = items[i].DesiredSize;
        }

        double limit = horizontal ? availableSize.Width : availableSize.Height;
        double used = 0;
        double cross = 0;
        bool hasOverflow = false;

        for (var i = 0; i < items.Count; i++)
        {
            Control item = items[i];
            OverflowMode mode = ToolBar.GetOverflowMode(item);
            bool overflow;
            if (!item.IsVisible)
            {
                // Items hidden by the consumer take no space and must not keep a stale overflow
                // state, otherwise the `:overflow` pseudo class and the popup content drift apart.
                overflow = false;
            }
            else if (mode == OverflowMode.Always)
            {
                overflow = true;
            }
            else if (mode == OverflowMode.Never)
            {
                overflow = false;
            }
            else
            {
                // Order is preserved: once an item does not fit, every following AsNeeded item
                // overflows as well.
                double need = horizontal ? desired[i].Width : desired[i].Height;
                overflow = hasOverflow || (!double.IsInfinity(limit) && used + need > limit);
            }

            ToolBar.SetIsOverflowItem(item, overflow);
            if (overflow)
            {
                hasOverflow = true;
                continue;
            }

            if (!item.IsVisible) continue;
            used += horizontal ? desired[i].Width : desired[i].Height;
            cross = Math.Max(cross, horizontal ? desired[i].Height : desired[i].Width);
        }

        parent.HasOverflowItems = hasOverflow;
        return horizontal ? new Size(used, cross) : new Size(cross, used);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var parent = ParentToolBar;
        var items = GetItems();
        if (parent is null) return finalSize;

        bool horizontal = Orientation == Orientation.Horizontal;
        Panel? overflowPanel = OverflowPanel;
        bool popupOpen = parent.IsOverflowPopupOpen;
        bool changed = false;

        if (overflowPanel is not null)
        {
            changed |= SyncPopupItems(items, overflowPanel, popupOpen);
        }

        changed |= SyncChildOrder(items);
        changed |= UpdateSeparatorVisibility(items);

        if (changed)
        {
            // Re-parenting during arrange leaves stale desired/arranged sizes behind unless the
            // affected panels are invalidated explicitly.
            InvalidateMeasure();
            overflowPanel?.InvalidateMeasure();
        }

        // Overflow items are arranged outside the visible area instead of being detached into the
        // (closed) popup. They stay in the visual tree, so DataContext, bindings and content of the
        // toolbar buttons keep working no matter how often the boundary moves.
        double offset = 0;
        foreach (Control item in items)
        {
            if (!Children.Contains(item)) continue;
            Size itemSize = item.DesiredSize;
            if (!item.IsVisible || ToolBar.GetIsOverflowItem(item))
            {
                item.Arrange(horizontal
                    ? new Rect(finalSize.Width, 0, itemSize.Width, finalSize.Height)
                    : new Rect(0, finalSize.Height, finalSize.Width, itemSize.Height));
                continue;
            }

            item.Arrange(horizontal
                ? new Rect(offset, 0, itemSize.Width, finalSize.Height)
                : new Rect(0, offset, finalSize.Width, itemSize.Height));
            offset += horizontal ? itemSize.Width : itemSize.Height;
        }

        return finalSize;
    }

    /// <summary>
    /// Moves overflow items in and out of the popup panel. Items only live in the popup while it is
    /// open; every other time they stay in the toolbar panel (arranged outside the visible area).
    /// </summary>
    private bool SyncPopupItems(List<Control> items, Panel overflowPanel, bool popupOpen)
    {
        bool changed = false;
        foreach (Control item in items)
        {
            bool isOverflow = ToolBar.GetIsOverflowItem(item);
            bool inPopup = overflowPanel.Children.Contains(item);
            if (popupOpen && isOverflow && !inPopup)
            {
                // A control must be detached from its current panel before it can be hosted by
                // another one.
                Children.Remove(item);
                InsertInOrder(item, overflowPanel, items);
                changed = true;
            }
            else if ((!popupOpen || !isOverflow) && inPopup)
            {
                overflowPanel.Children.Remove(item);
                InsertInOrder(item, this, items);
                changed = true;
            }
        }

        return changed;
    }

    /// <summary>Inserts <paramref name="item"/> after the closest preceding sibling of the same panel.</summary>
    private static void InsertInOrder(Control item, Panel target, List<Control> items)
    {
        int index = 0;
        int position = items.IndexOf(item);
        for (int i = position - 1; i >= 0; i--)
        {
            int siblingIndex = target.Children.IndexOf(items[i]);
            if (siblingIndex >= 0)
            {
                index = siblingIndex + 1;
                break;
            }
        }

        target.Children.Insert(index, item);
    }

    /// <summary>Keeps the panel's children in the declared item order.</summary>
    private bool SyncChildOrder(List<Control> items)
    {
        List<Control> ordered = items.Where(Children.Contains).ToList();
        if (ordered.Count == Children.Count)
        {
            bool same = true;
            for (int i = 0; i < ordered.Count; i++)
            {
                if (!ReferenceEquals(ordered[i], Children[i]))
                {
                    same = false;
                    break;
                }
            }

            if (same) return false;
        }

        Children.Clear();
        Children.AddRange(ordered);
        return true;
    }

    /// <summary>Hides separators that have no visible neighbour at either end of a visible run.</summary>
    private static bool UpdateSeparatorVisibility(List<Control> items)
    {
        bool changed = false;
        List<Control> visible = items.Where(item => item.IsVisible && !ToolBar.GetIsOverflowItem(item)).ToList();
        List<Control> overflow = items.Where(item => item.IsVisible && ToolBar.GetIsOverflowItem(item)).ToList();

        foreach (Control item in items)
        {
            if (item is not ToolBarSeparator separator) continue;
            bool show = visible.Contains(item) || overflow.Contains(item);
            if (item == visible.LastOrDefault()) show = false;
            if (item == overflow.FirstOrDefault()) show = false;
            if (separator.IsVisible != show)
            {
                separator.IsVisible = show;
                changed = true;
            }
        }

        return changed;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        Panel? overflowPanel = OverflowPanel;
        if (overflowPanel is not null)
        {
            List<Control> items = GetItems();
            foreach (Control item in overflowPanel.Children.ToList())
            {
                overflowPanel.Children.Remove(item);
                InsertInOrder(item, this, items);
            }
        }

        base.OnDetachedFromVisualTree(e);
    }
}
