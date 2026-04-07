using System.Diagnostics;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
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
public class DatePickerCalendarView : TemplatedControl
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

    internal static readonly DirectProperty<DatePickerCalendarView, DatePickerCalendarViewMode> ModeProperty =
        AvaloniaProperty.RegisterDirect<DatePickerCalendarView, DatePickerCalendarViewMode>(
            nameof(Mode), o => o.Mode, (o, v) => o.Mode = v);

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty =
        DatePickerBase.IsTodayHighlightedProperty.AddOwner<DatePickerCalendarView>();

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        DatePickerBase.FirstDayOfWeekProperty.AddOwner<DatePickerCalendarView>();

    private readonly Calendar _calendar = new GregorianCalendar();
    private Button? _fastNextButton;

    private Button? _fastPreviousButton;

    private Button? _headerButton;

    private DatePickerCalendarViewMode _mode;
    private Button? _monthButton;
    private Grid? _monthGrid;
    private Button? _nextButton;
    private Button? _previousButton;
    private Button? _yearButton;
    private Grid? _yearGrid;

    private DateTime? _start;
    private DateTime? _end;
    private DateTime? _previewStart;
    private DateTime? _previewEnd;

    static DatePickerCalendarView()
    {
        FirstDayOfWeekProperty.Changed.AddClassHandler<DatePickerCalendarView, DayOfWeek>((view, args) =>
            view.OnFirstDayOfWeekChanged(args));
        ModeProperty.Changed.AddClassHandler<DatePickerCalendarView, DatePickerCalendarViewMode>((view, args) =>
        {
            view.PseudoClasses.Set(PC_Month, args.NewValue.Value == DatePickerCalendarViewMode.Month);
        });
        ContextDateProperty.Changed.AddClassHandler<DatePickerCalendarView, DatePickerCalendarContext>((view, args) =>
            view.OnContextDateChanged(args));
    }

    private void OnContextDateChanged(AvaloniaPropertyChangedEventArgs<DatePickerCalendarContext> args)
    {
        if (!_dateContextSyncing)
        {
            ContextDateChanged?.Invoke(this, args.NewValue.Value);
        }
    }

    internal DatePickerCalendarViewMode Mode
    {
        get => _mode;
        set => SetAndRaise(ModeProperty, ref _mode, value);
    }

    private DatePickerCalendarContext _contextDate = new();

    internal static readonly DirectProperty<DatePickerCalendarView, DatePickerCalendarContext> ContextDateProperty = AvaloniaProperty.RegisterDirect<DatePickerCalendarView, DatePickerCalendarContext>(
        nameof(ContextDate), o => o.ContextDate, (o, v) => o.ContextDate = v);

    internal DatePickerCalendarContext ContextDate
    {
        get => _contextDate;
        set => SetAndRaise(ContextDateProperty, ref _contextDate, value);
    }

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
    
    public static readonly RoutedEvent<DatePickerCalendarDayButtonEventArgs> DateSelectedEvent =
        RoutedEvent.Register<DatePickerCalendarView, DatePickerCalendarDayButtonEventArgs>(
            nameof(DateSelected), RoutingStrategies.Bubble);
    
    public event EventHandler<DatePickerCalendarDayButtonEventArgs> DateSelected
    {
        add => AddHandler(DateSelectedEvent, value);
        remove => RemoveHandler(DateSelectedEvent, value);
    }
    
    public static readonly RoutedEvent<DatePickerCalendarDayButtonEventArgs> DatePreviewedEvent =
        RoutedEvent.Register<DatePickerCalendarView, DatePickerCalendarDayButtonEventArgs>(
            nameof(DatePreviewed), RoutingStrategies.Bubble);
    
    public event EventHandler<DatePickerCalendarDayButtonEventArgs>? DatePreviewed
    {
        add => AddHandler(DatePreviewedEvent, value);
        remove => RemoveHandler(DatePreviewedEvent, value);
    }
    
    internal event EventHandler<DatePickerCalendarContext>? ContextDateChanged; 

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

        ContextDate = new DatePickerCalendarContext(DateTime.Today.Year, DateTime.Today.Month);
        PseudoClasses.Set(PC_Month, Mode == DatePickerCalendarViewMode.Month);
        InitializeGridButtons();
        UpdateDayButtons();
        UpdateYearButtons();
    }

    private void OnFastNext(object? sender, RoutedEventArgs e)
    {
        if (Mode == DatePickerCalendarViewMode.Month)
        {
            ContextDate = ContextDate.With(year: ContextDate.Year + 1);
            UpdateDayButtons();
        }
    }

    private void OnNext(object? sender, RoutedEventArgs e)
    {
        if (Mode == DatePickerCalendarViewMode.Month)
        {
            ContextDate = ContextDate.NextMonth();
            UpdateDayButtons();
        }
        else if (Mode == DatePickerCalendarViewMode.Year)
        {
            ContextDate = ContextDate.NextYear();
            UpdateYearButtons();
        }
        else if (Mode == DatePickerCalendarViewMode.Decade)
        {
            ContextDate = ContextDate.With(startYear: ContextDate.StartYear + 10, endYear: ContextDate.EndYear + 10);
            UpdateYearButtons();
        }
        else if (Mode == DatePickerCalendarViewMode.Century)
        {
            ContextDate = ContextDate.With(startYear: ContextDate.StartYear + 100, endYear: ContextDate.EndYear + 100);
            UpdateYearButtons();
        }
    }

    private void OnPrevious(object? sender, RoutedEventArgs e)
    {
        if (Mode == DatePickerCalendarViewMode.Month)
        {
            ContextDate = ContextDate.PreviousMonth();
            UpdateDayButtons();
        }
        else if (Mode == DatePickerCalendarViewMode.Year)
        {
            ContextDate = ContextDate.With(year: ContextDate.Year - 1);
            UpdateYearButtons();
        }
        else if (Mode == DatePickerCalendarViewMode.Decade)
        {
            ContextDate = ContextDate.With(startYear: ContextDate.StartYear - 10, endYear: ContextDate.EndYear - 10);
            UpdateYearButtons();
        }
        else if (Mode == DatePickerCalendarViewMode.Century)
        {
            ContextDate = ContextDate.With(startYear: ContextDate.StartYear - 100, endYear: ContextDate.EndYear - 100);
            UpdateYearButtons();
        }
    }

    private void OnFastPrevious(object? sender, RoutedEventArgs e)
    {
        if (Mode == DatePickerCalendarViewMode.Month)
        {
            ContextDate = ContextDate.PreviousYear();
            UpdateDayButtons();
        }
    }

    /// <summary>
    ///     Rule:
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnHeaderButtonClick(object? sender, RoutedEventArgs e)
    {
        // Header button should be hidden in Month mode.
        if (Mode == DatePickerCalendarViewMode.Month) return;
        if (Mode == DatePickerCalendarViewMode.Year)
        {
            Mode = DatePickerCalendarViewMode.Decade;
            var range = DateTimeHelper.GetDecadeViewRangeByYear(ContextDate.Year!.Value);
            _dateContextSyncing = true;
            ContextDate =  ContextDate.With(startYear: range.start, endYear: range.end);
            _dateContextSyncing = false;
            UpdateYearButtons();
            return;
        }

        if (Mode == DatePickerCalendarViewMode.Decade)
        {
            Mode = DatePickerCalendarViewMode.Century;
            var range = DateTimeHelper.GetCenturyViewRangeByYear(ContextDate.StartYear!.Value);
            _dateContextSyncing = true;
            ContextDate = ContextDate.With(startYear: range.start, endYear: range.end);
            _dateContextSyncing = false;
            UpdateYearButtons();
        }
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
            var cell = new DatePickerCalendarDayButton();
            cell.SetValue(Grid.RowProperty, i);
            cell.SetValue(Grid.ColumnProperty, j);
            cell.AddHandler(DatePickerCalendarDayButton.DateSelectedEvent, OnCellDateSelected);
            cell.AddHandler(DatePickerCalendarDayButton.DatePreviewedEvent, OnCellDatePreviewed);
            children.Add(cell);
        }

        _monthGrid?.Children.AddRange(children);

        // Generate month/year buttons. 
        for (var i = 0; i < 12; i++)
        {
            var button = new DatePickerCalendarYearButton();
            Grid.SetRow(button, i / 3);
            Grid.SetColumn(button, i % 3);
            button.AddHandler(DatePickerCalendarYearButton.ItemSelectedEvent, OnYearItemSelected);
            _yearGrid?.Children.Add(button);
        }
    }

    internal void UpdateDayButtons()
    {
        if (_monthGrid is null || Mode != DatePickerCalendarViewMode.Month) return;
        var children = _monthGrid.Children;
        var info = DateTimeHelper.GetCurrentDateTimeFormatInfo();
        var date = new DateTime(ContextDate.Year ?? ContextDate.StartYear!.Value, ContextDate.Month!.Value, 1);
        var dayBefore = PreviousMonthDays(date);
        var dateToSet = date.GetFirstDayOfMonth().AddDays(-dayBefore);
        for (var i = 7; i < children.Count; i++)
        {
            var day = dateToSet;
            var cell = children[i] as DatePickerCalendarDayButton;
            if (cell is null) continue;
            cell.DataContext = day;
            if (IsTodayHighlighted) cell.IsToday = day == DateTime.Today;
            cell.Content = day.Day.ToString(info);
            dateToSet = dateToSet.AddDays(1);
        }

        FadeOutDayButtons();
        MarkDates(_start, _end, _previewStart, _previewEnd);
        UpdateHeaderButtons();
    }

    private void UpdateYearButtons()
    {
        if (_yearGrid is null) return;
        var mode = Mode;
        var contextDate = ContextDate;
        if (mode == DatePickerCalendarViewMode.Century && contextDate.StartYear.HasValue)
        {
            var range = DateTimeHelper.GetCenturyViewRangeByYear(contextDate.StartYear.Value);
            var start = range.start - 10;
            for (var i = 0; i < 12; i++)
            {
                var child = _yearGrid.Children[i] as DatePickerCalendarYearButton;
                child?.SetContext(DatePickerCalendarViewMode.Century,
                    new DatePickerCalendarContext(startYear: start, endYear: start + 10));
                start += 10;
            }
        }
        else if (mode == DatePickerCalendarViewMode.Decade && contextDate.StartYear.HasValue)
        {
            var range = DateTimeHelper.GetDecadeViewRangeByYear(contextDate.StartYear.Value);
            var year = range.start - 1;
            for (var i = 0; i < 12; i++)
            {
                var child = _yearGrid.Children[i] as DatePickerCalendarYearButton;
                child?.SetContext(DatePickerCalendarViewMode.Decade,
                    new DatePickerCalendarContext(year: year));
                year++;
            }
        }
        else if (mode == DatePickerCalendarViewMode.Year)
        {
            for (var i = 0; i < 12; i++)
            {
                var child = _yearGrid.Children[i] as DatePickerCalendarYearButton;
                child?.SetContext(DatePickerCalendarViewMode.Year, new DatePickerCalendarContext(month: i + 1));
            }
        }
        UpdateHeaderButtons();
    }

    private void FadeOutDayButtons()
    {
        if (_monthGrid is null) return;
        var children = _monthGrid.Children;
        for (var i = 7; i < children.Count; i++)
            if (children[i] is DatePickerCalendarDayButton { DataContext: DateTime d } button)
                button.IsNotCurrentMonth = d.Month != ContextDate.Month;
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

    private void OnCellDatePreviewed(object? sender, DatePickerCalendarDayButtonEventArgs e)
    {
        RaiseEvent(new DatePickerCalendarDayButtonEventArgs(e.Date) { RoutedEvent = DatePreviewedEvent, Source = this });
    }

    private void OnCellDateSelected(object? sender, DatePickerCalendarDayButtonEventArgs e)
    {
        if (e.Date.HasValue && e.Date.Value.Month != ContextDate.Month)
        {
            ContextDate = ContextDate.With(year: e.Date.Value.Year, month: e.Date.Value.Month);
            UpdateDayButtons();
        }
        RaiseEvent(new DatePickerCalendarDayButtonEventArgs(e.Date) { RoutedEvent = DateSelectedEvent, Source = this });
    }

    /// <summary>
    ///     Click on Month Header button. Calendar switch from month mode to year mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnHeaderMonthButtonClick(object? sender, RoutedEventArgs e)
    {
        SetCurrentValue(ModeProperty, DatePickerCalendarViewMode.Year);
        UpdateYearButtons();
    }

    /// <summary>
    ///     Click on Year Header button. Calendar switch from month mode to decade mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnHeaderYearButtonClick(object? sender, RoutedEventArgs e)
    {
        if (_yearGrid is null) return;
        SetCurrentValue(ModeProperty, DatePickerCalendarViewMode.Decade);
        var range = DateTimeHelper.GetDecadeViewRangeByYear(ContextDate.Year!.Value);
        _dateContextSyncing = true;
        ContextDate = ContextDate.With(startYear: range.start, endYear: range.end);
        _dateContextSyncing = false;
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
    private void OnYearItemSelected(object? sender, DatePickerCalendarYearButtonEventArgs e)
    {
        if (_yearGrid is null) return;
        if (Mode == DatePickerCalendarViewMode.Century)
        {
            Mode = DatePickerCalendarViewMode.Decade;
            ContextDate = e.Context.With(year: null);
        }
        else if (Mode == DatePickerCalendarViewMode.Decade)
        {
            Mode = DatePickerCalendarViewMode.Year;
            ContextDate = e.Context.Clone();
        }
        else if (Mode == DatePickerCalendarViewMode.Year)
        {
            Mode = DatePickerCalendarViewMode.Month;
            ContextDate = ContextDate.With(null, e.Context.Month);
            UpdateDayButtons();
        }
        else if (Mode == DatePickerCalendarViewMode.Month)
        {
            return;
        }
        UpdateHeaderButtons();
        UpdateYearButtons();
    }

    private void UpdateHeaderButtons()
    {
        if (Mode == DatePickerCalendarViewMode.Century)
        {
            IsVisibleProperty.SetValue(true, _headerButton, _yearGrid);
            IsVisibleProperty.SetValue(false, _yearButton, _monthButton, _monthGrid, _fastPreviousButton,
                _fastNextButton);
            _headerButton?.SetValue(ContentControl.ContentProperty,
                ContextDate.StartYear + "-" + ContextDate.EndYear);
        }
        else if (Mode == DatePickerCalendarViewMode.Decade)
        {
            IsVisibleProperty.SetValue(true, _headerButton, _yearGrid);
            IsVisibleProperty.SetValue(false, _yearButton, _monthButton, _monthGrid, _fastPreviousButton,
                _fastNextButton);
            _headerButton?.SetValue(ContentControl.ContentProperty,
                ContextDate.StartYear + "-" + ContextDate.EndYear);
        }
        else if (Mode == DatePickerCalendarViewMode.Year)
        {
            IsVisibleProperty.SetValue(true, _headerButton, _yearGrid);
            IsVisibleProperty.SetValue(false, _yearButton, _monthButton, _monthGrid, _fastPreviousButton,
                _fastNextButton);
            _headerButton?.SetValue(ContentControl.ContentProperty, ContextDate.Year);
        }
        else if (Mode == DatePickerCalendarViewMode.Month)
        {
            IsVisibleProperty.SetValue(false, _headerButton, _yearGrid);
            IsVisibleProperty.SetValue(true, _yearButton, _monthButton, _monthGrid, _fastPreviousButton,
                _fastNextButton);
            // _headerButton?.SetValue(ContentControl.ContentProperty, ContextCalendar.Year);
            _yearButton?.SetValue(ContentControl.ContentProperty, ContextDate.Year);
            _monthButton?.SetValue(ContentControl.ContentProperty,
                DateTimeHelper.GetCurrentDateTimeFormatInfo().AbbreviatedMonthNames[ContextDate.Month-1 ?? 0]);
        }

        bool canForward = !(ContextDate.EndYear <= 0) && !(ContextDate.Year <= 0);
        bool canNext = !(ContextDate.StartYear > 9999) && !(ContextDate.EndYear > 9999);
        IsEnabledProperty.SetValue(canForward, _previousButton, _fastPreviousButton);
        IsEnabledProperty.SetValue(canNext, _nextButton, _fastNextButton);
    }
    
    internal void MarkDates(DateTime? startDate = null, DateTime? endDate = null, DateTime? previewStartDate = null, DateTime? previewEndDate = null)
    {
        _start = startDate;
        _end = endDate;
        _previewStart = previewStartDate;
        _previewEnd = previewEndDate;
        if (_monthGrid?.Children is null) return;
        DateTime start = startDate ?? DateTime.MaxValue;
        DateTime end = endDate ?? DateTime.MinValue;
        DateTime previewStart = previewStartDate ?? DateTime.MaxValue;
        DateTime previewEnd = previewEndDate ?? DateTime.MinValue;
        DateTime rangeStart = DateTimeHelper.Min(start, previewStart);
        DateTime rangeEnd = DateTimeHelper.Max(end, previewEnd);
        foreach (var child in _monthGrid.Children)
        {
            if (child is not DatePickerCalendarDayButton { DataContext: DateTime d } button) continue;
            button.ResetSelection();
            if(d.Month != ContextDate.Month) continue;
            if (d < rangeEnd && d > rangeStart) button.IsInRange = true;
            if (d == previewStart) button.IsPreviewStartDate = true;
            if (d == previewEnd) button.IsPreviewEndDate = true;
            if (d == startDate) button.IsStartDate = true;
            if (d == endDate) button.IsEndDate = true;
            if (d == startDate && d == endDate) button.IsSelected = true;
        }
    }

    public void ClearSelection(bool start = true, bool end = true)
    {
        if (start)
        {
            _previewStart = null;
            _start = null;
        }

        if (end)
        {
            _previewEnd = null;
            _end = null;
        }
        
        if (_monthGrid?.Children is null) return;
        foreach (var child in _monthGrid.Children)
        {
            if (child is not DatePickerCalendarDayButton button) continue;
            if (start)
            {
                button.IsPreviewStartDate = false;
                button.IsStartDate = false;
            }
            if (end)
            {
                button.IsEndDate = false;
                button.IsInRange = false;
            }
            button.IsPreviewEndDate = false;
        }
        UpdateDayButtons();
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        RaiseEvent(new DatePickerCalendarDayButtonEventArgs(null) { RoutedEvent = DatePreviewedEvent, Source = this });
    }

    private bool _dateContextSyncing;
    /// <summary>
    /// Used for syncing the context date for DateRangePicker. mark a flag to avoid infinitely loop. 
    /// </summary>
    /// <param name="context"></param>
    internal void SyncContextDate(DatePickerCalendarContext? context)
    {
        if (context is null) return;
        _dateContextSyncing = true;
        ContextDate = context;
        _dateContextSyncing = false;
        UpdateDayButtons();
        UpdateYearButtons();
    }
}