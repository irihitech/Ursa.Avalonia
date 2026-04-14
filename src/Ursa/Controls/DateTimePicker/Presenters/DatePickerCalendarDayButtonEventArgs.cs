using Avalonia.Interactivity;

namespace Ursa.Controls;

public class DatePickerCalendarDayButtonEventArgs(DateOnly? date) : RoutedEventArgs
{
    public DateOnly? Date { get; private set; } = date;
}
