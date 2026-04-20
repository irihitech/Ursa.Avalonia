using System.Globalization;
using Avalonia;
using Avalonia.Data;

namespace Ursa.Controls;

public class DateOffsetRangePicker : DateRangePickerBase<DateTimeOffset>
{
    public static readonly StyledProperty<OffsetDefinitions?> OffsetDefinitionsProperty =
        AvaloniaProperty.Register<DateOffsetRangePicker, OffsetDefinitions?>(nameof(OffsetDefinitions));

    public static readonly StyledProperty<OffsetDefinition?> SelectedOffsetProperty =
        AvaloniaProperty.Register<DateOffsetRangePicker, OffsetDefinition?>(
            nameof(SelectedOffset), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> ShowOffsetSelectionProperty =
        AvaloniaProperty.Register<DateOffsetRangePicker, bool>(nameof(ShowOffsetSelection), defaultValue: true);

    public OffsetDefinitions? OffsetDefinitions
    {
        get => GetValue(OffsetDefinitionsProperty);
        set => SetValue(OffsetDefinitionsProperty, value);
    }

    public OffsetDefinition? SelectedOffset
    {
        get => GetValue(SelectedOffsetProperty);
        set => SetValue(SelectedOffsetProperty, value);
    }

    public bool ShowOffsetSelection
    {
        get => GetValue(ShowOffsetSelectionProperty);
        set => SetValue(ShowOffsetSelectionProperty, value);
    }

    public DateOffsetRangePicker()
    {
        SetCurrentValue(OffsetDefinitionsProperty, [OffsetDefinition.Local]);
    }



    private TimeSpan GetCurrentOffset()
    {
        var definition = ShowOffsetSelection
            ? (SelectedOffset ?? OffsetDefinitions?.FirstOrDefault() ?? OffsetDefinition.Local)
            : (OffsetDefinitions?.FirstOrDefault() ?? OffsetDefinition.Local);
        return definition.Resolve();
    }

    protected override DateOnly? ToDateOnly(DateTimeOffset? value) =>
        value.HasValue ? DateOnly.FromDateTime(value.Value.DateTime) : null;

    protected override DateTimeOffset FromDateOnly(DateOnly date) =>
        new DateTimeOffset(date.ToDateTime(TimeOnly.MinValue), GetCurrentOffset());

    protected override DateTimeOffset? Parse(string? text, string? format) =>
        DateTimeOffset.TryParseExact(text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var d)
            ? d
            : null;

    protected override string? Format(DateTimeOffset? value, string? format) => value?.ToString(format);

    /// <summary>
    /// Note: This need to be kept as is to make sure XAML binding to base class won't fail.
    /// </summary>
    public override void Clear()
    {
        base.Clear();
    }
}
