using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using HeadlessTest.Ursa.TestHelpers;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.DateAndTimePickerTests;

public class DateRangePickerTests
{
    [AvaloniaFact]
    public void DefaultDateKind_Utc_Applied_When_Start_Date_Selected_From_Calendar()
    {
        var window = new Window();
        var picker = new DateRangePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd",
            DefaultDateKind = DateTimeKind.Utc,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var startTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePicker.PART_StartTextBox);
        startTextBox?.Focus();
        Dispatcher.UIThread.RunJobs();
        var popup = picker.GetTemplateChildOfType<Popup>(DateRangePicker.PART_Popup);
        var startCalendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        startCalendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateTime(2025, 3, 10))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedStartDate);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedStartDate!.Value.Kind);
        Assert.Equal(new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc), picker.SelectedStartDate!.Value);
    }

    [AvaloniaFact]
    public void DefaultDateKind_Utc_Applied_When_End_Date_Selected_From_Calendar()
    {
        var window = new Window();
        var picker = new DateRangePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd",
            DefaultDateKind = DateTimeKind.Utc,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        // Focus start box first to set Status to Start
        var startTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePicker.PART_StartTextBox);
        startTextBox?.Focus();
        Dispatcher.UIThread.RunJobs();
        var popup = picker.GetTemplateChildOfType<Popup>(DateRangePicker.PART_Popup);
        var startCalendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        // Select start date first
        startCalendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateTime(2025, 3, 10))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();
        // Now focus end box and select end date
        var endTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePicker.PART_EndTextBox);
        endTextBox?.Focus();
        Dispatcher.UIThread.RunJobs();
        var calendars = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().ToList();
        var endCalendar = calendars?.ElementAtOrDefault(1) ?? calendars?.FirstOrDefault();
        endCalendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateTime(2025, 3, 20))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedEndDate);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedEndDate!.Value.Kind);
    }

    [AvaloniaFact]
    public void DefaultDateKind_Utc_Applied_When_Start_Text_Parsed()
    {
        var window = new Window();
        var picker = new DateRangePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd",
            DefaultDateKind = DateTimeKind.Utc,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        var button = new Button() { Width = 100 };
        window.Content = new StackPanel()
        {
            Children = { picker, button }
        };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var startTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePicker.PART_StartTextBox);
        startTextBox?.Focus();
        Dispatcher.UIThread.RunJobs();
        startTextBox?.SetValue(TextBox.TextProperty, "2025-04-01");
        button.Focus();
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedStartDate);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedStartDate!.Value.Kind);
        Assert.Equal(new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc), picker.SelectedStartDate!.Value);
    }

    [AvaloniaFact]
    public void DefaultDateKind_Utc_Applied_When_End_Text_Parsed()
    {
        var window = new Window();
        var picker = new DateRangePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd",
            DefaultDateKind = DateTimeKind.Utc,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        var button = new Button() { Width = 100 };
        window.Content = new StackPanel()
        {
            Children = { picker, button }
        };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var endTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePicker.PART_EndTextBox);
        endTextBox?.Focus();
        Dispatcher.UIThread.RunJobs();
        endTextBox?.SetValue(TextBox.TextProperty, "2025-04-30");
        button.Focus();
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedEndDate);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedEndDate!.Value.Kind);
        Assert.Equal(new DateTime(2025, 4, 30, 0, 0, 0, DateTimeKind.Utc), picker.SelectedEndDate!.Value);
    }

    [AvaloniaFact]
    public void DefaultDateKind_Unspecified_Is_Default()
    {
        var window = new Window();
        var picker = new DateRangePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        var button = new Button() { Width = 100 };
        window.Content = new StackPanel()
        {
            Children = { picker, button }
        };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var startTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePicker.PART_StartTextBox);
        startTextBox?.Focus();
        Dispatcher.UIThread.RunJobs();
        startTextBox?.SetValue(TextBox.TextProperty, "2025-04-01");
        button.Focus();
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedStartDate);
        Assert.Equal(DateTimeKind.Unspecified, picker.SelectedStartDate!.Value.Kind);
    }
}
