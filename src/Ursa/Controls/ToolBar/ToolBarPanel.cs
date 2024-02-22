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
        for(int i = 0; i < logicalChildren.Count; i++)
        {
            Control control = logicalChildren[i];
            var mode = ToolBar.GetOverflowMode(control);
            if (mode != OverflowMode.AsNeeded) continue;
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
                ToolBar.SetIsOverflowItem(control, true);
            }
        }
        /*
        for (int count = logicalChildren.Count; index < count; ++index)
        {
            Control control = logicalChildren[index];
            var mode = ToolBar.GetOverflowMode(control);
            if (mode == OverflowMode.Always)
            {
                ToolBar.SetIsOverflowItem(control, true);
                continue;
            }
            bool isVisible = control.IsVisible;
            if (isVisible)
            {
                hasVisibleChildren = true;
            }
            control.Measure(measureSize);
            Size desiredSize = control.DesiredSize;
            if (horizontal)
            {
                size = size.WithWidth(size.Width + desiredSize.Width + (isVisible ? spacing : 0));
                size = size.WithHeight(Math.Max(size.Height, desiredSize.Height));
            }
            else
            {
                size = size.WithHeight(size.Height + desiredSize.Height + (isVisible ? spacing : 0));
                size = size.WithWidth(Math.Max(size.Width, desiredSize.Width));
            }
        }
        */
        if (hasVisibleChildren)
        {
            size = horizontal ? size.WithWidth(size.Width - spacing) : size.WithHeight(size.Height - spacing);
        }

        return size;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var logicalChildren = _parent?.GetLogicalChildren().OfType<Control>().ToList();
        Children.Clear();
        OverflowPanel?.Children.Clear();
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
        if (_parent is null)
        {
            return finalSize;
        }
        var all = _parent.GetLogicalChildren().OfType<Control>().ToList();
        Children.Clear();
        OverflowPanel?.Children.Clear();
        Size currentSize = new Size();
        bool horizontal = Orientation == Orientation.Horizontal;
        double spacing = 0;
        Rect arrangeRect = new Rect(finalSize);
        double previousChildSize = 0.0;
        for (var i = 0; i < all.Count; i++)
        {
            var control = all[i];
            if (!control.IsVisible) continue;
            var desiredSize = control.DesiredSize;
            var mode = ToolBar.GetOverflowMode(control);
            if (mode == OverflowMode.Always)
            {
                OverflowPanel?.Children.Add(control);
            }
            else
            {
                Children.Add(control);
                if (horizontal)
                {
                    arrangeRect = arrangeRect.WithX(arrangeRect.X + previousChildSize);
                    previousChildSize = control.DesiredSize.Width;
                    arrangeRect = arrangeRect.WithWidth(previousChildSize);
                    arrangeRect = arrangeRect.WithHeight(Math.Max(finalSize.Height, control.DesiredSize.Height));
                    previousChildSize += spacing;
                    currentSize = currentSize.WithWidth(currentSize.Width + desiredSize.Width + spacing);
                    currentSize = currentSize.WithHeight(Math.Max(currentSize.Height, desiredSize.Height));
                }
                else
                {
                    arrangeRect = arrangeRect.WithY(arrangeRect.Y + previousChildSize);
                    previousChildSize = control.DesiredSize.Height;
                    arrangeRect = arrangeRect.WithHeight(previousChildSize);
                    arrangeRect = arrangeRect.WithWidth(Math.Max(finalSize.Width, control.DesiredSize.Width));
                    previousChildSize += spacing;
                    currentSize = currentSize.WithHeight(currentSize.Height + desiredSize.Height + spacing);
                    currentSize = currentSize.WithWidth(Math.Max(currentSize.Width, desiredSize.Width));
                }
                control.Arrange(arrangeRect);
            }
        }
        return currentSize;
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