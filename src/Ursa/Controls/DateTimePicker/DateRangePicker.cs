using System.Globalization;

namespace Ursa.Controls;

public class DateRangePicker : DateRangePickerBase<DateTime>
{
    protected override DateOnly? ToDateOnly(DateTime? value) => value?.ToDateOnly();

    protected override DateTime FromDateOnly(DateOnly date) => date.ToDateTime(TimeOnly.MinValue);

    protected override DateTime? Parse(string text, string format) =>
        DateTime.TryParseExact(text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var d) ? d : null;

    protected override string Format(DateTime value, string format) => value.ToString(format);
}
