using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Utilities;

namespace Ursa.Controls;

[TemplatePart(PART_Track, typeof(RangeTrack))]
public class RangeSlider: TemplatedControl
{
    public const string PART_Track = "PART_Track";

    private RangeTrack? _track;
    private bool _isDragging;
    private IDisposable? _pointerPressedDisposable;
    private IDisposable? _pointerMoveDisposable;
    private IDisposable? _pointerReleasedDisposable;

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
    
    static RangeSlider()
    {
        PressedMixin.Attach<RangeSlider>();
        FocusableProperty.OverrideDefaultValue<RangeSlider>(true);
        IsHitTestVisibleProperty.OverrideDefaultValue<RangeSlider>(true);
        OrientationProperty.OverrideDefaultValue<RangeSlider>(Orientation.Horizontal);
        MinimumProperty.OverrideDefaultValue<RangeSlider>(0);
        MaximumProperty.OverrideDefaultValue<RangeSlider>(100);
        LowerValueProperty.OverrideDefaultValue<RangeSlider>(0);
        UpperValueProperty.OverrideDefaultValue<RangeSlider>(100);
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

    private void PointerPress(object sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            var point = e.GetCurrentPoint(_track);
            MoveToPoint(point);
            _isDragging = true;
        }
    }
    
    private void PointerMove(object sender, PointerEventArgs args)
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
    
    private void PointerRelease(object sender, PointerReleasedEventArgs e)
    {
        _isDragging = false;
    }

    private void MoveToPoint(PointerPoint posOnTrack)
    {
        if (_track is null) return;
        var isHorizontal = Orientation == Orientation.Horizontal;
        var thumbLength = _track.GetThumbLength();
        var trackLength = _track.GetTrackLength() - thumbLength;
        var pos = isHorizontal ? posOnTrack.Position.X : posOnTrack.Position.Y;
        var lowerPosition = isHorizontal? _track.LowerThumb.Bounds.X : _track.LowerThumb.Bounds.Y;
        var upperPosition = isHorizontal? _track.UpperThumb.Bounds.X : _track.UpperThumb.Bounds.Y;
        bool lower =  Math.Abs(pos - lowerPosition) < Math.Abs(pos - upperPosition);
        var logicalPosition = MathUtilities.Clamp((pos - thumbLength*0.5) / trackLength, 0.0, 1.0);
        var invert = isHorizontal ? IsDirectionReversed ? 1.0 : 0 :
            IsDirectionReversed ? 0 : 1.0;
        var calValue = Math.Abs(invert - logicalPosition);
        var range = Maximum - Minimum;
        var finalValue = calValue * range + Minimum;
        SetCurrentValue(lower? LowerValueProperty: UpperValueProperty, finalValue);
    }

    private double SnapToTick(double value)
    {
        return value;
    }
}