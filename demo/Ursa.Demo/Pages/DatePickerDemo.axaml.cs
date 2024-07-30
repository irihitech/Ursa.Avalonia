using System.Diagnostics;
using Avalonia.Controls;
using Ursa.Controls;

namespace Ursa.Demo.Pages;

public partial class DatePickerDemo : UserControl
{
    public DatePickerDemo()
    {
        InitializeComponent();
    }

    private void CalendarView_OnOnDateSelected(object? _, CalendarDayButtonEventArgs e)
    {
        Debug.WriteLine("Pressed: "+ e.Date?.ToLongDateString());
    }

    private void CalendarView_OnOnDatePreviewed(object? _, CalendarDayButtonEventArgs e)
    {
        Debug.WriteLine("Hovered: "+e.Date?.ToLongDateString());
    }
}