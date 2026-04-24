using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace Ursa.Controls;

[TemplatePart(PART_OffsetComboBox, typeof(ComboBox))]
public class DateOffsetRangePicker : DateRangePickerBase<DateTimeOffset>
{
    public const string PART_OffsetComboBox = "PART_OffsetComboBox";

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

    private ComboBox? _offsetComboBox;

    public DateOffsetRangePicker()
    {
        SetCurrentValue(OffsetDefinitionsProperty, [OffsetDefinition.Local]);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == OffsetDefinitionsProperty)
        {
            var definitions = change.GetNewValue<OffsetDefinitions?>();
            if (definitions is not null && (SelectedOffset is null || !definitions.Contains(SelectedOffset)))
                SetCurrentValue(SelectedOffsetProperty, definitions.FirstOrDefault());
        }
        else if (change.Property == SelectedOffsetProperty)
        {
            var newOffset = GetCurrentOffset();
            if (SelectedStartDate.HasValue)
                SetCurrentValue(SelectedStartDateProperty,
                    new DateTimeOffset(SelectedStartDate.Value.DateTime, newOffset));
            if (SelectedEndDate.HasValue)
                SetCurrentValue(SelectedEndDateProperty,
                    new DateTimeOffset(SelectedEndDate.Value.DateTime, newOffset));
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _offsetComboBox = e.NameScope.Find<ComboBox>(PART_OffsetComboBox);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        if (_offsetComboBox is not null && e.Source is Visual source &&
            (ReferenceEquals(source, _offsetComboBox) || _offsetComboBox.IsVisualAncestorOf(source)))
            return;
        base.OnPointerPressed(e);
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
