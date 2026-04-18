using System;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

namespace Ursa.Demo.ViewModels;

public partial class ToastDemoViewModel : ObservableObject
{
    public WindowToastManager? ToastManager { get; set; }

    [ObservableProperty] private bool _showIcon = true;
    [ObservableProperty] private bool _showClose = true;
    [ObservableProperty] private MessageCloseReason? _reason;

    [RelayCommand]
    public void ShowNormal(object obj)
    {
        if (obj is string s)
        {
            Enum.TryParse<NotificationType>(s, out var notificationType);
            ToastManager?.Show(
                new Toast("This is message"),
                showIcon: ShowIcon,
                showClose: ShowClose,
                type: notificationType,
                onClose: OnClose);
        }

        // ToastManager?.Show(new ToastDemoViewModel
        // {
        //     Content = "This is message",
        //     ToastManager = ToastManager
        // });
    }

    [RelayCommand]
    public void ShowLight(object obj)
    {
        if (obj is string s)
        {
            Enum.TryParse<NotificationType>(s, out var notificationType);
            ToastManager?.Show(
                new Toast("This is message"),
                showIcon: ShowIcon,
                showClose: ShowClose,
                type: notificationType,
                onClose: OnClose,
                classes: ["Light"]);
        }
    }
    
    private void OnClose(MessageCloseReason reason)
    {
        Reason = reason;
    }

    public string? Content { get; set; }

    [RelayCommand]
    public void YesCommand()
    {
        ToastManager?.Show(new Toast("Yes!"));
    }

    [RelayCommand]
    public void NoCommand()
    {
        ToastManager?.Show(new Toast("No!"));
    }
}
