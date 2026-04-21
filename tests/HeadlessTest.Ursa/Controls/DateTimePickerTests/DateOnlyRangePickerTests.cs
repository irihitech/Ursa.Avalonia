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

namespace HeadlessTest.Ursa.Controls.DateTimePickerTests;

public class DateOnlyRangePickerTests
{
    private static DateOnlyRangePicker CreatePicker() => new()
    {
        Width = 500,
        DisplayFormat = "yyyy-MM-dd",
        HorizontalAlignment = HorizontalAlignment.Left,
        VerticalAlignment = VerticalAlignment.Top
    };

    [AvaloniaFact]
    public void Set_SelectedDates_Update_TextBoxes()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        picker.SelectedStartDate = new DateOnly(2025, 1, 10);
        picker.SelectedEndDate = new DateOnly(2025, 1, 20);
        Dispatcher.UIThread.RunJobs();

        var startTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_StartTextBox);
        var endTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_EndTextBox);
        Assert.NotNull(startTextBox);
        Assert.NotNull(endTextBox);
        Assert.Equal("2025-01-10", startTextBox.Text);
        Assert.Equal("2025-01-20", endTextBox.Text);
    }

    [AvaloniaFact]
    public void Clear_Clears_Both_Dates()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        picker.SelectedStartDate = new DateOnly(2025, 1, 10);
        picker.SelectedEndDate = new DateOnly(2025, 1, 20);
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedStartDate);
        Assert.NotNull(picker.SelectedEndDate);

        picker.Clear();
        Dispatcher.UIThread.RunJobs();
        Assert.Null(picker.SelectedStartDate);
        Assert.Null(picker.SelectedEndDate);
    }

    [AvaloniaFact]
    public void Click_StartTextBox_Opens_Popup()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var startTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_StartTextBox);
        Assert.NotNull(startTextBox);
        var position = startTextBox.TranslatePoint(new Point(5, 5), window);
        Assert.NotNull(position);

        Assert.False(picker.IsDropdownOpen);
        window.MouseDown(position.Value, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);
    }

    [AvaloniaFact]
    public void Click_EndTextBox_Opens_Popup()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var endTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_EndTextBox);
        Assert.NotNull(endTextBox);
        var position = endTextBox.TranslatePoint(new Point(5, 5), window);
        Assert.NotNull(position);

        Assert.False(picker.IsDropdownOpen);
        window.MouseDown(position.Value, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);
    }

    [AvaloniaFact]
    public void Press_Escape_Closes_Popup()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var startTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_StartTextBox);
        Assert.NotNull(startTextBox);
        var position = startTextBox.TranslatePoint(new Point(5, 5), window);
        Assert.NotNull(position);

        window.MouseDown(position.Value, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);

        window.KeyPressQwerty(PhysicalKey.Escape, RawInputModifiers.None);
        Assert.False(picker.IsDropdownOpen);
    }

    [AvaloniaFact]
    public void Set_SelectedStartDate_To_Null_Clears_Start_TextBox()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        picker.SelectedStartDate = new DateOnly(2025, 5, 1);
        picker.SelectedEndDate = new DateOnly(2025, 5, 31);
        Dispatcher.UIThread.RunJobs();

        picker.SelectedStartDate = null;
        Dispatcher.UIThread.RunJobs();

        var startTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_StartTextBox);
        Assert.NotNull(startTextBox);
        Assert.True(string.IsNullOrEmpty(startTextBox.Text));
    }

    [AvaloniaFact]
    public void Calendar_Date_Selected_Sets_StartDate_Then_EndDate()
    {
        var window = new Window { Width = 800, Height = 600 };
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var startTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_StartTextBox);
        Assert.NotNull(startTextBox);
        var position = startTextBox.TranslatePoint(new Point(5, 5), window);
        Assert.NotNull(position);

        window.MouseDown(position.Value, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();

        var popup = picker.GetTemplateChildOfType<Popup>(DateRangePickerBase.PART_Popup);
        var calendars = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().ToList();
        Assert.NotNull(calendars);
        Assert.NotEmpty(calendars);

        // Select start date
        calendars[0].RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateOnly(2025, 3, 10))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(new DateOnly(2025, 3, 10), picker.SelectedStartDate);

        // Select end date
        calendars[0].RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateOnly(2025, 3, 20))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(new DateOnly(2025, 3, 20), picker.SelectedEndDate);
    }
}
