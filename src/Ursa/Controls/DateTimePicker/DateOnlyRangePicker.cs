using System.Globalization;

namespace Ursa.Controls;

public class DateOnlyRangePicker : DateRangePickerBase<DateOnly>
{
    protected override Type StyleKeyOverride { get; } = typeof(DateRangePickerBase);

    protected override DateOnly? ToDateOnly(DateOnly? value) => value;

    protected override DateOnly FromDateOnly(DateOnly date) => date;

    protected override DateOnly? Parse(string? text, string? format) =>
        DateOnly.TryParseExact(text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var date)
            ? date
            : null;

    protected override string? Format(DateOnly? value, string? format) => value?.ToString(format);
    
    /// <summary>
    /// Note: This need to be kept as is to make sure XAML binding to base class won't fail. 
    /// </summary>
    public override void Clear()
    {
        base.Clear();
    }
}
