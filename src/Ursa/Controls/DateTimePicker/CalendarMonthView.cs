using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;

namespace Ursa.Controls;

/*
/// <summary>
/// Show days in a month. CalendarMonthView itself doesn't handle any date range selection logic.
/// it provides a method to mark preview range and selection range. The range limit may out of current displayed month. 
/// </summary>
[TemplatePart(PART_Grid, typeof(Grid))]
public class CalendarMonthView : TemplatedControl
{
    public const string PART_Grid = "PART_Grid";
    private const string ShortestDayName = "ShortestDayName";

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        DatePickerBase.FirstDayOfWeekProperty.AddOwner<CalendarMonthView>();

    private readonly System.Globalization.Calendar _calendar = new GregorianCalendar();

    private DateTime _contextDate = DateTime.Today;

    private Grid? _grid;

    static CalendarMonthView()
    {
        FirstDayOfWeekProperty.Changed.AddClassHandler<CalendarMonthView, DayOfWeek>((view, args) =>
            view.OnDayOfWeekChanged(args));
    }

    internal CalendarView? Owner { get; set; }

    /// <summary>
    ///     The DateTime used to generate the month view. This date will be within the month.
    /// </summary>
    public DateTime ContextDate
    {
        get => _contextDate;
        set => _contextDate = value;
        // GenerateGridElements();
    }

    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _grid = e.NameScope.Find<Grid>(PART_Grid);
        GenerateGridElements();
        SetDayButtons(ContextDate);
    }

    private void OnDayOfWeekChanged(AvaloniaPropertyChangedEventArgs<DayOfWeek> args)
    {
        // throw new NotImplementedException();
        UpdateGridElements();
        SetDayButtons(ContextDate);
    }


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

        _grid?.Children.AddRange(children);
    }

    private void UpdateGridElements()
    {
        var count = 7 + 7 * 7;
        // var children = new List<Control>(count);
        var dayOfWeek = (int)FirstDayOfWeek;
        var info = DateTimeHelper.GetCurrentDateTimeFormatInfo();
        var textblocks = _grid?.Children.Where(a => a is TextBlock { Tag: ShortestDayName }).ToList();
        if (textblocks is not null)
        {
            for (var i = 0; i < 7; i++)
            {
                var d = (dayOfWeek + i) % DateTimeHelper.NumberOfDaysPerWeek;
                textblocks[i].SetValue(TextBlock.TextProperty, info.ShortestDayNames[d]);
                textblocks[i].SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
                textblocks[i].SetValue(Grid.RowProperty, 0);
                textblocks[i].SetValue(Grid.ColumnProperty, i);
            }
        }
        SetDayButtons(ContextDate);
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
    /// Set day buttons according to context date. 
    /// </summary>
    /// <param name="date"></param>
    private void SetDayButtons(DateTime date)
    {
        if (_grid is null) return;
        var children = _grid.Children;
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

    private int PreviousMonthDays(DateTime date)
    {
        var firstDay = date.GetFirstDayOfMonth();
        var dayOfWeek = _calendar.GetDayOfWeek(firstDay);
        var firstDayOfWeek = FirstDayOfWeek;
        var i = (dayOfWeek - firstDayOfWeek + DateTimeHelper.NumberOfDaysPerWeek) % DateTimeHelper.NumberOfDaysPerWeek;
        return i == 0 ? DateTimeHelper.NumberOfDaysPerWeek : i;
    }

    /// <summary>
    ///     Make days out of current month fade out. These buttons are not disabled. They are just visually faded out.
    /// </summary>
    private void FadeOutDayButtons()
    {
        if (_grid is null) return;
        var children = _grid.Children;
        for (var i = 8; i < children.Count; i++)
            if (children[i] is CalendarDayButton { DataContext: DateTime d } button && d.Month != _contextDate.Month)
                button.IsNotCurrentMonth = true;
    }
    
    public event EventHandler<CalendarDayButtonEventArgs>? OnDateSelected;
    public event EventHandler<CalendarDayButtonEventArgs>? OnDatePreviewed;

    public void MarkDates(DateTime? startDate = null, DateTime? endDate = null, DateTime? previewStartDate = null, DateTime? previewEndDate = null)
    {
        if (_grid?.Children is null) return;
        DateTime start = startDate ?? DateTime.MaxValue;
        DateTime end = endDate ?? DateTime.MinValue;
        DateTime previewStart = previewStartDate ?? DateTime.MaxValue;
        DateTime previewEnd = previewEndDate ?? DateTime.MinValue;
        DateTime rangeStart = DateTimeHelper.Min(start, previewStart);
        DateTime rangeEnd = DateTimeHelper.Max(end, previewEnd);
        foreach (var child in _grid.Children)
        {
            if (child is not CalendarDayButton { DataContext: DateTime d } button) continue;
            if(d.Month != _contextDate.Month) continue;
            button.ResetSelection();
            if (d < rangeEnd && d > rangeStart) button.IsInRange = true;
            if (d == previewStart) button.IsPreviewStartDate = true;
            if (d == previewEnd) button.IsPreviewEndDate = true;
            if (d == startDate) button.IsStartDate = true;
            if (d == endDate) button.IsEndDate = true;
            if (d == startDate && d == endDate) button.IsSelected = true;
        }
    }
    
    public void ClearSelection()
    {
        if (_grid?.Children is null) return;
        foreach (var child in _grid.Children)
        {
            if (child is not CalendarDayButton button) continue;
            button.IsStartDate = false;
            button.IsEndDate = false;
            button.IsInRange = false;
        }
    }

    public void ClearPreview()
    {
        if (_grid?.Children is null) return;
        foreach (var child in _grid.Children)
        {
            if (child is not CalendarDayButton button) continue;
            button.IsPreviewStartDate = false;
            button.IsPreviewEndDate = false;
            button.IsInRange = false;
        }
    }
}

*/