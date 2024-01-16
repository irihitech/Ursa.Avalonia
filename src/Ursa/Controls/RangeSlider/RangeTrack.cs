using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;

namespace Ursa.Controls.RangeSlider;

/// <summary>
/// Notice that this is not used in ScrollBar, so ViewportSize related feature is not necessary. 
/// </summary>
public class RangeTrack: Control
{
    public static readonly StyledProperty<double> MinimumProperty = AvaloniaProperty.Register<RangeTrack, double>(
        nameof(Minimum));

    public double Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public static readonly StyledProperty<double> MaximumProperty = AvaloniaProperty.Register<RangeTrack, double>(
        nameof(Maximum));

    public double Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }
    
    public static readonly StyledProperty<double> LowerValueProperty = AvaloniaProperty.Register<RangeTrack, double>(
        nameof(LowerValue));
    
    public double LowerValue
    {
        get => GetValue(LowerValueProperty);
        set => SetValue(LowerValueProperty, value);
    }
    
    public static readonly StyledProperty<double> UpperValueProperty = AvaloniaProperty.Register<RangeTrack, double>(
        nameof(UpperValue));
    
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

    public static readonly StyledProperty<RepeatButton> UpperButtonProperty = AvaloniaProperty.Register<RangeTrack, RepeatButton>(
        nameof(UpperButton));

    public RepeatButton UpperButton
    {
        get => GetValue(UpperButtonProperty);
        set => SetValue(UpperButtonProperty, value);
    }

    public static readonly StyledProperty<Button?> LowerButtonProperty = AvaloniaProperty.Register<RangeTrack, Button?>(
        nameof(LowerButton));

    public Button? LowerButton
    {
        get => GetValue(LowerButtonProperty);
        set => SetValue(LowerButtonProperty, value);
    }

    public static readonly StyledProperty<Button?> InnerButtonProperty = AvaloniaProperty.Register<RangeTrack, Button?>(
        nameof(InnerButton));

    public Button? InnerButton
    {
        get => GetValue(InnerButtonProperty);
        set => SetValue(InnerButtonProperty, value);
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
    
    static RangeTrack()
    {
        AffectsArrange<RangeTrack>(MinimumProperty, MaximumProperty, LowerValueProperty, UpperValueProperty, OrientationProperty, IsDirectionReversedProperty);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var desiredSize = new Size(0.0, 0.0);
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
}