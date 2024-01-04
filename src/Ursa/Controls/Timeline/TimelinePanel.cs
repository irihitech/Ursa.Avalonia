using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

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
        double width = 0;
        double height = 0;
        foreach (var child in Children)
        {
            child.Measure(availableSize);
            if (child is TimelineItem t)
            {
                var doubles = t.GetWidth();
            }
            width = Math.Max(width, child.DesiredSize.Width);
            height+=child.DesiredSize.Height;
        }
        foreach (var child in Children)
        {
            if (child is TimelineItem t)
            {
                t.LeftWidth = left;
                t.RightWidth = right;
                t.IconWidth = icon;
            }
        }

        return new Size(width, height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        Rect rect = new Rect();
        foreach (var child in Children)
        {
            rect = rect.WithWidth(Math.Max(rect.Width, child.DesiredSize.Width));
            rect = rect.WithHeight(rect.Height + child.DesiredSize.Height);
            child.Arrange(rect);
            rect = rect.WithY(rect.Y+child.DesiredSize.Height);
            if (child is TimelineItem t)
            {
                var doubles = t.GetWidth();
                t.SetWidth(0, 0, 0, 0);
            }
            
        }
        //return base.ArrangeOverride(finalSize);
        return rect.Size;
    }
}