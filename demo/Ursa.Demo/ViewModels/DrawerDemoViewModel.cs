using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Common;
using Ursa.Controls;
using Ursa.Controls.Options;
using Ursa.Demo.Dialogs;

namespace Ursa.Demo.ViewModels;

public partial class DrawerDemoViewModel: ObservableObject
{
    public ICommand OpenDrawerCommand { get; set; }
    public ICommand OpenDefaultDrawerCommand { get; set; }
    
    [ObservableProperty] private Position _selectedPosition;
    

    public DrawerDemoViewModel()
    {
        OpenDrawerCommand = new AsyncRelayCommand(OpenDrawer);
        OpenDefaultDrawerCommand = new AsyncRelayCommand(OpenDefaultDrawer);
        SelectedPosition = Position.Right;
    }

    private Task OpenDefaultDrawer()
    {
        return Drawer.Show<PlainDialog, PlainDialogViewModel, bool>(new PlainDialogViewModel(), new DefaultDrawerOptions()
        {
            Buttons = DialogButton.OKCancel,
            Position = SelectedPosition,
            Title = "Please select a date",
            CanClickOnMaskToClose = false,
        });
    }

    private async Task OpenDrawer()
    {
        await Drawer.ShowCustom<DialogWithAction, DialogWithActionViewModel, bool>(new DialogWithActionViewModel(),
            new CustomDrawerOptions() { Position = SelectedPosition, MinWidth = 400, MinHeight = 400});
    }
}