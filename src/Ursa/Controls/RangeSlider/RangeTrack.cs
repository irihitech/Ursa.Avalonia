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
    private double _density;
    
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

    public static readonly StyledProperty<Button?> UpperButtonProperty = AvaloniaProperty.Register<RangeTrack, Button?>(
        nameof(UpperButton));

    public Button? UpperButton
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
        OrientationProperty.Changed.AddClassHandler<RangeTrack, Orientation>((o, e) => o.OnOrientationChanged(e));
        AffectsArrange<RangeTrack>(MinimumProperty, MaximumProperty, LowerValueProperty, UpperValueProperty, OrientationProperty, IsDirectionReversedProperty);
    }

    private void OnOrientationChanged(AvaloniaPropertyChangedEventArgs<Orientation> args)
    {
        Orientation o = args.NewValue.Value;
        this.PseudoClasses.Set("", true);
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
        double lowerButtonLength, innerButtonLength, upperButtonLength, lowerThumbLength, upperThumbLength;
        ComputeSliderLengths(finalSize, vertical, out lowerButtonLength, out innerButtonLength, out upperButtonLength,
            out lowerThumbLength, out upperThumbLength);
        
        return base.ArrangeOverride(finalSize);
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
        
        double min = Minimum;
        double max = Maximum;
        double all = Math.Max(0, max - min);
        double lowerOffset = Math.Min(all, LowerValue - min);
        double upperOffset = Math.Min(all, UpperValue - min);

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
        
        double remainingLength = trackLength -lowerThumbLength - upperThumbLength;
        
        lowerButtonLength = remainingLength * lowerOffset / all;
        upperButtonLength = remainingLength * upperOffset / all;
        innerButtonLength = remainingLength - lowerButtonLength - upperButtonLength;

        _density = all / remainingLength;
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
}