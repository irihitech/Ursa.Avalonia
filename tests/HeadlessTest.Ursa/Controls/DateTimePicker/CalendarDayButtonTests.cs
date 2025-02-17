using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Threading;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.DateTimePicker;

public class CalendarDayButtonTests
{
    [AvaloniaFact]
    public void OnPointerReleased_RaisesDateSelectedEvent()
    {
        Window window = new Window();
        var button = new CalendarDayButton();
        var date = new DateTime(2023, 5, 15);
        button.DataContext = date;
        window.Content = button;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        int eventRaised = 0;
        DateTime? eventContext = null;

        void OnMouseClick(object? sender, CalendarDayButtonEventArgs args)
        {
            eventRaised++;
            eventContext = args.Date;
        }

        button.DateSelected += OnMouseClick;
        window.MouseUp(new Point(10, 10), MouseButton.Left);
        Assert.Equal(1, eventRaised);
        Assert.Equal(date, eventContext);
        button.DateSelected -= OnMouseClick;
        eventContext = null;
        window.MouseUp(new Point(10, 10), MouseButton.Left);
        Assert.Null(eventContext);
        Assert.Equal(1, eventRaised);
    }

    [AvaloniaFact]
    public void OnPointerEntered_RaisesDatePreviewedEvent()
    {
        Window window = new Window();
        var button = new CalendarDayButton();
        var date = new DateTime(2023, 5, 15);
        button.DataContext = date;
        window.Content = button;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        int eventRaised = 0;
        DateTime? eventContext = null;

        void OnMouseEnter(object? sender, CalendarDayButtonEventArgs args)
        {
            eventRaised++;
            eventContext = args.Date;
        }

        button.DatePreviewed += OnMouseEnter;
        window.MouseMove(new Point(10, 10));
        Assert.Equal(1, eventRaised);
        Assert.Equal(date, eventContext);
        window.MouseMove(new Point(100, 100));
        button.DatePreviewed -= OnMouseEnter;
        eventContext = null;
        window.MouseMove(new Point(10, 10));
        Assert.Null(eventContext);
        Assert.Equal(1, eventRaised);
    }
}