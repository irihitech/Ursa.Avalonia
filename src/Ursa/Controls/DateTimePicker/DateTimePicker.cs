using System.Globalization;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_Button, typeof(Button))]
[TemplatePart(PART_Popup, typeof(Popup))]
[TemplatePart(PART_TextBox, typeof(TextBox))]
[TemplatePart(PART_Calendar, typeof(CalendarView))]
[TemplatePart(PART_TimePicker, typeof(TimePickerPresenter))]
public class DateTimePicker : DatePickerBase
{
    public const string PART_Button = "PART_Button";
    public const string PART_Popup = "PART_Popup";
    public const string PART_TextBox = "PART_TextBox";
    public const string PART_Calendar = "PART_Calendar";
    public const string PART_TimePicker = "PART_TimePicker";

    public static readonly StyledProperty<DateTime?> SelectedDateProperty =
        AvaloniaProperty.Register<DateTimePicker, DateTime?>(
            nameof(SelectedDate), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<string?> WatermarkProperty =
        AvaloniaProperty.Register<DateTimePicker, string?>(
            nameof(Watermark));

    public static readonly StyledProperty<string> PanelFormatProperty = AvaloniaProperty.Register<TimePicker, string>(
        nameof(PanelFormat), "HH mm ss");

    public static readonly StyledProperty<bool> NeedConfirmationProperty = AvaloniaProperty.Register<TimePicker, bool>(
        nameof(NeedConfirmation));

    private Button? _button;
    private CalendarView? _calendar;
    private TextBox? _textBox;
    private TimePickerPresenter? _timePickerPresenter;

    static DateTimePicker()
    {
        DisplayFormatProperty.OverrideDefaultValue<DateTimePicker>(CultureInfo.InvariantCulture.DateTimeFormat.FullDateTimePattern);
        SelectedDateProperty.Changed.AddClassHandler<DateTimePicker, DateTime?>((picker, args) =>
            picker.OnSelectionChanged(args));
    }

    public DateTime? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public string PanelFormat
    {
        get => GetValue(PanelFormatProperty);
        set => SetValue(PanelFormatProperty, value);
    }

    public bool NeedConfirmation
    {
        get => GetValue(NeedConfirmationProperty);
        set => SetValue(NeedConfirmationProperty, value);
    }

    private void OnSelectionChanged(AvaloniaPropertyChangedEventArgs<DateTime?> args)
    {
        if (_fromText) return;
        SyncSelectedDateToText(args.NewValue.Value);
    }

    private void SyncSelectedDateToText(DateTime? date)
    {
        if (date is null)
        {
            _textBox?.SetValue(TextBox.TextProperty, null);
            _calendar?.ClearSelection();
            _timePickerPresenter?.SyncTime(null);
        }
        else
        {
            _textBox?.SetValue(TextBox.TextProperty,
                date.Value.ToString(DisplayFormat ?? CultureInfo.InvariantCulture.DateTimeFormat.FullDateTimePattern));
            _calendar?.MarkDates(date.Value.Date, date.Value.Date);
            _timePickerPresenter?.SyncTime(date.Value.TimeOfDay);
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        GotFocusEvent.RemoveHandler(OnTextBoxGetFocus, _textBox);
        TextBox.TextChangedEvent.RemoveHandler(OnTextChanged, _textBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPointerPressed, _textBox);
        Button.ClickEvent.RemoveHandler(OnButtonClick, _button);
        CalendarView.DateSelectedEvent.RemoveHandler(OnDateSelected, _calendar);
        TimePickerPresenter.SelectedTimeChangedEvent.RemoveHandler(OnTimeSelectedChanged, _timePickerPresenter);
        _button = e.NameScope.Find<Button>(PART_Button);
        e.NameScope.Find<Popup>(PART_Popup);
        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        _calendar = e.NameScope.Find<CalendarView>(PART_Calendar);
        _timePickerPresenter = e.NameScope.Find<TimePickerPresenter>(PART_TimePicker);
        Button.ClickEvent.AddHandler(OnButtonClick, RoutingStrategies.Bubble, true, _button);
        GotFocusEvent.AddHandler(OnTextBoxGetFocus, _textBox);
        TextBox.TextChangedEvent.AddHandler(OnTextChanged, _textBox);
        PointerPressedEvent.AddHandler(OnTextBoxPointerPressed, RoutingStrategies.Tunnel, false, _textBox);
        CalendarView.DateSelectedEvent.AddHandler(OnDateSelected, RoutingStrategies.Bubble, true, _calendar);
        TimePickerPresenter.SelectedTimeChangedEvent.AddHandler(OnTimeSelectedChanged, _timePickerPresenter);
        SyncSelectedDateToText(SelectedDate);
    }

    private void OnDateSelected(object? sender, CalendarDayButtonEventArgs e)
    {
        if (SelectedDate is null)
        {
            if (e.Date is null) return;
            var date = e.Date.Value;
            var time = DateTime.Now.TimeOfDay;
            SetCurrentValue(SelectedDateProperty,
                new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds));
        }
        else
        {
            var selectedDate = SelectedDate;
            if (e.Date is null) return;
            var date = e.Date.Value;
            SetCurrentValue(SelectedDateProperty,
                new DateTime(date.Year, date.Month, date.Day, selectedDate.Value.Hour, selectedDate.Value.Minute,
                    selectedDate.Value.Second));
        }
    }

    private void OnTimeSelectedChanged(object? sender, TimeChangedEventArgs e)
    {
        if (SelectedDate is null)
        {
            if (e.NewTime is null) return;
            var time = e.NewTime.Value;
            var date = DateTime.Today;
            SetCurrentValue(SelectedDateProperty,
                new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds));
        }
        else
        {
            var selectedDate = SelectedDate;
            if (e.NewTime is null) return;
            var time = e.NewTime.Value;
            SetCurrentValue(SelectedDateProperty,
                new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, selectedDate.Value.Day, time.Hours,
                    time.Minutes,
                    time.Seconds));
        }
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        Focus(NavigationMethod.Pointer);
        SetCurrentValue(IsDropdownOpenProperty, !IsDropdownOpen);
    }

    private void OnTextBoxPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (_calendar is not null)
        {
            var date = SelectedDate ?? DateTime.Now;
            _calendar.ContextDate = new CalendarContext(date.Year, date.Month);
            _calendar.UpdateDayButtons();
            _timePickerPresenter?.SyncTime(SelectedDate?.TimeOfDay);
        }

        SetCurrentValue(IsDropdownOpenProperty, true);
    }

    private bool _fromText = false;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        SetSelectedDate(true);
    }

    private void SetSelectedDate(bool fromText = false)
    {
        var temp = _fromText;
        _fromText = fromText;
        if (string.IsNullOrEmpty(_textBox?.Text))
        {
            SetCurrentValue(SelectedDateProperty, null);
            _calendar?.ClearSelection();
            _timePickerPresenter?.SyncTime(null);
        }
        else if (DisplayFormat is null || DisplayFormat.Length == 0)
        {
            if (DateTime.TryParse(_textBox?.Text, out var defaultTime))
            {
                SetCurrentValue(SelectedDateProperty, defaultTime);
                _calendar?.MarkDates(defaultTime.Date, defaultTime.Date);
                _timePickerPresenter?.SyncTime(defaultTime.TimeOfDay);
            }
        }
        else
        {
            if (DateTime.TryParseExact(_textBox?.Text, DisplayFormat, CultureInfo.CurrentUICulture, DateTimeStyles.None,
                    out var date))
            {
                SetCurrentValue(SelectedDateProperty, date);
                if (_calendar is not null)
                {
                    _calendar.ContextDate = _calendar.ContextDate.With(date.Year, date.Month);
                    _calendar.UpdateDayButtons();
                }

                _calendar?.MarkDates(date.Date, date.Date);
                _timePickerPresenter?.SyncTime(date.TimeOfDay);
            }
            else
            {
                SetCurrentValue(SelectedDateProperty, null);
                if (!fromText) _textBox?.SetValue(TextBox.TextProperty, null);
                _calendar?.ClearSelection();
                _timePickerPresenter?.SyncTime(null);
            }
        }
        _fromText = temp;
    }

    private void OnTextBoxGetFocus(object? sender, GotFocusEventArgs e)
    {
        if (_calendar is not null)
        {
            var date = SelectedDate ?? DateTime.Today;
            _calendar.ContextDate = _calendar.ContextDate.With(date.Year, date.Month);
            _calendar.UpdateDayButtons();
            _timePickerPresenter?.SyncTime(date.TimeOfDay);
        }

        SetCurrentValue(IsDropdownOpenProperty, true);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        // SetCurrentValue(IsDropdownOpenProperty, false);
        SetSelectedDate();
    }


    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            SetCurrentValue(IsDropdownOpenProperty, false);
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Down)
        {
            SetCurrentValue(IsDropdownOpenProperty, true);
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Tab)
        {
            SetCurrentValue(IsDropdownOpenProperty, false);
            return;
        }

        base.OnKeyDown(e);
    }

    public void Clear()
    {
        SetCurrentValue(SelectedDateProperty, null);
    }
}