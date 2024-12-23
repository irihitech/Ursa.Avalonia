using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Avalonia.Shared.Contracts;
using Ursa.Controls;

namespace Ursa.Demo.Dialogs;

public partial class CustomDemoDialogViewModel : ObservableObject, IDialogContext
{
    [ObservableProperty] private string? _city;
    [ObservableProperty] private string? _department;
    [ObservableProperty] private string? _owner;
    [ObservableProperty] private string? _target;
    public WindowNotificationManager? NotificationManager { get; set; }
    public WindowToastManager? ToastManager { get; set; }

    public CustomDemoDialogViewModel()
    {
        Cities =
        [
            "Shanghai", "Beijing", "Hulunbuir", "Shenzhen", "Hangzhou", "Nanjing", "Chengdu", "Wuhan", "Chongqing",
            "Suzhou", "Tianjin", "Xi'an", "Qingdao", "Dalian"
        ];
        OKCommand = new RelayCommand(OK);
        CancelCommand = new RelayCommand(Cancel);
        DialogCommand = new AsyncRelayCommand(ShowDialog);
    }

    public ObservableCollection<string> Cities { get; set; }

    public void Close()
    {
        RequestClose?.Invoke(this, null);
    }

    public event EventHandler<object?>? RequestClose;

    public ICommand OKCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public ICommand DialogCommand { get; set; }

    private void OK()
    {
        RequestClose?.Invoke(this, true);
    }

    private void Cancel()
    {
        RequestClose?.Invoke(this, false);
    }

    private async Task ShowDialog()
    {
        await OverlayDialog.ShowCustomModal<CustomDemoDialog, CustomDemoDialogViewModel, bool>(
            new CustomDemoDialogViewModel());
    }

    [RelayCommand]
    private void ShowToast(object obj)
    {
        ToastManager?.Show("This is a Toast message");
    }

    [RelayCommand]
    private void ShowNotification(object obj)
    {
        NotificationManager?.Show("This is a Notification message");
    }
}