using System;
using System.Collections.ObjectModel;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;
using Ursa.Controls;
using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;
using Ursa.Demo.ViewModels.Controls;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace Ursa.Demo.Pages.NotificationDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DialogAndFeedbacksPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(NotificationDemo))]
public partial class NotificationDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "Notification";
    public const string Menu_Header = "Menu_Header_Notification";
    private const string ManagerWiringAnchorId = "notification-manager-wiring";
    private const string PositionAnchorId = "notification-position";
    private const string IconAnchorId = "notification-icon";
    private const string ShowCloseAnchorId = "notification-show-close";
    private const string LightStyleAnchorId = "notification-light-style";
    private const string CloseReasonAnchorId = "notification-close-reason";

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

    public DemoSectionViewModel ManagerWiringSection { get; }
    public DemoSectionViewModel PositionSection { get; }
    public DemoSectionViewModel IconSection { get; }
    public DemoSectionViewModel ShowCloseSection { get; }
    public DemoSectionViewModel LightStyleSection { get; }
    public DemoSectionViewModel CloseReasonSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_Notification_Section_Manager_Wiring_Header,
            AnchorId = ManagerWiringAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Notification_Section_Position_Header,
            AnchorId = PositionAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Notification_Section_Icon_Header,
            AnchorId = IconAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Notification_Section_Show_Close_Header,
            AnchorId = ShowCloseAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Notification_Section_Light_Style_Header,
            AnchorId = LightStyleAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Notification_Section_Close_Reason_Header,
            AnchorId = CloseReasonAnchorId
        },
    ];

    [ObservableProperty] public partial bool ShowIcon { get; set; } = true;
    [ObservableProperty] public partial bool ShowClose { get; set; } = true;
    [ObservableProperty] public partial MessageCloseReason? Reason { get; set; }

    public NotificationDemoViewModel()
    {
        ManagerWiringSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Notification_Section_Manager_Wiring_Header,
            Descriptions = { LanguageManager.Instance.Page_Notification_Section_Manager_Wiring_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ManagerWiringAnchorId
        };
        AddSnippet(ManagerWiringSection, """
            var topLevel = TopLevel.GetTopLevel(this);
            if (DataContext is NotificationDemoViewModel viewModel)
            {
                viewModel.NotificationManager =
                    WindowNotificationManager.TryGetNotificationManager(topLevel, out var manager)
                        ? manager
                        : new WindowNotificationManager(topLevel);
            }
            """, LanguageManager.Instance.DemoSection_Tab_ViewCode);
        AddSnippet(ManagerWiringSection, """
            using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

            public WindowNotificationManager? NotificationManager { get; set; }
            """);

        PositionSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Notification_Section_Position_Header,
            Descriptions = { LanguageManager.Instance.Page_Notification_Section_Position_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = PositionAnchorId
        };
        AddSnippet(PositionSection, """
            notificationManager.Position = NotificationPosition.BottomRight;
            """);

        IconSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Notification_Section_Icon_Header,
            Descriptions = { LanguageManager.Instance.Page_Notification_Section_Icon_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = IconAnchorId
        };
        AddSnippet(IconSection, """
            notificationManager.Show(
                new Notification("Welcome", "This is message"),
                NotificationType.Information,
                showIcon: false);
            """);

        ShowCloseSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Notification_Section_Show_Close_Header,
            Descriptions = { LanguageManager.Instance.Page_Notification_Section_Show_Close_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ShowCloseAnchorId
        };
        AddSnippet(ShowCloseSection, """
            notificationManager.Show(
                new Notification("Welcome", "This is message"),
                NotificationType.Information,
                showClose: false);
            """);

        LightStyleSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Notification_Section_Light_Style_Header,
            Descriptions = { LanguageManager.Instance.Page_Notification_Section_Light_Style_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = LightStyleAnchorId
        };
        AddSnippet(LightStyleSection, """
            notificationManager.Show(
                new Notification("Welcome", "This is message"),
                NotificationType.Information,
                classes: ["Light"]);
            """);

        CloseReasonSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Notification_Section_Close_Reason_Header,
            Descriptions = { LanguageManager.Instance.Page_Notification_Section_Close_Reason_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = CloseReasonAnchorId
        };
        AddSnippet(CloseReasonSection, """
            notificationManager.Show(
                new Notification("Welcome", "Close me", expiration: TimeSpan.Zero),
                NotificationType.Information,
                onClose: reason => CloseReason = reason);
            """);
    }

    [RelayCommand]
    public void ChangePosition(object obj)
    {
        if (obj is string value && Enum.TryParse<NotificationPosition>(value, out var position))
        {
            if (NotificationManager is { } manager)
            {
                manager.Position = position;
            }
        }
    }

    [RelayCommand]
    public void ShowNormal(object obj)
    {
        if (obj is not string value || !Enum.TryParse<NotificationType>(value, out var type))
        {
            return;
        }

        NotificationManager?.Show(
            new Notification("Welcome", "This is message"),
            type,
            showIcon: ShowIcon,
            showClose: ShowClose,
            onClose: OnClose);
    }

    [RelayCommand]
    public void ShowLight(object obj)
    {
        if (obj is not string value || !Enum.TryParse<NotificationType>(value, out var type))
        {
            return;
        }

        NotificationManager?.Show(
            new Notification("Welcome", "This is message"),
            type,
            showIcon: ShowIcon,
            showClose: ShowClose,
            onClose: OnClose,
            classes: ["Light"]);
    }

    [RelayCommand]
    public void ShowCloseReason()
    {
        Reason = null;
        NotificationManager?.Show(
            new Notification("Welcome", "Close me", expiration: TimeSpan.Zero),
            NotificationType.Information,
            showIcon: ShowIcon,
            showClose: true,
            onClose: OnClose);
    }

    private void OnClose(MessageCloseReason reason)
    {
        Reason = reason;
    }

    private static void AddSnippet(
        DemoSectionViewModel section,
        string code,
        IObservable<string?>? tabName = null)
    {
        section.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = tabName ?? LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = code
        });
    }
}
