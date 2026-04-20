using System.ComponentModel;
using System.Globalization;
using Avalonia.Collections;

namespace Ursa.Controls;

public enum OffsetDefinitionKind { Utc, Local, Fixed }

/// <summary>
/// Represents a UTC offset specification: UTC, Local machine timezone, or a fixed offset.
/// Parsable from strings like "UTC", "Local", "+08:00", "-05:30", "8".
/// Similar in spirit to <see cref="Avalonia.Controls.GridLength"/>.
/// </summary>
[TypeConverter(typeof(OffsetValueConverter))]
public readonly struct OffsetValue : IEquatable<OffsetValue>
{
    private readonly OffsetDefinitionKind _kind;
    private readonly TimeSpan _fixedOffset;

    private OffsetValue(OffsetDefinitionKind kind, TimeSpan fixedOffset)
    {
        _kind = kind;
        _fixedOffset = fixedOffset;
    }

    public bool IsUtc => _kind == OffsetDefinitionKind.Utc;
    public bool IsLocal => _kind == OffsetDefinitionKind.Local;
    public bool IsFixed => _kind == OffsetDefinitionKind.Fixed;

    /// <summary>The fixed offset value. Only meaningful when <see cref="IsFixed"/> is true.</summary>
    public TimeSpan FixedOffset => _fixedOffset;

    public static readonly OffsetValue Utc = new(OffsetDefinitionKind.Utc, TimeSpan.Zero);
    public static readonly OffsetValue Local = new(OffsetDefinitionKind.Local, TimeSpan.Zero);
    public static OffsetValue Fixed(TimeSpan offset) => new(OffsetDefinitionKind.Fixed, offset);

    /// <summary>Resolves to a concrete <see cref="TimeSpan"/> at call time.</summary>
    public TimeSpan Resolve() => _kind switch
    {
        OffsetDefinitionKind.Utc => TimeSpan.Zero,
        OffsetDefinitionKind.Local => TimeZoneInfo.Local.GetUtcOffset(DateTime.Now),
        OffsetDefinitionKind.Fixed => _fixedOffset,
        _ => TimeSpan.Zero
    };

    /// <summary>
    /// Parses a string into an <see cref="OffsetValue"/>.
    /// Accepts "UTC", "Local", or an offset like "+08:00", "-05:30", "8".
    /// </summary>
    public static OffsetValue Parse(string text)
    {
        var trimmed = text.Trim();

        if (string.Equals(trimmed, "UTC", StringComparison.OrdinalIgnoreCase))
            return Utc;

        if (string.Equals(trimmed, "Local", StringComparison.OrdinalIgnoreCase))
            return Local;

        bool negative = trimmed.StartsWith('-');
        var stripped = trimmed.TrimStart('+').TrimStart('-');

        if (TimeSpan.TryParseExact(stripped, [@"hh\:mm", @"h\:mm"], null, out var ts))
            return Fixed(negative ? -ts : ts);

        if (int.TryParse(stripped, out var hours) && hours is >= 0 and <= 14)
            return Fixed(TimeSpan.FromHours(negative ? -hours : hours));

        throw new FormatException(
            $"Cannot parse '{text}' as OffsetValue. Use 'UTC', 'Local', or an offset like '+08:00'.");
    }

    public override string ToString() => _kind switch
    {
        OffsetDefinitionKind.Utc => "UTC",
        OffsetDefinitionKind.Local => $"Local ({FormatOffset(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now))})",
        OffsetDefinitionKind.Fixed => FormatOffset(_fixedOffset),
        _ => "UTC"
    };

    private static string FormatOffset(TimeSpan offset)
    {
        var sign = offset < TimeSpan.Zero ? "-" : "+";
        var abs = offset.Duration();
        return $"{sign}{abs.Hours:D2}:{abs.Minutes:D2}";
    }

    public bool Equals(OffsetValue other) => _kind == other._kind && _fixedOffset == other._fixedOffset;
    public override bool Equals(object? obj) => obj is OffsetValue v && Equals(v);
    public override int GetHashCode() => HashCode.Combine(_kind, _fixedOffset);
    public static bool operator ==(OffsetValue left, OffsetValue right) => left.Equals(right);
    public static bool operator !=(OffsetValue left, OffsetValue right) => !left.Equals(right);
}

/// <summary>
/// Defines a UTC offset entry for DateOffset pickers. Similar in spirit to <c>ColumnDefinition</c>.
/// Set <see cref="Offset"/> as a XAML attribute: <c>Offset="UTC"</c>, <c>Offset="Local"</c>, <c>Offset="+08:00"</c>.
/// </summary>
[TypeConverter(typeof(OffsetDefinitionConverter))]
public class OffsetDefinition : IEquatable<OffsetDefinition>
{
    public OffsetValue Offset { get; set; }

    /// <summary>
    /// Optional display name shown in the UI. When <see langword="null"/>, falls back to
    /// the formatted offset string (e.g. "UTC", "Local (+08:00)", "+05:30").
    /// </summary>
    public string? DisplayName { get; set; }

    public OffsetDefinition() { }

    private OffsetDefinition(OffsetValue offset) => Offset = offset;

    public static readonly OffsetDefinition Utc = new(OffsetValue.Utc);
    public static readonly OffsetDefinition Local = new(OffsetValue.Local);
    public static OffsetDefinition Fixed(TimeSpan offset) => new(OffsetValue.Fixed(offset));

    public TimeSpan Resolve() => Offset.Resolve();

    /// <summary>
    /// Parses a string into an <see cref="OffsetDefinition"/>.
    /// Accepts the same formats as <see cref="OffsetValue.Parse"/>: "UTC", "Local", "+08:00", "8", etc.
    /// </summary>
    public static OffsetDefinition Parse(string s) => new() { Offset = OffsetValue.Parse(s.Trim()) };

    public override string ToString() => DisplayName ?? Offset.ToString();

    public bool Equals(OffsetDefinition? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Offset == other.Offset;
    }

    public override bool Equals(object? obj) => Equals(obj as OffsetDefinition);

    public override int GetHashCode() => Offset.GetHashCode();

    public static bool operator ==(OffsetDefinition? left, OffsetDefinition? right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(OffsetDefinition? left, OffsetDefinition? right) => !(left == right);
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

    /// <summary>
    /// Parses a comma-separated string into an <see cref="OffsetDefinitions"/> collection.
    /// Each entry is parsed by <see cref="OffsetDefinition.Parse"/>: e.g. "UTC, Local, +08:00".
    /// </summary>
    public static OffsetDefinitions Parse(string s) =>
        new(s.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(OffsetDefinition.Parse));
}

public class OffsetValueConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) =>
        value is string s ? OffsetValue.Parse(s) : base.ConvertFrom(context, culture, value);
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
            return OffsetDefinitions.Parse(s);
        return base.ConvertFrom(context, culture, value);
    }
}
