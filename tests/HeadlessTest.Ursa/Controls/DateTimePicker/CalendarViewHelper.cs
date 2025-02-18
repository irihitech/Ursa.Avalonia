using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.DateTimePicker;

internal static class CalendarViewHelper
{
    internal static void ClickPrevious(this CalendarView calendarView)
    {
        var previousButton = calendarView.GetTemplateChildren()
                                         .FirstOrDefault(a => a.Name == CalendarView.PART_PreviousButton);
        Assert.IsType<Button>(previousButton);
        var button = (Button)previousButton;
        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }
    
    internal static void ClickNext(this CalendarView calendarView)
    {
        var nextButton = calendarView.GetTemplateChildren()
                                     .FirstOrDefault(a => a.Name == CalendarView.PART_NextButton);
        Assert.IsType<Button>(nextButton);
        var button = (Button)nextButton;
        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }
    
    internal static void ClickFastNext(this CalendarView calendarView)
    {
        var nextButton = calendarView.GetTemplateChildren()
                                     .FirstOrDefault(a => a.Name == CalendarView.PART_FastNextButton);
        Assert.IsType<Button>(nextButton);
        var button = (Button)nextButton;
        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }
    
    internal static void ClickFastPrevious(this CalendarView calendarView)
    {
        var previousButton = calendarView.GetTemplateChildren()
                                         .FirstOrDefault(a => a.Name == CalendarView.PART_FastPreviousButton);
        Assert.IsType<Button>(previousButton);
        var button = (Button)previousButton;
        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }
    
    internal static void ClickHeaderButton(this CalendarView calendarView)
    {
        var headerButton = calendarView.GetTemplateChildren()
                                       .FirstOrDefault(a => a.Name == CalendarView.PART_HeaderButton);
        Assert.IsType<Button>(headerButton);
        var button = (Button)headerButton;
        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }
}