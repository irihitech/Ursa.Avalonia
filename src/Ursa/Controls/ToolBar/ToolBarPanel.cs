using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

public class ToolBarPanel: StackPanel
{
    private ToolBar? _parent;
    private Panel? _overflowPanel;

    internal Panel? OverflowPanel => _overflowPanel ??= _parent?.OverflowPanel;
    internal ToolBar? ParentToolBar => _parent ??= this.TemplatedParent as ToolBar;

    public static readonly StyledProperty<Orientation> OrientationProperty =
        StackPanel.OrientationProperty.AddOwner<ToolBar>();

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    static ToolBarPanel()
    {
        OrientationProperty.OverrideDefaultValue<ToolBarPanel>(Orientation.Horizontal);
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);
        _parent = this.TemplatedParent as ToolBar;
        if (_parent is null) return;
        this[!OrientationProperty] = _parent[!OrientationProperty];
    }
    
    protected override Size MeasureOverride(Size availableSize)
    {
        var logicalChildren = _parent?.GetLogicalChildren().OfType<Control>().ToList();
        var parent = this.GetLogicalParent();
        Size size = new Size();
        double spacing = 0;
        Size measureSize = availableSize;
        bool horizontal = Orientation == Orientation.Horizontal;
        bool hasVisibleChildren = false;
        int index = 0;
        if (logicalChildren is null) return size;
        for (int i = 0; i < logicalChildren.Count; i++)
        {
            Control control = logicalChildren[i];
            var mode = ToolBar.GetOverflowMode(control);
            control.Measure(measureSize);
            if (mode == OverflowMode.Always)
            {
                ToolBar.SetIsOverflowItem(control, true);
                continue;
            }
            else if (mode == OverflowMode.Never)
            {
                if (control.IsVisible)
                {
                    hasVisibleChildren = true;
                    size = horizontal
                        ? size.WithWidth(size.Width + control.DesiredSize.Width + spacing)
                            .WithHeight(Math.Max(size.Height, control.DesiredSize.Height))
                        : size.WithHeight(size.Height + control.DesiredSize.Height + spacing)
                            .WithWidth(Math.Max(size.Width, control.DesiredSize.Width));
                    ToolBar.SetIsOverflowItem(control, false);
                }
            }
        }
        bool isOverflow = false;
        for(int i = 0; i < logicalChildren.Count; i++)
        {
            Control control = logicalChildren[i];
            var mode = ToolBar.GetOverflowMode(control);
            if (mode != OverflowMode.AsNeeded) continue;
            //Always keeps the order of display. It's very un reasonable to display the second but short control
            //and push the first control to the overflow panel. So once a control is marked as overflow, the following
            //controls will be marked as overflow too.
            if (isOverflow)
            {
                ToolBar.SetIsOverflowItem(control, isOverflow);
                continue;
            }
            bool isFit = horizontal
                ? (size.Width + control.DesiredSize.Width <= availableSize.Width)
                : (size.Height + control.DesiredSize.Height <= availableSize.Height);
            if (isFit)
            {
                ToolBar.SetIsOverflowItem(control, false);
                size = horizontal
                    ? size.WithWidth(size.Width + control.DesiredSize.Width + spacing)
                        .WithHeight(Math.Max(size.Height, control.DesiredSize.Height))
                    : size.WithHeight(size.Height + control.DesiredSize.Height + spacing)
                        .WithWidth(Math.Max(size.Width, control.DesiredSize.Width));
            }
            else
            {
                isOverflow = true;
                ToolBar.SetIsOverflowItem(control, isOverflow);
            }
        }
        if (hasVisibleChildren)
        {
            size = horizontal ? size.WithWidth(size.Width - spacing) : size.WithHeight(size.Height - spacing);
        }

        return size;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        Children.Clear();
        OverflowPanel?.Children.Clear();
        InvalidateVisual();
        var logicalChildren = _parent?.GetLogicalChildren().OfType<Control>().ToList();
        if(logicalChildren is null) return finalSize;
        foreach (var child in logicalChildren)
        {
            if (ToolBar.GetIsOverflowItem(child))
            {
                OverflowPanel?.Children.Add(child);
            }
            else
            {
                this.Children.Add(child);
            }
        }
        return base.ArrangeOverride(finalSize);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        var list = OverflowPanel?.Children?.ToList();
        if (list is not null)
        {
            OverflowPanel?.Children?.Clear();
            this.Children.AddRange(list);
        }
        base.OnDetachedFromVisualTree(e);
    }
}