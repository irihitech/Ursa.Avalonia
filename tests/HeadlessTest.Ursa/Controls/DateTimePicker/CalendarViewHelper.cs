using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.DateTimePicker;

internal static class CalendarViewHelper
{
    internal static void ClickPrevious(this DatePickerCalendarView datePickerCalendarView)
    {
        var previousButton = datePickerCalendarView.GetTemplateChildren()
                                         .FirstOrDefault(a => a.Name == DatePickerCalendarView.PART_PreviousButton);
        Assert.IsAssignableFrom<Button>(previousButton);
        var button = previousButton as Button;
        button?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }
    
    internal static void ClickNext(this DatePickerCalendarView datePickerCalendarView)
    {
        var nextButton = datePickerCalendarView.GetTemplateChildren()
                                     .FirstOrDefault(a => a.Name == DatePickerCalendarView.PART_NextButton);
        Assert.IsAssignableFrom<Button>(nextButton);
        var button = nextButton as Button;
        button?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }
    
    internal static void ClickFastNext(this DatePickerCalendarView datePickerCalendarView)
    {
        var nextButton = datePickerCalendarView.GetTemplateChildren()
                                     .FirstOrDefault(a => a.Name == DatePickerCalendarView.PART_FastNextButton);
        Assert.IsAssignableFrom<Button>(nextButton);
        var button = nextButton as Button;
        button?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }
    
    internal static void ClickFastPrevious(this DatePickerCalendarView datePickerCalendarView)
    {
        var previousButton = datePickerCalendarView.GetTemplateChildren()
                                         .FirstOrDefault(a => a.Name == DatePickerCalendarView.PART_FastPreviousButton);
        Assert.IsAssignableFrom<Button>(previousButton);
        var button = previousButton as Button;
        button?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }
    
    internal static void ClickHeaderButton(this DatePickerCalendarView datePickerCalendarView)
    {
        var headerButton = datePickerCalendarView.GetTemplateChildren()
                                       .FirstOrDefault(a => a.Name == DatePickerCalendarView.PART_HeaderButton);
        Assert.IsAssignableFrom<Button>(headerButton);
        var button = headerButton as Button;
        button?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }
}