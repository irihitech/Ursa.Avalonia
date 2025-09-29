using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Logging;
using Avalonia.Threading;
using HeadlessTest.Ursa.Controls.PathPickerTests;
using HeadlessTest.Ursa.TestHelpers;
using Ursa.Controls;

namespace Test.Ursa.PathPickerTests;

public class PathPickerTests
{
    [AvaloniaFact]
    public void PathPicker_PatternsRegex_Validate()
    {
        var window = new Window();
        var pathPickerView = new PathPickerTestView();
        window.Content = pathPickerView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        //pass
        pathPickerView.TestPathPicker.FileFilter = "[txt,*.txt]";
        //fail
        //pathPickerView.TestPathPicker.FileFilter = "*.txt";
        var button = pathPickerView.TestPathPicker.GetTemplateChildOfType<Button>(PathPicker.PART_Button);
        var position = button?.TranslatePoint(new Point(5, 5), window);
        window.MouseDown(position.Value, MouseButton.Left);
        //window.MouseUp(position.Value, MouseButton.Left);
        var exception = Assert.Throws<Exception>(()=> window.MouseUp(position.Value, MouseButton.Left));
        Assert.Contains("Invalid parameter, please refer to the following content", exception.Message);
    }
}