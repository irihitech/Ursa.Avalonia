using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Ursa.Controls;

public class ClockTicks: Control
{
    private Matrix _hourRotationMatrix = Matrix.CreateRotation(Math.PI / 6);
    private Matrix _minuteRotationMatrix = Matrix.CreateRotation(Math.PI / 30);
    
    public static readonly StyledProperty<bool> ShowHourTicksProperty = AvaloniaProperty.Register<ClockTicks, bool>(
        nameof(ShowHourTicks), true);

    public bool ShowHourTicks
    {
        get => GetValue(ShowHourTicksProperty);
        set => SetValue(ShowHourTicksProperty, value);
    }

    public static readonly StyledProperty<bool> ShowMinuteTicksProperty = AvaloniaProperty.Register<ClockTicks, bool>(
        nameof(ShowMinuteTicks), true);

    public bool ShowMinuteTicks
    {
        get => GetValue(ShowMinuteTicksProperty);
        set => SetValue(ShowMinuteTicksProperty, value);
    }

    public static readonly StyledProperty<IBrush?> HourTickForegroundProperty = AvaloniaProperty.Register<ClockTicks, IBrush?>(
        nameof(HourTickForeground));

    public IBrush? HourTickForeground
    {
        get => GetValue(HourTickForegroundProperty);
        set => SetValue(HourTickForegroundProperty, value);
    }

    public static readonly StyledProperty<IBrush?> MinuteTickForegroundProperty = AvaloniaProperty.Register<ClockTicks, IBrush?>(
        nameof(MinuteTickForeground));

    public IBrush? MinuteTickForeground
    {
        get => GetValue(MinuteTickForegroundProperty);
        set => SetValue(MinuteTickForegroundProperty, value);
    }

    public static readonly StyledProperty<double> HourTickLengthProperty = AvaloniaProperty.Register<ClockTicks, double>(
        nameof(HourTickLength), 10);

    public double HourTickLength
    {
        get => GetValue(HourTickLengthProperty);
        set => SetValue(HourTickLengthProperty, value);
    }
    
    public static readonly StyledProperty<double> MinuteTickLengthProperty = AvaloniaProperty.Register<ClockTicks, double>(
        nameof(MinuteTickLength), 5);
    
    public double MinuteTickLength
    {
        get => GetValue(MinuteTickLengthProperty);
        set => SetValue(MinuteTickLengthProperty, value);
    }

    public static readonly StyledProperty<double> HourTickWidthProperty = AvaloniaProperty.Register<ClockTicks, double>(
        nameof(HourTickWidth), 2);

    public double HourTickWidth
    {
        get => GetValue(HourTickWidthProperty);
        set => SetValue(HourTickWidthProperty, value);
    }
    
    public static readonly StyledProperty<double> MinuteTickWidthProperty = AvaloniaProperty.Register<ClockTicks, double>(
        nameof(MinuteTickWidth), 1);
    
    public double MinuteTickWidth
    {
        get => GetValue(MinuteTickWidthProperty);
        set => SetValue(MinuteTickWidthProperty, value);
    }

    static ClockTicks()
    {
        AffectsRender<ClockTicks>(ShowHourTicksProperty);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        double minSize= Math.Min(availableSize.Width, availableSize.Height);
        return new Size(minSize, minSize);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var minSize = Math.Min(finalSize.Width, finalSize.Height);
        return new Size(minSize, minSize);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        var size = Math.Min(Bounds.Width, Bounds.Height);
        var center = size / 2;
        IPen hourTickPen = new Pen(HourTickForeground, HourTickWidth);
        IPen minuteTickPen = new Pen(MinuteTickForeground, MinuteTickWidth);
        double hourTickLength = Math.Min(center, HourTickLength);
        double minuteTickLength = Math.Min(center, MinuteTickLength);
        context.PushTransform(Matrix.CreateTranslation(center, center));
        if (ShowHourTicks)
        {
            for (int i = 0; i < 12; i++)
            {
                DrawTick(context, hourTickPen, center, hourTickLength);
                context.PushTransform(_hourRotationMatrix);
            }
        }

        if (ShowMinuteTicks)
        {
            for (int i = 0; i < 60; i++)
            {
                if (i % 5 != 0)
                {
                    DrawTick(context, minuteTickPen, center, minuteTickLength);
                }
                context.PushTransform(_minuteRotationMatrix);
            }
        }
    }

    private void DrawTick(DrawingContext context, IPen pen, double center, double length)
    {
        var start = new Point(0, -center);
        var end = new Point(0, length-center);
        context.DrawLine(pen, start, end);
    }
}