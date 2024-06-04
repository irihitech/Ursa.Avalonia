using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_NextYearButton, typeof(Button))]
[TemplatePart(PART_PreviousYearButton, typeof(Button))]
[TemplatePart(PART_NextButton, typeof(Button))]
[TemplatePart(PART_PreviousButton, typeof(Button))]
[TemplatePart(PART_YearButton, typeof(Button))]
[TemplatePart(PART_MonthButton, typeof(Button))]
[TemplatePart(PART_HeaderButton, typeof(Button))]
[TemplatePart(PART_MonthView, typeof(CalendarMonthView))]
[TemplatePart(PART_YearView, typeof(CalendarYearView))]
public class Calendar: TemplatedControl
{
    public const string PART_NextYearButton = "PART_NextYearButton";
    public const string PART_PreviousYearButton = "PART_PreviousYearButton";
    public const string PART_NextButton = "PART_NextButton";
    public const string PART_PreviousButton = "PART_PreviousButton";
    public const string PART_YearButton = "PART_YearButton";
    public const string PART_MonthButton = "PART_MonthButton";
    public const string PART_MonthView = "PART_MonthView";
    public const string PART_YearView = "PART_YearView";
    public const string PART_HeaderButton = "PART_HeaderButton";

    private CalendarMonthView? _monthView;
    private CalendarYearView? _yearView;
    private DatePickerState _state = DatePickerState.None;
    private Button? _yearButton;
    private Button? _monthButton;
    private Button? _headerButton;
    
    
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

    private bool _isMonthMode = true;

    public static readonly DirectProperty<Calendar, bool> IsMonthModeProperty = AvaloniaProperty.RegisterDirect<Calendar, bool>(
        nameof(IsMonthMode), o => o.IsMonthMode, (o, v) => o.IsMonthMode = v);

    public bool IsMonthMode
    {
        get => _isMonthMode;
        set => SetAndRaise(IsMonthModeProperty, ref _isMonthMode, value);
    }

    internal DateTime? StartDate;
    internal DateTime? EndDate;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (_monthView is not null)
        {
            _monthView.OnDateSelected -= OnDateSelected;
            _monthView.OnDatePreviewed -= OnDatePreviewed;
        }

        if (_yearView is not null)
        {
            _yearView.OnMonthSelected -= OnMonthSelected;
        }
        Button.ClickEvent.RemoveHandler(OnYearButtonClick, _yearButton);
        Button.ClickEvent.RemoveHandler(OnMonthButtonClick, _monthButton);
        _monthView = e.NameScope.Find<CalendarMonthView>(PART_MonthView);
        _yearView = e.NameScope.Find<CalendarYearView>(PART_YearView);
        _yearButton = e.NameScope.Find<Button>(PART_YearButton);
        _monthButton = e.NameScope.Find<Button>(PART_MonthButton);
        
        if(_monthView is not null)
        {
            _monthView.OnDateSelected += OnDateSelected;
            _monthView.OnDatePreviewed += OnDatePreviewed;
        }

        if (_yearView is not null)
        {
            _yearView.OnMonthSelected += OnMonthSelected;
        }
        Button.ClickEvent.AddHandler(OnYearButtonClick, _yearButton);
        Button.ClickEvent.AddHandler(OnMonthButtonClick, _monthButton);
    }

    private void OnMonthSelected(object sender, CalendarYearButtonEventArgs e)
    {
        SetCurrentValue(IsMonthModeProperty, true);
    }

    private void OnMonthButtonClick(object sender, RoutedEventArgs e)
    {
        SetCurrentValue(IsMonthModeProperty, false);
        if (_yearView is null) return;
        _yearView.Mode = CalendarYearViewMode.Month;
    }

    private void OnYearButtonClick(object sender, RoutedEventArgs e)
    {
        if (_yearView is null) return;
        _yearView.Mode = CalendarYearViewMode.Year;
        SetCurrentValue(IsMonthModeProperty, false);
        
    }

    private void OnDatePreviewed(object sender, CalendarDayButtonEventArgs e)
    {
        if(_monthView is null)
        {
            return;
        }
        var date = e.Date;
        if (_state is DatePickerState.None) return;
        if (_state == DatePickerState.PreviewStart)
        {
            _monthView.MarkPreview(date, EndDate);
        }
        else if (_state == DatePickerState.PreviewEnd)
        {
            _monthView.MarkPreview(StartDate, date);
        }
    }

    private void OnDateSelected(object sender, CalendarDayButtonEventArgs e)
    {
        if(_monthView is null)
        {
            return;
        }
        var date = e.Date;
        if (_state == DatePickerState.None)
        {
            _monthView.ClearSelection();
            _monthView.ClearPreview();
            _monthView.MarkSelection(date, null);
            _state = DatePickerState.PreviewEnd;
            StartDate = date;
        }
        else if (_state == DatePickerState.PreviewStart)
        {
            _monthView.MarkSelection(date, EndDate);
            _state = DatePickerState.SelectStart;
            StartDate = date;
        }
        else if (_state == DatePickerState.PreviewEnd)
        {
            _monthView.MarkSelection(StartDate, date);
            _state = DatePickerState.None;
            EndDate = date;
        }
    }
}
