using Avalonia.Interactivity;

namespace Ursa.Controls;

public class CalendarYearButtonEventArgs: RoutedEventArgs
{
    internal int? Year { get; }
    internal int? Month { get; }
    internal int? StartYear { get; }
    internal int? EndYear { get; }
    internal CalendarYearViewMode Mode { get; }
    internal CalendarYearButtonEventArgs( CalendarYearViewMode mode, int? year, int? month, int? startYear, int? endYear )
    {
        Year = year;
        Month = month;
        StartYear = startYear;
        EndYear = endYear;
        Mode = mode;
    }
}