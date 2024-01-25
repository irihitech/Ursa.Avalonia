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
    public ICommand ShowDefaultWindowCommand { get; }

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
        ShowDefaultWindowCommand = new AsyncRelayCommand(ShowDefaultWindow);
    }

    private async Task ShowDefaultWindow()
    {
        var result = await Dialog.ShowModalAsync<PlainDialog, PlainDialogViewModel>(new PlainDialogViewModel(),
            mode: DialogMode.Error, buttons: DialogButton.OKCancel, title:"确定取消预约吗？");
        Result = result;
        return;
    }

    private void ShowGlobalOverlayDialog()
    {
        OverlayDialog.ShowCustom<DialogWithAction, DialogWithActionViewModel>(new DialogWithActionViewModel());
    }

    private async Task ShowGlobalModalDialog()
    {
        var result = await Dialog.ShowCustomModalAsync<DialogWithAction, DialogWithActionViewModel, bool>(DialogViewModel);
        Result = result;
    }

    private async Task ShowGlobalOverlayModalDialog()
    {
        Result = await OverlayDialog.ShowModalAsync<PlainDialog, PlainDialogViewModel>(new PlainDialogViewModel(),
            title: "Please select a date", mode: DialogMode.Error, buttons: DialogButton.YesNoCancel);
    }

    private async Task ShowLocalOverlayModalDialog()
    {
        var vm = new DialogWithActionViewModel();
        var result = await OverlayDialog.ShowCustomModalAsync<DialogWithAction, DialogWithActionViewModel, bool>(
            vm, "LocalHost");
        Result = result;
    }
}