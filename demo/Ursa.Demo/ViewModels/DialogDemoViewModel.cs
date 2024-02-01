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

public class DialogDemoViewModel: ObservableObject
{
    public ICommand ShowDialogCommand { get; set; }
    public ICommand ShowCustomDialogCommand { get; set; }

    private DialogMode _selectedMode;
    public DialogMode SelectedMode
    {
        get => _selectedMode;
        set => SetProperty(ref _selectedMode, value);
    }
    
    public ObservableCollection<DialogMode> Modes { get; set; }
    
    private DialogButton _selectedButton;
    public DialogButton SelectedButton
    {
        get => _selectedButton;
        set => SetProperty(ref _selectedButton, value);
    }
    
    public ObservableCollection<DialogButton> Buttons { get; set; }

    private bool _isWindow;
    public bool IsWindow
    {
        get => _isWindow;
        set => SetProperty(ref _isWindow, value);
    }
    
    private bool _isGlobal;
    public bool IsGlobal
    {
        get => _isGlobal;
        set => SetProperty(ref _isGlobal, value);
    }

    private bool _isModal;
    public bool IsModal
    {
        get => _isModal;
        set => SetProperty(ref _isModal, value);
    }

    private DialogResult? _defaultResult;
    public DialogResult? DefaultResult
    {
        get => _defaultResult;
        set => SetProperty(ref _defaultResult, value);
    }

    private bool _result;
    public bool Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }

    private DateTime? _date;
    public DateTime? Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }
    
    
    public DialogDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
        ShowCustomDialogCommand = new AsyncRelayCommand(ShowCustomDialog);
        Modes = new ObservableCollection<DialogMode>(Enum.GetValues<DialogMode>());
        Buttons = new ObservableCollection<DialogButton>(Enum.GetValues<DialogButton>());
    }
    
    private async Task ShowDialog()
    {
        var vm = new PlainDialogViewModel();
        if (IsWindow)
        {
            DefaultResult = await Dialog.ShowModalAsync<PlainDialog, PlainDialogViewModel>(
                vm,
                "Please select a date",
                SelectedMode,
                SelectedButton);
            Date = vm.Date;
        }
        else
        {
            if (IsModal)
            {
                DefaultResult = await OverlayDialog.ShowModalAsync<PlainDialog, PlainDialogViewModel>(
                    vm,
                    IsGlobal ? null : "LocalHost",
                    "Please select a date",
                    SelectedMode,
                    SelectedButton
                );
                Date = vm.Date;
            }
            else
            {
                OverlayDialog.Show<PlainDialog, PlainDialogViewModel>(
                    new PlainDialogViewModel(),
                    IsGlobal ? null : "LocalHost",
                    "Please select a date",
                    SelectedMode,
                    SelectedButton);
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
                Result = await Dialog.ShowCustomModalAsync<DialogWithAction, DialogWithActionViewModel, bool>(
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
                Result = await OverlayDialog.ShowCustomModalAsync<DialogWithAction, DialogWithActionViewModel, bool>(
                    vm, IsGlobal ? null : "LocalHost");
                Date = vm.Date;
            }
            else
            {
                OverlayDialog.ShowCustom<DialogWithAction, DialogWithActionViewModel>(new DialogWithActionViewModel(),
                    IsGlobal ? null : "LocalHost");
            }
        }
        
    }
}