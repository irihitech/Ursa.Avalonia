using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;
using Xunit;

namespace HeadlessTest.Ursa.Controls.DialogTests.StyleClassTests;

public class StyleClassTests
{
    [AvaloniaFact]
    public void StyleClass_Changes_Button_Content()
    {
        var vm = new TestViewModel();
        var ursaWindow = new TestWindow()
        {
            DataContext = vm,
        };
        ursaWindow.Show();
        vm.InvokeDialog("Custom", ursaWindow.GetHashCode());
        Dispatcher.UIThread.RunJobs();
        var dialog = ursaWindow.GetVisualDescendants().OfType<DefaultDialogControl>().SingleOrDefault();
        Assert.NotNull(dialog);
        Assert.Contains("Custom", dialog.Classes);
        var okButton = dialog.GetVisualDescendants().OfType<Button>().SingleOrDefault(a=>a.Name == DefaultDialogControl.PART_OKButton);
        Assert.NotNull(okButton);
        Assert.Equal("CUSTOM", okButton.Content);
        var cancelButton = dialog.GetVisualDescendants().OfType<Button>().SingleOrDefault(a=>a.Name == DefaultDialogControl.PART_CancelButton);
        Assert.NotNull(cancelButton);
        Assert.Equal("取消", cancelButton.Content);
        ursaWindow.Close();
        Dispatcher.UIThread.RunJobs();
    }
    
    [AvaloniaFact]
    public void StyleClass_Changes_Button_Content_With_Multiple_Classes()
    {
        var vm = new TestViewModel();
        var ursaWindow = new TestWindow()
        {
            DataContext = vm,
        };
        ursaWindow.Show();
        vm.InvokeDialog("Custom Custom2", ursaWindow.GetHashCode());
        Dispatcher.UIThread.RunJobs();
        var dialog = ursaWindow.GetVisualDescendants().OfType<DefaultDialogControl>().SingleOrDefault();
        Assert.NotNull(dialog);
        Assert.Contains("Custom", dialog.Classes);
        Assert.Contains("Custom2", dialog.Classes);
        var okButton = dialog.GetVisualDescendants().OfType<Button>().SingleOrDefault(a=>a.Name == DefaultDialogControl.PART_OKButton);
        Assert.NotNull(okButton);
        Assert.Equal("CUSTOM", okButton.Content);
        Assert.Contains("Warning", okButton.Classes);
        Assert.Contains("Small", okButton.Classes);
        var cancelButton = dialog.GetVisualDescendants().OfType<Button>().SingleOrDefault(a=>a.Name == DefaultDialogControl.PART_CancelButton);
        Assert.NotNull(cancelButton);
        Assert.Equal("取消", cancelButton.Content);
        ursaWindow.Close();
        Dispatcher.UIThread.RunJobs();
    }
    
    [AvaloniaFact]
    public void StyleClass_Changes_Button_Content_With_No_StyleClass()
    {
        var vm = new TestViewModel();
        var ursaWindow = new TestWindow()
        {
            DataContext = vm,
        };
        ursaWindow.Show();
        vm.InvokeDialog(null, ursaWindow.GetHashCode());
        Dispatcher.UIThread.RunJobs();
        var dialog = ursaWindow.GetVisualDescendants().OfType<DefaultDialogControl>().SingleOrDefault();
        Assert.NotNull(dialog);
        Assert.DoesNotContain("Custom", dialog.Classes);
        var okButton = dialog.GetVisualDescendants().OfType<Button>().SingleOrDefault(a=>a.Name == DefaultDialogControl.PART_OKButton);
        Assert.NotNull(okButton);
        Assert.Equal("确定", okButton.Content);
        var cancelButton = dialog.GetVisualDescendants().OfType<Button>().SingleOrDefault(a=>a.Name == DefaultDialogControl.PART_CancelButton);
        Assert.NotNull(cancelButton);
        Assert.Equal("取消", cancelButton.Content);
        ursaWindow.Close();
        Dispatcher.UIThread.RunJobs();
    }
}