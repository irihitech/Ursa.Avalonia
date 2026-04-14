using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;

namespace Ursa.Controls;

public class DateTimePicker : DateTimePickerBase<DateTime>
{
    public static readonly StyledProperty<string?> PlaceholderTextProperty =
        TextBox.PlaceholderTextProperty.AddOwner<DateTimePicker>();

    public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty =
        TextBox.PlaceholderForegroundProperty.AddOwner<DateTimePicker>();

    [Obsolete("Use PlaceholderTextProperty instead.")]
    public static readonly StyledProperty<string?> WatermarkProperty = PlaceholderTextProperty;

    public static readonly StyledProperty<string> PanelFormatProperty =
        AvaloniaProperty.Register<DateTimePicker, string>(nameof(PanelFormat), "HH mm ss");

    public static readonly StyledProperty<bool> NeedConfirmationProperty =
        AvaloniaProperty.Register<DateTimePicker, bool>(nameof(NeedConfirmation));

    static DateTimePicker()
    {
        DisplayFormatProperty.OverrideDefaultValue<DateTimePicker>(
            CultureInfo.InvariantCulture.DateTimeFormat.FullDateTimePattern);
    }

    public string? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public IBrush? PlaceholderForeground
    {
        get => GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
    }

    [Obsolete("Use PlaceholderText instead.")]
    public string? Watermark
    {
        get => PlaceholderText;
        set => PlaceholderText = value;
    }

    public string PanelFormat
    {
        get => GetValue(PanelFormatProperty);
        set => SetValue(PanelFormatProperty, value);
    }

    public bool NeedConfirmation
    {
        get => GetValue(NeedConfirmationProperty);
        set => SetValue(NeedConfirmationProperty, value);
    }

    protected override DateOnly? ToDateOnly(DateTime value) => value.ToDateOnly();

    protected override TimeOnly? ToTimeOnly(DateTime value) => value.ToTimeOnly();

    protected override DateTime CombineDateTime(DateOnly date, TimeOnly time) => date.ToDateTime(time);

    protected override DateTime? Parse(string text, string format) =>
        DateTime.TryParseExact(text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var d) ? d : null;

    protected override string Format(DateTime value, string format) => value.ToString(format);
}

