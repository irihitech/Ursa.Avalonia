using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;
using Ursa.Demo.Dialogs;

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
    [ObservableProperty] private bool _fullScreen;
    [ObservableProperty] private bool _showInTaskBar;

    public ObservableCollection<DialogButton> DialogButtons =>
    [
        DialogButton.None,
        DialogButton.OK,
        DialogButton.OKCancel,
        DialogButton.YesNo,
        DialogButton.YesNoCancel,
    ];

    public ObservableCollection<DialogMode> DialogModes =>
    [
        DialogMode.Info,
        DialogMode.Warning,
        DialogMode.Error,
        DialogMode.Question,
        DialogMode.None,
        DialogMode.Success,
    ];
    
    public DialogDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
        ShowCustomDialogCommand = new AsyncRelayCommand(ShowCustomDialog);
        IsModal = true;
        IsGlobal = true;
        ShowInTaskBar = false;
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
                    Button = SelectedButton,
                    ShowInTaskBar = ShowInTaskBar,
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
                        FullScreen = FullScreen,
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
                        FullScreen = FullScreen,
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
                    vm,
                    options: new DialogOptions
                    {
                        ShowInTaskBar = ShowInTaskBar
                    });
                Date = vm.Date;
            }
            else
            {
                Dialog.ShowCustom<DialogWithAction, DialogWithActionViewModel>(
                    vm,
                    options: new DialogOptions
                    {
                        ShowInTaskBar = ShowInTaskBar
                    });
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
                        FullScreen = FullScreen,
                    });
                Date = vm.Date;
            }
            else
            {
                OverlayDialog.ShowCustom<DialogWithAction, DialogWithActionViewModel>(new DialogWithActionViewModel(),
                    IsGlobal ? null : "LocalHost",
                    options: new OverlayDialogOptions{ CanLightDismiss = CanLightDismiss, FullScreen = FullScreen});
            }
        }
        
    }
}