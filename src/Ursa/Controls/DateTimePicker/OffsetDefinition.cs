using System.ComponentModel;
using System.Globalization;
using Avalonia.Collections;

namespace Ursa.Controls;

public enum OffsetDefinitionKind
{
    Utc,
    Local,
    Fixed
}

/// <summary>
/// Defines a UTC offset for the DateOffset pickers. Accepts "UTC", "Local", or an offset string
/// like "+08:00" or "-05:30". Similar in spirit to <c>ColumnDefinition</c>.
/// </summary>
[TypeConverter(typeof(OffsetDefinitionConverter))]
public class OffsetDefinition
{
    public OffsetDefinitionKind Kind { get; }

    /// <summary>Only meaningful when <see cref="Kind"/> is <see cref="OffsetDefinitionKind.Fixed"/>.</summary>
    public TimeSpan Offset { get; set; }

    public OffsetDefinition() { }

    public OffsetDefinition(OffsetDefinitionKind kind, TimeSpan offset = default)
    {
        Kind = kind;
        Offset = offset;
    }

    public static readonly OffsetDefinition Utc = new(OffsetDefinitionKind.Utc);
    public static readonly OffsetDefinition Local = new(OffsetDefinitionKind.Local);

    public static OffsetDefinition Fixed(TimeSpan offset) => new(OffsetDefinitionKind.Fixed, offset);

    /// <summary>Resolves this definition to a concrete <see cref="TimeSpan"/> offset value.</summary>
    public TimeSpan Resolve() => Kind switch
    {
        OffsetDefinitionKind.Utc => TimeSpan.Zero,
        OffsetDefinitionKind.Local => TimeZoneInfo.Local.BaseUtcOffset,
        OffsetDefinitionKind.Fixed => Offset,
        _ => TimeSpan.Zero
    };

    /// <summary>
    /// Parses a string into an <see cref="OffsetDefinition"/>.
    /// Accepts "UTC", "Local", or an offset like "+08:00", "-05:30", "8:30".
    /// </summary>
    public static OffsetDefinition Parse(string text)
    {
        var trimmed = text.Trim();

        if (string.Equals(trimmed, "UTC", StringComparison.OrdinalIgnoreCase))
            return new OffsetDefinition(OffsetDefinitionKind.Utc);

        if (string.Equals(trimmed, "Local", StringComparison.OrdinalIgnoreCase))
            return new OffsetDefinition(OffsetDefinitionKind.Local);

        bool negative = trimmed.StartsWith('-');
        var stripped = trimmed.TrimStart('+').TrimStart('-');

        if (TimeSpan.TryParseExact(stripped, [@"hh\:mm", @"h\:mm"], null, out var ts))
            return new OffsetDefinition(OffsetDefinitionKind.Fixed, negative ? -ts : ts);

        if (int.TryParse(stripped, out var hours) && hours >= 0 && hours <= 14)
            return new OffsetDefinition(OffsetDefinitionKind.Fixed, TimeSpan.FromHours(negative ? -hours : hours));

        throw new FormatException(
            $"Cannot parse '{text}' as OffsetDefinition. Use 'UTC', 'Local', or an offset like '+08:00'.");
    }

    public override string ToString() => Kind switch
    {
        OffsetDefinitionKind.Utc => "UTC",
        OffsetDefinitionKind.Local => $"Local ({FormatOffset(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now))})",
        OffsetDefinitionKind.Fixed => FormatOffset(Offset),
        _ => "UTC"
    };

    private static string FormatOffset(TimeSpan offset)
    {
        var sign = offset < TimeSpan.Zero ? "-" : "+";
        var abs = offset.Duration();
        return $"{sign}{abs.Hours:D2}:{abs.Minutes:D2}";
    }
}

/// <summary>
/// An observable collection of <see cref="OffsetDefinition"/> items. Supports being set from a
/// comma-separated string like <c>"UTC, Local, +08:00"</c>.
/// </summary>
[TypeConverter(typeof(OffsetDefinitionsConverter))]
public class OffsetDefinitions : AvaloniaList<OffsetDefinition>
{
    public OffsetDefinitions() { }

    public OffsetDefinitions(IEnumerable<OffsetDefinition> items) : base(items) { }
}

public class OffsetDefinitionConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) =>
        value is string s ? OffsetDefinition.Parse(s) : base.ConvertFrom(context, culture, value);
}

public class OffsetDefinitionsConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string s)
            return new OffsetDefinitions(s.Split(',').Select(item => OffsetDefinition.Parse(item)));
        return base.ConvertFrom(context, culture, value);
    }
}
