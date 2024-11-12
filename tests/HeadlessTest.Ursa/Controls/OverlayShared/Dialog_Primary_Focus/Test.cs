using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.OverlayShared.Dialog_Primary_Focus;

public class Test
{
    [AvaloniaFact]
    public async Task Normal_Drawer_Focus_On_Border()
    {
        var ursaWindow = new TestWindow();
        ursaWindow.Show();
        ursaWindow.InvokeNormalDrawer();
        Dispatcher.UIThread.RunJobs();
        await Task.Delay(500);
        var dialog = ursaWindow.GetVisualDescendants().OfType<DefaultDrawerControl>().FirstOrDefault();
        Assert.NotNull(dialog);
        var border = dialog.GetVisualDescendants().OfType<Border>().FirstOrDefault(a=>a.Name == "PART_Root");
        var text = dialog.GetVisualDescendants().OfType<TextBox>().FirstOrDefault();
        Assert.True(border?.IsFocused);
        Assert.False(text?.IsFocused);
    }
    
    [AvaloniaFact]
    public async Task Focus_Drawer_Focus_On_Primary()
    {
        var ursaWindow = new TestWindow();
        ursaWindow.Show();
        ursaWindow.InvokeFocusDrawer();
        Dispatcher.UIThread.RunJobs();
        await Task.Delay(500);
        var dialog = ursaWindow.GetVisualDescendants().OfType<DefaultDrawerControl>().FirstOrDefault();
        Assert.NotNull(dialog);
        var border = dialog.GetVisualDescendants().OfType<Border>().FirstOrDefault(a=>a.Name == "PART_Root");
        var text = dialog.GetVisualDescendants().OfType<TextBox>().FirstOrDefault();
        Assert.False(border?.IsFocused);
        Assert.True(text?.IsFocused);
    }
    
    [AvaloniaFact]
    public async Task Normal_Dialog_Focus_On_Border()
    {
        var ursaWindow = new TestWindow();
        ursaWindow.Show();
        ursaWindow.InvokeNormalDialog();
        Dispatcher.UIThread.RunJobs();
        await Task.Delay(100);
        var dialog = ursaWindow.GetVisualDescendants().OfType<DefaultDialogControl>().FirstOrDefault();
        Assert.NotNull(dialog);
        var border = dialog.GetVisualDescendants().OfType<Border>().FirstOrDefault(a=>a.Name == "PART_Border");
        var text = dialog.GetVisualDescendants().OfType<TextBox>().FirstOrDefault();
        Assert.True(border?.IsFocused);
        Assert.False(text?.IsFocused);
    }
    
    [AvaloniaFact]
    public async Task Focus_Dialog_Focus_On_Primary() 
    {
        var ursaWindow = new TestWindow();
        ursaWindow.Show();
        ursaWindow.InvokeFocusDialog();
        Dispatcher.UIThread.RunJobs();
        await Task.Delay(100);
        var dialog = ursaWindow.GetVisualDescendants().OfType<DefaultDialogControl>().FirstOrDefault();
        Assert.NotNull(dialog);
        var border = dialog.GetVisualDescendants().OfType<Border>().FirstOrDefault(a=>a.Name == "PART_Border");
        var text = dialog.GetVisualDescendants().OfType<TextBox>().FirstOrDefault();
        Assert.False(border?.IsFocused);
        Assert.True(text?.IsFocused);
    }
}