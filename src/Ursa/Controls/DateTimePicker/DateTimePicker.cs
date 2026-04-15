using System.Globalization;

namespace Ursa.Controls;

public class DateTimePicker : DateTimePickerBase<DateTime>
{
    protected override Type StyleKeyOverride { get; } = typeof(DateTimePickerBase);

    static DateTimePicker()
    {
        DisplayFormatProperty.OverrideDefaultValue<DateTimePicker>(
            CultureInfo.InvariantCulture.DateTimeFormat.FullDateTimePattern);
    }

    protected override DateOnly? ToDateOnly(DateTime? value) => value?.ToDateOnly();

    protected override TimeOnly? ToTimeOnly(DateTime value) => value.ToTimeOnly();

    protected override DateTime CombineDateTime(DateOnly date, TimeOnly time) => date.ToDateTime(time);

    protected override DateTime? Parse(string? text, string? format) =>
        DateTime.TryParseExact(text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var d) ? d : null;

    protected override string? Format(DateTime? value, string? format) => value?.ToString(format);
    
    /// <summary>
    /// Note: This need to be kept as is to make sure XAML binding to base class won't fail. 
    /// </summary>
    public override void Clear()
    {
        base.Clear();
    }
}

