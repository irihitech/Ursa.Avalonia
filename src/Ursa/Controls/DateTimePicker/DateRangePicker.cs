using System.Globalization;
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
[TemplatePart(PART_StartCalendar, typeof(CalendarView))]
[TemplatePart(PART_EndCalendar, typeof(CalendarView))]
[TemplatePart(PART_StartTextBox, typeof(TextBox))]
[TemplatePart(PART_EndTextBox, typeof(TextBox))]
[PseudoClasses(PseudoClassName.PC_Empty)]
public class DateRangePicker : DatePickerBase, IClearControl
{
    public const string PART_Popup = "PART_Popup";
    public const string PART_StartCalendar = "PART_StartCalendar";
    public const string PART_EndCalendar = "PART_EndCalendar";
    public const string PART_StartTextBox = "PART_StartTextBox";
    public const string PART_EndTextBox = "PART_EndTextBox";

    public static readonly StyledProperty<DateTime?> SelectedStartDateProperty =
        AvaloniaProperty.Register<DateRangePicker, DateTime?>(
            nameof(SelectedStartDate), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<DateTime?> SelectedEndDateProperty =
        AvaloniaProperty.Register<DateRangePicker, DateTime?>(
            nameof(SelectedEndDate), defaultBindingMode: BindingMode.TwoWay);

    public DateTime? SelectedStartDate
    {
        get => GetValue(SelectedStartDateProperty);
        set => SetValue(SelectedStartDateProperty, value);
    }

    public DateTime? SelectedEndDate
    {
        get => GetValue(SelectedEndDateProperty);
        set => SetValue(SelectedEndDateProperty, value);
    }

    private CalendarView? _startCalendar;
    private CalendarView? _endCalendar;
    private TextBox? _startTextBox;
    private TextBox? _endTextBox;
    private Popup? _popup;

    private readonly DateRangePickerStatus _status = new();

    static DateRangePicker()
    {
        SelectedStartDateProperty.Changed.AddClassHandler<DateRangePicker, DateTime?>((picker, _) =>
            picker.OnSelectionChanged());
        SelectedEndDateProperty.Changed.AddClassHandler<DateRangePicker, DateTime?>((picker, _) =>
            picker.OnSelectionChanged());
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        CalendarView.DateSelectedEvent.RemoveHandler(OnDateSelected, _startCalendar, _endCalendar);
        CalendarView.DatePreviewedEvent.RemoveHandler(OnDatePreviewed, _startCalendar, _endCalendar);
        GotFocusEvent.RemoveHandler(OnTextBoxGotFocus, _startTextBox, _endTextBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPressed, _startTextBox, _endTextBox);
        LostFocusEvent.RemoveHandler(OnTextBoxLostFocus, _startTextBox, _endTextBox);

        _startCalendar = e.NameScope.Find<CalendarView>(PART_StartCalendar);
        _endCalendar = e.NameScope.Find<CalendarView>(PART_EndCalendar);
        _startTextBox = e.NameScope.Find<TextBox>(PART_StartTextBox);
        _endTextBox = e.NameScope.Find<TextBox>(PART_EndTextBox);
        _popup = e.NameScope.Find<Popup>(PART_Popup);

        CalendarView.DateSelectedEvent.AddHandler(OnDateSelected, _startCalendar, _endCalendar);
        CalendarView.DatePreviewedEvent.AddHandler(OnDatePreviewed, _startCalendar, _endCalendar);
        GotFocusEvent.AddHandler(OnTextBoxGotFocus, _startTextBox, _endTextBox);
        PointerPressedEvent.AddHandler(OnTextBoxPressed, strategies: RoutingStrategies.Tunnel, handledEventsToo: true,
            _startTextBox, _endTextBox);
        LostFocusEvent.AddHandler(OnTextBoxLostFocus, _startTextBox, _endTextBox);
        SyncDatesToTexts();
    }

    private void OnTextBoxLostFocus(object? sender, FocusChangedEventArgs e)
    {
        CommitInput();
    }

    private void OnTextBoxPressed(object? sender, PointerPressedEventArgs e)
    {
        InitializePopupOpen(sender as TextBox);
    }

    private void OnDatePreviewed(object? sender, CalendarDayButtonEventArgs e)
    {
        if (_status.Current == Status.Start)
        {
            // This means user is previewing start date, so mark on start. 
            _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, e.Date);
            _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, e.Date);
        }
        else if (_status.Current == Status.End)
        {
            // This means user is previewing end date, so mark on end. 
            _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, null, e.Date);
            _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, null, e.Date);
        }
        else
        {
            _startCalendar?.ClearSelection();
            _endCalendar?.ClearSelection();
        }
    }

    private void OnTextBoxGotFocus(object? sender, FocusChangedEventArgs e)
    {
        InitializePopupOpen(sender as TextBox);
    }

    private void InitializePopupOpen(TextBox? sender)
    {
        if (sender is null) return;
        SetCurrentValue(IsDropdownOpenProperty, true);
        SetCalendarContextDate();
        if (Equals(sender, _startTextBox))
        {
            _status.Push(Status.Start);
        }
        else if (Equals(sender, _endTextBox))
        {
            _status.Push(Status.End);
        }

        _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate);
        _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate);
    }


    private void OnDateSelected(object? sender, CalendarDayButtonEventArgs e)
    {
        if (_status is { Current: Status.Start, Previous: Status.None })
        {
            // User make first selection: Start.
            SetCurrentValue(SelectedStartDateProperty, e.Date);
            if (SelectedEndDate != null && e.Date > SelectedEndDate)
            {
                SetCurrentValue(SelectedEndDateProperty, null);
            }

            _status.Push(Status.End);
            _endTextBox?.Focus();
        }
        else if (_status is { Current: Status.Start, Previous: Status.End })
        {
            // User make second selection: Start. 
            SetCurrentValue(SelectedStartDateProperty, e.Date);
            _status.Reset();
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
        else if (_status is { Current: Status.End, Previous: Status.None })
        {
            // User make first selection: End.
            SetCurrentValue(SelectedEndDateProperty, e.Date);
            if (SelectedStartDate != null && e.Date < SelectedStartDate)
            {
                SetCurrentValue(SelectedStartDateProperty, null);
            }

            _status.Push(Status.Start);
            _startTextBox?.Focus();
        }
        else if (_status is { Current: Status.End, Previous: Status.Start })
        {
            // User make second selection: End.
            SetCurrentValue(SelectedEndDateProperty, e.Date);
            _status.Reset();
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
    }

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        base.OnLostFocus(e);
        var newItem = e.NewFocusedElement;
        if (Equals(newItem, _endTextBox) || Equals(newItem, _startTextBox))
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
        }
        else
        {
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
    }

    private void OnSelectionChanged()
    {
        SyncDatesToTexts();
        PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedStartDate is null && SelectedEndDate is null);
    }

    private void SyncDatesToTexts()
    {
        _startTextBox?.SetCurrentValue(TextBox.TextProperty,
            SelectedStartDate is not null
                ? SelectedStartDate.Value.ToString(DisplayFormat ?? DEFAULT_DATE_DISPLAY_FORMAT)
                : string.Empty);

        _endTextBox?.SetCurrentValue(TextBox.TextProperty,
            SelectedEndDate is not null
                ? SelectedEndDate.Value.ToString(DisplayFormat ?? DEFAULT_DATE_DISPLAY_FORMAT)
                : string.Empty);
    }

    private void SetCalendarContextDate()
    {
        var startDate = SelectedStartDate ?? DateTime.Today;
        _startCalendar?.SyncContextDate(new CalendarContext(startDate.Year, startDate.Month));
        var endDate = SelectedEndDate ?? startDate;
        if (endDate.Year == startDate.Year && endDate.Month == startDate.Month)
        {
            endDate = endDate.AddMonths(1);
        }

        _endCalendar?.SyncContextDate(new CalendarContext(endDate.Year, endDate.Month));
    }

    /// <summary>
    /// When TextBox lost focus or user pressed commit button (Enter here), try to parse text and set SelectedDates if they are different. 
    /// </summary>
    private void CommitInput()
    {
        DateTime? startDate = null;
        var format = this.DisplayFormat ?? DEFAULT_DATE_DISPLAY_FORMAT;
        if (string.IsNullOrWhiteSpace(_startTextBox?.Text))
        {
            startDate = null;
            SetCurrentValue(SelectedStartDateProperty, startDate);
        }
        else if (DateTime.TryParseExact(_startTextBox?.Text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None,
                     out var start))
        {
            startDate = start;
            SetCurrentValue(SelectedStartDateProperty, startDate);
        }

        DateTime? endDate = null;
        if (string.IsNullOrWhiteSpace(_endTextBox?.Text))
        {
            endDate = null;
            SetCurrentValue(SelectedEndDateProperty, endDate);
        }
        else if (DateTime.TryParseExact(_endTextBox?.Text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None,
                     out var end))
        {
            endDate = end;
            SetCurrentValue(SelectedEndDateProperty, endDate);
        }

        _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate);
        _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (e.Source is Visual source)
        {
            var inPopup = _popup?.IsInsidePopup(source);
            if (inPopup is true) return;
        }

        if (_startTextBox?.IsFocused == false)
        {
            _startTextBox?.Focus();
        }
        else
        {
            SetCurrentValue(IsDropdownOpenProperty, true);
            SetCalendarContextDate();
        }
    }

    public void Clear()
    {
        SetCurrentValue(SelectedStartDateProperty, null);
        SetCurrentValue(SelectedEndDateProperty, null);
    }

    private record DateRangePickerStatus
    {
        public Status Previous { get; private set; }
        public Status Current { get; private set; }

        public void Reset()
        {
            Previous = Status.None;
            Current = Status.None;
        }

        public void Push(Status status)
        {
            if (Current == status) return;
            Previous = Current;
            Current = status;
        }
    }

    private enum Status : byte
    {
        None,
        Start,
        End,
    }
}