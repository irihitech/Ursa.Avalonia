using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace Ursa.Controls;

[TemplatePart(PART_NextYearButton, typeof(Button))]
[TemplatePart(PART_PreviousYearButton, typeof(Button))]
[TemplatePart(PART_NextButton, typeof(Button))]
[TemplatePart(PART_PreviousButton, typeof(Button))]
[TemplatePart(PART_HeaderButton, typeof(Button))]
[TemplatePart(PART_BackButton, typeof(Button))]
[TemplatePart(PART_MonthView, typeof(CalendarMonthView))]
[TemplatePart(PART_YearView, typeof(CalendarYearView))]
public class Calendar: TemplatedControl
{
    public const string PART_NextYearButton = "PART_NextYearButton";
    public const string PART_PreviousYearButton = "PART_PreviousYearButton";
    public const string PART_NextButton = "PART_NextButton";
    public const string PART_PreviousButton = "PART_PreviousButton";
    public const string PART_HeaderButton = "PART_HeaderButton";
    public const string PART_BackButton = "PART_BackButton";
    public const string PART_MonthView = "PART_MonthView";
    public const string PART_YearView = "PART_YearView";

    private CalendarMonthView? _monthGrid;
    private DatePickerState _state = DatePickerState.None;
    
    
    public static readonly StyledProperty<DateTime> SelectedDateProperty = AvaloniaProperty.Register<Calendar, DateTime>(nameof(SelectedDate), DateTime.Now);
    public DateTime SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        AvaloniaProperty.Register<Calendar, DayOfWeek>(nameof(FirstDayOfWeek),
            defaultValue: DateTimeHelper.GetCurrentDateTimeFormatInfo().FirstDayOfWeek);
    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty = AvaloniaProperty.Register<Calendar, bool>(nameof(IsTodayHighlighted), true);
    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }

    public static readonly StyledProperty<AvaloniaList<DateRange>?> BlackoutDatesProperty =
        AvaloniaProperty.Register<Calendar, AvaloniaList<DateRange>?>(
            nameof(BlackoutDates));

    public AvaloniaList<DateRange>? BlackoutDates
    {
        get => GetValue(BlackoutDatesProperty);
        set => SetValue(BlackoutDatesProperty, value);
    }

    public static readonly StyledProperty<IDateSelector?> BlackoutDateRuleProperty = AvaloniaProperty.Register<Calendar, IDateSelector?>(
        nameof(BlackoutDateRule));

    public IDateSelector? BlackoutDateRule
    {
        get => GetValue(BlackoutDateRuleProperty);
        set => SetValue(BlackoutDateRuleProperty, value);
    }

    internal DateTime? StartDate;
    internal DateTime? EndDate;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (_monthGrid is not null)
        {
            _monthGrid.OnCalendarDayButtonPressed -= OnCalendarDayButtonPressed;
            _monthGrid.OnCalendarDayButtonPointerEnter -= OnCalendarDayButtonPointerEnter;
        }
        _monthGrid = e.NameScope.Find<CalendarMonthView>(PART_MonthView);
        if(_monthGrid is not null)
        {
            _monthGrid.OnCalendarDayButtonPressed += OnCalendarDayButtonPressed;
            _monthGrid.OnCalendarDayButtonPointerEnter += OnCalendarDayButtonPointerEnter;
        }
    }

    private void OnCalendarDayButtonPointerEnter(object sender, CalendarDayButtonEventArgs e)
    {
        if(_monthGrid is null)
        {
            return;
        }
        var date = e.Date;
        if (_state is DatePickerState.None) return;
        if (_state == DatePickerState.PreviewStart)
        {
            _monthGrid.MarkPreview(date, EndDate);
        }
        else if (_state == DatePickerState.PreviewEnd)
        {
            _monthGrid.MarkPreview(StartDate, date);
        }
    }

    private void OnCalendarDayButtonPressed(object sender, CalendarDayButtonEventArgs e)
    {
        if(_monthGrid is null)
        {
            return;
        }
        var date = e.Date;
        if (_state == DatePickerState.None)
        {
            _monthGrid.ClearSelection();
            _monthGrid.MarkSelection(date, null);
            _state = DatePickerState.PreviewEnd;
            StartDate = date;
        }
        else if (_state == DatePickerState.PreviewStart)
        {
            _monthGrid.MarkSelection(date, EndDate);
            _state = DatePickerState.SelectStart;
            StartDate = date;
        }
        else if (_state == DatePickerState.PreviewEnd)
        {
            _monthGrid.MarkSelection(StartDate, date);
            _state = DatePickerState.None;
            EndDate = date;
        }
    }
}
