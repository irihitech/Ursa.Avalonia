using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class WrapPanelWithTrailingItem : Panel
{
    public static readonly StyledProperty<Layoutable?> TrailingItemProperty =
        AvaloniaProperty.Register<WrapPanelWithTrailingItem, Layoutable?>(
            nameof(TrailingItem));

    public static readonly StyledProperty<double> TrailingWrapWidthProperty =
        AvaloniaProperty.Register<WrapPanelWithTrailingItem, double>(
            nameof(TrailingWrapWidth));

    static WrapPanelWithTrailingItem()
    {
        AffectsMeasure<WrapPanelWithTrailingItem>(TrailingItemProperty);
        AffectsArrange<WrapPanelWithTrailingItem>(TrailingItemProperty);
    }

    public Layoutable? TrailingItem
    {
        get => GetValue(TrailingItemProperty);
        set => SetValue(TrailingItemProperty, value);
    }

    public double TrailingWrapWidth
    {
        get => GetValue(TrailingWrapWidthProperty);
        set => SetValue(TrailingWrapWidthProperty, value);
    }
    

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == TrailingItemProperty)
        {
            if (change.GetOldValue<Layoutable?>() is { } oldValue)
            {
                VisualChildren.Remove(oldValue);
                if (!IsItemsHost) LogicalChildren.Remove(oldValue);
            }

            if (change.GetNewValue<Layoutable?>() is { } newValue)
            {
                VisualChildren.Add(newValue);
                if (!IsItemsHost) LogicalChildren.Add(newValue);
            }
        }

        WrapPanel p = new WrapPanel();
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        double currentLineX = 0;
        double currentLineHeight = 0;
        double totalHeight = 0;

        var children = Children;
        foreach (var child in children)
        {
            child.Measure(availableSize);
            var deltaX = availableSize.Width - currentLineX;
            // Width is enough to place next child
            if (MathHelpers.GreaterThan(deltaX, child.DesiredSize.Width))
            {
                currentLineX += child.DesiredSize.Width;
                currentLineHeight = Math.Max(currentLineHeight, child.DesiredSize.Height);
            }
            // Width is not enough to place next child
            // reset currentLineX and currentLineHeight
            // accumulate last line height to total height. 
            // Notice: last line height accumulation only happens when restarting a new line, so it needs to finally add one more time outside iteration. 
            else
            {
                currentLineX = child.DesiredSize.Width;
                totalHeight += currentLineHeight;
                currentLineHeight = child.DesiredSize.Height;
            }
        }

        var last = TrailingItem;
        if (last is null) return new Size(availableSize.Width, totalHeight);
        last.Measure(availableSize);
        var lastDeltaX = availableSize.Width - currentLineX;
        // If width is not enough, add a new line, and recalculate total height
        if (lastDeltaX < TrailingWrapWidth)
        {
            totalHeight += currentLineHeight;
            totalHeight += last.DesiredSize.Height;
        }
        else
        {
            currentLineHeight = Math.Max(currentLineHeight, last.DesiredSize.Height);
            totalHeight += currentLineHeight;
        }

        return new Size(availableSize.Width, totalHeight);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        double currentLineX = 0;
        double currentLineHeight = 0;
        double totalHeight = 0;
        var children = Children;

        foreach (var child in children)
        {
            double deltaX = finalSize.Width - currentLineX;
            // Width is enough to place next child
            if (MathHelpers.GreaterThan(deltaX, child.DesiredSize.Width))
            {
                child.Arrange(new Rect(currentLineX, totalHeight, child.DesiredSize.Width,
                    Math.Max(child.DesiredSize.Height, currentLineHeight)));
                currentLineX += child.Bounds.Width;
                currentLineHeight = Math.Max(currentLineHeight, child.Bounds.Height);
            }
            // Width is not enough to place next child
            // reset currentLineX and currentLineHeight
            // accumulate last line height to total height.
            // Notice: last line height accumulation only happens when restarting a new line, so it needs to finally add one more time outside iteration. 
            else
            {
                totalHeight += currentLineHeight;
                child.Arrange(new Rect(0, totalHeight, Math.Min(child.DesiredSize.Width, finalSize.Width),
                    child.DesiredSize.Height));
                currentLineX = child.Bounds.Width;
                currentLineHeight = child.Bounds.Height;
            }
        }

        var last = TrailingItem;
        if (last is null) return new Size(finalSize.Width, totalHeight);
        var lastDeltaX = finalSize.Width - currentLineX;
        // If width is not enough, add a new line, and recalculate total height
        if (lastDeltaX < TrailingWrapWidth)
        {
            totalHeight += currentLineHeight;
            last.Arrange(new Rect(0, totalHeight, finalSize.Width, last.DesiredSize.Height));
            totalHeight += last.DesiredSize.Height;
        }
        else
        {
            currentLineHeight = children.Count == 1 ? finalSize.Height : currentLineHeight;
            last.Arrange(new Rect(currentLineX, totalHeight, lastDeltaX,
                Math.Max(currentLineHeight, last.DesiredSize.Height)));
            currentLineHeight = Math.Max(currentLineHeight, last.DesiredSize.Height);
            totalHeight += currentLineHeight;
        }

        return new Size(finalSize.Width, totalHeight);
    }
}