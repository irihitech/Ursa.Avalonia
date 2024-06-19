using System.Diagnostics;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Irihi.Avalonia.Shared.Helpers;
using Calendar = System.Globalization.Calendar;

namespace Ursa.Controls;

[TemplatePart(PART_NextYearButton, typeof(Button))]
[TemplatePart(PART_PreviousYearButton, typeof(Button))]
[TemplatePart(PART_NextButton, typeof(Button))]
[TemplatePart(PART_PreviousButton, typeof(Button))]
[TemplatePart(PART_YearButton, typeof(Button))]
[TemplatePart(PART_MonthButton, typeof(Button))]
[TemplatePart(PART_HeaderButton, typeof(Button))]
[TemplatePart(PART_MonthGrid, typeof(Grid))]
[TemplatePart(PART_YearGrid, typeof(Grid))]
[PseudoClasses(PC_Month)]
public class CalendarView : TemplatedControl
{
    public const string PART_NextYearButton = "PART_NextYearButton";
    public const string PART_PreviousYearButton = "PART_PreviousYearButton";
    public const string PART_NextButton = "PART_NextButton";
    public const string PART_PreviousButton = "PART_PreviousButton";
    public const string PART_YearButton = "PART_YearButton";
    public const string PART_MonthButton = "PART_MonthButton";
    public const string PART_HeaderButton = "PART_HeaderButton";
    public const string PART_MonthGrid = "PART_MonthGrid";
    public const string PART_YearGrid = "PART_YearGrid";
    public const string PC_Month = ":month";

    private const string ShortestDayName = "ShortestDayName";

    internal static readonly DirectProperty<CalendarView, CalendarViewMode> ModeProperty =
        AvaloniaProperty.RegisterDirect<CalendarView, CalendarViewMode>(
            nameof(Mode), o => o.Mode, (o, v) => o.Mode = v, unsetValue: CalendarViewMode.Month);

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty =
        DatePickerBase.IsTodayHighlightedProperty.AddOwner<CalendarView>();

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        DatePickerBase.FirstDayOfWeekProperty.AddOwner<CalendarView>();

    private readonly Calendar _calendar = new GregorianCalendar();

    // Header button shows year in year mode, and year range in higher mode. 
    private Button? _headerButton;

    private CalendarViewMode _mode;

    // Month button only shows the month in month mode.
    private Button? _monthButton;
    
    private Grid? _monthGrid;

    // Year button only shows the year in month mode.
    private Button? _yearButton;
    private Grid? _yearGrid;

    static CalendarView()
    {
        FirstDayOfWeekProperty.Changed.AddClassHandler<CalendarView, DayOfWeek>((view, args) =>
            view.OnFirstDayOfWeekChanged(args));
        ModeProperty.Changed.AddClassHandler<CalendarView, CalendarViewMode>((view, args) =>
        {
            view.PseudoClasses.Set(PC_Month, args.NewValue.Value == CalendarViewMode.Month);
            Debug.WriteLine(args.NewValue.Value);
        });
    }

    internal CalendarViewMode Mode
    {
        get => _mode;
        set => SetAndRaise(ModeProperty, ref _mode, value);
    }

