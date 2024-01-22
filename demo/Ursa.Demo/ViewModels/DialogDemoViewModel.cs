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
    public ICommand ShowLocalOverlayDialogCommand { get; }
    public ICommand ShowGlobalOverlayDialogCommand { get; }
    public ICommand ShowGlobalDialogCommand { get; }

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
        ShowLocalOverlayDialogCommand = new AsyncRelayCommand(ShowLocalOverlayDialog);
        ShowGlobalOverlayDialogCommand = new AsyncRelayCommand(ShowGlobalOverlayDialog);
        ShowGlobalDialogCommand = new AsyncRelayCommand(ShowGlobalDialog);
    }

    private async Task ShowGlobalDialog()
    {
        var result = await DialogBox.ShowAsync<DialogWithAction, DialogWithActionViewModel, bool>(new DialogWithActionViewModel());
        Result = result;
    }

    private async Task ShowGlobalOverlayDialog()
    {
        await DialogBox.ShowOverlayAsync<DialogWithAction, DialogWithActionViewModel, bool>(new DialogWithActionViewModel(), "GlobalHost");
    }

    private async Task ShowLocalOverlayDialog()
    {
        var vm = new DialogWithActionViewModel();
        var result = await DialogBox.ShowOverlayAsync<DialogWithAction, DialogWithActionViewModel, bool>(
            DialogViewModel, "LocalHost");
        Date = vm.Date;
        Result = result;
    }
}