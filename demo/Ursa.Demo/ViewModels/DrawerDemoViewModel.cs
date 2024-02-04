using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

namespace Ursa.Demo.ViewModels;

public class DrawerDemoViewModel: ObservableObject
{
    public ICommand OpenDrawerCommand { get; set; }

    public DrawerDemoViewModel()
    {
        OpenDrawerCommand = new AsyncRelayCommand(OpenDrawer);
    }

    private async Task OpenDrawer()
    {
        await Drawer.Show<Calendar, string, bool>("Hello World");
    }
}