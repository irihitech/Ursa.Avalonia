using System;
using System.Collections.ObjectModel;
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
    public ICommand ShowDialogCommand { get; set; }
    public ICommand ShowCustomDialogCommand { get; set; }

    [ObservableProperty] private Position _selectedPosition;
    [ObservableProperty] private DialogButton _selectedButton;
    [ObservableProperty] private bool _isGlobal;
    [ObservableProperty] private bool _canCloseMaskToClose;
    [ObservableProperty] private DialogResult? _defaultResult;
    [ObservableProperty] private bool _result;
    [ObservableProperty] private bool _showMask;
    [ObservableProperty] private DateTime? _date;
    
    
    public DrawerDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDefaultDialog);
        ShowCustomDialogCommand = new AsyncRelayCommand(ShowCustomDrawer);
    }
    
    private async Task ShowDefaultDialog()
    {
        var vm = new PlainDialogViewModel();
        DefaultResult = await Drawer.Show<PlainDialog, PlainDialogViewModel>(
            vm,
            IsGlobal ? null : "LocalHost",
            new DefaultDrawerOptions()
            {
                Title = "Please select a date",
                Position = SelectedPosition,
                Buttons = SelectedButton,
                CanClickOnMaskToClose = CanCloseMaskToClose,
                ShowMask = ShowMask,
            });
        Date = vm.Date;
    }
    
    private async Task ShowCustomDrawer()
    {
        var vm = new DialogWithActionViewModel();
        Result = await Drawer.ShowCustom<DialogWithAction, DialogWithActionViewModel, bool>(
            vm,
            IsGlobal ? null : "LocalHost",
            new CustomDrawerOptions()
            {
                Position = SelectedPosition,
                CanClickOnMaskToClose = CanCloseMaskToClose,
                ShowMask = ShowMask,
            });
        Date = vm.Date;
    }
}