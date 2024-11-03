using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using Ursa.Controls;

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
        vm.InvokeDialog();
        var dialog = ursaWindow.GetVisualDescendants().OfType<DefaultDialogControl>().SingleOrDefault();
        Assert.NotNull(dialog);
        Assert.Contains("Custom", dialog.Classes);
        var okButton = dialog.GetVisualDescendants().OfType<Button>().SingleOrDefault(a=>a.Name == DefaultDialogControl.PART_OKButton);
        Assert.NotNull(okButton);
        Assert.Equal("CUSTOM", okButton.Content);
        var cancelButton = dialog.GetVisualDescendants().OfType<Button>().SingleOrDefault(a=>a.Name == DefaultDialogControl.PART_CancelButton);
        Assert.NotNull(cancelButton);
        Assert.Equal("取消", cancelButton.Content);
    }
}