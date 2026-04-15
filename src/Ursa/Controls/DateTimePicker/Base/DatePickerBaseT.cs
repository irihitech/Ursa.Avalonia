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

public abstract class DatePickerBase<T> : DatePickerBase, IClearControl where T : struct
{
    public static readonly StyledProperty<T?> SelectedDateProperty =
        AvaloniaProperty.Register<DatePickerBase<T>, T?>(
            nameof(SelectedDate), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    public T? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    protected TextBox? _textBox;
    protected DatePickerCalendarView? _calendar;
    protected Popup? _popup;

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

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DatePickerCalendarView.DateSelectedEvent.RemoveHandler(OnDateSelected, _calendar);
        GotFocusEvent.RemoveHandler(OnTextBoxGotFocus, _textBox);
        LostFocusEvent.RemoveHandler(OnTextBoxLostFocus, _textBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPressed, _textBox);

        _popup = e.NameScope.Find<Popup>(PART_Popup);
        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        _calendar = e.NameScope.Find<DatePickerCalendarView>(PART_Calendar);

        DatePickerCalendarView.DateSelectedEvent.AddHandler(OnDateSelected, RoutingStrategies.Bubble, true, _calendar);
        GotFocusEvent.AddHandler(OnTextBoxGotFocus, _textBox);
        LostFocusEvent.AddHandler(OnTextBoxLostFocus, _textBox);
        PointerPressedEvent.AddHandler(OnTextBoxPressed, RoutingStrategies.Tunnel, true, _textBox);

        SyncDateToText();
        PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedDate is null);
    }

    private void OnTextBoxPressed(object? sender, PointerPressedEventArgs e) => InitializePopupOpen();
    private void OnTextBoxGotFocus(object? sender, FocusChangedEventArgs e) => InitializePopupOpen();
    private void OnTextBoxLostFocus(object? sender, FocusChangedEventArgs e) => CommitInput();

    private void InitializePopupOpen()
    {
        SetCurrentValue(IsDropdownOpenProperty, true);
        var date = ToDateOnly(SelectedDate) ?? GetToday();
        _calendar?.SyncContextDate(new DatePickerCalendarContext(date.Year, date.Month));
        _calendar?.UpdateDayButtons();
        var selectedDate = ToDateOnly(SelectedDate);
        _calendar?.MarkDates(selectedDate, selectedDate);
    }

    private void OnDateSelected(object? sender, DatePickerCalendarDayButtonEventArgs e)
    {
        SetCurrentValue(SelectedDateProperty, e.Date.HasValue ? (T?)FromDateOnly(e.Date.Value) : null);
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    private void SyncDateToText()
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

    private void CommitInput()
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

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (!e.Handled && e.Source is Visual source)
        {
            if (_popup?.IsInsidePopup(source) == true)
                e.Handled = true;
            else
                _textBox?.Focus();
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape:
                SetCurrentValue(IsDropdownOpenProperty, false);
                e.Handled = true;
                return;
            case Key.Down:
                SetCurrentValue(IsDropdownOpenProperty, true);
                e.Handled = true;
                return;
            case Key.Tab:
                SetCurrentValue(IsDropdownOpenProperty, false);
                return;
            case Key.Enter:
                SetCurrentValue(IsDropdownOpenProperty, false);
                CommitInput();
                e.Handled = true;
                return;
            default:
                base.OnKeyDown(e);
                break;
        }
    }

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        base.OnLostFocus(e);
        var element = e.NewFocusedElement;
        if (Equals(element, _textBox)) return;
        if (element is Visual v && _popup?.IsInsidePopup(v) == true) return;
        CommitInput();
        SetCurrentValue(IsDropdownOpenProperty, false);
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

    public override void Clear() => SetCurrentValue(SelectedDateProperty, null);
}
