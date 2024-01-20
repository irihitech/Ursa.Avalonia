using System;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

namespace Ursa.Demo.ViewModels;

public class DialogDemoViewModel: ObservableObject
{
    public ICommand ShowLocalOverlayDialogCommand { get; }
    public ICommand ShowGlobalOverlayDialogCommand { get; }

    public DialogDemoViewModel()
    {
        ShowLocalOverlayDialogCommand = new RelayCommand(ShowLocalOverlayDialog);
        ShowGlobalOverlayDialogCommand = new RelayCommand(ShowGlobalOverlayDialog);
    }

    private async void ShowGlobalOverlayDialog()
    {
        await DialogBox.ShowOverlayAsync<Banner, DateTime>(DateTime.Now, "GlobalHost");
    }

    private async void ShowLocalOverlayDialog()
    {
        await DialogBox.ShowOverlayAsync<Banner, DateTime>(DateTime.Now, "LocalHost");
    }
}