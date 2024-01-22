using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;
using Ursa.Demo.Pages;

namespace Ursa.Demo.ViewModels;

public class DialogDemoViewModel: ObservableObject
{
    public ICommand ShowLocalOverlayDialogCommand { get; }
    public ICommand ShowGlobalOverlayDialogCommand { get; }
    public ICommand ShowGlobalDialogCommand { get; }

    public DialogDemoViewModel()
    {
        ShowLocalOverlayDialogCommand = new AsyncRelayCommand(ShowLocalOverlayDialog);
        ShowGlobalOverlayDialogCommand = new AsyncRelayCommand(ShowGlobalOverlayDialog);
        ShowGlobalDialogCommand = new AsyncRelayCommand(ShowGlobalDialog);
    }

    private async Task ShowGlobalDialog()
    {
        var result = await DialogBox.ShowAsync<ButtonGroupDemo, ButtonGroupDemoViewModel, string>(new ButtonGroupDemoViewModel());
    }

    private async Task ShowGlobalOverlayDialog()
    {
        await DialogBox.ShowOverlayAsync<Banner, DateTime>(DateTime.Now, "GlobalHost");
    }

    private async Task ShowLocalOverlayDialog()
    {
        await DialogBox.ShowOverlayAsync<Banner, DateTime>(DateTime.Now, "LocalHost");
    }
}