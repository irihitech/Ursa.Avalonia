using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_Popup, typeof(Popup))]
[TemplatePart(PART_TextBox, typeof(TextBox))]
[TemplatePart(PART_Calendar, typeof(CalendarView))]
[TemplatePart(PART_TimePicker, typeof(TimePickerPresenter))]
[PseudoClasses(PseudoClassName.PC_Empty)]
public class DateTimePicker : DatePickerBase, IClearControl
{
    public const string PART_Popup = "PART_Popup";
    public const string PART_TextBox = "PART_TextBox";
    public const string PART_Calendar = "PART_Calendar";
    public const string PART_TimePicker = "PART_TimePicker";

    public static readonly StyledProperty<DateTime?> SelectedDateProperty =
        AvaloniaProperty.Register<DateTimePicker, DateTime?>(
            nameof(SelectedDate), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<string?> PlaceholderTextProperty =
        AvaloniaProperty.Register<DateTimePicker, string?>(nameof(PlaceholderText));

    public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty =
        AvaloniaProperty.Register<DateTimePicker, IBrush?>(nameof(PlaceholderForeground));

    [Obsolete("Use PlaceholderTextProperty instead.")]
    public static readonly StyledProperty<string?> WatermarkProperty = PlaceholderTextProperty;

    public static readonly StyledProperty<string> PanelFormatProperty =
        AvaloniaProperty.Register<DateTimePicker, string>(
            nameof(PanelFormat), "HH mm ss");

    public static readonly StyledProperty<bool> NeedConfirmationProperty =
        AvaloniaProperty.Register<DateTimePicker, bool>(
            nameof(NeedConfirmation));

    public DateTime? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public string? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public IBrush? PlaceholderForeground
    {
        get => GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
    }

    [Obsolete("Use PlaceholderText instead.")]
    public string? Watermark
    {
        get => PlaceholderText;
        set => PlaceholderText = value;
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

    private CalendarView? _calendar;
    private TextBox? _textBox;
    private Popup? _popup;
    private TimePickerPresenter? _timePickerPresenter;

    static DateTimePicker()
    {
        FocusableProperty.OverrideDefaultValue<DateTimePicker>(true);
        DisplayFormatProperty.OverrideDefaultValue<DateTimePicker>(CultureInfo.InvariantCulture.DateTimeFormat
            .FullDateTimePattern);
        SelectedDateProperty.Changed.AddClassHandler<DateTimePicker, DateTime?>((o, e) => o.OnSelectionChanged(e));
    }

    private void OnSelectionChanged(AvaloniaPropertyChangedEventArgs<DateTime?> args)
    {
        SyncDateToText();
        PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedDate is null);
    }

    private void SyncDateToText()
    {
        var date = SelectedDate;
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

        TimePickerPresenter.SelectedTimeChangedEvent.RemoveHandler(OnTimeSelected, _timePickerPresenter);
        CalendarView.DateSelectedEvent.RemoveHandler(OnDateSelected, _calendar);
        PointerPressedEvent.RemoveHandler(OnTextBoxPressed, _textBox);

        _popup = e.NameScope.Find<Popup>(PART_Popup);
        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        _calendar = e.NameScope.Find<CalendarView>(PART_Calendar);
        _timePickerPresenter = e.NameScope.Find<TimePickerPresenter>(PART_TimePicker);

        TimePickerPresenter.SelectedTimeChangedEvent.AddHandler(OnTimeSelected, _timePickerPresenter);
        CalendarView.DateSelectedEvent.AddHandler(OnDateSelected, _calendar);
        PointerPressedEvent.AddHandler(OnTextBoxPressed, RoutingStrategies.Tunnel, true, _textBox);
    }

    private void OnTextBoxPressed(object? sender, PointerPressedEventArgs e)
    {
        InitializePopupOpen(sender as TextBox);
    }

    private void CommitInput()
    {
        var format = this.DisplayFormat ?? DEFAULT_DATETIME_DISPLAY_FORMAT;
        if (string.IsNullOrWhiteSpace(_textBox?.Text))
        {
            SetCurrentValue(SelectedDateProperty, null);
        }

        if (DateTime.TryParseExact(_textBox?.Text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None,
                out var date))
        {
            SetCurrentValue(SelectedDateProperty, date);
        }
        else if (DisplayFormat is null || DisplayFormat.Length == 0)
        {
            if (DateTime.TryParse(_textBox?.Text, out var defaultTime))
            {
                SetCurrentValue(SelectedDateProperty, defaultTime);
            }
        }
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

    private void OnTimeSelected(object? sender, TimeChangedEventArgs e)
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

    private void InitializePopupOpen(TextBox? sender)
    {
        if (sender is null) return;
        SetCurrentValue(IsDropdownOpenProperty, true);
        SetCalendarContextDate();
        _calendar?.MarkDates(SelectedDate?.Date, SelectedDate?.Date);
        var time = SelectedDate?.TimeOfDay;
        _timePickerPresenter?.SyncTime(time);
    }

    private void SetCalendarContextDate()
    {
        var startDate = SelectedDate ?? DateTime.Today;
        _calendar?.SyncContextDate(new CalendarContext(startDate.Year, startDate.Month));
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (!e.Handled && e.Source is Visual source)
        {
            if (_popup?.IsInsidePopup(source) == true)
            {
                e.Handled = true;
            }
            else
            {
                InitializePopupOpen(_textBox);
            }
        }
    }

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        base.OnLostFocus(e);
        var newItem = e.NewFocusedElement;
        if (Equals(newItem, _textBox))
        {
            return;
        }
        else if (newItem is Visual visual)
        {
            var insidePopup = _popup?.IsInsidePopup(visual);
            if (insidePopup == true)
            {
                return;
            }
            CommitInput();
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
        
    }

    public void Clear()
    {
        SetCurrentValue(SelectedDateProperty, null);
    }
}