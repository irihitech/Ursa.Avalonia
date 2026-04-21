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
using TimePickerPresenter = Ursa.Controls.TimePickerPresenter;

namespace HeadlessTest.Ursa.Controls.DateTimePickerTests;

public class TimeOnlyRangePickerTests
{
    private static TimeOnlyRangePicker CreatePicker() => new()
    {
        Width = 500,
        DisplayFormat = "HH:mm:ss",
        HorizontalAlignment = HorizontalAlignment.Left,
        VerticalAlignment = VerticalAlignment.Top
    };

    [AvaloniaFact]
    public void Set_SelectedTimes_Update_TextBoxes()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        picker.SelectedStartTime = new TimeOnly(8, 0, 0);
        picker.SelectedEndTime = new TimeOnly(17, 30, 0);
        Dispatcher.UIThread.RunJobs();

        var startTextBox = picker.GetTemplateChildOfType<TextBox>(TimeRangePickerBase.PART_StartTextBox);
        var endTextBox = picker.GetTemplateChildOfType<TextBox>(TimeRangePickerBase.PART_EndTextBox);
        Assert.NotNull(startTextBox);
        Assert.NotNull(endTextBox);
        Assert.Equal("08:00:00", startTextBox.Text);
        Assert.Equal("17:30:00", endTextBox.Text);
    }

    [AvaloniaFact]
    public void Clear_Clears_Both_Times()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        picker.SelectedStartTime = new TimeOnly(8, 0, 0);
        picker.SelectedEndTime = new TimeOnly(17, 30, 0);
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedStartTime);
        Assert.NotNull(picker.SelectedEndTime);

        picker.Clear();
        Dispatcher.UIThread.RunJobs();
        Assert.Null(picker.SelectedStartTime);
        Assert.Null(picker.SelectedEndTime);
    }

    [AvaloniaFact]
    public void Click_StartTextBox_Opens_Popup()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var startTextBox = picker.GetTemplateChildOfType<TextBox>(TimeRangePickerBase.PART_StartTextBox);
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

        var endTextBox = picker.GetTemplateChildOfType<TextBox>(TimeRangePickerBase.PART_EndTextBox);
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

        var startTextBox = picker.GetTemplateChildOfType<TextBox>(TimeRangePickerBase.PART_StartTextBox);
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
    public void Presenter_Time_Change_Updates_SelectedStartTime()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var startTextBox = picker.GetTemplateChildOfType<TextBox>(TimeRangePickerBase.PART_StartTextBox);
        Assert.NotNull(startTextBox);
        var position = startTextBox.TranslatePoint(new Point(5, 5), window);
        Assert.NotNull(position);
        window.MouseDown(position.Value, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();

        var popup = picker.GetTemplateChildOfType<Popup>(TimeRangePickerBase.PART_Popup);
        var startPresenter = popup?.GetLogicalDescendants().OfType<TimePickerPresenter>()
            .FirstOrDefault(p => p.Name == TimeRangePickerBase.PART_StartPresenter);
        Assert.NotNull(startPresenter);

        var newTime = new TimeOnly(9, 15, 0);
        startPresenter.RaiseEvent(new TimeChangedEventArgs(null, newTime)
            { RoutedEvent = TimePickerPresenter.SelectedTimeChangedEvent });
        Dispatcher.UIThread.RunJobs();

        Assert.Equal(newTime, picker.SelectedStartTime);
    }

    [AvaloniaFact]
    public void Presenter_Time_Change_Updates_SelectedEndTime()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var endTextBox = picker.GetTemplateChildOfType<TextBox>(TimeRangePickerBase.PART_EndTextBox);
        Assert.NotNull(endTextBox);
        var position = endTextBox.TranslatePoint(new Point(5, 5), window);
        Assert.NotNull(position);
        window.MouseDown(position.Value, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();

        var popup = picker.GetTemplateChildOfType<Popup>(TimeRangePickerBase.PART_Popup);
        var endPresenter = popup?.GetLogicalDescendants().OfType<TimePickerPresenter>()
            .FirstOrDefault(p => p.Name == TimeRangePickerBase.PART_EndPresenter);
        Assert.NotNull(endPresenter);

        var newTime = new TimeOnly(18, 45, 0);
        endPresenter.RaiseEvent(new TimeChangedEventArgs(null, newTime)
            { RoutedEvent = TimePickerPresenter.SelectedTimeChangedEvent });
        Dispatcher.UIThread.RunJobs();

        Assert.Equal(newTime, picker.SelectedEndTime);
    }
}
