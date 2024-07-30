using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

/// <summary>
/// 1. Notice that this is not used in ScrollBar, so ViewportSize related feature is not necessary.
/// 2. Maximum, Minimum, MaxValue and MinValue are coerced there.
/// </summary>
[PseudoClasses(PC_Horizontal, PC_Vertical)]
public class RangeTrack: Control
{
    public const string PC_Horizontal = ":horizontal";
    public const string PC_Vertical = ":vertical";

    private const double Tolerance = 0.0001;

    public static readonly StyledProperty<double> MinimumProperty = AvaloniaProperty.Register<RangeTrack, double>(
        nameof(Minimum), coerce: CoerceMinimum, defaultBindingMode:BindingMode.TwoWay);

    public double Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public static readonly StyledProperty<double> MaximumProperty = AvaloniaProperty.Register<RangeTrack, double>(
        nameof(Maximum), coerce: CoerceMaximum, defaultBindingMode: BindingMode.TwoWay);

    public double Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }
    
    public static readonly StyledProperty<double> LowerValueProperty = AvaloniaProperty.Register<RangeTrack, double>(
        nameof(LowerValue), coerce: CoerceLowerValue, defaultBindingMode: BindingMode.TwoWay);
    
    public double LowerValue
    {
        get => GetValue(LowerValueProperty);
        set => SetValue(LowerValueProperty, value);
    }
    
    public static readonly StyledProperty<double> UpperValueProperty = AvaloniaProperty.Register<RangeTrack, double>(
        nameof(UpperValue), coerce: CoerceUpperValue, defaultBindingMode: BindingMode.TwoWay);
    
    public double UpperValue
    {
        get => GetValue(UpperValueProperty);
        set => SetValue(UpperValueProperty, value);
    }

    public static readonly StyledProperty<Orientation> OrientationProperty = AvaloniaProperty.Register<RangeTrack, Orientation>(
        nameof(Orientation));

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly StyledProperty<Control?> UpperSectionProperty = AvaloniaProperty.Register<RangeTrack, Control?>(
        nameof(UpperSection));

    public Control? UpperSection
    {
        get => GetValue(UpperSectionProperty);
        set => SetValue(UpperSectionProperty, value);
    }

    public static readonly StyledProperty<Control?> LowerSectionProperty = AvaloniaProperty.Register<RangeTrack, Control?>(
        nameof(LowerSection));

    public Control? LowerSection
    {
        get => GetValue(LowerSectionProperty);
        set => SetValue(LowerSectionProperty, value);
    }

    public static readonly StyledProperty<Control?> InnerSectionProperty = AvaloniaProperty.Register<RangeTrack, Control?>(
        nameof(InnerSection));

    public Control? InnerSection
    {
        get => GetValue(InnerSectionProperty);
        set => SetValue(InnerSectionProperty, value);
    }

    public static readonly StyledProperty<Control?> TrackBackgroundProperty = AvaloniaProperty.Register<RangeTrack, Control?>(
        nameof(TrackBackground));

    public Control? TrackBackground
    {
        get => GetValue(TrackBackgroundProperty);
        set => SetValue(TrackBackgroundProperty, value);
    }
    
    public static readonly StyledProperty<Thumb?> UpperThumbProperty = AvaloniaProperty.Register<RangeTrack, Thumb?>(
        nameof(UpperThumb));
    
    public Thumb? UpperThumb
    {
        get => GetValue(UpperThumbProperty);
        set => SetValue(UpperThumbProperty, value);
    }
    
    public static readonly StyledProperty<Thumb?> LowerThumbProperty = AvaloniaProperty.Register<RangeTrack, Thumb?>(
        nameof(LowerThumb));
    
    public Thumb? LowerThumb
    {
        get => GetValue(LowerThumbProperty);
        set => SetValue(LowerThumbProperty, value);
    }

    public static readonly StyledProperty<bool> IsDirectionReversedProperty = AvaloniaProperty.Register<RangeTrack, bool>(
        nameof(IsDirectionReversed));

    public bool IsDirectionReversed
    {
        get => GetValue(IsDirectionReversedProperty);
        set => SetValue(IsDirectionReversedProperty, value);
    }

    public static readonly RoutedEvent<RangeValueChangedEventArgs> ValueChangedEvent =
        RoutedEvent.Register<RangeTrack, RangeValueChangedEventArgs>(nameof(ValueChanged), RoutingStrategies.Bubble);
    
    public event EventHandler<RangeValueChangedEventArgs> ValueChanged
    {
        add => AddHandler(ValueChangedEvent, value);
        remove => RemoveHandler(ValueChangedEvent, value);
    } 
    
    static RangeTrack()
    {
        OrientationProperty.Changed.AddClassHandler<RangeTrack, Orientation>((o, e) => o.OnOrientationChanged(e));
        LowerThumbProperty.Changed.AddClassHandler<RangeTrack, Thumb?>((o, e) => o.OnThumbChanged(e));
        UpperThumbProperty.Changed.AddClassHandler<RangeTrack, Thumb?>((o, e) => o.OnThumbChanged(e));
        LowerSectionProperty.Changed.AddClassHandler<RangeTrack, Control?>((o, e) => o.OnSectionChanged(e));
        UpperSectionProperty.Changed.AddClassHandler<RangeTrack, Control?>((o, e) => o.OnSectionChanged(e));
        InnerSectionProperty.Changed.AddClassHandler<RangeTrack, Control?>((o, e) => o.OnSectionChanged(e));
        MinimumProperty.Changed.AddClassHandler<RangeTrack, double>((o, e) => o.OnMinimumChanged(e));
        MaximumProperty.Changed.AddClassHandler<RangeTrack, double>((o, e) => o.OnMaximumChanged(e));
        LowerValueProperty.Changed.AddClassHandler<RangeTrack, double>((o, e) => o.OnValueChanged(e, true));
        UpperValueProperty.Changed.AddClassHandler<RangeTrack, double>((o, e) => o.OnValueChanged(e, false));
        AffectsArrange<RangeTrack>(
            MinimumProperty, 
            MaximumProperty, 
            LowerValueProperty, 
            UpperValueProperty, 
            OrientationProperty, 
            IsDirectionReversedProperty);
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

    private void OnMinimumChanged(AvaloniaPropertyChangedEventArgs<double> _)
    {
        if (IsInitialized)
        {
            CoerceValue(MaximumProperty);
            CoerceValue(LowerValueProperty);
            CoerceValue(UpperValueProperty);
        }
    }
    
    private void OnMaximumChanged(AvaloniaPropertyChangedEventArgs<double> _)
    {
        if (IsInitialized)
        {
            CoerceValue(LowerValueProperty);
            CoerceValue(UpperValueProperty);
        }
    }

    private void OnSectionChanged(AvaloniaPropertyChangedEventArgs<Control?> args)
    {
        var oldSection = args.OldValue.Value;
        var newSection = args.NewValue.Value;
        if (oldSection is not null)
        {
            LogicalChildren.Remove(oldSection);
            VisualChildren.Remove(oldSection);
        }
        if (newSection is not null)
        {
            LogicalChildren.Add(newSection);
            VisualChildren.Add(newSection);
        }
    }

    private void OnThumbChanged(AvaloniaPropertyChangedEventArgs<Thumb?> args)
    {
        var oldThumb = args.OldValue.Value;
        var newThumb = args.NewValue.Value;
        if(oldThumb is not null)
        {
            LogicalChildren.Remove(oldThumb);
            VisualChildren.Remove(oldThumb);
        }
        if (newThumb is not null)
        {
            newThumb.ZIndex = 5;
            LogicalChildren.Add(newThumb);
            VisualChildren.Add(newThumb);
        }
    }

    private void OnOrientationChanged(AvaloniaPropertyChangedEventArgs<Orientation> args)
    {
        Orientation o = args.NewValue.Value;
        PseudoClasses.Set(PC_Horizontal, o == Orientation.Horizontal);
        PseudoClasses.Set(PC_Vertical, o == Orientation.Vertical);
    }

    private static double CoerceMaximum(AvaloniaObject sender, double value)
    {
        return ValidateDouble(value)
            ? Math.Max(value, sender.GetValue(MinimumProperty))
            : sender.GetValue(MaximumProperty);
    }

    private static double CoerceMinimum(AvaloniaObject sender, double value)
    {
        return ValidateDouble(value) ? value : sender.GetValue(MinimumProperty);
    }

    private static double CoerceLowerValue(AvaloniaObject sender, double value)
    {
        if (!ValidateDouble(value)) return sender.GetValue(LowerValueProperty);
        value = MathHelpers.SafeClamp(value, sender.GetValue(MinimumProperty), sender.GetValue(MaximumProperty));
        value = MathHelpers.SafeClamp(value, sender.GetValue(MinimumProperty), sender.GetValue(UpperValueProperty));
        return value;
    }

    private static double CoerceUpperValue(AvaloniaObject sender, double value)
    {
        if (!ValidateDouble(value)) return sender.GetValue(UpperValueProperty);
        value = MathHelpers.SafeClamp(value, sender.GetValue(MinimumProperty), sender.GetValue(MaximumProperty));
        value = MathHelpers.SafeClamp(value, sender.GetValue(LowerValueProperty), sender.GetValue(MaximumProperty));
        return value;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        CoerceValue(MaximumProperty);
        CoerceValue(LowerValueProperty);
        CoerceValue(UpperValueProperty);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var desiredSize = new Size();
        if (LowerThumb is not null && UpperThumb is not null)
        {
            LowerThumb.Measure(availableSize);
            UpperThumb.Measure(availableSize);
            if (Orientation == Orientation.Horizontal)
            {
                desiredSize = new Size(LowerThumb.DesiredSize.Width + UpperThumb.DesiredSize.Width,
                    Math.Max(LowerThumb.DesiredSize.Height, UpperThumb.DesiredSize.Height));
            }
            else
            {
                desiredSize = new Size(Math.Max(LowerThumb.DesiredSize.Width, UpperThumb.DesiredSize.Width),
                    LowerThumb.DesiredSize.Height + UpperThumb.DesiredSize.Height);
            }
        }
        return desiredSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var vertical = Orientation == Orientation.Vertical;
        ComputeSliderLengths(finalSize, vertical, out var lowerButtonLength, out var innerButtonLength, out var upperButtonLength,
            out var lowerThumbLength, out var upperThumbLength);
        var offset = new Point();
        var pieceSize = finalSize;
        if (vertical)
        {
            CoerceLength(ref lowerButtonLength, finalSize.Height);
            CoerceLength(ref innerButtonLength, finalSize.Height);
            CoerceLength(ref upperButtonLength, finalSize.Height);
            CoerceLength(ref lowerThumbLength, finalSize.Height);
            CoerceLength(ref upperThumbLength, finalSize.Height);
            if (IsDirectionReversed)
            {
                offset = offset.WithY(lowerThumbLength * 0.5);
                pieceSize = pieceSize.WithHeight(lowerButtonLength);
                LowerSection?.Arrange(new Rect(offset, pieceSize));
                offset = offset.WithY(offset.Y + lowerButtonLength);
                pieceSize = pieceSize.WithHeight(innerButtonLength);
                InnerSection?.Arrange(new Rect(offset, pieceSize));
                offset = offset.WithY(offset.Y + innerButtonLength);
                pieceSize = pieceSize.WithHeight(upperButtonLength);
                UpperSection?.Arrange(new Rect(offset, pieceSize));

                offset = offset.WithY(lowerButtonLength);
                pieceSize = pieceSize.WithHeight(lowerThumbLength);
                LowerThumb?.Arrange(new Rect(offset, pieceSize));

                offset = offset.WithY(lowerButtonLength + innerButtonLength);
                pieceSize = pieceSize.WithHeight(upperThumbLength);
                UpperThumb?.Arrange(new Rect(offset, pieceSize));
            }
            else
            {
                offset = offset.WithY(upperThumbLength * 0.5);
                pieceSize = pieceSize.WithHeight(upperButtonLength);
                UpperSection?.Arrange(new Rect(offset, pieceSize));
                offset = offset.WithY(offset.Y + upperButtonLength);
                pieceSize = pieceSize.WithHeight(innerButtonLength);
                InnerSection?.Arrange(new Rect(offset, pieceSize));
                offset = offset.WithY(offset.Y + innerButtonLength);
                pieceSize = pieceSize.WithHeight(lowerButtonLength);
                LowerSection?.Arrange(new Rect(offset, pieceSize));

                offset = offset.WithY(upperButtonLength);
                pieceSize = pieceSize.WithHeight(upperThumbLength);
                UpperThumb?.Arrange(new Rect(offset, pieceSize));

                offset = offset.WithY(upperButtonLength + innerButtonLength);
                pieceSize = pieceSize.WithHeight(lowerThumbLength);
                LowerThumb?.Arrange(new Rect(offset, pieceSize));
            }
        }
        else
        {
            CoerceLength(ref lowerButtonLength, finalSize.Width);
            CoerceLength(ref innerButtonLength, finalSize.Width);
            CoerceLength(ref upperButtonLength, finalSize.Width);
            CoerceLength(ref lowerThumbLength, finalSize.Width);
            CoerceLength(ref upperThumbLength, finalSize.Width);
            if (IsDirectionReversed)
            {
                offset = offset.WithX(upperThumbLength * 0.5);
                pieceSize = pieceSize.WithWidth(upperButtonLength);
                UpperSection?.Arrange(new Rect(offset, pieceSize));
                offset = offset.WithX(offset.X + upperButtonLength);
                pieceSize = pieceSize.WithWidth(innerButtonLength);
                InnerSection?.Arrange(new Rect(offset, pieceSize));
                offset = offset.WithX(offset.X + innerButtonLength);
                pieceSize = pieceSize.WithWidth(lowerButtonLength);
                LowerSection?.Arrange(new Rect(offset, pieceSize));
                
                offset = offset.WithX(upperButtonLength);
                pieceSize = pieceSize.WithWidth(upperThumbLength);
                UpperThumb?.Arrange(new Rect(offset, pieceSize));
                
                offset = offset.WithX(upperButtonLength+innerButtonLength);
                pieceSize = pieceSize.WithWidth(lowerThumbLength);
                LowerThumb?.Arrange(new Rect(offset, pieceSize));
            }
            else
            {
                offset = offset.WithX(lowerThumbLength * 0.5);
                pieceSize = pieceSize.WithWidth(lowerButtonLength);
                LowerSection?.Arrange(new Rect(offset, pieceSize));
                offset = offset.WithX(offset.X + lowerButtonLength);
                pieceSize = pieceSize.WithWidth(innerButtonLength);
                InnerSection?.Arrange(new Rect(offset, pieceSize));
                offset = offset.WithX(offset.X + innerButtonLength);
                pieceSize = pieceSize.WithWidth(upperButtonLength);
                UpperSection?.Arrange(new Rect(offset, pieceSize));

                offset = offset.WithX(lowerButtonLength);
                pieceSize = pieceSize.WithWidth(lowerThumbLength);
                LowerThumb?.Arrange(new Rect(offset, pieceSize));

                offset = offset.WithX(lowerButtonLength + innerButtonLength);
                pieceSize = pieceSize.WithWidth(upperThumbLength);
                UpperThumb?.Arrange(new Rect(offset, pieceSize));
                
            }
        }
        return finalSize;
    }
    
    private void ComputeSliderLengths(
        Size arrangeSize, 
        bool isVertical, 
        out double lowerButtonLength, 
        out double innerButtonLength, 
        out double upperButtonLength, 
        out double lowerThumbLength, 
        out double upperThumbLength)
    {
        double range = Math.Max(0, Maximum - Minimum);
        range += double.Epsilon;
        double lowerOffset = Math.Min(range, LowerValue - Minimum);
        double upperOffset = Math.Min(range, UpperValue - Minimum);

        double trackLength;
        if (isVertical)
        {
            trackLength = arrangeSize.Height;
            lowerThumbLength = LowerThumb?.DesiredSize.Height ?? 0;
            upperThumbLength = UpperThumb?.DesiredSize.Height ?? 0;
        }
        else
        {
            trackLength = arrangeSize.Width;
            lowerThumbLength = LowerThumb?.DesiredSize.Width ?? 0;
            upperThumbLength = UpperThumb?.DesiredSize.Width ?? 0;
        }
        
        CoerceLength(ref lowerThumbLength, trackLength);
        CoerceLength(ref upperThumbLength, trackLength);

        double remainingLength = trackLength - lowerThumbLength * 0.5 - upperThumbLength * 0.5;
        
        lowerButtonLength = remainingLength * lowerOffset / range;
        upperButtonLength = remainingLength * (range-upperOffset) / range;
        innerButtonLength = remainingLength - lowerButtonLength - upperButtonLength;
    }

    private static void CoerceLength(ref double componentLength, double trackLength)
    {
        if (componentLength < 0)
        {
            componentLength = 0.0;
        }
        else if (componentLength > trackLength || double.IsNaN(componentLength))
        {
            componentLength = trackLength;
        }
    }

    private static bool ValidateDouble(double value)
    {
        return !double.IsInfinity(value) && !double.IsNaN(value);
    }

    internal double GetRatioByPoint(double position)
    {
        bool isHorizontal = Orientation == Orientation.Horizontal;
        var range = isHorizontal? 
            LowerSection?.Bounds.Width + InnerSection?.Bounds.Width + UpperSection?.Bounds.Width ?? double.Epsilon
            : LowerSection?.Bounds.Height + InnerSection?.Bounds.Height + UpperSection?.Bounds.Height ?? double.Epsilon;
        if (isHorizontal)
        {
            if (IsDirectionReversed)
            {
                double trackStart = UpperThumb?.Bounds.Width/2 ?? 0;
                double trackEnd = trackStart + range;
                if (position < trackStart) return 1.0;
                if (position > trackEnd) return 0.0;
                double diff = trackEnd - position;
                return diff / range;
            }
            else
            {
                double trackStart = LowerThumb?.Bounds.Width/2 ?? 0;
                double trackEnd = trackStart + range;
                if (position < trackStart) return 0.0;
                if (position > trackEnd) return 1.0;
                double diff = position - trackStart;
                return diff / range;
            }
        }
        else
        {
            if (IsDirectionReversed)
            {
                double trackStart = LowerThumb?.Bounds.Height / 2 ?? 0;
                double trackEnd = trackStart + range;
                if (position < trackStart) return 0.0;
                if (position > trackEnd) return 1.0;
                double diff = position - trackStart;
                return diff / range;
            }
            else
            {
                double trackStart = UpperThumb?.Bounds.Height / 2 ?? 0;
                double trackEnd = trackStart + range;
                if (position < trackStart) return 1.0;
                if (position > trackEnd) return 0.0;
                double diff = trackEnd - position;
                return diff / range;
            }
        }
    }
}