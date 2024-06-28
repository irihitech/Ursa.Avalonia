using Avalonia.Interactivity;

namespace Ursa.Controls;

public class CalendarYearButtonEventArgs: RoutedEventArgs
{
    internal CalendarContext Context { get;  }
    internal CalendarViewMode Mode { get; }

    /// <inheritdoc />
    internal CalendarYearButtonEventArgs(CalendarViewMode mode, CalendarContext context)
    {
        Context = context;
        Mode = mode;
    }
}