using System.Globalization;
using Avalonia;
using Avalonia.Data;

namespace Ursa.Controls;

public class DateTimeOffsetPicker : DateTimePickerBase<DateTimeOffset>
{
    public static readonly StyledProperty<OffsetDefinitions?> OffsetDefinitionsProperty =
        AvaloniaProperty.Register<DateTimeOffsetPicker, OffsetDefinitions?>(nameof(OffsetDefinitions));

    public static readonly StyledProperty<OffsetDefinition?> SelectedOffsetProperty =
        AvaloniaProperty.Register<DateTimeOffsetPicker, OffsetDefinition?>(
            nameof(SelectedOffset), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> ShowOffsetSelectionProperty =
        AvaloniaProperty.Register<DateTimeOffsetPicker, bool>(nameof(ShowOffsetSelection), defaultValue: true);

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

    protected override Type StyleKeyOverride { get; } = typeof(DateTimePickerBase);

    static DateTimeOffsetPicker()
    {
        DisplayFormatProperty.OverrideDefaultValue<DateTimeOffsetPicker>(
            CultureInfo.InvariantCulture.DateTimeFormat.FullDateTimePattern);
    }

    public DateTimeOffsetPicker()
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

    protected override TimeOnly? ToTimeOnly(DateTimeOffset value) =>
        TimeOnly.FromTimeSpan(value.TimeOfDay);

    protected override DateTimeOffset CombineDateTime(DateOnly date, TimeOnly time) =>
        new DateTimeOffset(date.ToDateTime(time), GetCurrentOffset());

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
