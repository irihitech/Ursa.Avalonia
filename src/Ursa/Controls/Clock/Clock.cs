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

    public static readonly StyledProperty<double> HourHandMarginProperty = AvaloniaProperty.Register<Clock, double>(
        nameof(HourHandMargin));

    public double HourHandMargin
    {
        get => GetValue(HourHandMarginProperty);
        set => SetValue(HourHandMarginProperty, value);
    }
    
    public static readonly StyledProperty<double> MinuteHandMarginProperty = AvaloniaProperty.Register<Clock, double>(
        nameof(MinuteHandMargin));
    
    public double MinuteHandMargin
    {
        get => GetValue(MinuteHandMarginProperty);
        set => SetValue(MinuteHandMarginProperty, value);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        double min = Math.Min(availableSize.Height, availableSize.Width);
        var newSize = new Size(min, min);
        var size = base.MeasureOverride(newSize);
        return size;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        double min = Math.Min(finalSize.Height, finalSize.Width);
        var newSize = new Size(min, min);
        var size = base.ArrangeOverride(newSize);
        return size;
    }
}