    public DateContext ContextDate { get; set; } = new();

    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }

    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    public event EventHandler<CalendarDayButtonEventArgs>? OnDateSelected;
    public event EventHandler<CalendarDayButtonEventArgs>? OnDatePreviewed;

    private void OnFirstDayOfWeekChanged(AvaloniaPropertyChangedEventArgs<DayOfWeek> args)
    {
        UpdateMonthViewHeader(args.NewValue.Value);
        RefreshDayButtons();
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
        
        ContextDate = new DateContext(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        PseudoClasses.Set(PC_Month, Mode == CalendarViewMode.Month);
        GenerateGridElements();
        RefreshDayButtons();
        RefreshYearButtons();
    }

    /// <summary>
    ///     Rule:
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnHeaderButtonClick(object sender, RoutedEventArgs e)
    {
        // Header button should be hidden in Month mode.
        if (Mode == CalendarViewMode.Month) return;
        if (Mode == CalendarViewMode.Year)
        {
            Mode = CalendarViewMode.Decade;
            RefreshYearButtons();
            return; 
        }
        if(Mode == CalendarViewMode.Decade)
        {
            Mode = CalendarViewMode.Century;
            RefreshYearButtons();
            return;
        }
        if (Mode == CalendarViewMode.Century) return;
    }

    /// <summary>
    ///     Generate Buttons and labels for MonthView.
    ///     Generate Buttons for YearView.
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

    private void RefreshDayButtons()
    {
        if (_monthGrid is null) return;
        var children = _monthGrid.Children;
        var info = DateTimeHelper.GetCurrentDateTimeFormatInfo();
        var date = new DateTime(ContextDate.Year.Value, ContextDate.Month.Value, ContextDate.Day.Value);
        var dayBefore =
            PreviousMonthDays(date);
        var dateToSet = date.GetFirstDayOfMonth().AddDays(-dayBefore);
        for (var i = 7; i < children.Count; i++)
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

    private void RefreshYearButtons()
    {
        if (_yearGrid is null) return;
        var mode = Mode;
        var contextDate = ContextDate;
        for (var i = 0; i < 12; i++)
        {
            var child = _yearGrid.Children[i] as CalendarYearButton;
            if (child is null) continue;
            switch (mode)
            {
                case CalendarViewMode.Month:
                    child.SetValues(CalendarViewMode.Year, i);
                    break;
                case CalendarViewMode.Year:
                    child.SetValues(CalendarViewMode.Decade, year: ContextDate.Year / 10 * 10 + i - 1);
                    break;
                case CalendarViewMode.Decade:
                    var startYear = (ContextDate.Year / 10 + i - 1) * 10;
                    var endYear = (ContextDate.Year / 10 + i - 1) * 10 + 10;
                    child.SetValues(CalendarViewMode.Century, startYear: startYear, endYear: endYear);
                    break;
            }
        }
    }

    private void FadeOutDayButtons()
    {
        if (_monthGrid is null) return;
        var children = _monthGrid.Children;
        for (var i = 7; i < children.Count; i++)
            if (children[i] is CalendarDayButton { DataContext: DateTime d } button && d.Month != ContextDate.Month)
                button.IsNotCurrentMonth = true;
    }

    private void UpdateMonthViewHeader(DayOfWeek day)
    {
        var dayOfWeek = (int)day;
        var info = DateTimeHelper.GetCurrentDateTimeFormatInfo();
        var texts = _monthGrid?.Children.Where(a => a is TextBlock { Tag: ShortestDayName }).ToList();
        if (texts is not null)
            for (var i = 0; i < 7; i++)
            {
                var d = (dayOfWeek + i) % DateTimeHelper.NumberOfDaysPerWeek;
                texts[i].SetValue(TextBlock.TextProperty, info.ShortestDayNames[d]);
                texts[i].SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
                texts[i].SetValue(Grid.RowProperty, 0);
                texts[i].SetValue(Grid.ColumnProperty, i);
            }
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
    ///     Click on Month Header button. Calendar switch from month mode to year mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnHeaderMonthButtonClick(object sender, RoutedEventArgs e)
    {
        _headerButton?.SetValue(ContentControl.ContentProperty, ContextDate.Year);
        SetCurrentValue(ModeProperty, CalendarViewMode.Year);
    }

    /// <summary>
    ///     Click on Year Header button. Calendar switch from month mode to decade mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnHeaderYearButtonClick(object sender, RoutedEventArgs e)
    {
        if (_yearGrid is null) return;
        var decadeStart = ContextDate.Year / 10 * 10;
        _headerButton?.SetValue(ContentControl.ContentProperty, decadeStart + "-" + (decadeStart + 10));
        SetCurrentValue(ModeProperty, CalendarViewMode.Decade);
    }

    /// <summary>
    ///     Click on CalendarYearButton in YearView.
    ///     Mode switch rules are:
    ///     1. Month -> Not supported, buttons are hidden.
    ///     2. Year -> Month: Set the date to the selected year and switch to Month mode.
    ///     3. Decade -> Year: Set the date to the selected year and switch to Year mode.
    ///     4. Century -> Decade: Set the date to the selected year and switch to Decade mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnYearItemSelected(object sender, CalendarYearButtonEventArgs e)
    {
        if (_yearGrid is null) return;
        var buttons = _yearGrid.Children.OfType<CalendarYearButton>().ToList();
        if (Mode == CalendarViewMode.Year)
        {
            Mode = CalendarViewMode.Month;
            if (e.Month is null) return;
            var day = MathHelpers.SafeClamp(e.Month.Value, 0,
                DateTime.DaysInMonth(ContextDate.Year!.Value, e.Month.Value + 1));
            ContextDate = new DateContext { Year = ContextDate.Year, Month = e.Month + 1, Day = day };
        }
        else if (Mode == CalendarViewMode.Decade)
        {
            Mode = CalendarViewMode.Year;
            for (var i = 0; i < 12; i++) buttons[i].SetValues(CalendarViewMode.Year, i);
            ContextDate = new DateContext { Year = e.Year!.Value, Month = null, Day = null };
        }
        else if (Mode == CalendarViewMode.Century)
        {
            Mode = CalendarViewMode.Decade;
            for (var i = 0; i < 12; i++)
            {
                if (e.StartYear is null || e.EndYear is null) continue;
                var year = e.StartYear.Value - 1 + i;
                buttons[i].SetValues(CalendarViewMode.Decade, year: year);
            }
        }

        RefreshYearButtons();
    }
}