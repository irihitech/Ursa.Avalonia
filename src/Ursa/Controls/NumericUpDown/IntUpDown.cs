using System.Globalization;
using Avalonia.Controls;
using Avalonia.Utilities;

namespace Ursa.Controls;

public class IntUpDown : NumericUpDownBase<int>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static IntUpDown()
    {
        MaximumProperty.OverrideDefaultValue<IntUpDown>(int.MaxValue);
        MinimumProperty.OverrideDefaultValue<IntUpDown>(int.MinValue);
        StepProperty.OverrideDefaultValue<IntUpDown>(1);
    }

    protected override bool ParseText(string? text, out int? number)
    {
        var result = int.TryParse(text, ParsingNumberStyle, NumberFormat, out var value);
        number = value;
        return result;
    }

    protected override string? ValueToString(int? value) => value?.ToString(FormatString, NumberFormat);

    protected override int Zero => 0;

    protected override int? Add(int? a, int? b) => a + b;

    protected override int? Minus(int? a, int? b) => a - b;
}