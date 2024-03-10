using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Common;
using Ursa.Controls;
using Ursa.Demo.Dialogs;
using Ursa.Demo.Pages;

namespace Ursa.Demo.ViewModels;

public partial class DialogDemoViewModel: ObservableObject
{
    public ICommand ShowDialogCommand { get; set; }
    public ICommand ShowCustomDialogCommand { get; set; }

    [ObservableProperty] private DialogMode _selectedMode;
    [ObservableProperty] private DialogButton _selectedButton;
    [ObservableProperty] private bool _isWindow;
    [ObservableProperty] private bool _isGlobal;
    [ObservableProperty] private bool _isModal;
    [ObservableProperty] private bool _canLightDismiss;
    [ObservableProperty] private DialogResult? _defaultResult;
    [ObservableProperty] private bool _result;
    [ObservableProperty] private DateTime? _date;
    
    public DialogDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
        ShowCustomDialogCommand = new AsyncRelayCommand(ShowCustomDialog);
        IsModal = true;
        IsGlobal = true;
    }
    
    private async Task ShowDialog()
    {
        var vm = new PlainDialogViewModel();
        if (IsWindow)
        {
            DefaultResult = await Dialog.ShowModal<PlainDialog, PlainDialogViewModel>(
                vm, options: new DialogOptions()
                {
                    Title = "Please select a date",
                    Mode = SelectedMode,
                    Button = SelectedButton
                });
            Date = vm.Date;
        }
        else
        {
            if (IsModal)
            {
                DefaultResult = await OverlayDialog.ShowModal<PlainDialog, PlainDialogViewModel>(
                    vm,
                    IsGlobal ? null : "LocalHost",
                    new OverlayDialogOptions()
                    {
                        Title = "Please select a date",
                        Mode = SelectedMode,
                        Buttons = SelectedButton,
                        CanLightDismiss = CanLightDismiss,
                        HorizontalAnchor = HorizontalPosition.Right,
                        HorizontalOffset = 50,
                        VerticalAnchor = VerticalPosition.Top,
                        VerticalOffset = 50,
                    }
                );
                Date = vm.Date;
            }
            else
            {
                OverlayDialog.Show<PlainDialog, PlainDialogViewModel>(
                    new PlainDialogViewModel(),
                    IsGlobal ? null : "LocalHost",
                    new OverlayDialogOptions()
                    {
                        Title = "Please select a date",
                        Mode = SelectedMode,
                        Buttons = SelectedButton,
                        CanLightDismiss = CanLightDismiss,
                    });
            }
        }
        
    }
    
    private async Task ShowCustomDialog()
    {
        var vm = new DialogWithActionViewModel();
        if (IsWindow)
        {
            if (IsModal)
            {
                Result = await Dialog.ShowCustomModal<DialogWithAction, DialogWithActionViewModel, bool>(
                    vm);
                Date = vm.Date;
            }
            else
            {
                Dialog.ShowCustom<DialogWithAction, DialogWithActionViewModel>(
                    vm);
            }
        }
        else
        {
            if (IsModal)
            {
                Result = await OverlayDialog.ShowCustomModal<DialogWithAction, DialogWithActionViewModel, bool>(
                    vm, IsGlobal ? null : "LocalHost", options: new OverlayDialogOptions()
                    {
                        CanLightDismiss = CanLightDismiss,
                    });
                Date = vm.Date;
            }
            else
            {
                OverlayDialog.ShowCustom<DialogWithAction, DialogWithActionViewModel>(new DialogWithActionViewModel(),
                    IsGlobal ? null : "LocalHost",
                    options: new OverlayDialogOptions{ CanLightDismiss = CanLightDismiss });
            }
        }
        
    }
}