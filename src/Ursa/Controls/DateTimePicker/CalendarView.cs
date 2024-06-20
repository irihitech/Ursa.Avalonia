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

[TemplatePart(PART_FastNextButton, typeof(Button))]
[TemplatePart(PART_FastPreviousButton, typeof(Button))]
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
    public const string PART_FastNextButton = "PART_FastNextButton";
    public const string PART_FastPreviousButton = "PART_FastPreviousButton";
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
            nameof(Mode), o => o.Mode, (o, v) => o.Mode = v);

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty =
        DatePickerBase.IsTodayHighlightedProperty.AddOwner<CalendarView>();

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        DatePickerBase.FirstDayOfWeekProperty.AddOwner<CalendarView>();

    private readonly Calendar _calendar = new GregorianCalendar();
    private Button? _fastNextButton;

    private Button? _fastPreviousButton;

    private Button? _headerButton;

    private CalendarViewMode _mode;
    private Button? _monthButton;
    private Grid? _monthGrid;
    private Button? _nextButton;
    private Button? _previousButton;
    private Button? _yearButton;
    private Grid? _yearGrid;


    static CalendarView()
    {
        FirstDayOfWeekProperty.Changed.AddClassHandler<CalendarView, DayOfWeek>((view, args) =>
            view.OnFirstDayOfWeekChanged(args));
        ModeProperty.Changed.AddClassHandler<CalendarView, CalendarViewMode>((view, args) =>
        {
            view.PseudoClasses.Set(PC_Month, args.NewValue.Value == CalendarViewMode.Month);
        });
    }

    internal CalendarViewMode Mode
    {
        get => _mode;
        set => SetAndRaise(ModeProperty, ref _mode, value);
    }

    public CalendarContext ContextCalendar { get; set; } = new();

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
        UpdateDayButtons();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        Button.ClickEvent.RemoveHandler(OnHeaderYearButtonClick, _yearButton);
        Button.ClickEvent.RemoveHandler(OnHeaderMonthButtonClick, _monthButton);
        Button.ClickEvent.RemoveHandler(OnHeaderButtonClick, _headerButton);
        Button.ClickEvent.RemoveHandler(OnFastPrevious, _fastPreviousButton);
        Button.ClickEvent.RemoveHandler(OnPrevious, _previousButton);
        Button.ClickEvent.RemoveHandler(OnNext, _nextButton);
        Button.ClickEvent.RemoveHandler(OnFastNext, _fastNextButton);

        _monthGrid = e.NameScope.Find<Grid>(PART_MonthGrid);
        _yearGrid = e.NameScope.Find<Grid>(PART_YearGrid);
        _yearButton = e.NameScope.Find<Button>(PART_YearButton);
        _monthButton = e.NameScope.Find<Button>(PART_MonthButton);
        _headerButton = e.NameScope.Find<Button>(PART_HeaderButton);
        _fastPreviousButton = e.NameScope.Find<Button>(PART_FastPreviousButton);
        _previousButton = e.NameScope.Find<Button>(PART_PreviousButton);
        _nextButton = e.NameScope.Find<Button>(PART_NextButton);
        _fastNextButton = e.NameScope.Find<Button>(PART_FastNextButton);

        Button.ClickEvent.AddHandler(OnHeaderYearButtonClick, _yearButton);
        Button.ClickEvent.AddHandler(OnHeaderMonthButtonClick, _monthButton);
        Button.ClickEvent.AddHandler(OnHeaderButtonClick, _headerButton);
        Button.ClickEvent.AddHandler(OnFastPrevious, _fastPreviousButton);
        Button.ClickEvent.AddHandler(OnPrevious, _previousButton);
        Button.ClickEvent.AddHandler(OnNext, _nextButton);
        Button.ClickEvent.AddHandler(OnFastNext, _fastNextButton);

        ContextCalendar = new CalendarContext(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        PseudoClasses.Set(PC_Month, Mode == CalendarViewMode.Month);
        InitializeGridButtons();
        UpdateDayButtons();
        UpdateYearButtons();
        UpdateHeaderButtons();
    }

    private void OnFastNext(object sender, RoutedEventArgs e)
    {
        if (Mode == CalendarViewMode.Month)
        {
            ContextCalendar.Year += 1;
            UpdateDayButtons();
        }

        UpdateHeaderButtons();
    }

    private void OnNext(object sender, RoutedEventArgs e)
    {
        if (Mode == CalendarViewMode.Month)
        {
            ContextCalendar.Month += 1;
            if (ContextCalendar.Month > 12)
            {
                ContextCalendar.Month = 1;
                ContextCalendar.Year += 1;
            }

            UpdateDayButtons();
        }
        else if (Mode == CalendarViewMode.Year)
        {
            ContextCalendar.Year += 1;
            UpdateYearButtons();
        }
        else if (Mode == CalendarViewMode.Decade)
        {
            ContextCalendar.StartYear += 10;
            ContextCalendar.EndYear += 10;
            UpdateYearButtons();
        }
        else if (Mode == CalendarViewMode.Century)
        {
            ContextCalendar.StartYear += 100;
            ContextCalendar.EndYear += 100;
            UpdateYearButtons();
        }

        UpdateHeaderButtons();
    }

    private void OnPrevious(object sender, RoutedEventArgs e)
    {
        if (Mode == CalendarViewMode.Month)
        {
            ContextCalendar.Month -= 1;
            if (ContextCalendar.Month < 1)
            {
                ContextCalendar.Month = 12;
                ContextCalendar.Year -= 1;
            }

            UpdateDayButtons();
        }
        else if (Mode == CalendarViewMode.Year)
        {
            ContextCalendar.Year -= 1;
            UpdateYearButtons();
        }
        else if (Mode == CalendarViewMode.Decade)
        {
            ContextCalendar.StartYear -= 10;
            ContextCalendar.EndYear -= 10;
            UpdateYearButtons();
        }
        else if (Mode == CalendarViewMode.Century)
        {
            ContextCalendar.StartYear -= 100;
            ContextCalendar.EndYear -= 100;
            UpdateYearButtons();
        }

        UpdateHeaderButtons();
    }

    private void OnFastPrevious(object sender, RoutedEventArgs e)
    {
        if (Mode == CalendarViewMode.Month)
        {
            ContextCalendar.Year -= 1;
            UpdateDayButtons();
        }

        UpdateHeaderButtons();
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
            var range = DateTimeHelper.GetDecadeViewRangeByYear(ContextCalendar.Year!.Value);
            ContextCalendar.StartYear = range.start;
            ContextCalendar.EndYear = range.end;
            UpdateHeaderButtons();
            UpdateYearButtons();
            return;
        }

        if (Mode == CalendarViewMode.Decade)
        {
            Mode = CalendarViewMode.Century;
            var range = DateTimeHelper.GetCenturyViewRangeByYear(ContextCalendar.StartYear!.Value);
            ContextCalendar.StartYear = range.start;
            ContextCalendar.EndYear = range.end;
            UpdateHeaderButtons();
            UpdateYearButtons();
            return;
        }

        if (Mode == CalendarViewMode.Century) return;
    }

    /// <summary>
    ///     Generate Buttons and labels for MonthView.
    ///     Generate Buttons for YearView.
    ///     This method should be called only once.
    /// </summary>
    private void InitializeGridButtons()
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

    private void UpdateDayButtons()
    {
        if (_monthGrid is null || Mode != CalendarViewMode.Month) return;
        var children = _monthGrid.Children;
        var info = DateTimeHelper.GetCurrentDateTimeFormatInfo();
        var date = new DateTime(ContextCalendar.Year.Value, ContextCalendar.Month.Value, ContextCalendar.Day.Value);
        var dayBefore = PreviousMonthDays(date);
        var dateToSet = date.GetFirstDayOfMonth().AddDays(-dayBefore);
        for (var i = 7; i < children.Count; i++)
        {
            var day = dateToSet;
            var cell = children[i] as CalendarDayButton;
            if (cell is null) continue;
            cell.DataContext = day;
            if (IsTodayHighlighted) cell.IsToday = day == DateTime.Today;
            cell.Content = day.Day.ToString(info);
            dateToSet = dateToSet.AddDays(1);
        }

        FadeOutDayButtons();
    }

    private void UpdateYearButtons()
    {
        if (_yearGrid is null) return;
        var mode = Mode;
        var contextDate = ContextCalendar;
        if (mode == CalendarViewMode.Century && contextDate.StartYear.HasValue)
        {
            var range = DateTimeHelper.GetCenturyViewRangeByYear(contextDate.StartYear.Value);
            var start = range.start - 10;
            for (var i = 0; i < 12; i++)
            {
                var child = _yearGrid.Children[i] as CalendarYearButton;
                child?.SetContext(CalendarViewMode.Century,
                    new CalendarContext { StartYear = start, EndYear = start + 10 });
                start += 10;
            }
        }
        else if (mode == CalendarViewMode.Decade && contextDate.StartYear.HasValue)
        {
            var range = DateTimeHelper.GetDecadeViewRangeByYear(contextDate.StartYear.Value);
            var year = range.start - 1;
            for (var i = 0; i < 12; i++)
            {
                var child = _yearGrid.Children[i] as CalendarYearButton;
                child?.SetContext(CalendarViewMode.Decade,
                    new CalendarContext { Year = year });
                year++;
            }
        }
        else if (mode == CalendarViewMode.Year)
        {
            for (var i = 0; i < 12; i++)
            {
                var child = _yearGrid.Children[i] as CalendarYearButton;
                child?.SetContext(CalendarViewMode.Year, new CalendarContext { Month = i });
            }
        }
    }

    private void FadeOutDayButtons()
    {
        if (_monthGrid is null) return;
        var children = _monthGrid.Children;
        for (var i = 7; i < children.Count; i++)
            if (children[i] is CalendarDayButton { DataContext: DateTime d } button)
                button.IsNotCurrentMonth = d.Month != ContextCalendar.Month;
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
        if (e.Date.Month != ContextCalendar.Month)
        {
            ContextCalendar.Month = e.Date.Month;
            UpdateDayButtons();
        }

        OnDateSelected?.Invoke(sender, e);
    }

    /// <summary>
    ///     Click on Month Header button. Calendar switch from month mode to year mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnHeaderMonthButtonClick(object sender, RoutedEventArgs e)
    {
        SetCurrentValue(ModeProperty, CalendarViewMode.Year);
        UpdateHeaderButtons();
        UpdateYearButtons();
    }

    /// <summary>
    ///     Click on Year Header button. Calendar switch from month mode to decade mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnHeaderYearButtonClick(object sender, RoutedEventArgs e)
    {
        if (_yearGrid is null) return;
        SetCurrentValue(ModeProperty, CalendarViewMode.Decade);
        var range = DateTimeHelper.GetDecadeViewRangeByYear(ContextCalendar.Year!.Value);
        ContextCalendar.StartYear = range.start;
        ContextCalendar.EndYear = range.end;
        UpdateHeaderButtons();
        UpdateYearButtons();
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
        if (Mode == CalendarViewMode.Century)
        {
            Mode = CalendarViewMode.Decade;
            ContextCalendar.Year = null;
            ContextCalendar.StartYear = e.Context.StartYear;
            ContextCalendar.EndYear = e.Context.EndYear;
        }
        else if (Mode == CalendarViewMode.Decade)
        {
            Mode = CalendarViewMode.Year;
            ContextCalendar.Year = e.Context.Year;
            ContextCalendar.StartYear = e.Context.StartYear;
            ContextCalendar.EndYear = e.Context.EndYear;
        }
        else if (Mode == CalendarViewMode.Year)
        {
            Mode = CalendarViewMode.Month;
            ContextCalendar.StartYear = null;
            ContextCalendar.EndYear = null;
            // ContextCalendar.Year = e.Context.Year;
            ContextCalendar.Month = e.Context.Month;
            UpdateDayButtons();
        }
        else if (Mode == CalendarViewMode.Month)
        {
            throw new NotImplementedException();
        }

        UpdateHeaderButtons();
        UpdateYearButtons();
    }

    private void UpdateHeaderButtons()
    {
        if (Mode == CalendarViewMode.Century)
        {
            IsVisibleProperty.SetValue(true, _headerButton, _yearGrid);
            IsVisibleProperty.SetValue(false, _yearButton, _monthButton, _monthGrid, _fastPreviousButton,
                _fastNextButton);
            _headerButton?.SetValue(ContentControl.ContentProperty,
                ContextCalendar.StartYear + "-" + ContextCalendar.EndYear);
        }
        else if (Mode == CalendarViewMode.Decade)
        {
            IsVisibleProperty.SetValue(true, _headerButton, _yearGrid);
            IsVisibleProperty.SetValue(false, _yearButton, _monthButton, _monthGrid, _fastPreviousButton,
                _fastNextButton);
            _headerButton?.SetValue(ContentControl.ContentProperty,
                ContextCalendar.StartYear + "-" + ContextCalendar.EndYear);
        }
        else if (Mode == CalendarViewMode.Year)
        {
            IsVisibleProperty.SetValue(true, _headerButton, _yearGrid);
            IsVisibleProperty.SetValue(false, _yearButton, _monthButton, _monthGrid, _fastPreviousButton,
                _fastNextButton);
            _headerButton?.SetValue(ContentControl.ContentProperty, ContextCalendar.Year);
        }
        else if (Mode == CalendarViewMode.Month)
        {
            IsVisibleProperty.SetValue(false, _headerButton, _yearGrid);
            IsVisibleProperty.SetValue(true, _yearButton, _monthButton, _monthGrid, _fastPreviousButton,
                _fastNextButton);
            // _headerButton?.SetValue(ContentControl.ContentProperty, ContextCalendar.Year);
            _yearButton?.SetValue(ContentControl.ContentProperty, ContextCalendar.Year);
            _monthButton?.SetValue(ContentControl.ContentProperty,
                DateTimeHelper.GetCurrentDateTimeFormatInfo().AbbreviatedMonthNames[ContextCalendar.Month-1 ?? 0]);
        }

        bool canForward = !(ContextCalendar.EndYear <= 0) && !(ContextCalendar.Year <= 0);
        bool canNext = !(ContextCalendar.StartYear > 9999) && !(ContextCalendar.EndYear > 9999);
        IsEnabledProperty.SetValue(canForward, _previousButton, _fastPreviousButton);
        IsEnabledProperty.SetValue(canNext, _nextButton, _fastNextButton);
    }
}