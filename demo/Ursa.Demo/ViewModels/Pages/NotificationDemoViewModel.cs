using System;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class NotificationDemoViewModel : ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "Notification",
        Description = "Notification shows informational messages in a non-blocking overlay.",
        Breadcrumbs = ["Dialog & Feedbacks", "Notification"],
        Tags = ["Notification", "Alert", "Toast"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/NotificationDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/NotificationDemoViewModel.cs",
        InlineXamlSupport = false,
        MvvmSupport = true,
        AvaloniaExclusive = true,
    };

    public WindowNotificationManager? NotificationManager { get; set; }

    [ObservableProperty] private bool _showIcon = true;
    [ObservableProperty] private bool _showClose = true;

    [ObservableProperty] private MessageCloseReason? _reason;

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
            type: notificationType,
            onClose: OnClose);
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
            onClose: OnClose,
            classes: ["Light"]);
    }

    private void OnClose(MessageCloseReason reason)
    {
        Reason = reason;
    }
}
