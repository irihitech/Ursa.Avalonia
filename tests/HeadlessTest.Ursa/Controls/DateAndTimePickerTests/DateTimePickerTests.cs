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
using Irihi.Avalonia.Shared.Common;
using Ursa.Controls;
using TimePickerPresenter = Ursa.Controls.TimePickerPresenter;

namespace HeadlessTest.Ursa.Controls.DateAndTimePickerTests;

public class DateTimePickerTests
{
    [AvaloniaFact]
    public void DefaultDateKind_Utc_Applied_When_Calendar_Date_Selected()
    {
        var window = new Window();
        var picker = new DateTimePicker()
        {
            Width = 300,
            DefaultDateKind = DateTimeKind.Utc,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        var popup = picker.GetTemplateChildOfType<Popup>(PartNames.PART_Popup);
        var calendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        calendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateTime(2025, 2, 17))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedDate!.Value.Kind);
        Assert.Equal(2025, picker.SelectedDate!.Value.Year);
        Assert.Equal(2, picker.SelectedDate!.Value.Month);
        Assert.Equal(17, picker.SelectedDate!.Value.Day);
    }

    [AvaloniaFact]
    public void DefaultDateKind_Local_Applied_When_Calendar_Date_Selected()
    {
        var window = new Window();
        var picker = new DateTimePicker()
        {
            Width = 300,
            DefaultDateKind = DateTimeKind.Local,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        var popup = picker.GetTemplateChildOfType<Popup>(DateTimePicker.PART_Popup);
        var calendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        calendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateTime(2025, 5, 20))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Local, picker.SelectedDate!.Value.Kind);
    }

    [AvaloniaFact]
    public void DefaultDateKind_Utc_Applied_When_Text_Parsed()
    {
        var window = new Window();
        var picker = new DateTimePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd HH:mm:ss",
            DefaultDateKind = DateTimeKind.Utc,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var textBox = picker.GetTemplateChildOfType<TextBox>(DateTimePicker.PART_TextBox);
        textBox?.Focus();
        Dispatcher.UIThread.RunJobs();
        textBox?.SetValue(TextBox.TextProperty, "2025-06-15 10:30:00");
        window.KeyPressQwerty(PhysicalKey.Enter, RawInputModifiers.None);
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedDate!.Value.Kind);
        Assert.Equal(new DateTime(2025, 6, 15, 10, 30, 0, DateTimeKind.Utc), picker.SelectedDate!.Value);
    }

    [AvaloniaFact]
    public void DefaultDateKind_Unspecified_Is_Default()
    {
        var window = new Window();
        var picker = new DateTimePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd HH:mm:ss",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var textBox = picker.GetTemplateChildOfType<TextBox>(DateTimePicker.PART_TextBox);
        textBox?.Focus();
        Dispatcher.UIThread.RunJobs();
        textBox?.SetValue(TextBox.TextProperty, "2025-06-15 10:30:00");
        window.KeyPressQwerty(PhysicalKey.Enter, RawInputModifiers.None);
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Unspecified, picker.SelectedDate!.Value.Kind);
    }

    [AvaloniaFact]
    public void DefaultDateKind_Preserved_When_Time_Changes()
    {
        var window = new Window();
        var picker = new DateTimePicker()
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd HH:mm:ss",
            DefaultDateKind = DateTimeKind.Utc,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var textBox = picker.GetTemplateChildOfType<TextBox>(DateTimePicker.PART_TextBox);
        textBox?.Focus();
        Dispatcher.UIThread.RunJobs();
        textBox?.SetValue(TextBox.TextProperty, "2025-06-15 10:30:00");
        window.KeyPressQwerty(PhysicalKey.Enter, RawInputModifiers.None);
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedDate!.Value.Kind);

        // Simulate time change via TimePickerPresenter event
        var popup = picker.GetTemplateChildOfType<Popup>(DateTimePicker.PART_Popup);
        var timePicker = popup?.GetLogicalDescendants().OfType<TimePickerPresenter>().FirstOrDefault();
        timePicker?.RaiseEvent(new TimeChangedEventArgs(new TimeSpan(10, 30, 0), new TimeSpan(14, 0, 0))
            { RoutedEvent = TimePickerPresenter.SelectedTimeChangedEvent });
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedDate!.Value.Kind);
        Assert.Equal(14, picker.SelectedDate!.Value.Hour);
    }
}
