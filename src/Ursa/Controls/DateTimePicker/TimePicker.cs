using System.Globalization;

namespace Ursa.Controls;

public class TimePicker : TimePickerBase<TimeSpan>
{
    protected override Type StyleKeyOverride { get; } = typeof(TimePickerBase);

    protected override TimeOnly? ToTimeOnly(TimeSpan? value) => value.HasValue ? TimeOnly.FromTimeSpan(value.Value) : null;

    protected override TimeSpan FromTimeOnly(TimeOnly time) => time.ToTimeSpan();

    protected override TimeSpan? Parse(string? text, string? format) =>
        TimeOnly.TryParseExact(text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var time)
            ? time.ToTimeSpan()
            : null;

    protected override string? Format(TimeSpan? value, string? format) =>
        value.HasValue ? TimeOnly.FromTimeSpan(value.Value).ToString(format) : null;
}
