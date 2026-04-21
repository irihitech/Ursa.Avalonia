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

public class DateOnlyPickerTests
{
    private static DateOnlyPicker CreatePicker() => new()
    {
        Width = 300,
        DisplayFormat = "yyyy-MM-dd",
        HorizontalAlignment = HorizontalAlignment.Left,
        VerticalAlignment = VerticalAlignment.Top
    };

    [AvaloniaFact]
    public void Click_Opens_Popup()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        Assert.False(picker.IsDropdownOpen);
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);
    }

    [AvaloniaFact]
    public void Click_Button_Toggles_Popup()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();

        var button = picker.GetTemplateChildOfType<PathIcon>("PART_Button");
        var position = button?.TranslatePoint(new Point(5, 5), window);
        Assert.NotNull(position);

        Assert.False(picker.IsDropdownOpen);
        Dispatcher.UIThread.RunJobs();
        window.MouseDown(position.Value, MouseButton.Left);
        window.MouseUp(position.Value, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);

        window.MouseDown(position.Value, MouseButton.Left);
        window.MouseUp(position.Value, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.False(picker.IsDropdownOpen);
    }

    [AvaloniaFact]
    public void Clear_Sets_SelectedDate_To_Null()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();

        picker.SelectedDate = new DateOnly(2025, 6, 15);
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedDate);

        picker.Clear();
        Dispatcher.UIThread.RunJobs();
        Assert.Null(picker.SelectedDate);
    }

    [AvaloniaFact]
    public void Press_Escape_Closes_Popup()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);

        window.KeyPressQwerty(PhysicalKey.Escape, RawInputModifiers.None);
        Assert.False(picker.IsDropdownOpen);
    }

    [AvaloniaFact]
    public void Press_Down_Opens_Popup()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        Assert.False(picker.IsDropdownOpen);
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        window.KeyPressQwerty(PhysicalKey.ArrowDown, RawInputModifiers.None);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);
    }

    [AvaloniaFact]
    public void Press_Tab_Closes_Popup()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = new StackPanel
        {
            Children =
            {
                picker,
                new TextBox(),
            }
        };
        window.Show();
        Dispatcher.UIThread.RunJobs();

        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);

        window.KeyPressQwerty(PhysicalKey.Tab, RawInputModifiers.None);
        Assert.False(picker.IsDropdownOpen);
    }

    [AvaloniaFact]
    public void Set_SelectedDate_Updates_TextBox()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePickerBase.PART_TextBox);
        picker.SelectedDate = new DateOnly(2025, 3, 21);
        Dispatcher.UIThread.RunJobs();

        Assert.Equal("2025-03-21", textBox?.Text);
    }

    [AvaloniaFact]
    public void Set_SelectedDate_To_Null_Clears_TextBox()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        picker.SelectedDate = new DateOnly(2025, 3, 21);
        Dispatcher.UIThread.RunJobs();
        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePickerBase.PART_TextBox);
        Assert.NotNull(textBox);
        Assert.Equal("2025-03-21", textBox.Text);

        picker.SelectedDate = null;
        Dispatcher.UIThread.RunJobs();
        Assert.Null(textBox.Text);
    }

    [AvaloniaFact]
    public void SelectedDate_Set_Via_Calendar_Updates_TextBox()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();

        var popup = picker.GetTemplateChildOfType<Popup>(DatePickerBase.PART_Popup);
        var calendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        calendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateOnly(2025, 7, 4))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();

        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePickerBase.PART_TextBox);
        Assert.NotNull(textBox);
        Assert.Equal("2025-07-04", textBox.Text);
        Assert.False(picker.IsDropdownOpen);
    }

    [AvaloniaFact]
    public void SelectedDate_Set_Via_Calendar_Sets_SelectedDate()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();

        var popup = picker.GetTemplateChildOfType<Popup>(DatePickerBase.PART_Popup);
        var calendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        calendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateOnly(2025, 7, 4))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();

        Assert.Equal(new DateOnly(2025, 7, 4), picker.SelectedDate);
    }
}
