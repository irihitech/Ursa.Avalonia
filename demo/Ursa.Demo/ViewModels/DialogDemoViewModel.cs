using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    }

    private void ShowGlobalOverlayDialog()
    {
        DialogBox.ShowOverlay<DialogWithAction, DialogWithActionViewModel>(new DialogWithActionViewModel(), "GlobalHost");
    }

    private async Task ShowGlobalModalDialog()
    {
        var result = await DialogBox.ShowModalAsync<DialogWithAction, DialogWithActionViewModel, bool>(DialogViewModel);
        Result = result;
    }

    private async Task ShowGlobalOverlayModalDialog()
    {
        Result = await DialogBox.ShowOverlayModalAsync<DialogWithAction, DialogWithActionViewModel, bool>(DialogViewModel, "GlobalHost");
    }

    private async Task ShowLocalOverlayModalDialog()
    {
        var vm = new DialogWithActionViewModel();
        var result = await DialogBox.ShowOverlayModalAsync<DialogWithAction, DialogWithActionViewModel, bool>(
            DialogViewModel, "LocalHost", new DialogOptions(){ ExtendToClientArea = true });
        Result = result;
    }
}