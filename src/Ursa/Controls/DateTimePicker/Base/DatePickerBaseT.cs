using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

public abstract class DatePickerBase<T> : DatePickerBase where T : struct
{
    public static readonly StyledProperty<T?> SelectedDateProperty =
        AvaloniaProperty.Register<DatePickerBase<T>, T?>(
            nameof(SelectedDate), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    public T? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    /// <summary>Converts a <typeparamref name="T"/> value to <see cref="DateOnly"/> for calendar operations.</summary>
    protected abstract DateOnly? ToDateOnly(T? value);

    /// <summary>Creates a <typeparamref name="T"/> value from a calendar-selected <see cref="DateOnly"/>.</summary>
    protected abstract T FromDateOnly(DateOnly date);

    /// <summary>Parses a text string with the given format into a <typeparamref name="T"/> value, or <see langword="null"/> on failure.</summary>
    protected abstract T? Parse(string? text, string? format);

    /// <summary>Formats a <typeparamref name="T"/> value to a display string using the given format.</summary>
    protected abstract string? Format(T? value, string? format);

    /// <summary>Returns today as a <see cref="DateOnly"/> for use as the calendar context when no date is selected.</summary>
    protected virtual DateOnly GetToday() => DateOnly.FromDateTime(DateTime.Today);

    protected override DateOnly? GetSelectedDateOnly() => ToDateOnly(SelectedDate);

    protected override DateOnly GetCalendarContextDate() =>
        GetSelectedDateOnly() ?? GetToday();

    protected override void SyncToUI()
    {
        if (SelectedDate is null)
        {
            _textBox?.SetValue(TextBox.TextProperty, null);
            _calendar?.ClearSelection();
        }
        else
        {
            _textBox?.SetValue(TextBox.TextProperty, Format(SelectedDate, DisplayFormat ?? DEFAULT_DATE_DISPLAY_FORMAT));
            var dateOnly = ToDateOnly(SelectedDate);
            if (dateOnly.HasValue)
                _calendar?.MarkDates(startDate: dateOnly.Value, endDate: dateOnly.Value);
        }
    }

    protected override void CommitInput()
    {
        if (string.IsNullOrWhiteSpace(_textBox?.Text))
        {
            SetCurrentValue(SelectedDateProperty, (T?)null);
            _calendar?.ClearSelection();
            return;
        }

        var format = DisplayFormat ?? DEFAULT_DATE_DISPLAY_FORMAT;
        var parsed = Parse(_textBox?.Text, format);
        if (parsed.HasValue)
        {
            SetCurrentValue(SelectedDateProperty, parsed);
            var dateOnly = ToDateOnly(parsed);
            if (dateOnly.HasValue && _calendar is not null)
            {
                _calendar.ContextDate = _calendar.ContextDate.With(year: dateOnly.Value.Year, month: dateOnly.Value.Month);
                _calendar.UpdateDayButtons();
                _calendar.MarkDates(startDate: dateOnly.Value, endDate: dateOnly.Value);
            }
        }
        else
        {
            SetCurrentValue(SelectedDateProperty, (T?)null);
            _calendar?.ClearSelection();
        }
    }

    protected override void OnCalendarDateSelected(DateOnly? date)
    {
        SetCurrentValue(SelectedDateProperty, date.HasValue ? (T?)FromDateOnly(date.Value) : null);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == SelectedDateProperty)
            SyncToUI();
    }

    public override void Clear() => SetCurrentValue(SelectedDateProperty, (T?)null);
}
