using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Styling;

namespace Ursa.Controls;

[TemplatePart(PART_ClockTicks, typeof(ClockTicks))]
public class Clock : TemplatedControl
{
    public const string PART_ClockTicks = "PART_ClockTicks";

    public static readonly StyledProperty<DateTime> TimeProperty = AvaloniaProperty.Register<Clock, DateTime>(
        nameof(Time), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> ShowHourTicksProperty =
        ClockTicks.ShowHourTicksProperty.AddOwner<Clock>();

    public static readonly StyledProperty<bool> ShowMinuteTicksProperty =
        ClockTicks.ShowMinuteTicksProperty.AddOwner<Clock>();

    public static readonly StyledProperty<IBrush?> HandBrushProperty = AvaloniaProperty.Register<Clock, IBrush?>(
        nameof(HandBrush));

    public static readonly StyledProperty<bool> ShowHourHandProperty = AvaloniaProperty.Register<Clock, bool>(
        nameof(ShowHourHand), true);

    public static readonly StyledProperty<bool> ShowMinuteHandProperty = AvaloniaProperty.Register<Clock, bool>(
        nameof(ShowMinuteHand), true);

    public static readonly StyledProperty<bool> ShowSecondHandProperty = AvaloniaProperty.Register<Clock, bool>(
        nameof(ShowSecondHand), true);

    public static readonly StyledProperty<bool> IsSmoothProperty = AvaloniaProperty.Register<Clock, bool>(
        nameof(IsSmooth));


    public static readonly DirectProperty<Clock, double> HourAngleProperty =
        AvaloniaProperty.RegisterDirect<Clock, double>(
            nameof(HourAngle), o => o.HourAngle);

    public static readonly DirectProperty<Clock, double> MinuteAngleProperty =
        AvaloniaProperty.RegisterDirect<Clock, double>(
            nameof(MinuteAngle), o => o.MinuteAngle);

    public static readonly DirectProperty<Clock, double> SecondAngleProperty =
        AvaloniaProperty.RegisterDirect<Clock, double>(
            nameof(SecondAngle), o => o.SecondAngle, (o, v) => o.SecondAngle = v);

    private double _hourAngle;
    private double _minuteAngle;

    private double _secondAngle;

    static Clock()
    {
        TimeProperty.Changed.AddClassHandler<Clock, DateTime>((clock, args) => clock.OnTimeChanged(args));
        IsSmoothProperty.Changed.AddClassHandler<Clock, bool>((clock, args) => clock.OnIsSmoothChanged(args));
    }

    private void OnIsSmoothChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.NewValue.Value && !_cts.IsCancellationRequested )
        {
            _cts.Cancel();
        }
    }

    public DateTime Time
    {
        get => GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public bool ShowHourTicks
    {
        get => GetValue(ShowHourTicksProperty);
        set => SetValue(ShowHourTicksProperty, value);
    }

    public bool ShowMinuteTicks
    {
        get => GetValue(ShowMinuteTicksProperty);
        set => SetValue(ShowMinuteTicksProperty, value);
    }

    public IBrush? HandBrush
    {
        get => GetValue(HandBrushProperty);
        set => SetValue(HandBrushProperty, value);
    }

    public bool ShowHourHand
    {
        get => GetValue(ShowHourHandProperty);
        set => SetValue(ShowHourHandProperty, value);
    }

    public bool ShowMinuteHand
    {
        get => GetValue(ShowMinuteHandProperty);
        set => SetValue(ShowMinuteHandProperty, value);
    }

    public bool ShowSecondHand
    {
        get => GetValue(ShowSecondHandProperty);
        set => SetValue(ShowSecondHandProperty, value);
    }

    public bool IsSmooth
    {
        get => GetValue(IsSmoothProperty);
        set => SetValue(IsSmoothProperty, value);
    }

    public double HourAngle
    {
        get => _hourAngle;
        private set => SetAndRaise(HourAngleProperty, ref _hourAngle, value);
    }

    public double MinuteAngle
    {
        get => _minuteAngle;
        private set => SetAndRaise(MinuteAngleProperty, ref _minuteAngle, value);
    }

    public double SecondAngle
    {
        get => _secondAngle;
        private set => SetAndRaise(SecondAngleProperty, ref _secondAngle, value);
    }

    private Animation _secondsAnimation = new Animation()
    {
        FillMode = FillMode.Forward,
        Duration = TimeSpan.FromSeconds(1),
        Children =
        {
            new KeyFrame
            {
                Cue = new Cue(0.0),
                Setters = { new Setter { Property = SecondAngleProperty } }
            },
            new KeyFrame
            {
                Cue = new Cue(1.0),
                Setters = { new Setter { Property = SecondAngleProperty } }
            }
        }
    };

    private CancellationTokenSource _cts = new CancellationTokenSource();

    private void OnTimeChanged(AvaloniaPropertyChangedEventArgs<DateTime> args)
    {
        var oldSeconds = args.OldValue.Value.Second;
        var time = args.NewValue.Value;
        var hour = time.Hour;
        var minute = time.Minute;
        var second = time.Second;
        var hourAngle = 360.0 / 12 * hour + 360.0 / 12 / 60 * minute;
        var minuteAngle = 360.0 / 60 * minute + 360.0 / 60 / 60 * second;
        if (second == 0) second = 60;
        var oldSecondAngle = 360.0 / 60 * oldSeconds; 
        var secondAngle = 360.0 / 60 * second;
        HourAngle = hourAngle;
        MinuteAngle = minuteAngle;
        if (!IsLoaded || !IsSmooth)
        {
            SecondAngle = secondAngle;
        }
        else
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
            if (_secondsAnimation.Children[0].Setters[0] is Setter start)
            {
                start.Value = oldSecondAngle;
            }
            if (_secondsAnimation.Children[1].Setters[0] is Setter end)
            {
                end.Value = secondAngle;
            }
            _secondsAnimation.RunAsync(this, _cts.Token);
        }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var min = Math.Min(availableSize.Height, availableSize.Width);
        var newSize = new Size(min, min);
        var size = base.MeasureOverride(newSize);
        return size;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var min = Math.Min(finalSize.Height, finalSize.Width);
        var newSize = new Size(min, min);
        var size = base.ArrangeOverride(newSize);
        return size;
    }
}