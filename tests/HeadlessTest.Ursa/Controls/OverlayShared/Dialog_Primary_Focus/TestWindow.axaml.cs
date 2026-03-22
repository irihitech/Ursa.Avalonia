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
        Drawer.ShowModal<NormalDialog, object>("Hello World",
            options: new DrawerOptions() { TopLevelHashCode = GetHashCode() });
    }

    public void InvokeFocusDrawer()
    {
        Drawer.ShowModal<FocusDialog, object>("Hello World",
            options: new DrawerOptions() { TopLevelHashCode = GetHashCode() });
    }
    
    public void InvokeNormalDialog()
    {
        OverlayDialog.ShowDefaultAsync<NormalDialog, object>("Hello World",
            options: new OverlayDialogOptions() { TopLevelHashCode = GetHashCode() });
    }

    public void InvokeFocusDialog()
    {
        OverlayDialog.ShowDefaultAsync<FocusDialog, object>("Hello World",
            options: new OverlayDialogOptions() { TopLevelHashCode = GetHashCode() });
    }
}