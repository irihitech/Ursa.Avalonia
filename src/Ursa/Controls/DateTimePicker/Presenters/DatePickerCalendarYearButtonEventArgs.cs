using Avalonia.Interactivity;

namespace Ursa.Controls;

public class DatePickerCalendarYearButtonEventArgs: RoutedEventArgs
{
    internal DatePickerCalendarContext Context { get;  }
    internal DatePickerCalendarViewMode Mode { get; }

    /// <inheritdoc />
    internal DatePickerCalendarYearButtonEventArgs(DatePickerCalendarViewMode mode, DatePickerCalendarContext context)
    {
        Context = context;
        Mode = mode;
    }
}