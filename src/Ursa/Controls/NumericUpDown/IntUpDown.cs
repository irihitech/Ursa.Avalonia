namespace Ursa.Controls;

public class NumericIntUpDown : NumericUpDownBase<int>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static NumericIntUpDown()
    {
        MaximumProperty.OverrideDefaultValue<NumericIntUpDown>(int.MaxValue);
        MinimumProperty.OverrideDefaultValue<NumericIntUpDown>(int.MinValue);
        StepProperty.OverrideDefaultValue<NumericIntUpDown>(1);
    }

    protected override bool ParseText(string? text, out int number) =>
        int.TryParse(text, ParsingNumberStyle, NumberFormat, out number);

    protected override string? ValueToString(int? value)
    {
        return value?.ToString(FormatString, NumberFormat);
    }

    protected override int Zero => 0;

    protected override int? Add(int? a, int? b)
    {
        var result = a + b;
        return result < Value ? Maximum : result;
    }

    protected override int? Minus(int? a, int? b)
    {
        var result = a - b;
        return result > Value ? Minimum : result;
    }
}

public class NumericUIntUpDown : NumericUpDownBase<uint>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static NumericUIntUpDown()
    {
        MaximumProperty.OverrideDefaultValue<NumericUIntUpDown>(uint.MaxValue);
        MinimumProperty.OverrideDefaultValue<NumericUIntUpDown>(uint.MinValue);
        StepProperty.OverrideDefaultValue<NumericUIntUpDown>(1);
    }

    protected override bool ParseText(string? text, out uint number)
    {
        return uint.TryParse(text, ParsingNumberStyle, NumberFormat, out number);
    }

    protected override string? ValueToString(uint? value)
    {
        return value?.ToString(FormatString, NumberFormat);
    }

    protected override uint Zero => 0;

    protected override uint? Add(uint? a, uint? b)
    {
        var result = a + b;
        return result < Value ? Maximum : result;
    }

    protected override uint? Minus(uint? a, uint? b)
    {
        var result = a - b;
        return result > Value ? Minimum : result;
    }
}

public class NumericDoubleUpDown : NumericUpDownBase<double>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static NumericDoubleUpDown()
    {
        MaximumProperty.OverrideDefaultValue<NumericDoubleUpDown>(double.MaxValue);
        MinimumProperty.OverrideDefaultValue<NumericDoubleUpDown>(double.MinValue);
        StepProperty.OverrideDefaultValue<NumericDoubleUpDown>(1);
    }

    protected override bool ParseText(string? text, out double number) =>
        double.TryParse(text, ParsingNumberStyle, NumberFormat, out number);

    protected override string? ValueToString(double? value) => value?.ToString(FormatString, NumberFormat);

    protected override double Zero => 0;

    protected override double? Add(double? a, double? b) => a + b;

    protected override double? Minus(double? a, double? b) => a - b;
}

public class NumericByteUpDown : NumericUpDownBase<byte>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static NumericByteUpDown()
    {
        MaximumProperty.OverrideDefaultValue<NumericByteUpDown>(byte.MaxValue);
        MinimumProperty.OverrideDefaultValue<NumericByteUpDown>(byte.MinValue);
        StepProperty.OverrideDefaultValue<NumericByteUpDown>(1);
    }

    protected override bool ParseText(string? text, out byte number) =>
        byte.TryParse(text, ParsingNumberStyle, NumberFormat, out number);

    protected override string? ValueToString(byte? value) => value?.ToString(FormatString, NumberFormat);

    protected override byte Zero => 0;

    protected override byte? Add(byte? a, byte? b)
    {
        var result = a + b;
        return (byte?)(result < Value ? Maximum : result);
    }

    protected override byte? Minus(byte? a, byte? b)
    {
        var result = a - b;
        return (byte?)(result > Value ? Minimum : result);
    }
}

public class NumericSByteUpDown : NumericUpDownBase<sbyte>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static NumericSByteUpDown()
    {
        MaximumProperty.OverrideDefaultValue<NumericSByteUpDown>(sbyte.MaxValue);
        MinimumProperty.OverrideDefaultValue<NumericSByteUpDown>(sbyte.MinValue);
        StepProperty.OverrideDefaultValue<NumericSByteUpDown>(1);
    }

    protected override bool ParseText(string? text, out sbyte number) =>
        sbyte.TryParse(text, ParsingNumberStyle, NumberFormat, out number);

    protected override string? ValueToString(sbyte? value) => value?.ToString(FormatString, NumberFormat);

    protected override sbyte Zero => 0;

    protected override sbyte? Add(sbyte? a, sbyte? b)
    {
        var result = a + b;
        return (sbyte?)(result < Value ? Maximum : result);
    }

    protected override sbyte? Minus(sbyte? a, sbyte? b)
    {
        var result = a - b;
        return (sbyte?)(result > Value ? Minimum : result);
    }
}

