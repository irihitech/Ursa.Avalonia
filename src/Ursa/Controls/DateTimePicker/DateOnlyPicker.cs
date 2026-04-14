using System.Globalization;

namespace Ursa.Controls;

public class DateOnlyPicker : DatePickerBase<DateOnly>
{
    protected override Type StyleKeyOverride { get; } = typeof(DatePickerBase);

    protected override DateOnly? ToDateOnly(DateOnly? value) => value;

    protected override DateOnly FromDateOnly(DateOnly date) => date;

    protected override DateOnly? Parse(string text, string format) =>
        DateOnly.TryParseExact(text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var date)
            ? date
            : null;

    protected override string Format(DateOnly value, string format) => value.ToString(format);
}
