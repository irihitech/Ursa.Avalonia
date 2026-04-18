using System.Globalization;
using Avalonia;
using Avalonia.Data;

namespace Ursa.Controls;

public class TimeRangePicker : TimeRangePickerBase<TimeSpan>
{
    protected override Type StyleKeyOverride { get; } = typeof(TimeRangePickerBase);

    [Obsolete("Use SelectedStartTimeProperty instead.")]
    public static readonly StyledProperty<TimeSpan?> StartTimeProperty = SelectedStartTimeProperty;

    [Obsolete("Use SelectedEndTimeProperty instead.")]
    public static readonly StyledProperty<TimeSpan?> EndTimeProperty = SelectedEndTimeProperty;

    [Obsolete("Use SelectedStartTime instead.")]
    public TimeSpan? StartTime
    {
        get => SelectedStartTime;
        set => SelectedStartTime = value;
    }

    [Obsolete("Use SelectedEndTime instead.")]
    public TimeSpan? EndTime
    {
        get => SelectedEndTime;
        set => SelectedEndTime = value;
    }

    protected override TimeOnly? ToTimeOnly(TimeSpan? value) => value.HasValue ? TimeOnly.FromTimeSpan(value.Value) : null;

    protected override TimeSpan FromTimeOnly(TimeOnly time) => time.ToTimeSpan();

    protected override TimeSpan? Parse(string? text, string? format) =>
        TimeOnly.TryParseExact(text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var time)
            ? time.ToTimeSpan()
            : null;

    protected override string? Format(TimeSpan? value, string? format) =>
        value.HasValue ? TimeOnly.FromTimeSpan(value.Value).ToString(format) : null;
    
    /// <summary>
    /// Note: This need to be kept as is to make sure XAML binding to base class won't fail. 
    /// </summary>
    public override void Clear()
    {
        base.Clear();
    }
}
