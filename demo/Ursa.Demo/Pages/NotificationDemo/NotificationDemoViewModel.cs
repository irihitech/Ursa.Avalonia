using System;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.NotificationDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DialogAndFeedbacksPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(NotificationDemo))]
public partial class NotificationDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "Notification";
    public const string Menu_Header = "Menu_Header_Notification";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Notification,
        Description = LanguageManager.Instance.Page_Description_Notification,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Notification)],
        Tags = ["Notification", "Alert", "Toast"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/NotificationDemo/NotificationDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/NotificationDemo/NotificationDemoViewModel.cs",
        InlineXamlSupport = false,
        MvvmSupport = true,
        AvaloniaExclusive = true,
    };

    public WindowNotificationManager? NotificationManager { get; set; }

    [ObservableProperty] public partial bool ShowIcon { get; set; } = true;
    [ObservableProperty] public partial bool ShowClose { get; set; } = true;

    [ObservableProperty] public partial MessageCloseReason? Reason { get; set; }

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
