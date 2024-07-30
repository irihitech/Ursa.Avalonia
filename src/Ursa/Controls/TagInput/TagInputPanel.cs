using Avalonia;
using Avalonia.Controls;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

/// <summary>
/// TagInputPanel is a horizontal wrap with last item filling last row. 
/// </summary>
public class TagInputPanel: Panel
{
    protected override Size MeasureOverride(Size availableSize)
    {
        // return base.MeasureOverride(availableSize);
        double currentLineX = 0;
        double currentLineHeight = 0;
        double totalHeight = 0;
        
        var children = Children;
        for (int i = 0; i < children.Count-1; i++)
        {
            var child = children[i];
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

        var last = children[children.Count - 1];
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

    protected override Size ArrangeOverride(Size finalSize)
    {
        double currentLineX = 0;
        double currentLineHeight = 0;
        double totalHeight = 0;
        var children = Children;
        for (int i = 0; i < children.Count - 1; i++)
        {
            var child = children[i];
            double deltaX = finalSize.Width - currentLineX;
            // Width is enough to place next child
            if (MathHelpers.GreaterThan(deltaX, child.DesiredSize.Width))
            {
                child.Arrange(new Rect(currentLineX, totalHeight, child.DesiredSize.Width, Math.Max(child.DesiredSize.Height, currentLineHeight)));
                currentLineX += child.DesiredSize.Width;
                currentLineHeight = Math.Max(currentLineHeight, child.DesiredSize.Height);
            }
            // Width is not enough to place next child
            // reset currentLineX and currentLineHeight
            // accumulate last line height to total height.
            // Notice: last line height accumulation only happens when restarting a new line, so it needs to finally add one more time outside iteration. 
            else
            {
                totalHeight += currentLineHeight;
                child.Arrange(new Rect(0, totalHeight, Math.Min(child.DesiredSize.Width, finalSize.Width), child.DesiredSize.Height));
                currentLineX = child.DesiredSize.Width;
                currentLineHeight = child.DesiredSize.Height;
            }
        }
        
        var last = children[children.Count - 1];
        double lastDeltaX = finalSize.Width - currentLineX;
        // If width is not enough, add a new line, and recalculate total height
        if (lastDeltaX < 10)
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