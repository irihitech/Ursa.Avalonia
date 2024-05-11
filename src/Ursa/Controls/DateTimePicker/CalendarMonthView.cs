using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Irihi.Avalonia.Shared;

namespace Ursa.Controls;

/// <summary>
/// Show days in a month. 
/// </summary>
[TemplatePart(PART_Grid, typeof(Grid))]
public class CalendarMonthView: TemplatedControl
{
    public const string PART_Grid = "PART_Grid";
    internal Calendar? Owner { get; set; }
    
    private Grid? _grid;
    private readonly System.Globalization.Calendar _calendar = new GregorianCalendar();
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _grid = e.NameScope.Find<Grid>(PART_Grid);
        GenerateGridElements();
        SetDayButtons(DateTime.Today);
    }

    private DateTime _contextDate = DateTime.Today;
    /// <summary>
    /// The DateTime used to generate the month view. This date will be within the month.
    /// </summary>
    public DateTime ContextDate
    {
        get => _contextDate;
        set
        {
            _contextDate = value;
            // GenerateGridElements();
        }
    }

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty = AvaloniaProperty.Register<CalendarMonthView, DayOfWeek>(
        nameof(FirstDayOfWeek));

    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }
    
    static CalendarMonthView()
    {
        FirstDayOfWeekProperty.Changed.AddClassHandler<CalendarMonthView, DayOfWeek>((view, args) => view.OnDayOfWeekChanged(args));
    }

    private void OnDayOfWeekChanged(AvaloniaPropertyChangedEventArgs<DayOfWeek> args)
    {
        // throw new NotImplementedException();
    }
    

    private void GenerateGridElements()
    {
        // Generate Day titles (Sun, Mon, Tue, Wed, Thu, Fri, Sat) based on FirstDayOfWeek and culture.
        int count = 7 + 7 * 7;
        var children = new List<Control>(count);
        int dayOfWeek = (int)FirstDayOfWeek;
        var info = DateTimeHelper.GetCurrentDateTimeFormatInfo();
        for (int i = 0; i < 7; i++)
        {
            int d = ((dayOfWeek + i) % DateTimeHelper.NumberOfDaysPerWeek);
            var cell = new TextBlock(){ Text = info.ShortestDayNames[d] };
            cell.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
            cell.SetValue(Grid.RowProperty, 0);
            cell.SetValue(Grid.ColumnProperty, i);
            children.Add(cell);
        }
        
        // Generate day buttons.
        for (int i = 2; i < DateTimeHelper.NumberOfWeeksPerMonth+2; i++)
        {
            for (int j = 0; j < DateTimeHelper.NumberOfDaysPerWeek; j++)
            {
                var cell = new CalendarDayButton();
                cell.SetValue(Grid.RowProperty, i);
                cell.SetValue(Grid.ColumnProperty, j);
                cell.PointerPressed += OnDayButtonPressed;
                cell.PointerEntered += OnDayButtonPointerEnter;
                children.Add(cell);
            }
        }
        
        _grid?.Children.AddRange(children);
    }
    
    private void SetDayButtons(DateTime date)
    {
        if (_grid is null) return;
        var children = _grid.Children;
        var info = DateTimeHelper.GetCurrentDateTimeFormatInfo();
        int dayBefore = PreviousMonthDays(date);
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
    
    private void OnDayButtonPressed(object sender, PointerPressedEventArgs e)
    {
        if (sender is CalendarDayButton { DataContext: DateTime d })
        {
            OnCalendarDayButtonPressed?.Invoke(this, new CalendarDayButtonEventArgs(d));
        }
    }
    
    private void OnDayButtonPointerEnter(object sender, PointerEventArgs e)
    {
        if(sender is CalendarDayButton {DataContext: DateTime d})
        {
            OnCalendarDayButtonPointerEnter?.Invoke(this, new CalendarDayButtonEventArgs(d));
        }
    }
    
    private int PreviousMonthDays(DateTime date)
    {
        var firstDay = date.GetFirstDayOfMonth();
        var dayOfWeek = _calendar.GetDayOfWeek(firstDay);
        var firstDayOfWeek = this.FirstDayOfWeek;
        int i = (dayOfWeek - firstDayOfWeek + DateTimeHelper.NumberOfDaysPerWeek) % DateTimeHelper.NumberOfDaysPerWeek;
        return i == 0 ? DateTimeHelper.NumberOfDaysPerWeek : i;
    }
    
    /// <summary>
    /// Make days out of current month fade out. These buttons are not disabled. They are just visually faded out. 
    /// </summary>
    private void FadeOutDayButtons()
    {
        if (_grid is null) return;
        var children = _grid.Children;
        for (var i = 8; i < children.Count; i++)
        {
            if (children[i] is CalendarDayButton { DataContext: DateTime d } button && d.Month != _contextDate.Month)
            {
                button.IsNotCurrentMonth = true;
            }
        }
    }


    public event EventHandler<CalendarDayButtonEventArgs>? OnCalendarDayButtonPressed;
    public event EventHandler<CalendarDayButtonEventArgs>? OnCalendarDayButtonPointerEnter;

    public void MarkSelection(DateTime start, DateTime end)
    {
        if(_grid?.Children is null) return;
        foreach (var child in _grid.Children)
        {
            if (child is CalendarDayButton { DataContext: DateTime d } button)
            {
                if (d == start)
                {
                    button.IsStartDate = true;
                }
                else if (d == end)
                {
                    button.IsEndDate = true;
                }
                else if (d > start && d < end)
                {
                    button.IsInRange = true;
                }
            }
        }
    }

}