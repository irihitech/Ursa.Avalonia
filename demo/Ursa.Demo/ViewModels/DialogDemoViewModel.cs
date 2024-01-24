using System;
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
    public ICommand ShowLocalOverlayModalDialogCommand { get; }
    public ICommand ShowGlobalOverlayModalDialogCommand { get; }
    public ICommand ShowGlobalModalDialogCommand { get; }
    
    public ICommand ShowGlobalOverlayDialogCommand { get; }
    
    public ICommand ShowPlainGlobalOverlayDialogCommand { get; }

    private object? _result;

    public object? Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }

    private DateTime _date;

    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    public DialogWithActionViewModel DialogViewModel { get; set; } = new DialogWithActionViewModel();

    public DialogDemoViewModel()
    {
        ShowLocalOverlayModalDialogCommand = new AsyncRelayCommand(ShowLocalOverlayModalDialog);
        ShowGlobalOverlayModalDialogCommand = new AsyncRelayCommand(ShowGlobalOverlayModalDialog);
        ShowGlobalModalDialogCommand = new AsyncRelayCommand(ShowGlobalModalDialog);
        ShowGlobalOverlayDialogCommand = new RelayCommand(ShowGlobalOverlayDialog);
        ShowPlainGlobalOverlayDialogCommand = new AsyncRelayCommand(ShowPlainGlobalOverlayDialog);
    }

    private void ShowGlobalOverlayDialog()
    {
        OverlayDialog.Show<DialogWithAction, DialogWithActionViewModel>(new DialogWithActionViewModel());
    }

    private async Task ShowGlobalModalDialog()
    {
        var result = await Dialog.ShowModalAsync<DialogWithAction, DialogWithActionViewModel, bool>(DialogViewModel);
        Result = result;
    }

    private async Task ShowGlobalOverlayModalDialog()
    {
        Result = await OverlayDialog.ShowModalAsync<DialogWithAction, DialogWithActionViewModel, bool>(DialogViewModel);
    }

    private async Task ShowLocalOverlayModalDialog()
    {
        var vm = new DialogWithActionViewModel();
        var result = await OverlayDialog.ShowModalAsync<DialogWithAction, DialogWithActionViewModel, bool>(
            DialogViewModel, new DialogOptions() { ExtendToClientArea = true}, "LocalHost");
        Result = result;
    }
    
    public async Task ShowPlainGlobalOverlayDialog()
    {
        var result = await OverlayDialog.ShowModalAsync<PlainDialog, PlainDialogViewModel, object?>(
            new PlainDialogViewModel(),
            new DialogOptions()
            {
                DefaultButtons = DialogButton.OKCancel,
            }, 
            "LocalHost");
    }
}