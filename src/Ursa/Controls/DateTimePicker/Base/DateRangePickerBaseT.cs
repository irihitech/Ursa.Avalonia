using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

public abstract class DateRangePickerBase<T> : DateRangePickerBase where T : struct
{
    public static readonly StyledProperty<T?> SelectedStartDateProperty =
        AvaloniaProperty.Register<DateRangePickerBase<T>, T?>(
            nameof(SelectedStartDate), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    public static readonly StyledProperty<T?> SelectedEndDateProperty =
        AvaloniaProperty.Register<DateRangePickerBase<T>, T?>(
            nameof(SelectedEndDate), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    public T? SelectedStartDate
    {
        get => GetValue(SelectedStartDateProperty);
        set => SetValue(SelectedStartDateProperty, value);
    }

    public T? SelectedEndDate
    {
        get => GetValue(SelectedEndDateProperty);
        set => SetValue(SelectedEndDateProperty, value);
    }

    /// <summary>Converts a <typeparamref name="T"/> value to <see cref="DateOnly"/> for calendar operations.</summary>
    protected abstract DateOnly? ToDateOnly(T? value);

    /// <summary>Creates a <typeparamref name="T"/> value from a <see cref="DateOnly"/> (at day start).</summary>
    protected abstract T FromDateOnly(DateOnly date);

    /// <summary>Parses a text string with the given format into a <typeparamref name="T"/> value, or <see langword="null"/> on failure.</summary>
    protected abstract T? Parse(string text, string format);

    /// <summary>Formats a <typeparamref name="T"/> value to a display string using the given format.</summary>
    protected abstract string Format(T value, string format);

    protected override DateOnly? GetStartDateOnly() => ToDateOnly(SelectedStartDate);

    protected override DateOnly? GetEndDateOnly() => ToDateOnly(SelectedEndDate);

    protected override void SetSelectedStartDate(DateOnly? date) =>
        SetCurrentValue(SelectedStartDateProperty, date.HasValue ? (T?)FromDateOnly(date.Value) : null);

    protected override void SetSelectedEndDate(DateOnly? date) =>
        SetCurrentValue(SelectedEndDateProperty, date.HasValue ? (T?)FromDateOnly(date.Value) : null);

    protected override void SyncToUI()
    {
        _startTextBox?.SetCurrentValue(TextBox.TextProperty,
            SelectedStartDate.HasValue
                ? Format(SelectedStartDate.Value, DisplayFormat ?? DEFAULT_DATE_DISPLAY_FORMAT)
                : string.Empty);
        _endTextBox?.SetCurrentValue(TextBox.TextProperty,
            SelectedEndDate.HasValue
                ? Format(SelectedEndDate.Value, DisplayFormat ?? DEFAULT_DATE_DISPLAY_FORMAT)
                : string.Empty);
    }

    protected override void CommitInput()
    {
        var format = DisplayFormat ?? DEFAULT_DATE_DISPLAY_FORMAT;

        if (string.IsNullOrWhiteSpace(_startTextBox?.Text))
            SetCurrentValue(SelectedStartDateProperty, (T?)null);
        else
            SetCurrentValue(SelectedStartDateProperty, Parse(_startTextBox.Text, format));

        if (string.IsNullOrWhiteSpace(_endTextBox?.Text))
            SetCurrentValue(SelectedEndDateProperty, (T?)null);
        else
            SetCurrentValue(SelectedEndDateProperty, Parse(_endTextBox.Text, format));

        _startCalendar?.MarkDates(GetStartDateOnly(), GetEndDateOnly());
        _endCalendar?.MarkDates(GetStartDateOnly(), GetEndDateOnly());
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == SelectedStartDateProperty || change.Property == SelectedEndDateProperty)
        {
            SyncToUI();
            _startCalendar?.MarkDates(GetStartDateOnly(), GetEndDateOnly());
            _endCalendar?.MarkDates(GetStartDateOnly(), GetEndDateOnly());
            PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedStartDate is null && SelectedEndDate is null);
        }
    }

    public override void Clear()
    {
        SetCurrentValue(SelectedStartDateProperty, (T?)null);
        SetCurrentValue(SelectedEndDateProperty, (T?)null);
        ResetStatus();
    }
}
