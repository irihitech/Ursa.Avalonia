using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public abstract class DateTimePickerBase<T> : DateTimePickerBase where T : struct
{
    public static readonly StyledProperty<T?> SelectedDateProperty =
        AvaloniaProperty.Register<DateTimePickerBase<T>, T?>(
            nameof(SelectedDate), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    public T? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    protected TextBox? _textBox;
    protected DatePickerCalendarView? _calendar;
    protected Popup? _popup;
    protected TimePickerPresenter? _timePickerPresenter;

    /// <summary>Extracts the date component from a <typeparamref name="T"/> value.</summary>
    protected abstract DateOnly? ToDateOnly(T? value);

    /// <summary>Extracts the time component from a <typeparamref name="T"/> value.</summary>
    protected abstract TimeOnly? ToTimeOnly(T value);

    /// <summary>Creates a <typeparamref name="T"/> value from a date and time.</summary>
    protected abstract T CombineDateTime(DateOnly date, TimeOnly time);

    /// <summary>Parses a text string with the given format into a <typeparamref name="T"/> value, or <see langword="null"/> on failure.</summary>
    protected abstract T? Parse(string? text, string? format);

    /// <summary>Formats a <typeparamref name="T"/> value to a display string using the given format.</summary>
    protected abstract string? Format(T? value, string? format);

    /// <summary>Returns today as a <see cref="DateOnly"/> for calendar context fallback.</summary>
    protected virtual DateOnly GetToday() => DateOnly.FromDateTime(DateTime.Today);

    /// <summary>Returns the current time for use when no time component is available.</summary>
    protected virtual TimeOnly GetCurrentTime() => TimeOnly.FromDateTime(DateTime.Now);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        TimePickerPresenter.SelectedTimeChangedEvent.RemoveHandler(OnTimeSelected, _timePickerPresenter);
        DatePickerCalendarView.DateSelectedEvent.RemoveHandler(OnDateSelected, _calendar);
        GotFocusEvent.RemoveHandler(OnTextBoxGotFocus, _textBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPressed, _textBox);
        LostFocusEvent.RemoveHandler(OnTextBoxLostFocus, _textBox);

        _popup = e.NameScope.Find<Popup>(PART_Popup);
        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        _calendar = e.NameScope.Find<DatePickerCalendarView>(PART_Calendar);
        _timePickerPresenter = e.NameScope.Find<TimePickerPresenter>(PART_TimePicker);

        TimePickerPresenter.SelectedTimeChangedEvent.AddHandler(OnTimeSelected, _timePickerPresenter);
        DatePickerCalendarView.DateSelectedEvent.AddHandler(OnDateSelected, _calendar);
        GotFocusEvent.AddHandler(OnTextBoxGotFocus, _textBox);
        PointerPressedEvent.AddHandler(OnTextBoxPressed, RoutingStrategies.Tunnel, true, _textBox);
        LostFocusEvent.AddHandler(OnTextBoxLostFocus, _textBox);

        SyncDateToText();
        PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedDate is null);
    }

    private void OnTextBoxLostFocus(object? sender, FocusChangedEventArgs e) => CommitInput();
    private void OnTextBoxGotFocus(object? sender, FocusChangedEventArgs e) => InitializePopupOpen();
    private void OnTextBoxPressed(object? sender, PointerPressedEventArgs e) => InitializePopupOpen();

    private void InitializePopupOpen()
    {
        SetCurrentValue(IsDropdownOpenProperty, true);
        var date = GetCalendarContextDate();
        _calendar?.SyncContextDate(new DatePickerCalendarContext(date.Year, date.Month));
        _timePickerPresenter?.SyncTime(GetSelectedTimeOnly());
    }

    private void OnDateSelected(object? sender, DatePickerCalendarDayButtonEventArgs e)
    {
        if (e.Date is null) return;
        var time = SelectedDate.HasValue
            ? ToTimeOnly(SelectedDate.Value) ?? GetCurrentTime()
            : GetCurrentTime();
        SetCurrentValue(SelectedDateProperty, (T?)CombineDateTime(e.Date.Value, time));
    }

    private void OnTimeSelected(object? sender, TimeChangedEventArgs e)
    {
        if (e.NewTime is null) return;
        var date = GetSelectedDateOnly() ?? GetToday();
        SetCurrentValue(SelectedDateProperty, (T?)CombineDateTime(date, e.NewTime.Value));
    }

    private DateOnly? GetSelectedDateOnly() => ToDateOnly(SelectedDate);

    private TimeOnly? GetSelectedTimeOnly() =>
        SelectedDate.HasValue ? ToTimeOnly(SelectedDate.Value) : null;

    private DateOnly GetCalendarContextDate() => GetSelectedDateOnly() ?? GetToday();

    private void SyncDateToText()
    {
        if (SelectedDate is null)
        {
            _textBox?.SetValue(TextBox.TextProperty, null);
            _calendar?.ClearSelection();
            _timePickerPresenter?.SyncTime(null);
        }
        else
        {
            _textBox?.SetValue(TextBox.TextProperty, Format(SelectedDate, DisplayFormat ?? DEFAULT_DATETIME_DISPLAY_FORMAT));
            var dateOnly = ToDateOnly(SelectedDate);
            if (dateOnly.HasValue)
                _calendar?.MarkDates(dateOnly.Value, dateOnly.Value);
            _timePickerPresenter?.SyncTime(ToTimeOnly(SelectedDate.Value));
        }
    }

    private void CommitInput()
    {
        var format = DisplayFormat ?? DEFAULT_DATETIME_DISPLAY_FORMAT;
        if (string.IsNullOrWhiteSpace(_textBox?.Text))
        {
            SetCurrentValue(SelectedDateProperty, (T?)null);
            return;
        }
        var parsed = Parse(_textBox?.Text, format);
        SetCurrentValue(SelectedDateProperty, parsed);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (!e.Handled && e.Source is Visual source)
        {
            if (_popup?.IsInsidePopup(source) == true)
                e.Handled = true;
            else
                InitializePopupOpen();
        }
    }

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        base.OnLostFocus(e);
        var newItem = e.NewFocusedElement;
        if (Equals(newItem, _textBox)) return;
        if (newItem is Visual visual)
        {
            if (_popup?.IsInsidePopup(visual) == true) return;
            CommitInput();
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == SelectedDateProperty)
        {
            SyncDateToText();
            PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedDate is null);
        }
    }

    public override void Clear() => SetCurrentValue(SelectedDateProperty, (T?)null);
}
