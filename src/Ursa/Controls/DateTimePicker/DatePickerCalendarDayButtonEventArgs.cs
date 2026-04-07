using Avalonia.Interactivity;

namespace Ursa.Controls;

public class DatePickerCalendarDayButtonEventArgs(DateTime? date) : RoutedEventArgs
{
    public DateTime? Date { get; private set; } = date;
}