using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.OverlayShared.Dialog_Primary_Focus;

public partial class TestWindow : UrsaWindow
{
    public TestWindow()
    {
        InitializeComponent();
    }

    public void InvokeNormalDialog()
    {
        OverlayDialog.ShowModal<NormalDialog, object>("Hello World",
            options: new OverlayDialogOptions { TopLevelHashCode = GetHashCode() });
    }

    public void InvokeFocusDialog()
    {
        OverlayDialog.ShowModal<FocusDialog, object>("Hello World",
            options: new OverlayDialogOptions { TopLevelHashCode = GetHashCode() });
    }
}