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

public class TimeOnlyPickerTests
{
    private static TimeOnlyPicker CreatePicker() => new()
    {
        Width = 300,
        DisplayFormat = "HH:mm:ss",
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
    public void Clear_Sets_SelectedTime_To_Null()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        picker.SelectedTime = new TimeOnly(14, 30, 0);
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedTime);

        picker.Clear();
        Dispatcher.UIThread.RunJobs();
        Assert.Null(picker.SelectedTime);
    }

    [AvaloniaFact]
    public void Set_SelectedTime_Updates_TextBox()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        picker.SelectedTime = new TimeOnly(9, 5, 30);
        Dispatcher.UIThread.RunJobs();

        var textBox = picker.GetTemplateChildOfType<TextBox>(TimePickerBase.PART_TextBox);
        Assert.NotNull(textBox);
        Assert.Equal("09:05:30", textBox.Text);
    }

    [AvaloniaFact]
    public void Set_SelectedTime_To_Null_Clears_TextBox()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        picker.SelectedTime = new TimeOnly(14, 30, 0);
        Dispatcher.UIThread.RunJobs();
        var textBox = picker.GetTemplateChildOfType<TextBox>(TimePickerBase.PART_TextBox);
        Assert.NotNull(textBox);
        Assert.Equal("14:30:00", textBox.Text);

        picker.SelectedTime = null;
        Dispatcher.UIThread.RunJobs();
        Assert.Null(textBox.Text);
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
    public void Presenter_Time_Change_Updates_SelectedTime()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        // Open popup so the presenter becomes part of the visual/logical tree
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);

        var popup = picker.GetTemplateChildOfType<Popup>(TimePickerBase.PART_Popup);
        var presenter = popup?.GetLogicalDescendants().OfType<TimePickerPresenter>().FirstOrDefault();
        Assert.NotNull(presenter);

        var newTime = new TimeOnly(10, 20, 30);
        presenter.RaiseEvent(new TimeChangedEventArgs(null, newTime)
            { RoutedEvent = TimePickerPresenter.SelectedTimeChangedEvent });
        Dispatcher.UIThread.RunJobs();

        Assert.Equal(newTime, picker.SelectedTime);
    }
}
