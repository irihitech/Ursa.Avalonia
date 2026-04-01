using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

[TemplatePart(PART_Popup, typeof(Popup))]
[TemplatePart(PART_StartCalendar, typeof(CalendarView))]
[TemplatePart(PART_EndCalendar, typeof(CalendarView))]
[TemplatePart(PART_StartTextBox, typeof(TextBox))]
[TemplatePart(PART_EndTextBox, typeof(TextBox))]
[PseudoClasses(PseudoClassName.PC_Empty)]
public class DateRangePickerV2: DatePickerBase, IClearControl
{
    public const string PART_Popup = "PART_Popup";
    public const string PART_StartCalendar = "PART_StartCalendar";
    public const string PART_EndCalendar = "PART_EndCalendar";
    public const string PART_StartTextBox = "PART_StartTextBox";
    public const string PART_EndTextBox = "PART_EndTextBox";

    public static readonly StyledProperty<DateTime?> SelectedStartDateProperty =
        AvaloniaProperty.Register<DateRangePickerV2, DateTime?>(
            nameof(SelectedStartDate), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<DateTime?> SelectedEndDateProperty =
        AvaloniaProperty.Register<DateRangePickerV2, DateTime?>(
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

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _startCalendar?.RemoveHandler(CalendarView.DateSelectedEvent, OnDateSelected);
        _endCalendar?.RemoveHandler(CalendarView.DateSelectedEvent, OnDateSelected);
        _startCalendar?.RemoveHandler(CalendarView.DatePreviewedEvent, OnDatePreviewed);
        _endCalendar?.RemoveHandler(CalendarView.DatePreviewedEvent, OnDatePreviewed);
        _startTextBox?.RemoveHandler(GotFocusEvent, OnTextBoxGotFocus);
        _endTextBox?.RemoveHandler(GotFocusEvent, OnTextBoxGotFocus);
        
        _startCalendar = e.NameScope.Find<CalendarView>(PART_StartCalendar);
        _endCalendar = e.NameScope.Find<CalendarView>(PART_EndCalendar);
        _startTextBox = e.NameScope.Find<TextBox>(PART_StartTextBox);
        _endTextBox = e.NameScope.Find<TextBox>(PART_EndTextBox);
        _popup = e.NameScope.Find<Popup>(PART_Popup);
        
        _startCalendar?.AddHandler(CalendarView.DateSelectedEvent, OnDateSelected);
        _endCalendar?.AddHandler(CalendarView.DateSelectedEvent, OnDateSelected);
        _startCalendar?.AddHandler(CalendarView.DatePreviewedEvent, OnDatePreviewed);
        _endCalendar?.AddHandler(CalendarView.DatePreviewedEvent, OnDatePreviewed);
        _startTextBox?.AddHandler(GotFocusEvent, OnTextBoxGotFocus);
        _endTextBox?.AddHandler(GotFocusEvent, OnTextBoxGotFocus);
        


    }

    private void OnEndDatePreviewed(object? sender, CalendarDayButtonEventArgs e)
    {
        _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, null, e.Date);
        _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, null, e.Date);
    }

    private void OnDatePreviewed(object? sender, CalendarDayButtonEventArgs e)
    {
        if (_status.Current == Status.Start)
        {
            // This means user is previewing start date, so mark on start. 
            _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, e.Date, null);
            _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, e.Date, null);
        }
        else if (_status.Current == Status.End)
        {
            // This means user is previewing end date, so mark on end. 
            _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate,null, e.Date);
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
        SetCurrentValue(IsDropdownOpenProperty, true);
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
            SetCurrentValue(SelectedStartDateProperty, e.Date);
            // User make first selection: Start.
            _status.Push(Status.End);
            _endTextBox?.Focus();
            return;
        }
        else if (_status.Current == Status.Start && _status.Previous == Status.End)
        {
            SetCurrentValue(SelectedStartDateProperty, e.Date);
            // User make second selection: End.
            _status.Reset();
            SetCurrentValue(IsDropdownOpenProperty, false);
            return;
        }
        else if (_status.Current == Status.End && _status.Previous == Status.None)
        {
            SetCurrentValue(SelectedEndDateProperty, e.Date);
            // User make first selection: End.
            _status.Push(Status.Start);
            _startTextBox?.Focus();
            return;
        }
        else if(_status.Current == Status.End && _status.Previous == Status.Start)
        {
            SetCurrentValue(SelectedEndDateProperty, e.Date);
            // User make second selection: End
            _status.Reset();
            SetCurrentValue(IsDropdownOpenProperty, false);
            return;
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
        }
    }

    public void Clear()
    {
        SetCurrentValue(SelectedStartDateProperty, null);
        SetCurrentValue(SelectedEndDateProperty, null);
    }

    private class DateRangePickerStatus
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

    private enum Status
    {
        None,
        Start,
        End,
    }
}