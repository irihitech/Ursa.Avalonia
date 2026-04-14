using System.Globalization;

namespace Ursa.Controls;

public class TimeOnlyRangePicker : TimeRangePickerBase<TimeOnly>
{
    protected override Type StyleKeyOverride { get; } = typeof(TimeRangePickerBase);

    protected override TimeOnly? ToTimeOnly(TimeOnly? value) => value;

    protected override TimeOnly FromTimeOnly(TimeOnly time) => time;

    protected override TimeOnly? Parse(string? text, string? format) =>
        TimeOnly.TryParseExact(text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var time)
            ? time
            : null;

    protected override string? Format(TimeOnly? value, string? format) => value?.ToString(format);
}