public class NumericShortUpDown : NumericUpDownBase<short>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static NumericShortUpDown()
    {
        MaximumProperty.OverrideDefaultValue<NumericShortUpDown>(short.MaxValue);
        MinimumProperty.OverrideDefaultValue<NumericShortUpDown>(short.MinValue);
        StepProperty.OverrideDefaultValue<NumericShortUpDown>(1);
    }

    protected override bool ParseText(string? text, out short number) =>
        short.TryParse(text, ParsingNumberStyle, NumberFormat, out number);

    protected override string? ValueToString(short? value) => value?.ToString(FormatString, NumberFormat);

    protected override short Zero => 0;

    protected override short? Add(short? a, short? b)
    {
        var result = a + b;
        return (short?)(result < Value ? Maximum : result);
    }

    protected override short? Minus(short? a, short? b)
    {
        var result = a - b;
        return (short?)(result > Value ? Minimum : result);
    }
}

public class NumericUShortUpDown : NumericUpDownBase<ushort>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static NumericUShortUpDown()
    {
        MaximumProperty.OverrideDefaultValue<NumericUShortUpDown>(ushort.MaxValue);
        MinimumProperty.OverrideDefaultValue<NumericUShortUpDown>(ushort.MinValue);
        StepProperty.OverrideDefaultValue<NumericUShortUpDown>(1);
    }

    protected override bool ParseText(string? text, out ushort number) =>
        ushort.TryParse(text, ParsingNumberStyle, NumberFormat, out number);

    protected override string? ValueToString(ushort? value) => value?.ToString(FormatString, NumberFormat);

    protected override ushort Zero => 0;

    protected override ushort? Add(ushort? a, ushort? b)
    {
        var result = a + b;
        return (ushort?)(result < Value ? Maximum : result);
    }

    protected override ushort? Minus(ushort? a, ushort? b)
    {
        var result = a - b;
        return (ushort?)(result > Value ? Minimum : result);
    }
}

public class NumericLongUpDown : NumericUpDownBase<long>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static NumericLongUpDown()
    {
        MaximumProperty.OverrideDefaultValue<NumericLongUpDown>(long.MaxValue);
        MinimumProperty.OverrideDefaultValue<NumericLongUpDown>(long.MinValue);
        StepProperty.OverrideDefaultValue<NumericLongUpDown>(1);
    }

    protected override bool ParseText(string? text, out long number) =>
        long.TryParse(text, ParsingNumberStyle, NumberFormat, out number);

    protected override string? ValueToString(long? value) => value?.ToString(FormatString, NumberFormat);

    protected override long Zero => 0;

    protected override long? Add(long? a, long? b)
    {
        var result = a + b;
        return result < Value ? Maximum : result;
    }

    protected override long? Minus(long? a, long? b)
    {
        var result = a - b;
        return result > Value ? Minimum : result;
    }
}

public class NumericULongUpDown : NumericUpDownBase<ulong>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static NumericULongUpDown()
    {
        MaximumProperty.OverrideDefaultValue<NumericULongUpDown>(ulong.MaxValue);
        MinimumProperty.OverrideDefaultValue<NumericULongUpDown>(ulong.MinValue);
        StepProperty.OverrideDefaultValue<NumericULongUpDown>(1);
    }

    protected override bool ParseText(string? text, out ulong number) =>
        ulong.TryParse(text, ParsingNumberStyle, NumberFormat, out number);

    protected override string? ValueToString(ulong? value) => value?.ToString(FormatString, NumberFormat);

    protected override ulong Zero => 0;

    protected override ulong? Add(ulong? a, ulong? b) => a + b;

    protected override ulong? Minus(ulong? a, ulong? b) => a - b;
}

public class NumericFloatUpDown : NumericUpDownBase<float>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static NumericFloatUpDown()
    {
        MaximumProperty.OverrideDefaultValue<NumericFloatUpDown>(float.MaxValue);
        MinimumProperty.OverrideDefaultValue<NumericFloatUpDown>(float.MinValue);
        StepProperty.OverrideDefaultValue<NumericFloatUpDown>(1);
    }

    protected override bool ParseText(string? text, out float number) =>
        float.TryParse(text, ParsingNumberStyle, NumberFormat, out number);

    protected override string? ValueToString(float? value) => value?.ToString(FormatString, NumberFormat);

    protected override float Zero => 0;

    protected override float? Add(float? a, float? b) => a + b;

    protected override float? Minus(float? a, float? b) => a - b;
}

public class NumericDecimalUpDown : NumericUpDownBase<decimal>
{
    protected override Type StyleKeyOverride { get; } = typeof(NumericUpDown);

    static NumericDecimalUpDown()
    {
        MaximumProperty.OverrideDefaultValue<NumericDecimalUpDown>(decimal.MaxValue);
        MinimumProperty.OverrideDefaultValue<NumericDecimalUpDown>(decimal.MinValue);
        StepProperty.OverrideDefaultValue<NumericDecimalUpDown>(1);
    }

    protected override bool ParseText(string? text, out decimal number) =>
        decimal.TryParse(text, ParsingNumberStyle, NumberFormat, out number);

    protected override string? ValueToString(decimal? value) => value?.ToString(FormatString, NumberFormat);

    protected override decimal Zero => 0;

    protected override decimal? Add(decimal? a, decimal? b) => a + b;

    protected override decimal? Minus(decimal? a, decimal? b) => a - b;
}
