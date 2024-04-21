using Avalonia;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

[TemplatePart(PART_ClockTicks, typeof(ClockTicks))]
public class Clock: TemplatedControl
{
    public const string PART_ClockTicks = "PART_ClockTicks";

    public static readonly StyledProperty<TimeSpan> TimeProperty = AvaloniaProperty.Register<Clock, TimeSpan>(
        nameof(Time));

    public TimeSpan Time
    {
        get => GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public static readonly StyledProperty<bool> ShowHourTicksProperty =
        ClockTicks.ShowHourTicksProperty.AddOwner<Clock>();
    
    public bool ShowHourTicks
    {
        get => GetValue(ShowHourTicksProperty);
        set => SetValue(ShowHourTicksProperty, value);
    }

    public static readonly StyledProperty<bool> ShowMinuteTicksProperty =
        ClockTicks.ShowMinuteTicksProperty.AddOwner<Clock>();
    
    public bool ShowMinuteTicks
    {
        get => GetValue(ShowMinuteTicksProperty);
        set => SetValue(ShowMinuteTicksProperty, value);
    }
    
}