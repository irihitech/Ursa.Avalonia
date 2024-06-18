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
public class CalendarDisplayControl: TemplatedControl
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
    // Year button only shows the year in month mode.
    private Button? _yearButton;
    // Month button only shows the month in month mode.
    private Button? _monthButton;
    // Header button shows year in year mode, and year range in higher mode. 
    private Button? _headerButton;
    
    public event EventHandler<CalendarDayButtonEventArgs>? OnDateSelected;
    public event EventHandler<CalendarDayButtonEventArgs>? OnDatePreviewed;

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty =
        DatePickerBase.IsTodayHighlightedProperty.AddOwner<CalendarDisplayControl>();
    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        DatePickerBase.FirstDayOfWeekProperty.AddOwner<CalendarDisplayControl>();
    
    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    private bool _isMonthMode = true;

    public static readonly DirectProperty<CalendarDisplayControl, bool> IsMonthModeProperty = AvaloniaProperty.RegisterDirect<CalendarDisplayControl, bool>(
        nameof(IsMonthMode), o => o.IsMonthMode, (o, v) => o.IsMonthMode = v);

    public bool IsMonthMode
    {
        get => _isMonthMode;
        set => SetAndRaise(IsMonthModeProperty, ref _isMonthMode, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (_monthView is not null)
        {
            _monthView.OnDateSelected -= OnMonthViewDateSelected;
            _monthView.OnDatePreviewed -= OnMonthViewDatePreviewed;
        }

        if (_yearView is not null)
        {
            _yearView.OnMonthSelected -= OnMonthSelected;
        }
        Button.ClickEvent.RemoveHandler(OnYearButtonClick, _yearButton);
        Button.ClickEvent.RemoveHandler(OnMonthButtonClick, _monthButton);
        Button.ClickEvent.RemoveHandler(OnHeaderButtonClick, _headerButton);
        _monthView = e.NameScope.Find<CalendarMonthView>(PART_MonthView);
        _yearView = e.NameScope.Find<CalendarYearView>(PART_YearView);
        _yearButton = e.NameScope.Find<Button>(PART_YearButton);
        _monthButton = e.NameScope.Find<Button>(PART_MonthButton);
        _headerButton = e.NameScope.Find<Button>(PART_HeaderButton);
        if(_monthView is not null)
        {
            _monthView.OnDateSelected += OnMonthViewDateSelected;
            _monthView.OnDatePreviewed += OnMonthViewDatePreviewed;
        }

        if (_yearView is not null)
        {
            _yearView.OnMonthSelected += OnMonthSelected;
        }
        Button.ClickEvent.AddHandler(OnYearButtonClick, _yearButton);
        Button.ClickEvent.AddHandler(OnMonthButtonClick, _monthButton);
        Button.ClickEvent.AddHandler(OnHeaderButtonClick, _headerButton);
    }

    /// <summary>
    /// Rule: 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnHeaderButtonClick(object sender, RoutedEventArgs e)
    {
        if (_yearView?.Mode == CalendarYearViewMode.Month)
        {
            _headerButton?.SetValue(ContentControl.ContentProperty, _yearView.ContextDate.Year);
            _yearView?.UpdateMode(CalendarYearViewMode.Year);
        }
        else if (_yearView?.Mode == CalendarYearViewMode.Year)
        {
            _headerButton?.SetCurrentValue(ContentControl.ContentProperty,
                _yearView.ContextDate.Year + "-" + (_yearView.ContextDate.Year + 100));
            _yearView?.UpdateMode(CalendarYearViewMode.YearRange);
        }
    }

    private void OnMonthSelected(object sender, CalendarYearButtonEventArgs e)
    {
        SetCurrentValue(IsMonthModeProperty, true);
    }

    private void OnMonthButtonClick(object sender, RoutedEventArgs e)
    {
        SetCurrentValue(IsMonthModeProperty, false);
        if (_yearView is null) return;
        _headerButton?.SetValue(ContentControl.ContentProperty, _yearView.ContextDate.Year);
        _yearView?.UpdateMode(CalendarYearViewMode.Month);
    }

    private void OnYearButtonClick(object sender, RoutedEventArgs e)
    {
        if (_yearView is null) return;
        _headerButton?.SetValue(ContentControl.ContentProperty,
            _yearView?.ContextDate.Year + "-" + (_yearView?.ContextDate.Year + 10));
        _yearView?.UpdateMode(CalendarYearViewMode.Year);
        SetCurrentValue(IsMonthModeProperty, false);
    }

    private void OnMonthViewDatePreviewed(object sender, CalendarDayButtonEventArgs e)
    {
        OnDatePreviewed?.Invoke(sender, e);
    }

    private void OnMonthViewDateSelected(object sender, CalendarDayButtonEventArgs e)
    {
        OnDateSelected?.Invoke(sender, e);
    }
}
