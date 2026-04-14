using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_Popup, typeof(Popup))]
[TemplatePart(PART_TextBox, typeof(TextBox))]
[TemplatePart(PART_Calendar, typeof(DatePickerCalendarView))]
[TemplatePart(PART_TimePicker, typeof(TimePickerPresenter))]
[PseudoClasses(PseudoClassName.PC_Empty)]
public abstract class DateTimePickerBase<T> : DateTimePickerBase, IClearControl where T : struct
{
    public const string PART_Popup = "PART_Popup";
    public const string PART_TextBox = "PART_TextBox";
    public const string PART_Calendar = "PART_Calendar";
    public const string PART_TimePicker = "PART_TimePicker";

    private TextBox? _textBox;
    private DatePickerCalendarView? _calendar;
    private Popup? _popup;
    private TimePickerPresenter? _timePickerPresenter;

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

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == SelectedDateProperty)
        {
            SyncDateToText();
            PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedDate is null);
        }
    }

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
    }

    private void OnTextBoxLostFocus(object? sender, FocusChangedEventArgs e) => CommitInput();

    private void OnTextBoxGotFocus(object? sender, FocusChangedEventArgs e) => InitializePopupOpen();

    private void OnTextBoxPressed(object? sender, PointerPressedEventArgs e) => InitializePopupOpen();

    private void InitializePopupOpen()
    {
        SetCurrentValue(IsDropdownOpenProperty, true);
        SetCalendarContextDate();
    }

    private void SetCalendarContextDate()
    {
        var dateOnly = SelectedDate.HasValue ? ToDateOnly(SelectedDate.Value) : null;
        var date = dateOnly ?? GetToday();
        _calendar?.SyncContextDate(new DatePickerCalendarContext(date.Year, date.Month));
        var time = SelectedDate.HasValue ? ToTimeOnly(SelectedDate.Value) : null;
        _timePickerPresenter?.SyncTime(time);
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
        var date = (SelectedDate.HasValue ? ToDateOnly(SelectedDate.Value) : null) ?? GetToday();
        SetCurrentValue(SelectedDateProperty, (T?)CombineDateTime(date, e.NewTime.Value));
    }

    private void SyncDateToText()
    {
        var value = SelectedDate;
        if (value is null)
        {
            _textBox?.SetValue(TextBox.TextProperty, null);
            _calendar?.ClearSelection();
            _timePickerPresenter?.SyncTime(null);
        }
        else
        {
            _textBox?.SetValue(TextBox.TextProperty, Format(value.Value, DisplayFormat ?? DEFAULT_DATETIME_DISPLAY_FORMAT));
            var dateOnly = ToDateOnly(value.Value);
            if (dateOnly.HasValue)
                _calendar?.MarkDates(dateOnly.Value, dateOnly.Value);
            _timePickerPresenter?.SyncTime(ToTimeOnly(value.Value));
        }
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

    private void CommitInput()
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

    public void Clear() => SetCurrentValue(SelectedDateProperty, (T?)null);

    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        base.UpdateDataValidation(property, state, error);
        if (property == SelectedDateProperty) DataValidationErrors.SetError(this, error);
    }
}
