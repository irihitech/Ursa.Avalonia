using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_Track, typeof(RangeTrack))]
[PseudoClasses(PC_Horizontal, PC_Vertical, PC_Pressed)]
public class RangeSlider: TemplatedControl
{
    public const string PART_Track = "PART_Track";
    private const string PC_Horizontal= ":horizontal";
    private const string PC_Vertical = ":vertical";
    private const string PC_Pressed = ":pressed";

    private RangeTrack? _track;
    private bool _isDragging;
    private IDisposable? _pointerPressedDisposable;
    private IDisposable? _pointerMoveDisposable;
    private IDisposable? _pointerReleasedDisposable;

    private const double Tolerance = 0.0001;

    public static readonly StyledProperty<double> MinimumProperty = RangeTrack.MinimumProperty.AddOwner<RangeSlider>();
    public double Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }
    
    public static readonly StyledProperty<double> MaximumProperty = RangeTrack.MaximumProperty.AddOwner<RangeSlider>();
    public double Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }
    
    public static readonly StyledProperty<double> LowerValueProperty = RangeTrack.LowerValueProperty.AddOwner<RangeSlider>();
    public double LowerValue
    {
        get => GetValue(LowerValueProperty);
        set => SetValue(LowerValueProperty, value);
    }
    
    public static readonly StyledProperty<double> UpperValueProperty = RangeTrack.UpperValueProperty.AddOwner<RangeSlider>();
    public double UpperValue
    {
        get => GetValue(UpperValueProperty);
        set => SetValue(UpperValueProperty, value);
    }

    public static readonly StyledProperty<double> TrackWidthProperty = AvaloniaProperty.Register<RangeSlider, double>(
        nameof(TrackWidth));

    public double TrackWidth
    {
        get => GetValue(TrackWidthProperty);
        set => SetValue(TrackWidthProperty, value);
    }

    public static readonly StyledProperty<Orientation> OrientationProperty = RangeTrack.OrientationProperty.AddOwner<RangeSlider>();

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly StyledProperty<bool> IsDirectionReversedProperty =
        RangeTrack.IsDirectionReversedProperty.AddOwner<RangeSlider>();

    public bool IsDirectionReversed
    {
        get => GetValue(IsDirectionReversedProperty);
        set => SetValue(IsDirectionReversedProperty, value);
    }

    public static readonly StyledProperty<double> TickFrequencyProperty = AvaloniaProperty.Register<RangeSlider, double>(
        nameof(TickFrequency));

    public double TickFrequency
    {
        get => GetValue(TickFrequencyProperty);
        set => SetValue(TickFrequencyProperty, value);
    }

    public static readonly StyledProperty<AvaloniaList<double>?> TicksProperty =
        TickBar.TicksProperty.AddOwner<RangeSlider>();

    public AvaloniaList<double>? Ticks
    {
        get => GetValue(TicksProperty);
        set => SetValue(TicksProperty, value);
    }

    public static readonly StyledProperty<TickPlacement> TickPlacementProperty =
        Slider.TickPlacementProperty.AddOwner<RangeSlider>();

    public TickPlacement TickPlacement
    {
        get => GetValue(TickPlacementProperty);
        set => SetValue(TickPlacementProperty, value);
    }

    public static readonly StyledProperty<bool> IsSnapToTickProperty = AvaloniaProperty.Register<RangeSlider, bool>(
        nameof(IsSnapToTick));

    public bool IsSnapToTick
    {
        get => GetValue(IsSnapToTickProperty);
        set => SetValue(IsSnapToTickProperty, value);
    }
    
    public static readonly RoutedEvent<RangeValueChangedEventArgs> ValueChangedEvent =
        RoutedEvent.Register<RangeSlider, RangeValueChangedEventArgs>(nameof(ValueChanged), RoutingStrategies.Bubble);
    
    public event EventHandler<RangeValueChangedEventArgs> ValueChanged
    {
        add => AddHandler(ValueChangedEvent, value);
        remove => RemoveHandler(ValueChangedEvent, value);
    } 
    
    static RangeSlider()
    {
        PressedMixin.Attach<RangeSlider>();
        FocusableProperty.OverrideDefaultValue<RangeSlider>(true);
        IsHitTestVisibleProperty.OverrideDefaultValue<RangeSlider>(true);
        OrientationProperty.OverrideDefaultValue<RangeSlider>(Orientation.Horizontal);
        OrientationProperty.Changed.AddClassHandler<RangeSlider, Orientation>((o,e)=>o.OnOrientationChanged(e));
        MinimumProperty.OverrideDefaultValue<RangeSlider>(0);
        MaximumProperty.OverrideDefaultValue<RangeSlider>(100);
        LowerValueProperty.OverrideDefaultValue<RangeSlider>(0);
        UpperValueProperty.OverrideDefaultValue<RangeSlider>(100);
        LowerValueProperty.Changed.AddClassHandler<RangeSlider, double>((o, e) => o.OnValueChanged(e, true));
        UpperValueProperty.Changed.AddClassHandler<RangeSlider, double>((o, e) => o.OnValueChanged(e, false));
    }

    private void OnValueChanged(AvaloniaPropertyChangedEventArgs<double> args, bool isLower)
    {
        var oldValue = args.OldValue.Value;
        var newValue = args.NewValue.Value;
        if (Math.Abs(oldValue - newValue) > Tolerance)
        {
            RaiseEvent(new RangeValueChangedEventArgs(ValueChangedEvent, this, oldValue, newValue, isLower));
        }
    }

    public RangeSlider()
    {
        UpdatePseudoClasses(Orientation);
    }

    private void OnOrientationChanged(AvaloniaPropertyChangedEventArgs<Orientation> args)
    {
        var value = args.NewValue.Value;
        UpdatePseudoClasses(value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _pointerMoveDisposable?.Dispose();
        _pointerPressedDisposable?.Dispose();
        _pointerReleasedDisposable?.Dispose();
        _track = e.NameScope.Find<RangeTrack>(PART_Track);
        _pointerMoveDisposable = this.AddDisposableHandler(PointerMovedEvent, PointerMove, RoutingStrategies.Tunnel);
        _pointerPressedDisposable = this.AddDisposableHandler(PointerPressedEvent, PointerPress, RoutingStrategies.Tunnel);
        _pointerReleasedDisposable = this.AddDisposableHandler(PointerReleasedEvent, PointerRelease, RoutingStrategies.Tunnel);
    }

    private Thumb? _currentThumb;
    
    private void PointerPress(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            var point = e.GetCurrentPoint(_track);
            _currentThumb = GetThumbByPoint(point);
            MoveToPoint(point);
            _isDragging = true;
        }
    }
    
    private void PointerMove(object? sender, PointerEventArgs args)
    {
        if (!IsEnabled)
        {
            _isDragging = false;
            return;
        }
        if (_isDragging)
        {
            MoveToPoint(args.GetCurrentPoint(_track));
        }
    }
    
    private void PointerRelease(object? sender, PointerReleasedEventArgs e)
    {
        _isDragging = false;
        _currentThumb = null;
    }

    private void MoveToPoint(PointerPoint posOnTrack)
    {
        if (_track is null) return;
        var value = GetValueByPoint(posOnTrack);
        var thumb = _isDragging ? _currentThumb : GetThumbByPoint(posOnTrack);
        if (_currentThumb !=null && _currentThumb != thumb) return;
        if (thumb is null) return;
        if (thumb == _track.LowerThumb)
        {
            if (UpperValue < value) SetCurrentValue(UpperValueProperty, IsSnapToTick ? SnapToTick(value) : value);
            SetCurrentValue(LowerValueProperty, IsSnapToTick ? SnapToTick(value) : value);
        }
        else
        {
            if (LowerValue > value) SetCurrentValue(LowerValueProperty, IsSnapToTick ? SnapToTick(value) : value);
            SetCurrentValue(UpperValueProperty, IsSnapToTick ? SnapToTick(value) : value);
        }
    }

    private double SnapToTick(double value)
    {
        if (IsSnapToTick)
        {
            var previous = Minimum;
            var next = Maximum;
            
            var ticks = Ticks;
            
            if (ticks != null && ticks.Count > 0)
            {
                foreach (var tick in ticks)
                {
                    if (MathHelpers.AreClose(tick, value))
                    {
                        return value;
                    }

                    if (MathHelpers.LessThan(tick, value) && MathHelpers.GreaterThan(tick, previous))
                    {
                        previous = tick;
                    }
                    else if (MathHelpers.GreaterThan(tick, value) && MathHelpers.LessThan(tick, next))
                    {
                        next = tick;
                    }
                }
            }
            else if (MathHelpers.GreaterThan(TickFrequency, 0.0))
            {
                previous = Minimum + Math.Round((value - Minimum) / TickFrequency) * TickFrequency;
                next = Math.Min(Maximum, previous + TickFrequency);
            }
            value = MathHelpers.GreaterThanOrClose(value, (previous + next) * 0.5) ? next : previous;
        }

        return value;
    }

    private Thumb? GetThumbByPoint(PointerPoint point)
    {
        var isHorizontal = Orientation == Orientation.Horizontal;
        var lowerThumbPosition = isHorizontal? _track?.LowerThumb?.Bounds.Center.X : _track?.LowerThumb?.Bounds.Center.Y;
        var upperThumbPosition = isHorizontal? _track?.UpperThumb?.Bounds.Center.X : _track?.UpperThumb?.Bounds.Center.Y;
        var mid = isHorizontal? _track?.Bounds.Center.X : _track?.Bounds.Center.Y;
        var pointerPosition = isHorizontal? point.Position.X : point.Position.Y;

        var lowerDistance = Math.Abs((lowerThumbPosition ?? 0) - pointerPosition);
        var upperDistance = Math.Abs((upperThumbPosition ?? 0) - pointerPosition);

        if (lowerDistance<upperDistance)
        {
            return _track?.LowerThumb;
        }
        if(lowerDistance>upperDistance)
        {
            return _track?.UpperThumb;
        }
        if (IsDirectionReversed) return pointerPosition < mid ? _track?.LowerThumb : _track?.UpperThumb;
        return pointerPosition > mid ? _track?.LowerThumb : _track?.UpperThumb;
    }
    
    private double GetValueByPoint(PointerPoint point)
    {
        if (_track is null) return 0;
        var isHorizontal = Orientation == Orientation.Horizontal;

        var pointPosition = isHorizontal ? point.Position.X : point.Position.Y;
        var ratio = _track.GetRatioByPoint(pointPosition);
        var range = Maximum - Minimum;
        var finalValue = ratio * range + Minimum;
        return finalValue;
    }
    
    private void UpdatePseudoClasses(Orientation o)
    {
        this.PseudoClasses.Set(PC_Vertical, o == Orientation.Vertical);
        this.PseudoClasses.Set(PC_Horizontal, o == Orientation.Horizontal);
    }
}