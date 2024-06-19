using Avalonia.Interactivity;

namespace Ursa.Controls;

public class CalendarYearButtonEventArgs: RoutedEventArgs
{
    public int? Year { get; }
    public int? Month { get; }
    public int? StartYear { get; }
    public int? EndYear { get; }
    internal CalendarViewMode Mode { get; }

    /// <inheritdoc />
    internal CalendarYearButtonEventArgs(CalendarViewMode mode, int? year, int? month, int? startYear, int? endYear )
    {
        Year = year;
        Month = month;
        StartYear = startYear;
        EndYear = endYear;
        Mode = mode;
    }
}