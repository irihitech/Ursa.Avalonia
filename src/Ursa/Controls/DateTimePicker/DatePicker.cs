using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;

namespace Ursa.Controls;

public class DatePicker : DatePickerBase<DateTime>
{
    public static readonly StyledProperty<string?> PlaceholderTextProperty =
        TextBox.PlaceholderTextProperty.AddOwner<DatePicker>();

    public string? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty =
        TextBox.PlaceholderForegroundProperty.AddOwner<DatePicker>();

    public IBrush? PlaceholderForeground
    {
        get => GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
    }

    [Obsolete("Use PlaceholderTextProperty instead.")]
    public static readonly StyledProperty<string?> WatermarkProperty = PlaceholderTextProperty;

    [Obsolete("Use PlaceholderText instead.")]
    public string? Watermark
    {
        get => PlaceholderText;
        set => PlaceholderText = value;
    }

    protected override DateOnly? ToDateOnly(DateTime? value) => value?.ToDateOnly();

    protected override DateTime FromDateOnly(DateOnly date) => date.ToDateTime(TimeOnly.MinValue);

    protected override DateTime? Parse(string text, string format) =>
        DateTime.TryParseExact(text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var date)
            ? date
            : null;

    protected override string Format(DateTime value, string format) => value.ToString(format);
}

