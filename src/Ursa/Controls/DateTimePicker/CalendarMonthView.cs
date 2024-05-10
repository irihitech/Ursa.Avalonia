using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

/// <summary>
/// Show days in a month. 
/// </summary>
[TemplatePart(PART_Grid, typeof(Grid))]
public class CalendarMonthView: TemplatedControl
{
    public const string PART_Grid = "PART_Grid";
    
    private Grid? _grid;
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _grid = e.NameScope.Find<Grid>(PART_Grid);
        // GenerateGridElements();
    }
    
    private int _month;
    public int Month
    {
        get => _month;
        set
        {
            _month = value;
            // Update();
        }
    }

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty = AvaloniaProperty.Register<CalendarMonthView, DayOfWeek>(
        nameof(FirstDayOfWeek));

    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    private void GenerateGridElements()
    {
        // Generate Day titles (Sun, Mon, Tue, Wed, Thu, Fri, Sat) based on FirstDayOfWeek and culture.
        
    }
}