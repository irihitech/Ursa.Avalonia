using System.Globalization;
using System.Reflection;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media.TextFormatting;
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
[TemplatePart(PART_MonthGrid, typeof(Grid))]
[TemplatePart(PART_YearGrid, typeof(Grid))]
public class CalendarView: TemplatedControl
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
    public const string PART_MonthGrid = "PART_MonthGrid";
    public const string PART_YearGrid = "PART_YearGried";
    
    private const string ShortestDayName = "ShortestDayName";
    
    private readonly System.Globalization.Calendar _calendar = new GregorianCalendar();

    internal CalendarViewMode Mode;

    //private CalendarMonthView? _monthView;
    //private CalendarYearView? _yearView;
    private Grid? _monthGrid;
    private Grid? _yearGrid;
    // Year button only shows the year in month mode.
    private Button? _yearButton;
    // Month button only shows the month in month mode.
    private Button? _monthButton;
    // Header button shows year in year mode, and year range in higher mode. 
    private Button? _headerButton;

    public DateContext ContextDate { get; set; } = new DateContext();
    
    public event EventHandler<CalendarDayButtonEventArgs>? OnDateSelected;
    public event EventHandler<CalendarDayButtonEventArgs>? OnDatePreviewed;

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty =
        DatePickerBase.IsTodayHighlightedProperty.AddOwner<CalendarView>();
    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        DatePickerBase.FirstDayOfWeekProperty.AddOwner<CalendarView>();
    
    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        Button.ClickEvent.RemoveHandler(OnHeaderYearButtonClick, _yearButton);
        Button.ClickEvent.RemoveHandler(OnHeaderMonthButtonClick, _monthButton);
        Button.ClickEvent.RemoveHandler(OnHeaderButtonClick, _headerButton);
  
        _monthGrid = e.NameScope.Find<Grid>(PART_MonthGrid);
        _yearGrid = e.NameScope.Find<Grid>(PART_YearGrid);
        _yearButton = e.NameScope.Find<Button>(PART_YearButton);
        _monthButton = e.NameScope.Find<Button>(PART_MonthButton);
        _headerButton = e.NameScope.Find<Button>(PART_HeaderButton);

        Button.ClickEvent.AddHandler(OnHeaderYearButtonClick, _yearButton);
        Button.ClickEvent.AddHandler(OnHeaderMonthButtonClick, _monthButton);
        Button.ClickEvent.AddHandler(OnHeaderButtonClick, _headerButton);
        
        GenerateGridElements();
    }

    /// <summary>
    /// Rule: 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnHeaderButtonClick(object sender, RoutedEventArgs e)
    {
        if (Mode == CalendarViewMode.Month)
        {
            throw new NotImplementedException();
        }

        if (Mode == CalendarViewMode.Year)
        {
            
        }
    }

    /// <summary>
    /// Generate Buttons and labels for MonthView.
    /// Generate Buttons for YearView.  
    /// </summary>
    private void GenerateGridElements()
    {
        // Generate Day titles (Sun, Mon, Tue, Wed, Thu, Fri, Sat) based on FirstDayOfWeek and culture.
        var count = 7 + 7 * 7;
        var children = new List<Control>(count);
        var dayOfWeek = (int)FirstDayOfWeek;
        var info = DateTimeHelper.GetCurrentDateTimeFormatInfo();
        for (var i = 0; i < 7; i++)
        {
            var d = (dayOfWeek + i) % DateTimeHelper.NumberOfDaysPerWeek;
            var cell = new TextBlock { Text = info.ShortestDayNames[d], Tag = ShortestDayName };
            cell.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
            cell.SetValue(Grid.RowProperty, 0);
            cell.SetValue(Grid.ColumnProperty, i);
            children.Add(cell);
        }

        // Generate day buttons.
        for (var i = 2; i < DateTimeHelper.NumberOfWeeksPerMonth + 2; i++)
        for (var j = 0; j < DateTimeHelper.NumberOfDaysPerWeek; j++)
        {
            var cell = new CalendarDayButton();
            cell.SetValue(Grid.RowProperty, i);
            cell.SetValue(Grid.ColumnProperty, j);
            cell.AddHandler(CalendarDayButton.DateSelectedEvent, OnCellDateSelected);
            cell.AddHandler(CalendarDayButton.DatePreviewedEvent, OnCellDatePreviewed);
            children.Add(cell);
        }
        _monthGrid?.Children.AddRange(children);
        
        // Generate month/year buttons. 
        for (var i = 0; i < 12; i++)
        {
            var button = new CalendarYearButton();
            Grid.SetRow(button, i / 3);
            Grid.SetColumn(button, i % 3);
            button.AddHandler(CalendarYearButton.ItemSelectedEvent, OnYearItemSelected);
            _yearGrid?.Children.Add(button);
        }
    }

    private void RefreshButtons(DateTime date)
    {
        if (_monthGrid is null) return;
        var children = _monthGrid.Children;
        var info = DateTimeHelper.GetCurrentDateTimeFormatInfo();
        var dayBefore = PreviousMonthDays(date);
        var dateToSet = date.GetFirstDayOfMonth().AddDays(-dayBefore);
        for (var i = 8; i < children.Count; i++)
        {
            var day = dateToSet;
            var cell = children[i] as CalendarDayButton;
            if (cell is null) continue;
            cell.DataContext = day;
            cell.IsToday = day == DateTime.Today;
            cell.Content = day.Day.ToString(info);
            dateToSet = dateToSet.AddDays(1);
        }

        FadeOutDayButtons();
    }
    
    private void FadeOutDayButtons()
    {
        if (_monthGrid is null) return;
        var children = _monthGrid.Children;
        for (var i = 8; i < children.Count; i++)
            if (children[i] is CalendarDayButton { DataContext: DateTime d } button && d.Month != ContextDate.Month)
                button.IsNotCurrentMonth = true;
    }
    
    private int PreviousMonthDays(DateTime date)
    {
        var firstDay = date.GetFirstDayOfMonth();
        var dayOfWeek = _calendar.GetDayOfWeek(firstDay);
        var firstDayOfWeek = FirstDayOfWeek;
        var i = (dayOfWeek - firstDayOfWeek + DateTimeHelper.NumberOfDaysPerWeek) % DateTimeHelper.NumberOfDaysPerWeek;
        return i == 0 ? DateTimeHelper.NumberOfDaysPerWeek : i;
    }
    
    private void OnCellDatePreviewed(object sender, CalendarDayButtonEventArgs e)
    {
        OnDatePreviewed?.Invoke(sender, e);
    }

    private void OnCellDateSelected(object sender, CalendarDayButtonEventArgs e)
    {
        OnDateSelected?.Invoke(sender, e);
    }

    /// <summary>
    /// Click on Month Header button. Calendar switch from month mode to year mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnHeaderMonthButtonClick(object sender, RoutedEventArgs e)
    {
        _headerButton?.SetValue(ContentControl.ContentProperty, ContextDate.Year);
        Mode = CalendarViewMode.Year;
        IsVisibleProperty.SetValue(true, _yearGrid);
        IsVisibleProperty.SetValue(false, _monthGrid);
    }

    /// <summary>
    /// Click on Year Header button. Calendar switch from month mode to decade mode. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnHeaderYearButtonClick(object sender, RoutedEventArgs e)
    {
        if (_yearGrid is null) return;
        int? decadeStart = ContextDate.Year / 10 * 10;
        _headerButton?.SetValue(ContentControl.ContentProperty,
            decadeStart + "-" + (decadeStart + 10));
        Mode = CalendarViewMode.Decade;
        IsVisibleProperty.SetValue(true, _yearGrid);
        IsVisibleProperty.SetValue(false, _monthGrid);
    }
    
    private void OnYearItemSelected(object sender, CalendarYearButtonEventArgs e)
    {
        if (_yearGrid is null) return;
        var buttons = _yearGrid.Children.OfType<CalendarYearButton>().ToList();
        if (e.Mode == CalendarViewMode.Year)
        {
            if (e.Month is null) return;
            var day = MathHelpers.SafeClamp(e.Month.Value, 0,
                DateTime.DaysInMonth(ContextDate.Year!.Value, e.Month.Value + 1));
            ContextDate = new DateContext { Year = ContextDate.Year, Month = e.Month + 1, Day = day };
            Mode = CalendarViewMode.Month;
        }
        else if (e.Mode == CalendarViewMode.Decade)
        {
            // Set CalendarYearView to Month mode
            for (var i = 0; i < 12; i++)
            {
                buttons[i].SetValues(CalendarViewMode.Month, month: i);
            }
            ContextDate = new DateContext() { Year = e.Year!.Value, Month = ContextDate.Month, Day = null };
            Mode = CalendarViewMode.Month;
        }
        else if (e.Mode == CalendarViewMode.Century)
        {
            // Set CalendarYearView to Year mode
            for (var i = 0; i < 12; i++)
            {
                if (e.StartYear is null || e.EndYear is null) continue;
                var year = e.StartYear.Value - 1 + i;
                buttons[i].SetValues(CalendarViewMode.Year, year: year);
            }
            Mode = CalendarViewMode.Year;
        }
    }
}
