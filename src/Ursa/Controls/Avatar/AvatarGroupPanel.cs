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
        if (children.Count <= 0) return size;
        children[0].Measure(availableSize);
        Size first = children[0].DesiredSize;
        var group = this.GetLogicalAncestors().OfType<AvatarGroup>().FirstOrDefault();
        var count = children.Count;
        if (group?.MaxCount is not null && group.MaxCount >= 0)
        {
            count = group.MaxCount.Value + 1;
        }

        var width = first.Width + first.Width * (count - 1) * 0.75;
        size = size.WithWidth(width);
        size = size.WithHeight(first.Height);
        return size;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        Rect rect = new Rect(finalSize);
        double num = 0d;
        var children = Children;
        var group = this.GetLogicalAncestors().OfType<AvatarGroup>().FirstOrDefault();
        var overlapFrom = group?.OverlapFrom;
        int? maxCount = null;
        var count = children.Count;
        if (group?.MaxCount is not null && group.MaxCount >= 0)
        {
            maxCount = group.MaxCount;
            count = maxCount.Value + 1;
        }

        for (var i = 0; i < count; i++)
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

        if (maxCount is not null && children.Count > 0)
        {
            if (children[maxCount.Value] is Avatar avatar)
            {
                avatar.Content = $"+{children.Count - maxCount}";
            }
        }

        return finalSize;
    }
}