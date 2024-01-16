using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls.RangeSlider;

[TemplatePart(PART_DecreaseButton, typeof(Button))]
[TemplatePart(PART_IncreaseButton, typeof(Button))]
[TemplatePart(PART_Track, typeof(Track))]
public class RangeSlider: TemplatedControl
{
    public const string PART_DecreaseButton = "PART_DecreaseButton";
    public const string PART_IncreaseButton = "PART_IncreaseButton";
    public const string PART_Track          = "PART_Track";
}