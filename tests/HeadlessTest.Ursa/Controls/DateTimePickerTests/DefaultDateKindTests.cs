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
using DatePicker = Ursa.Controls.DatePicker;

namespace HeadlessTest.Ursa.Controls.DateTimePickerTests;

public class DefaultDateKindTests
{
    // --- DatePicker ---

    [AvaloniaFact]
    public void DatePicker_DefaultDateKind_Unspecified_Is_Default()
    {
        var picker = new DatePicker();
        Assert.Equal(DateTimeKind.Unspecified, picker.DefaultDateKind);
    }

    [AvaloniaFact]
    public void DatePicker_CalendarSelection_AppliesUtcKind()
    {
        var window = new Window();
        var picker = new DatePicker
        {
            Width = 300,
            DefaultDateKind = DateTimeKind.Utc,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        window.MouseDown(new Avalonia.Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();

        var popup = picker.GetTemplateChildOfType<Popup>(DatePicker.PART_Popup);
        var calendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        calendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateOnly(2025, 6, 15))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();

        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedDate!.Value.Kind);
        Assert.Equal(new DateOnly(2025, 6, 15), DateOnly.FromDateTime(picker.SelectedDate.Value));
    }

    [AvaloniaFact]
    public void DatePicker_CalendarSelection_AppliesLocalKind()
    {
        var window = new Window();
        var picker = new DatePicker
        {
            Width = 300,
            DefaultDateKind = DateTimeKind.Local,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        window.MouseDown(new Avalonia.Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();

        var popup = picker.GetTemplateChildOfType<Popup>(DatePicker.PART_Popup);
        var calendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        calendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateOnly(2025, 3, 20))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();

        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Local, picker.SelectedDate!.Value.Kind);
    }

