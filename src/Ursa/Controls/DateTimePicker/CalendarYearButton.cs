using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
namespace Ursa.Controls;

internal enum CalendarYearViewMode
{
    Month,
    Year,
    // The button represents ten year, with one year before and one year after, 12 in total. 
    YearRange,
} 
public class CalendarYearButton: ContentControl
{
    static CalendarYearButton()
    {
        PressedMixin.Attach<CalendarYearButton>();
    }
    
    internal int Year { get; private set; }
    
    internal int Month { get; private set; }
    
    internal int StartYear { get; private set; }
    
    internal int EndYear { get; private set; }
    
    internal CalendarYearViewMode Mode { get; private set; }

    internal void SetValues(CalendarYearViewMode mode, DateTime contextDate, int? month = null, int? year = null, int? startYear = null, int? endYear = null)
    {
        Debug.Assert(!(month is null && year is null && startYear is null && endYear is null));
        Mode = mode;
        Month = month ?? 0;
        Year = year ?? 0;
        StartYear = startYear ?? 0;
        EndYear = endYear ?? 0;
        Content = Mode switch
        {
            CalendarYearViewMode.Month => DateTimeHelper.GetCurrentDateTimeFormatInfo().AbbreviatedMonthNames[Month],
            CalendarYearViewMode.Year => Year.ToString(),
            CalendarYearViewMode.YearRange => StartYear + "-" + EndYear,
            _ => Content
        };
    }
}