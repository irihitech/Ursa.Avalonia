using System.Globalization;
using Avalonia;

namespace Ursa.Controls;

public class DateTimePicker : DateTimePickerBase<DateTime>
{
    public static readonly StyledProperty<DateTimeKind> DefaultDateKindProperty =
        AvaloniaProperty.Register<DateTimePicker, DateTimeKind>(nameof(DefaultDateKind), DateTimeKind.Unspecified);

    public DateTimeKind DefaultDateKind
    {
        get => GetValue(DefaultDateKindProperty);
        set => SetValue(DefaultDateKindProperty, value);
    }

    protected override Type StyleKeyOverride { get; } = typeof(DateTimePickerBase);

    static DateTimePicker()
    {
        DisplayFormatProperty.OverrideDefaultValue<DateTimePicker>(
            CultureInfo.InvariantCulture.DateTimeFormat.FullDateTimePattern);
    }

    protected override DateOnly? ToDateOnly(DateTime? value) => value?.ToDateOnly();

    protected override TimeOnly? ToTimeOnly(DateTime value) => value.ToTimeOnly();

    protected override DateTime CombineDateTime(DateOnly date, TimeOnly time) =>
        DateTime.SpecifyKind(date.ToDateTime(time), DefaultDateKind);

    protected override DateTime? Parse(string? text, string? format) =>
        DateTime.TryParseExact(text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var d)
            ? DateTime.SpecifyKind(d, DefaultDateKind)
            : null;

    protected override string? Format(DateTime? value, string? format) => value?.ToString(format);
    
    /// <summary>
    /// Note: This need to be kept as is to make sure XAML binding to base class won't fail. 
    /// </summary>
    public override void Clear()
    {
        base.Clear();
    }
}

