using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

/*
/// <summary>
/// Three modes:
/// 1. show 12 months in a year
/// 2. show 12 years, one year per button (but only 10 buttons clickable)
/// 3. show 120 years, ten year per button (but only 10 buttons clickable)
/// </summary>
[TemplatePart(PART_Grid, typeof(Grid))]
public class CalendarYearView: TemplatedControl
{
    public const string PART_Grid = "PART_Grid";
    private readonly CalendarYearButton[] _buttons = new CalendarYearButton[12];

    public event EventHandler<CalendarYearButtonEventArgs>? OnMonthSelected; 

    internal CalendarViewMode Mode { get; set; } = CalendarViewMode.Month;
    internal DateTime ContextDate { get; set; } = DateTime.Today;

    private Grid? _grid;
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        ContextDate = DateTime.Today;
        base.OnApplyTemplate(e);
        _grid = e.NameScope.Find<Grid>(PART_Grid);
        GenerateGridElements();
        RefreshButtons();
    }
    
    public void GenerateGridElements()
    {
        if (_grid is null)
        {
            return;
        }
        _grid.Children.Clear();
        for (var i = 0; i < 12; i++)
        {
            var button = new CalendarYearButton();
            Grid.SetRow(button, i / 3);
            Grid.SetColumn(button, i % 3);
            button.AddHandler(CalendarYearButton.ItemSelectedEvent, OnItemSelected);
            _grid.Children.Add(button);
            _buttons[i] = button;
        }
    }
    
    private void OnItemSelected(object sender, CalendarYearButtonEventArgs e)
    {
        if (_grid is null) return;
        var buttons = _grid.Children.OfType<CalendarYearButton>().ToList();
        if (e.Mode == CalendarViewMode.Month)
        {
            if (e.Month is null) return;
            var day = MathHelpers.SafeClamp(e.Month.Value, 0, DateTime.DaysInMonth(ContextDate.Year, e.Month.Value+1));
            ContextDate = new DateTime(ContextDate.Year, e.Month.Value+1, day+1);
            OnMonthSelected?.Invoke(this, e);
        }
        else if (e.Mode == CalendarViewMode.Year)
        {
            // Set CalendarYearView to Month mode
            for (var i = 0; i < 12; i++)
            {
                buttons[i].SetValues(CalendarViewMode.Month, month: i);
            }
            ContextDate = new DateTime(e.Year!.Value, ContextDate.Month, 1);
            Mode = CalendarViewMode.Month;
        }
        else if (e.Mode == CalendarViewMode.Decade)
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

    private void RefreshButtons()
    {
        if (_grid is null) return;
        var mode = Mode;
        var contextDate = ContextDate;
        for (var i = 0; i < 12; i++)
        {
            var child = _grid.Children[i] as CalendarYearButton;
            if (child is null) continue;
            switch (mode)
            {
                case CalendarViewMode.Month:
                    child.SetValues(CalendarViewMode.Month, month: i);
                    break;
                case CalendarViewMode.Year:
                    child.SetValues(CalendarViewMode.Year, year: ContextDate.Year / 10 * 10 + i - 1);
                    break;
                case CalendarViewMode.Decade:
                    var startYear = (ContextDate.Year / 10 + i - 1) * 10;
                    var endYear = (ContextDate.Year / 10 + i - 1) * 10 + 10;
                    child.SetValues(CalendarViewMode.Decade, startYear: startYear, endYear: endYear);
                    break;
            }
        }
    }

    internal void UpdateMode(CalendarViewMode mode)
    {
        Mode = mode;
        RefreshButtons();
    }
}
*/