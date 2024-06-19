using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ursa.Controls;

namespace Ursa.Demo.Pages;

public partial class DatePickerDemo : UserControl
{
    public DatePickerDemo()
    {
        InitializeComponent();
    }

    private void CalendarView_OnOnDateSelected(object? sender, CalendarDayButtonEventArgs e)
    {
        Debug.WriteLine("Pressed: "+ e.Date.ToLongDateString());
    }

    private void CalendarView_OnOnDatePreviewed(object? sender, CalendarDayButtonEventArgs e)
    {
        Debug.WriteLine("Hovered: "+e.Date.ToLongDateString());
    }
}