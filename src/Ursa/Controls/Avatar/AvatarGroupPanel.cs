using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

public class AvatarGroupPanel : Panel
{
    protected override Size MeasureOverride(Size availableSize)
    {
        if (Children.Count <= 0) return new Size();

        availableSize = availableSize.WithWidth(double.PositiveInfinity);
        foreach (var child in Children)
        {
            child.Measure(availableSize);
        }

        Size first = Children[0].DesiredSize;
        var group = this.GetLogicalAncestors().OfType<AvatarGroup>().FirstOrDefault();
        var maxCount = group?.MaxCount;
        var count = Children.Count;
        if (maxCount >= 0 && maxCount < count)
        {
            count = maxCount.Value;
        }

        var width = first.Width * count * 0.75;
        return new Size(width, first.Height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        Rect rect = new Rect(finalSize);
        double offset = 0d;
        foreach (var child in Children)
        {
            double desiredWidth = child.DesiredSize.Width;
            double desiredHeight = Math.Max(child.DesiredSize.Height, finalSize.Height);
            rect = new Rect(rect.X + offset, rect.Y, desiredWidth, desiredHeight);
            child.Arrange(rect);
            offset = desiredWidth * 0.75;
        }

        return finalSize;
    }
}