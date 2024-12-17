using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Irihi.Avalonia.Shared.Helpers;
using Timer = System.Timers.Timer;

namespace Ursa.Controls;

public class Marquee : ContentControl
{
    /// <summary>
    ///     Defines the <see cref="IsRunning" /> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsRunningProperty = AvaloniaProperty.Register<Marquee, bool>(
        nameof(IsRunning));

    /// <summary>
    ///     Defines the <see cref="MarqueeMode" /> property.
    /// </summary>
    public static readonly StyledProperty<Direction> DirectionProperty = AvaloniaProperty.Register<Marquee, Direction>(
        nameof(Direction));

    /// <summary>
    ///     Defines the <see cref="Speed" /> property.
    /// </summary>
    public static readonly StyledProperty<double> SpeedProperty = AvaloniaProperty.Register<Marquee, double>(
        nameof(Speed), 60.0);

    private readonly Timer _timer;

    static Marquee()
    {
        ClipToBoundsProperty.OverrideDefaultValue<Marquee>(true);
    }

    public Marquee()
    {
        _timer = new Timer();
        _timer.Interval = 1000 / 60.0;
        _timer.Elapsed += TimerOnTick;
        _timer.Start();
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the marquee is running.
    /// </summary>
    public bool IsRunning
    {
        get => GetValue(IsRunningProperty);
        set => SetValue(IsRunningProperty, value);
    }

    /// <summary>
    ///     Gets or sets the direction of the marquee.
    /// </summary>
    public Direction Direction
    {
        get => GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    /// <summary>
    ///     Gets or sets the speed of the marquee.
    /// </summary>
    public double Speed
    {
        get => GetValue(SpeedProperty);
        set => SetValue(SpeedProperty, value);
    }

    private void TimerOnTick(object sender, System.EventArgs e)
    {
        Dispatcher.UIThread.Post(UpdateLocation, DispatcherPriority.Background);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var result =  base.MeasureOverride(availableSize);
        var presenter = Presenter;
        if (presenter is null) return result;
        var size = presenter.DesiredSize;
        if (double.IsInfinity(result.Width) || result.Width == 0)
        {
            result = result.WithWidth(size.Width);
        }
        if (double.IsInfinity(result.Height) || result.Height == 0)
        {
            result = result.WithHeight(size.Height);
        }
        return result;
    }

    private void UpdateLocation()
    {
        if (Presenter is null) return;
        var horizontalOffset = Direction switch
        {
            Direction.Up or Direction.Down => 0,
            Direction.Left or Direction.Right => Canvas.GetLeft(Presenter),
        };
        var verticalOffset = Direction switch
        {
            Direction.Up or Direction.Down => Canvas.GetTop(Presenter),
            Direction.Left or Direction.Right => 0,
        };
        if (horizontalOffset is double.NaN) horizontalOffset = 0.0;
        if (verticalOffset is double.NaN) verticalOffset = 0.0;
        var speed = Speed / 60.0;
        var diff = Direction switch
        {
            Direction.Up => -speed,
            Direction.Down => speed,
            Direction.Left => -speed,
            Direction.Right => speed,
            _ => 0
        };
        switch (Direction)
        {
            case Direction.Up:
            case Direction.Down:
                verticalOffset += diff;
                break;
            case Direction.Left:
            case Direction.Right:
                horizontalOffset += diff;
                break;
        }
        switch (Direction)
        {
            case Direction.Down:
                if (verticalOffset > Bounds.Height) verticalOffset = -Presenter.Bounds.Height;
                verticalOffset = MathHelpers.SafeClamp(verticalOffset, -Presenter.Bounds.Height, Bounds.Height);
                break;
            case Direction.Up:
                if (verticalOffset < -Presenter.Bounds.Height) verticalOffset = Bounds.Height;
                verticalOffset = MathHelpers.SafeClamp(verticalOffset, -Presenter.Bounds.Height, Bounds.Height);
                break;
            case Direction.Right:
                if (horizontalOffset > Bounds.Width) horizontalOffset = -Presenter.Bounds.Width;
                horizontalOffset = MathHelpers.SafeClamp(horizontalOffset, -Presenter.Bounds.Width, Bounds.Width);
                break;
            case Direction.Left:
                if (horizontalOffset < -Presenter.Bounds.Width) horizontalOffset = Bounds.Width;
                horizontalOffset = MathHelpers.SafeClamp(horizontalOffset, -Presenter.Bounds.Width, Bounds.Width);
                break;
        }
        Canvas.SetTop(Presenter, verticalOffset);
        Canvas.SetLeft(Presenter, horizontalOffset);
    }
}