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
        foreach (var child in Children)
        {
            if (child is TimelineItem t)
            {
                
            }
        }
        return base.MeasureOverride(availableSize);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        return base.ArrangeOverride(finalSize);
        
    }
}