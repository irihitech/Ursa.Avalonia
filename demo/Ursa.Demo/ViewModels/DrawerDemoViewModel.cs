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
    [ObservableProperty] private bool _canLightDismiss;
    [ObservableProperty] private DialogResult? _defaultResult;
    [ObservableProperty] private bool _result;
    [ObservableProperty] private bool _isModal;
    [ObservableProperty] private DateTime? _date;
    
    
    public DrawerDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDefaultDialog);
        ShowCustomDialogCommand = new AsyncRelayCommand(ShowCustomDrawer);
    }
    
    private async Task ShowDefaultDialog()
    {
        var vm = new PlainDialogViewModel();
        if (IsModal)
        {
            DefaultResult = await Drawer.ShowModal<PlainDialog, PlainDialogViewModel>(
                vm,
                IsGlobal ? null : "LocalHost",
                new DrawerOptions()
                {
                    Title = "Please select a date",
                    Position = SelectedPosition,
                    Buttons = SelectedButton,
                    CanLightDismiss = CanLightDismiss,
                });
            Date = vm.Date;
        }
        else
        {
            Drawer.Show<PlainDialog, PlainDialogViewModel>(
                vm,
                IsGlobal ? null : "LocalHost",
                new DrawerOptions()
                {
                    Title = "Please select a date",
                    Position = SelectedPosition,
                    Buttons = SelectedButton,
                    CanLightDismiss = CanLightDismiss,
                });
        }
    }
    
    private async Task ShowCustomDrawer()
    {
        var vm = new DialogWithActionViewModel();
        if (IsModal)
        {
            Result = await Drawer.ShowCustomModal<DialogWithAction, DialogWithActionViewModel, bool>(
                vm,
                IsGlobal ? null : "LocalHost",
                new DrawerOptions()
                {
                    Position = SelectedPosition,
                    CanLightDismiss = CanLightDismiss,
                });
            Date = vm.Date;
        }
        else
        {
            Drawer.ShowCustom<DialogWithAction, DialogWithActionViewModel>(
                vm,
                IsGlobal ? null : "LocalHost",
                new DrawerOptions()
                {
                    Position = SelectedPosition,
                    CanLightDismiss = CanLightDismiss,
                });
        }
    }
}