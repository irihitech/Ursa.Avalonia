using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Threading;

namespace HeadlessTest.Ursa.Controls.OverlayShared.Case_Close_Dialog_Clear_Content_Parent;

public class Test
{
    [AvaloniaFact]
    public void Dialog_Parent_Is_Cleared_After_Close()
    {
        var ursaWindow = new TestWindow();
        ursaWindow.Show();
        var button = ursaWindow.FindControl<Button>("button"); 
        ursaWindow.MouseDown(new Point(280, 400), MouseButton.Left);
        ursaWindow.MouseUp(new Point(280, 400), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        var parent = ursaWindow.TextBox.Parent;
        Assert.NotNull(ursaWindow.TextBox.Parent);
        ursaWindow.DialogViewModel.Close();
        Dispatcher.UIThread.RunJobs();
        Assert.Null(ursaWindow.TextBox.Parent);
    }
}