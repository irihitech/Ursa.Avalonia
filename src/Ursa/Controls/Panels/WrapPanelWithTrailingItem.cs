using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls.Panels;

public class WrapPanelWithTrailingItem: Panel
{
    public static readonly StyledProperty<Layoutable?> TrailingItemProperty = AvaloniaProperty.Register<WrapPanelWithTrailingItem, Layoutable?>(
        nameof(TrailingItem));

    public Layoutable? TrailingItem
    {
        get => GetValue(TrailingItemProperty);
        set => SetValue(TrailingItemProperty, value);
    }
    
    

    static WrapPanelWithTrailingItem()
    {
        AffectsMeasure<WrapPanelWithTrailingItem>(TrailingItemProperty);
        AffectsArrange<WrapPanelWithTrailingItem>(TrailingItemProperty);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == TrailingItemProperty)
        {
            if(change.GetOldValue<Visual?>() is { } oldValue)
            {
                VisualChildren.Remove(oldValue);
                LogicalChildren.Remove(oldValue);
            }
            if(change.GetNewValue<Visual?>() is {} newValue)
            {
                VisualChildren.Add(newValue);
                LogicalChildren.Add(newValue);
            }
        }

        WrapPanel p = new WrapPanel();
    }

    protected override void ChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        base.ChildrenChanged(sender, e);
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
            double deltaX = availableSize.Width - currentLineX;
            // Width is enough to place next child
            if (MathHelpers.GreaterThan(deltaX, child.DesiredSize.Width))
            {
                currentLineX+=child.DesiredSize.Width;
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
        last.Measure(availableSize);
        double lastDeltaX = availableSize.Width - currentLineX;
        // If width is not enough, add a new line, and recalculate total height
        if (lastDeltaX < 30)
        {
            totalHeight+=currentLineHeight;
            totalHeight += last.DesiredSize.Height;
        }
        else
        {
            currentLineHeight = Math.Max(currentLineHeight, last.DesiredSize.Height);
            totalHeight += currentLineHeight;
        }

        return new Size(availableSize.Width, totalHeight);
    }
}