using System.Globalization;

namespace Ursa.Controls;

public class TimeOnlyPicker : TimePickerBase<TimeOnly>
{
    protected override Type StyleKeyOverride { get; } = typeof(TimePickerBase);

    protected override TimeOnly? ToTimeOnly(TimeOnly? value) => value;

    protected override TimeOnly FromTimeOnly(TimeOnly time) => time;

    protected override TimeOnly? Parse(string? text, string? format) =>
        TimeOnly.TryParseExact(text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var time)
            ? time
            : null;

    protected override string? Format(TimeOnly? value, string? format) => value?.ToString(format);
    
    /// <summary>
    /// Note: This need to be kept as is to make sure XAML binding to base class won't fail. 
    /// </summary>
    public override void Clear()
    {
        base.Clear();
    }
}
