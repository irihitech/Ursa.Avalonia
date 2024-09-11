using System;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace Ursa.Demo.ViewModels;

public partial class NotificationDemoViewModel : ObservableObject
{
    public WindowNotificationManager? NotificationManager { get; set; }

    [ObservableProperty] private bool _showIcon = true;
    [ObservableProperty] private bool _showClose = true;

    [RelayCommand]
    public void ChangePosition(object obj)
    {
        if (obj is string s && NotificationManager is not null)
        {
            Enum.TryParse<NotificationPosition>(s, out var notificationPosition);
            NotificationManager.Position = notificationPosition;
        }
    }

    [RelayCommand]
    public void ShowNormal(object obj)
    {
        if (obj is not string s) return;
        Enum.TryParse<NotificationType>(s, out var notificationType);
        NotificationManager?.Show(
            new Notification("Welcome", "This is message"),
            showIcon: ShowIcon,
            showClose: ShowClose,
            type: notificationType);
    }

    [RelayCommand]
    public void ShowLight(object obj)
    {
        if (obj is not string s) return;
        Enum.TryParse<NotificationType>(s, out var notificationType);
        NotificationManager?.Show(
            new Notification("Welcome", "This is message"),
            showIcon: ShowIcon,
            showClose: ShowClose,
            type: notificationType,
            classes: ["Light"]);
    }
}