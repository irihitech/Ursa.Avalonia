using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.OverlayShared.Dialog_Primary_Focus;

public class Test
{
    [AvaloniaFact]
    public void Normal_Dialog_Focus_On_Border()
    {
        var ursaWindow = new TestWindow();
        ursaWindow.Show();
        ursaWindow.InvokeNormalDialog();
        Dispatcher.UIThread.RunJobs();
        var dialog = ursaWindow.GetVisualDescendants().OfType<DefaultDialogControl>().FirstOrDefault();
        Assert.NotNull(dialog);
        var border = dialog.GetVisualDescendants().OfType<Border>().FirstOrDefault(a=>a.Name == "PART_Border");
        var text = dialog.GetVisualDescendants().OfType<TextBox>().FirstOrDefault();
        Assert.True(border?.IsFocused);
        Assert.False(text?.IsFocused);
    }
    
    [AvaloniaFact]
    public void Focus_Dialog_Focus_On_Primary()
    {
        var ursaWindow = new TestWindow();
        ursaWindow.Show();
        ursaWindow.InvokeFocusDialog();
        Dispatcher.UIThread.RunJobs();
        var dialog = ursaWindow.GetVisualDescendants().OfType<DefaultDialogControl>().FirstOrDefault();
        Assert.NotNull(dialog);
        var border = dialog.GetVisualDescendants().OfType<Border>().FirstOrDefault(a=>a.Name == "PART_Border");
        var text = dialog.GetVisualDescendants().OfType<TextBox>().FirstOrDefault();
        Assert.False(border?.IsFocused);
        Assert.True(text?.IsFocused);
    }
}