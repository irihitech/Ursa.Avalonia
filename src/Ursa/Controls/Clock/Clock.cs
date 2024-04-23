using Avalonia;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;

namespace Ursa.Controls;

[TemplatePart(PART_ClockTicks, typeof(ClockTicks))]
public class Clock: TemplatedControl
{
    public const string PART_ClockTicks = "PART_ClockTicks";

    public static readonly StyledProperty<DateTime> TimeProperty = AvaloniaProperty.Register<Clock, DateTime>(
        nameof(Time), defaultBindingMode: BindingMode.TwoWay);

    public DateTime Time
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

    public static readonly StyledProperty<IBrush?> HandBrushProperty = AvaloniaProperty.Register<Clock, IBrush?>(
        nameof(HandBrush));

    public IBrush? HandBrush
    {
        get => GetValue(HandBrushProperty);
        set => SetValue(HandBrushProperty, value);
    }

    public static readonly StyledProperty<bool> ShowHourHandProperty = AvaloniaProperty.Register<Clock, bool>(
        nameof(ShowHourHand), defaultValue: true);

    public bool ShowHourHand
    {
        get => GetValue(ShowHourHandProperty);
        set => SetValue(ShowHourHandProperty, value);
    }

    public static readonly StyledProperty<bool> ShowMinuteHandProperty = AvaloniaProperty.Register<Clock, bool>(
        nameof(ShowMinuteHand), defaultValue: true);
    
    public bool ShowMinuteHand
    {
        get => GetValue(ShowMinuteHandProperty);
        set => SetValue(ShowMinuteHandProperty, value);
    }

    public static readonly StyledProperty<bool> ShowSecondHandProperty = AvaloniaProperty.Register<Clock, bool>(
        nameof(ShowSecondHand), defaultValue: true);
    
    public bool ShowSecondHand
    {
        get => GetValue(ShowSecondHandProperty);
        set => SetValue(ShowSecondHandProperty, value);
    }

    

    public static readonly DirectProperty<Clock, double> HourAngleProperty = AvaloniaProperty.RegisterDirect<Clock, double>(
        nameof(HourAngle), o => o.HourAngle);
    private double _hourAngle;
    public double HourAngle
    {
        get => _hourAngle;
        private set => SetAndRaise(HourAngleProperty, ref _hourAngle, value);
    }
    
    public static readonly DirectProperty<Clock, double> MinuteAngleProperty = AvaloniaProperty.RegisterDirect<Clock, double>(
        nameof(MinuteAngle), o => o.MinuteAngle);
    private double _minuteAngle;
    public double MinuteAngle
    {
        get => _minuteAngle;
        private set => SetAndRaise(MinuteAngleProperty, ref _minuteAngle, value);
    }
    
    public static readonly DirectProperty<Clock, double> SecondAngleProperty = AvaloniaProperty.RegisterDirect<Clock, double>(
        nameof(SecondAngle), o => o.SecondAngle);
    
    private double _secondAngle;
    public double SecondAngle
    {
        get => _secondAngle;
        private set => SetAndRaise(SecondAngleProperty, ref _secondAngle, value);
    }

    static Clock()
    {
        TimeProperty.Changed.AddClassHandler<Clock, DateTime>((clock, args)=>clock.OnTimeChanged(args));
    }

    private void OnTimeChanged(AvaloniaPropertyChangedEventArgs<DateTime> args)
    {
        DateTime time = args.NewValue.Value;
        var hour = time.Hour;
        var minute = time.Minute;
        var second = time.Second;
        var hourAngle = 360.0 / 12 * hour + 360.0 / 12 / 60 * minute;
        var minuteAngle = 360.0 / 60 * minute + 360.0 / 60 / 60 * second;
        var secondAngle = 360.0 / 60 * second;
        HourAngle = hourAngle;
        MinuteAngle = minuteAngle;
        SecondAngle = secondAngle;
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