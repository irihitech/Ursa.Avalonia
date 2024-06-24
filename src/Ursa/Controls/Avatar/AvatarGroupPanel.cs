using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

public class AvatarGroupPanel : Panel
{
    protected override Size MeasureOverride(Size availableSize)
    {
        var children = Children;
        if (children.Count <= 0) return new Size();

        availableSize = availableSize.WithWidth(double.PositiveInfinity);
        children[0].Measure(availableSize);
        Size first = children[0].DesiredSize;
        var group = this.GetLogicalAncestors().OfType<AvatarGroup>().FirstOrDefault();
        var maxCount = group?.MaxCount;
        var count = children.Count;
        if (maxCount >= 0 && maxCount < count)
        {
            count = maxCount.Value + 1;
        }

        var width = first.Width + first.Width * (count - 1) * 0.75;
        return new Size(width, first.Height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        Rect rect = new Rect(finalSize);
        double num = 0d;
        var children = Children;
        var group = this.GetLogicalAncestors().OfType<AvatarGroup>().FirstOrDefault();
        var overlapFrom = group?.OverlapFrom;
        var maxCount = group?.MaxCount;
        var childrenCount = children.Count;
        var count = maxCount < childrenCount ? maxCount.Value : childrenCount;
        for (var i = 0; i < count; i++)
        {
            if (overlapFrom is OverlapFromType.Start)
            {
                children[i].ZIndex = childrenCount - i;
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

        if (maxCount is not null)
        {
            //TODO: RenderMore
        }

        return finalSize;
    }
}