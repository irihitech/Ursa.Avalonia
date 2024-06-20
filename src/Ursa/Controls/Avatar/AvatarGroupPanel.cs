using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

public class AvatarGroupPanel : Panel
{
    protected override Size MeasureOverride(Size availableSize)
    {
        Size size = new Size();
        availableSize = availableSize.WithWidth(double.PositiveInfinity);
        var children = Children;
        if (children.Count > 0)
        {
            children[0].Measure(availableSize);
            Size first = children[0].DesiredSize;
            var width = first.Width + first.Width * (children.Count - 1) * 0.75;
            size = size.WithWidth(width);
            size = size.WithHeight(first.Height);
        }

        size = size.WithWidth(size.Width);
        return size;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        Rect rect = new Rect(finalSize);
        double num = 0d;
        var group = this.GetLogicalAncestors().OfType<AvatarGroup>().FirstOrDefault();
        var overlapFrom = group?.OverlapFrom;
        var children = Children;
        for (var i = 0; i < children.Count; i++)
        {
            if (overlapFrom is OverlapFromType.Start)
            {
                children[i].ZIndex = children.Count - i;
            }

            children[i].Measure(finalSize);
            Size desiredSize = children[i].DesiredSize;
            double width = desiredSize.Width;
            double height = Math.Max(desiredSize.Height, finalSize.Height);
            rect = rect.WithX(rect.X + num);
            rect = rect.WithWidth(width);
            rect = rect.WithHeight(height);
            num = width * 0.75;
            children[i].Arrange(rect);
        }

        return finalSize;
    }
}