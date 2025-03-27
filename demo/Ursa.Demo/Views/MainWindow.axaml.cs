using System.Threading.Tasks;
using Ursa.Controls;

namespace Ursa.Demo.Views;

public partial class MainWindow : UrsaWindow
{
    public WindowNotificationManager? NotificationManager { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        NotificationManager = new WindowNotificationManager(this) { MaxItems = 3 };
    }

    protected override async Task<bool> CanClose()
    {
        var result = await MessageBox.ShowOverlayAsync("Are you sure you want to exit?\n您确定要退出吗？", "Exit", button: MessageBoxButton.YesNo);
        return result == MessageBoxResult.Yes;
    }
}