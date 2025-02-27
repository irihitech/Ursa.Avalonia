using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
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
        nameof(IsRunning), true);

    /// <summary>
    ///     Defines the <see cref="Direction" /> property.
    /// </summary>
    public static readonly StyledProperty<Direction> DirectionProperty = AvaloniaProperty.Register<Marquee, Direction>(
        nameof(Direction));

    /// <summary>
    ///     Defines the <see cref="Speed" /> property.
    /// </summary>
    public static readonly StyledProperty<double> SpeedProperty = AvaloniaProperty.Register<Marquee, double>(
        nameof(Speed), 60.0, coerce: OnCoerceSpeed);

    private static double OnCoerceSpeed(AvaloniaObject arg1, double arg2)
    {
        if (arg2 < 0) return 0;
        return arg2;
    }

    private Timer? _timer;

    static Marquee()
    {
        ClipToBoundsProperty.OverrideDefaultValue<Marquee>(true);
        HorizontalContentAlignmentProperty.OverrideDefaultValue<Marquee>(HorizontalAlignment.Center);
        VerticalContentAlignmentProperty.OverrideDefaultValue<Marquee>(VerticalAlignment.Center);
        HorizontalContentAlignmentProperty.Changed.AddClassHandler<Marquee>((o,_)=>o.InvalidatePresenterPosition());
        VerticalContentAlignmentProperty.Changed.AddClassHandler<Marquee>((o,_)=>o.InvalidatePresenterPosition());
        IsRunningProperty.Changed.AddClassHandler<Marquee, bool>((o, args) => o.OnIsRunningChanged(args));
    }

    private void OnIsRunningChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.NewValue.Value)
        {
            _timer?.Start();
        }
        else
        {
            _timer?.Stop();
        }
    }

    public Marquee()
    {
        _timer = new Timer();
        _timer.Interval = 1000 / 60.0;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (Presenter is not null)
        {
            Presenter.SizeChanged+= OnPresenterSizeChanged;
        }
        _timer?.Stop();
        _timer?.Dispose();
        _timer = new Timer();
        _timer.Interval = 1000 / 60.0;
        _timer.Elapsed += TimerOnTick;
        if (IsRunning)
        {
            _timer.Start();
        }
    }
    
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (_timer is not null)
        {
            _timer.Elapsed -= TimerOnTick;
            _timer.Stop();
            _timer.Dispose();
        }
        if (Presenter is not null)
        {
            Presenter.SizeChanged -= OnPresenterSizeChanged;
        }
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
    ///     Gets or sets the speed of the marquee. Point per second.
    /// </summary>
    public double Speed
    {
        get => GetValue(SpeedProperty);
        set => SetValue(SpeedProperty, value);
    }

    private void OnPresenterSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        InvalidatePresenterPosition();
    }

    

    private void TimerOnTick(object? sender, System.EventArgs e)
    {
        if (Presenter is null) return;
        var layoutValues = Dispatcher.UIThread.Invoke(GetLayoutValues);
        var location = UpdateLocation(layoutValues);
        if (location is null) return;
        Dispatcher.UIThread.Post(() =>
        {
            Canvas.SetTop(Presenter, location.Value.top);
            Canvas.SetLeft(Presenter, location.Value.left);
        }, DispatcherPriority.Render);
    }

    private void InvalidatePresenterPosition()
    {
        if (Presenter is null) return;
        var layoutValues = GetLayoutValues();
        var location = UpdateLocation(layoutValues);
        if (location is null) return;
        Canvas.SetTop(Presenter, location.Value.top);
        Canvas.SetLeft(Presenter, location.Value.left);
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

    private (double top, double left)? UpdateLocation(LayoutValues values)
    {
        var horizontalOffset = values.Direction switch
        {
            Direction.Up or Direction.Down => GetHorizontalOffset(values.Bounds, values.PresenterSize, values.HorizontalAlignment),
            Direction.Left or Direction.Right => values.Left,
            _ => throw new NotImplementedException(),
        };
        var verticalOffset = values.Direction switch
        {
            Direction.Up or Direction.Down => values.Top,
            Direction.Left or Direction.Right => GetVerticalOffset(values.Bounds, values.PresenterSize, values.VerticalAlignment),
            _ => throw new NotImplementedException(),
        };
        if (horizontalOffset is double.NaN) horizontalOffset = 0.0;
        if (verticalOffset is double.NaN) verticalOffset = 0.0;
        var speed = values.Diff;
        var diff = values.Direction switch
        {
            Direction.Up => -speed,
            Direction.Down => speed,
            Direction.Left => -speed,
            Direction.Right => speed,
            _ => 0
        };
        switch (values.Direction)
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
        switch (values.Direction)
        {
            case Direction.Down:
                if (verticalOffset > values.Bounds.Height) verticalOffset = -values.PresenterSize.Height;
                break;
            case Direction.Up:
                if (verticalOffset < -values.PresenterSize.Height) verticalOffset = values.Bounds.Height;
                break;
            case Direction.Right:
                if (horizontalOffset > values.Bounds.Width) horizontalOffset = -values.PresenterSize.Width;
                break;
            case Direction.Left:
                if (horizontalOffset < -values.PresenterSize.Width) horizontalOffset = values.Bounds.Width;
                break;
        }
        verticalOffset = MathHelpers.SafeClamp(verticalOffset, -values.PresenterSize.Height, values.Bounds.Height);
        horizontalOffset = MathHelpers.SafeClamp(horizontalOffset, -values.PresenterSize.Width, values.Bounds.Width);
        return (verticalOffset, horizontalOffset);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private double GetHorizontalOffset(Size bounds, Size presenterBounds, HorizontalAlignment horizontalAlignment)
    {
        return horizontalAlignment switch
        {
            HorizontalAlignment.Left => 0,
            HorizontalAlignment.Center => (bounds.Width - presenterBounds.Width) / 2,
            HorizontalAlignment.Right => bounds.Width - presenterBounds.Width,
            _ => 0
        };
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private double GetVerticalOffset(Size bounds, Size presenterBounds, VerticalAlignment verticalAlignment)
    {
        return verticalAlignment switch
        {
            VerticalAlignment.Top => 0,
            VerticalAlignment.Center => (bounds.Height - presenterBounds.Height) / 2,
            VerticalAlignment.Bottom => bounds.Height - presenterBounds.Height,
            _ => 0
        };
    }
    
    private LayoutValues GetLayoutValues()
    {
        return new LayoutValues
        {
            Bounds = Bounds.Size,
            PresenterSize = Presenter?.Bounds.Size ?? new Size(),
            Left = Presenter is null? 0 : Canvas.GetLeft(Presenter),
            Top = Presenter is null? 0 : Canvas.GetTop(Presenter),
            Diff = IsRunning ? Speed / 60.0 : 0,
            HorizontalAlignment = HorizontalContentAlignment,
            VerticalAlignment = VerticalContentAlignment,
            Direction = Direction
        };
    }
}

struct LayoutValues
{
    public Size Bounds { get; set; }
    public Size PresenterSize { get; set; }
    public double Left { get; set; }
    public double Top { get; set; }
    public double Diff { get; set; }
    public Direction Direction { get; set; }
    public HorizontalAlignment HorizontalAlignment { get; set; }
    public VerticalAlignment VerticalAlignment { get; set; }
}