using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

[TemplatePart(PART_DecreaseButton, typeof(Button))]
[TemplatePart(PART_IncreaseButton, typeof(Button))]
[TemplatePart(PART_Track, typeof(Track))]
public class RangeSlider: TemplatedControl
{
    public const string PART_DecreaseButton = "PART_DecreaseButton";
    public const string PART_IncreaseButton = "PART_IncreaseButton";
    public const string PART_Track          = "PART_Track";

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

    static RangeSlider()
    {
        MinimumProperty.OverrideDefaultValue<RangeSlider>(0);
        MaximumProperty.OverrideDefaultValue<RangeSlider>(100);
        LowerValueProperty.OverrideDefaultValue<RangeSlider>(0);
        UpperValueProperty.OverrideDefaultValue<RangeSlider>(100);
    }
    
}