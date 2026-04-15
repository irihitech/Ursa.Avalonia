using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;

namespace Ursa.Controls;

public class DatePicker : DatePickerBase<DateTime>
{
    protected override Type StyleKeyOverride { get; } =  typeof(DatePickerBase);

    protected override DateOnly? ToDateOnly(DateTime? value) => value?.ToDateOnly();

    protected override DateTime FromDateOnly(DateOnly date) => date.ToDateTime(TimeOnly.MinValue);

    protected override DateTime? Parse(string? text, string? format) =>
        DateTime.TryParseExact(text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var date)
            ? date
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

