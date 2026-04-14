using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

public abstract class DateTimePickerBase<T> : DateTimePickerBase where T : struct
{
    public static readonly StyledProperty<T?> SelectedDateProperty =
        AvaloniaProperty.Register<DateTimePickerBase<T>, T?>(
            nameof(SelectedDate), defaultBindingMode: BindingMode.TwoWay);

    public T? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    /// <summary>Extracts the date component from a <typeparamref name="T"/> value.</summary>
    protected abstract DateOnly? ToDateOnly(T value);

    /// <summary>Extracts the time component from a <typeparamref name="T"/> value.</summary>
    protected abstract TimeOnly? ToTimeOnly(T value);

    /// <summary>Creates a <typeparamref name="T"/> value from a date and time.</summary>
    protected abstract T CombineDateTime(DateOnly date, TimeOnly time);

    /// <summary>Parses a text string with the given format into a <typeparamref name="T"/> value, or <see langword="null"/> on failure.</summary>
    protected abstract T? Parse(string text, string format);

    /// <summary>Formats a <typeparamref name="T"/> value to a display string using the given format.</summary>
    protected abstract string Format(T value, string format);

    /// <summary>Returns today as a <see cref="DateOnly"/> for calendar context fallback.</summary>
    protected virtual DateOnly GetToday() => DateOnly.FromDateTime(DateTime.Today);

    /// <summary>Returns the current time for use when no time component is available.</summary>
    protected virtual TimeOnly GetCurrentTime() => TimeOnly.FromDateTime(DateTime.Now);

    protected override DateOnly? GetSelectedDateOnly() =>
        SelectedDate.HasValue ? ToDateOnly(SelectedDate.Value) : null;

    protected override TimeOnly? GetSelectedTimeOnly() =>
        SelectedDate.HasValue ? ToTimeOnly(SelectedDate.Value) : null;

    protected override DateOnly GetCalendarContextDate() =>
        GetSelectedDateOnly() ?? GetToday();

    protected override void SyncToUI()
    {
        if (SelectedDate is null)
        {
            _textBox?.SetValue(TextBox.TextProperty, null);
            _calendar?.ClearSelection();
            _timePickerPresenter?.SyncTime(null);
        }
        else
        {
            _textBox?.SetValue(TextBox.TextProperty, Format(SelectedDate.Value, DisplayFormat ?? DEFAULT_DATETIME_DISPLAY_FORMAT));
            var dateOnly = ToDateOnly(SelectedDate.Value);
            if (dateOnly.HasValue)
                _calendar?.MarkDates(dateOnly.Value, dateOnly.Value);
            _timePickerPresenter?.SyncTime(ToTimeOnly(SelectedDate.Value));
        }
    }

    protected override void CommitInput()
    {
        var format = DisplayFormat ?? DEFAULT_DATETIME_DISPLAY_FORMAT;
        if (string.IsNullOrWhiteSpace(_textBox?.Text))
        {
            SetCurrentValue(SelectedDateProperty, (T?)null);
            return;
        }
        var parsed = Parse(_textBox!.Text, format);
        SetCurrentValue(SelectedDateProperty, parsed);
    }

    protected override void OnCalendarDateSelected(DateOnly date)
    {
        var time = SelectedDate.HasValue
            ? ToTimeOnly(SelectedDate.Value) ?? GetCurrentTime()
            : GetCurrentTime();
        SetCurrentValue(SelectedDateProperty, (T?)CombineDateTime(date, time));
    }

    protected override void OnTimePickerTimeSelected(TimeOnly time)
    {
        var date = GetSelectedDateOnly() ?? GetToday();
        SetCurrentValue(SelectedDateProperty, (T?)CombineDateTime(date, time));
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == SelectedDateProperty)
        {
            SyncToUI();
            PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedDate is null);
        }
    }

    public override void Clear() => SetCurrentValue(SelectedDateProperty, (T?)null);

    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        base.UpdateDataValidation(property, state, error);
        if (property == SelectedDateProperty) DataValidationErrors.SetError(this, error);
    }
}
