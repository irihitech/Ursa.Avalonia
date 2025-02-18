using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Threading;
using HeadlessTest.Ursa.TestHelpers;
using TimePicker = Ursa.Controls.TimePicker;

namespace HeadlessTest.Ursa.Controls.DateTimePicker;

public class TimePickerTests
{
    [AvaloniaFact]
    public void Click_Opens_Popup()
    {
        var window = new Window();
        var timePicker = new TimePicker()
        {
            Width = 300,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
        };
        window.Content = timePicker;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        Assert.False(timePicker.IsDropdownOpen);
        window.MouseDown(new Point(10, 10), MouseButton.Left);
        Assert.True(timePicker.IsDropdownOpen);
    }
    
    [AvaloniaFact]
    public void Click_Button_Toggles_Popup()
    {
        var window = new Window();
        var picker = new TimePicker()
        {
            Width = 300,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        window.Content = picker;
        window.Show();

        var button = picker.GetTemplateChildOfType<Button>(TimePicker.PART_Button);
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
}