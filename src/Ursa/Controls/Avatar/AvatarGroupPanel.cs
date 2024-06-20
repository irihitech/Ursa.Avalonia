using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Ursa.Controls;

public class AvatarGroupPanel : Panel
{
    protected override Size MeasureOverride(Size availableSize)
    {
        Size size = new Size();
        availableSize = availableSize.WithWidth(double.PositiveInfinity);
        var children = Children;
        foreach (var child in children)
        {
            child.Measure(availableSize);
            Size desiredSize = child.DesiredSize;
            size = size.WithWidth(size.Width + desiredSize.Width);
            size = size.WithHeight(Math.Max(size.Height, desiredSize.Height));
        }

        size = size.WithWidth(size.Width);
        return size;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        Rect rect = new Rect(finalSize);
        double num = 0d;
        var children = Children;
        foreach (var child in children)
        {
            Size desiredSize = child.DesiredSize;
            double width = desiredSize.Width;
            double height = Math.Max(desiredSize.Height, finalSize.Height);
            rect = rect.WithX(rect.X + num);
            rect = rect.WithWidth(width);
            rect = rect.WithHeight(height);
            num = width;
            child.Arrange(rect);
        }

        RaiseEvent(new RoutedEventArgs(StackPanel.HorizontalSnapPointsChangedEvent));
        return finalSize;
    }
}