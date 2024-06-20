using Avalonia.Interactivity;

namespace Ursa.Controls;

public class CalendarDayButtonEventArgs(DateTime? date) : RoutedEventArgs
{
    public DateTime? Date { get; private set; } = date;
}