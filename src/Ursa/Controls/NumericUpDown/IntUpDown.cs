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

public class DoubleUpDown : NumericUpDownBase<double>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static DoubleUpDown()
    {
        MaximumProperty.OverrideDefaultValue<DoubleUpDown>(double.MaxValue);
        MinimumProperty.OverrideDefaultValue<DoubleUpDown>(double.MinValue);
        StepProperty.OverrideDefaultValue<DoubleUpDown>(1);
    }

    protected override bool ParseText(string? text, out double? number)
    {
        // Weird bug
        var result = double.TryParse(text, out var value);
        number = value;
        return result;
    }

    protected override string? ValueToString(double? value) => value?.ToString(FormatString, NumberFormat);

    protected override double Zero => 0;

    protected override double? Add(double? a, double? b) => a + b;

    protected override double? Minus(double? a, double? b) => a - b;
}

public class ByteUpDown : NumericUpDownBase<byte>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static ByteUpDown()
    {
        MaximumProperty.OverrideDefaultValue<ByteUpDown>(byte.MaxValue);
        MinimumProperty.OverrideDefaultValue<ByteUpDown>(byte.MinValue);
        StepProperty.OverrideDefaultValue<ByteUpDown>(1);
    }

    protected override bool ParseText(string? text, out byte? number)
    {
        var result = byte.TryParse(text, ParsingNumberStyle, NumberFormat, out var value);
        number = value;
        return result;
    }

    protected override string? ValueToString(byte? value) => value?.ToString(FormatString, NumberFormat);

    protected override byte Zero => 0;

    protected override byte? Add(byte? a, byte? b) => (byte?) (a + b);

    protected override byte? Minus(byte? a, byte? b) => (byte?) (a - b);
}