    [AvaloniaFact]
    public void DatePicker_TextInput_AppliesUtcKind()
    {
        var window = new Window();
        var picker = new DatePicker
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd",
            DefaultDateKind = DateTimeKind.Utc,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        var button = new Button { Width = 100 };
        window.Content = new StackPanel { Children = { picker, button } };
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePicker.PART_TextBox);
        textBox?.Focus();
        textBox?.SetValue(TextBox.TextProperty, "2025-06-15");
        button.Focus();
        Dispatcher.UIThread.RunJobs();

        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedDate!.Value.Kind);
        Assert.Equal(new DateTime(2025, 6, 15, 0, 0, 0, DateTimeKind.Utc), picker.SelectedDate.Value);
    }

    [AvaloniaFact]
    public void DatePicker_TextInput_AppliesLocalKind()
    {
        var window = new Window();
        var picker = new DatePicker
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd",
            DefaultDateKind = DateTimeKind.Local,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        var button = new Button { Width = 100 };
        window.Content = new StackPanel { Children = { picker, button } };
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePicker.PART_TextBox);
        textBox?.Focus();
        textBox?.SetValue(TextBox.TextProperty, "2025-06-15");
        button.Focus();
        Dispatcher.UIThread.RunJobs();

        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Local, picker.SelectedDate!.Value.Kind);
    }

    // --- DateRangePicker ---

    [AvaloniaFact]
    public void DateRangePicker_DefaultDateKind_Unspecified_Is_Default()
    {
        var picker = new DateRangePicker();
        Assert.Equal(DateTimeKind.Unspecified, picker.DefaultDateKind);
    }

    [AvaloniaFact]
    public void DateRangePicker_CalendarSelection_AppliesUtcKindToBothDates()
    {
        var window = new Window();
        var picker = new DateRangePicker
        {
            Width = 600,
            DefaultDateKind = DateTimeKind.Utc,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        // Click to focus start textbox and open popup (pushes Status.Start)
        window.MouseDown(new Avalonia.Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();

        var popup = picker.GetTemplateChildOfType<Popup>(DateRangePickerBase.PART_Popup);
        var startCalendar = popup?.GetLogicalDescendants()
            .OfType<DatePickerCalendarView>()
            .FirstOrDefault();

        // Select start date
        startCalendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateOnly(2025, 6, 1))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();

        // Selecting start date focuses end textbox (Status.End). Select end date.
        var endCalendar = popup?.GetLogicalDescendants()
            .OfType<DatePickerCalendarView>()
            .LastOrDefault();
        endCalendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateOnly(2025, 6, 30))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();

        Assert.NotNull(picker.SelectedStartDate);
        Assert.NotNull(picker.SelectedEndDate);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedStartDate!.Value.Kind);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedEndDate!.Value.Kind);
    }

    [AvaloniaFact]
    public void DateRangePicker_TextInput_AppliesLocalKind()
    {
        var window = new Window();
        var picker = new DateRangePicker
        {
            Width = 600,
            DisplayFormat = "yyyy-MM-dd",
            DefaultDateKind = DateTimeKind.Local,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        var button = new Button { Width = 100 };
        window.Content = new StackPanel { Children = { picker, button } };
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var startTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_StartTextBox);
        var endTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_EndTextBox);

        startTextBox?.Focus();
        startTextBox?.SetValue(TextBox.TextProperty, "2025-06-01");
        endTextBox?.Focus();
        endTextBox?.SetValue(TextBox.TextProperty, "2025-06-30");
        button.Focus();
        Dispatcher.UIThread.RunJobs();

        Assert.NotNull(picker.SelectedStartDate);
        Assert.NotNull(picker.SelectedEndDate);
        Assert.Equal(DateTimeKind.Local, picker.SelectedStartDate!.Value.Kind);
        Assert.Equal(DateTimeKind.Local, picker.SelectedEndDate!.Value.Kind);
    }

    // --- DateTimePicker ---

    [AvaloniaFact]
    public void DateTimePicker_DefaultDateKind_Unspecified_Is_Default()
    {
        var picker = new DateTimePicker();
        Assert.Equal(DateTimeKind.Unspecified, picker.DefaultDateKind);
    }

    [AvaloniaFact]
    public void DateTimePicker_CalendarSelection_AppliesUtcKind()
    {
        var window = new Window();
        var picker = new DateTimePicker
        {
            Width = 300,
            DefaultDateKind = DateTimeKind.Utc,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        window.MouseDown(new Avalonia.Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();

        var popup = picker.GetTemplateChildOfType<Popup>(DateTimePickerBase.PART_Popup);
        var calendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        calendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateOnly(2025, 6, 15))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();

        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedDate!.Value.Kind);
        Assert.Equal(new DateOnly(2025, 6, 15), DateOnly.FromDateTime(picker.SelectedDate.Value));
    }

    [AvaloniaFact]
    public void DateTimePicker_CalendarSelection_AppliesLocalKind()
    {
        var window = new Window();
        var picker = new DateTimePicker
        {
            Width = 300,
            DefaultDateKind = DateTimeKind.Local,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        window.MouseDown(new Avalonia.Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();

        var popup = picker.GetTemplateChildOfType<Popup>(DateTimePickerBase.PART_Popup);
        var calendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        calendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateOnly(2025, 3, 20))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();

        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Local, picker.SelectedDate!.Value.Kind);
    }

    [AvaloniaFact]
    public void DateTimePicker_TextInput_AppliesUtcKind()
    {
        var window = new Window();
        var picker = new DateTimePicker
        {
            Width = 300,
            DisplayFormat = "yyyy-MM-dd HH:mm:ss",
            DefaultDateKind = DateTimeKind.Utc,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        var button = new Button { Width = 100 };
        window.Content = new StackPanel { Children = { picker, button } };
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var textBox = picker.GetTemplateChildOfType<TextBox>(DateTimePickerBase.PART_TextBox);
        textBox?.Focus();
        textBox?.SetValue(TextBox.TextProperty, "2025-06-15 10:30:00");
        button.Focus();
        Dispatcher.UIThread.RunJobs();

        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(DateTimeKind.Utc, picker.SelectedDate!.Value.Kind);
        Assert.Equal(new DateTime(2025, 6, 15, 10, 30, 0, DateTimeKind.Utc), picker.SelectedDate.Value);
    }
}
