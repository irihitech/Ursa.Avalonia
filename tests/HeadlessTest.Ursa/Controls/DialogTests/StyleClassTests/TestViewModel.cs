using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.DialogTests.StyleClassTests;

public class TestViewModel: ObservableObject
{
    public void InvokeDialog(string? classes, int? hash)
    {
        OverlayDialog.Show<TextBlock, string>("Hello World", options: new OverlayDialogOptions()
        {
            Buttons = DialogButton.OKCancel,
            StyleClass = classes,
            TopLevelHashCode = hash,
        });
    }
}