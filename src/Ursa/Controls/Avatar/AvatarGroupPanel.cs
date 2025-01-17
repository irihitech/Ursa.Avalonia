using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

public class AvatarGroupPanel : Panel
{
    public static readonly AttachedProperty<int> OverflowedItemCountProperty =
        AvaloniaProperty.RegisterAttached<AvatarGroupPanel, AvatarGroup, int>("OverflowedItemCount");

    public static void SetOverflowedItemCount(AvatarGroup obj, int value) =>
        obj.SetValue(OverflowedItemCountProperty, value);

    public static int GetOverflowedItemCount(AvatarGroup obj) => obj.GetValue(OverflowedItemCountProperty);

    protected override Size MeasureOverride(Size availableSize)
    {
        if (Children.Count <= 0) return new Size();
        var group = this.GetLogicalAncestors().OfType<AvatarGroup>().FirstOrDefault();
        var maxCount = group?.MaxCount;
        int inlineCount = 0, overflowCount = 0;
        availableSize = availableSize.WithWidth(double.PositiveInfinity);
        foreach (var child in Children)
        {
            child.Measure(availableSize);
            if (inlineCount >= maxCount)
            {
                child.IsVisible = false;
                overflowCount++;
            }
            else
            {
                child.IsVisible = true;
                inlineCount++;
            }
        }

        if (group is not null)
        {
            SetOverflowedItemCount(group, overflowCount);
        }

        Size first = Children[0].DesiredSize;
        var width = first.Width * inlineCount * 0.75;
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