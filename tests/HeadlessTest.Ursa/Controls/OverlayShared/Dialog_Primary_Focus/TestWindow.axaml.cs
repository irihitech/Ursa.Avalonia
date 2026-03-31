using Ursa.Controls;
using Ursa.Controls.Options;

namespace HeadlessTest.Ursa.Controls.OverlayShared.Dialog_Primary_Focus;

public partial class TestWindow : UrsaWindow
{
    public TestWindow()
    {
        InitializeComponent();
    }

    public void InvokeNormalDrawer()
    {
        OverlayDrawer.ShowStandardAsync<NormalDialog, object>("Hello World",
            options: new DrawerOptions() { TopLevelHashCode = GetHashCode() });
    }

    public void InvokeFocusDrawer()
    {
        OverlayDrawer.ShowStandardAsync<FocusDialog, object>("Hello World",
            options: new DrawerOptions() { TopLevelHashCode = GetHashCode() });
    }
    
    public void InvokeNormalDialog()
    {
        OverlayDialog.ShowStandardAsync<NormalDialog, object>("Hello World",
            options: new OverlayDialogOptions() { TopLevelHashCode = GetHashCode() });
    }

    public void InvokeFocusDialog()
    {
        OverlayDialog.ShowStandardAsync<FocusDialog, object>("Hello World",
            options: new OverlayDialogOptions() { TopLevelHashCode = GetHashCode() });
    }
}