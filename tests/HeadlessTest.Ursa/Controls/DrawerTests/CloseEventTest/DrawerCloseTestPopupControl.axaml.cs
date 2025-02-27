using Avalonia.Controls;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls;

public partial class DrawerCloseTestPopupControl : UserControl
{
    public DrawerCloseTestPopupControl()
    {
        InitializeComponent();
        _overlayDialogHost.HostId = _hostid;
    }

    private readonly string _hostid = Path.GetRandomFileName();
    public DrawerCloseTestPopupControl? Popup { get; set; }
    public int LResult { get; set; }
    public int RResult { get; set; }

    public async void OpenPopup()
    {
        var vm = new DrawerCloseTestPopupControlVM();
        LResult = vm.Result;
        RResult = await Drawer.ShowCustomModal<int>(Popup = new(), vm, _hostid);
    }

    public void ClosePopup()
    {
        (Popup?.DataContext as DrawerCloseTestPopupControlVM)?.Close();
    }


    public void Close()
    {
        (DataContext as DrawerCloseTestPopupControlVM)?.Close();
    }
}