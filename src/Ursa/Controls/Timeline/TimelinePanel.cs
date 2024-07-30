using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;

public class TimelinePanel: Panel
{
    public static readonly StyledProperty<TimelineDisplayMode> ModeProperty =
        Timeline.ModeProperty.AddOwner<TimelinePanel>();

    public TimelineDisplayMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    static TimelinePanel()
    {
        AffectsMeasure<TimelinePanel>(ModeProperty);
    }
    
    protected override Size MeasureOverride(Size availableSize)
    {
        double left = 0;
        double right = 0;
        double icon = 0;
        double height = 0;
        foreach (var child in Children)
        {
            child.Measure(availableSize);
            if (child is TimelineItem t)
            {
                var doubles = t.GetWidth();
                left = Math.Max(left, doubles.left);
                icon = Math.Max(icon, doubles.mid);
                right = Math.Max(right, doubles.right);
            }
            height+=child.DesiredSize.Height;
        }
        return new Size(left+icon+right, height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        
        double left = 0, mid = 0, right = 0;
        double height = 0;
        foreach (var child in Children)
        {
            if (child is TimelineItem t)
            {
                var doubles = t.GetWidth();
                left = Math.Max(left, doubles.left);
                mid = Math.Max(mid, doubles.mid);
                right = Math.Max(right, doubles.right);
            }
        }

        Rect rect = new Rect(0, 0, left + mid + right, 0);
        foreach (var child in Children)
        {
            if (child is TimelineItem t)
            {
                t.SetWidth(left, mid, right);
                t.InvalidateArrange();
                rect = rect.WithHeight(t.DesiredSize.Height);
                child.Arrange(rect);
                rect = rect.WithY(rect.Y + t.DesiredSize.Height);
                height+=t.DesiredSize.Height;
            }
        }
        return new Size(left + mid + right, height);
    }
}