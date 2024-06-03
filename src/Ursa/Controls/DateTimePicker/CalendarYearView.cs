using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

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
    
    private CalendarYearViewMode _mode = CalendarYearViewMode.Month;

    internal CalendarYearViewMode Mode
    {
        get => _mode;
        set
        {
            _mode = value;
            RefreshButtons();
        }
    }

    private DateTime _contextDate = DateTime.Today;
    internal DateTime ContextDate
    {
        get => _contextDate;
        set
        {
            _contextDate = value;
            RefreshButtons();
        }
    }

    private Grid? _grid;
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _grid = e.NameScope.Find<Grid>(PART_Grid);
        GenerateGridElements();
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
            _grid.Children.Add(button);
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
                case CalendarYearViewMode.Month:
                    child.SetValues(CalendarYearViewMode.Month, contextDate, month: i);
                    break;
                case CalendarYearViewMode.Year:
                    child.SetValues(CalendarYearViewMode.Year, contextDate, year: ContextDate.Year + i);
                    break;
                case CalendarYearViewMode.YearRange:
                    var startYear = ContextDate.Year - 1;
                    var endYear = ContextDate.Year + 10;
                    child.SetValues(CalendarYearViewMode.YearRange, contextDate, startYear: startYear, endYear: endYear);
                    break;
            }
        }
    }
    
}