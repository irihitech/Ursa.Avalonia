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
[TemplatePart(PART_Calendar, typeof(DatePickerCalendarView))]
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

    private DatePickerCalendarView? _calendar;
    private TextBox? _textBox;
    private Popup? _popup;
    private TimePickerPresenter? _timePickerPresenter;

    static DateTimePicker()
    {
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
            var selectedDate = date.ToDateOnly();
            _calendar?.MarkDates(selectedDate, selectedDate);
            _timePickerPresenter?.SyncTime(date.Value.ToTimeOnly());
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

    private void OnTextBoxLostFocus(object? sender, FocusChangedEventArgs e)
    {
        CommitInput();
    }

    private void OnTextBoxGotFocus(object? sender, FocusChangedEventArgs e)
    {
        InitializePopupOpen();
    }

    private void OnTextBoxPressed(object? sender, PointerPressedEventArgs e)
    {
        InitializePopupOpen();
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
        else
        {
            SetCurrentValue(SelectedDateProperty, null);
        }
    }

    private void OnDateSelected(object? sender, DatePickerCalendarDayButtonEventArgs e)
    {
        if (SelectedDate is null)
        {
            if (e.Date is null) return;
            var date = e.Date.Value;
            var time = DateTime.Now.ToTimeOnly();
            SetCurrentValue(SelectedDateProperty, date.ToDateTime(time));
        }
        else
        {
            var selectedDate = SelectedDate;
            if (e.Date is null) return;
            var date = e.Date.Value;
            SetCurrentValue(SelectedDateProperty, date.ToDateTime(selectedDate.Value.ToTimeOnly()));
        }
    }

    private void OnTimeSelected(object? sender, TimeChangedEventArgs e)
    {
        if (SelectedDate is null)
        {
            if (e.NewTime is null) return;
            var time = e.NewTime.Value;
            SetCurrentValue(SelectedDateProperty, DateTime.Today.ToDateOnly().ToDateTime(time));
        }
        else
        {
            var selectedDate = SelectedDate;
            if (e.NewTime is null) return;
            var time = e.NewTime.Value;
            SetCurrentValue(SelectedDateProperty, selectedDate.Value.ToDateOnly().ToDateTime(time));
        }
    }

    private void InitializePopupOpen()
    {
        SetCurrentValue(IsDropdownOpenProperty, true);
        SetCalendarContextDate();
    }

    private void SetCalendarContextDate()
    {
        var startDate = SelectedDate ?? DateTime.Today;
        _calendar?.SyncContextDate(new DatePickerCalendarContext(startDate.Year, startDate.Month));
        var time = SelectedDate?.ToTimeOnly();
        _timePickerPresenter?.SyncTime(time);
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
                InitializePopupOpen();
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
        if (newItem is Visual visual)
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

    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        base.UpdateDataValidation(property, state, error);
        if (property == SelectedDateProperty) DataValidationErrors.SetError(this, error);
    }
}
