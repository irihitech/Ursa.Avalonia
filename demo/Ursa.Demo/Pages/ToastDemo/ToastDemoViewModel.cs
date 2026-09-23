using System;
using System.Collections.ObjectModel;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;
using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;
using Ursa.Demo.ViewModels.Controls;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;

namespace Ursa.Demo.Pages.ToastDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DialogAndFeedbacksPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(ToastDemo))]
public partial class ToastDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "Toast";
    public const string Menu_Header = "Menu_Header_Toast";
    private const string ManagerSetupAnchorId = "toast-manager-setup";
    private const string TypesAnchorId = "toast-notification-types";
    private const string IconAnchorId = "toast-icon";
    private const string CloseButtonAnchorId = "toast-close-button";
    private const string ExpirationAnchorId = "toast-expiration";
    private const string LightStyleAnchorId = "toast-light-style";
    private const string ClickCallbackAnchorId = "toast-click-callback";
    private const string CloseReasonAnchorId = "toast-close-reason";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Toast,
        Description = LanguageManager.Instance.Page_Description_Toast,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Toast)],
        Tags = ["Toast", "Notification", "Message"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ToastDemo/ToastDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ToastDemo/ToastDemoViewModel.cs",
        InlineXamlSupport = false,
        MvvmSupport = true,
        AvaloniaExclusive = true,
    };

    public WindowToastManager? ToastManager { get; set; }

    public DemoSectionViewModel ManagerSetupSection { get; }
    public DemoSectionViewModel TypesSection { get; }
    public DemoSectionViewModel IconSection { get; }
    public DemoSectionViewModel CloseButtonSection { get; }
    public DemoSectionViewModel ExpirationSection { get; }
    public DemoSectionViewModel LightStyleSection { get; }
    public DemoSectionViewModel ClickCallbackSection { get; }
    public DemoSectionViewModel CloseReasonSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Manager_Setup_Header,
            AnchorId = ManagerSetupAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Types_Header,
            AnchorId = TypesAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Icon_Header,
            AnchorId = IconAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Close_Button_Header,
            AnchorId = CloseButtonAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Expiration_Header,
            AnchorId = ExpirationAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Light_Style_Header,
            AnchorId = LightStyleAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Click_Callback_Header,
            AnchorId = ClickCallbackAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Close_Reason_Header,
            AnchorId = CloseReasonAnchorId
        },
    ];

    [ObservableProperty] public partial bool ShowIcon { get; set; } = true;
    [ObservableProperty] public partial bool ShowClose { get; set; } = true;
    [ObservableProperty] public partial MessageCloseReason? Reason { get; set; }
    [ObservableProperty] public partial int ClickCount { get; set; }

    public ToastDemoViewModel()
    {
        ManagerSetupSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Manager_Setup_Header,
            Descriptions = { LanguageManager.Instance.Page_Toast_Section_Manager_Setup_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ManagerSetupAnchorId
        };
        AddSnippet(ManagerSetupSection, """
            private ToastDemoViewModel? _viewModel;

            protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
            {
                base.OnAttachedToVisualTree(e);
                if (DataContext is not ToastDemoViewModel vm) return;
                _viewModel = vm;
                _viewModel.ToastManager =
                    new WindowToastManager(TopLevel.GetTopLevel(this)) { MaxItems = 3 };
            }

            protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
            {
                base.OnDetachedFromVisualTree(e);
                _viewModel?.ToastManager?.Uninstall();
            }
            """, LanguageManager.Instance.DemoSection_Tab_ViewCode);
        AddSnippet(ManagerSetupSection, """
            public WindowToastManager? ToastManager { get; set; }
            """);

        TypesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Types_Header,
            Descriptions = { LanguageManager.Instance.Page_Toast_Section_Types_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = TypesAnchorId
        };
        AddSnippet(TypesSection, """
            ToastManager.Show(
                new Toast("File saved successfully."),
                NotificationType.Success);
            """);

        IconSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Icon_Header,
            Descriptions = { LanguageManager.Instance.Page_Toast_Section_Icon_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = IconAnchorId
        };
        AddSnippet(IconSection, """
            ToastManager.Show(
                new Toast("Information"),
                NotificationType.Information,
                showIcon: false);
            """);

        CloseButtonSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Close_Button_Header,
            Descriptions = { LanguageManager.Instance.Page_Toast_Section_Close_Button_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = CloseButtonAnchorId
        };
        AddSnippet(CloseButtonSection, """
            ToastManager.Show(
                new Toast("Dismiss me"),
                NotificationType.Information,
                showClose: false);
            """);

        ExpirationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Expiration_Header,
            Descriptions = { LanguageManager.Instance.Page_Toast_Section_Expiration_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ExpirationAnchorId
        };
        AddSnippet(ExpirationSection, """
            ToastManager.Show(
                new Toast("Closes after five seconds"),
                NotificationType.Information,
                expiration: TimeSpan.FromSeconds(5));

            ToastManager.Show(
                new Toast("Close me manually"),
                NotificationType.Information,
                expiration: TimeSpan.Zero);
            """);

        LightStyleSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Light_Style_Header,
            Descriptions = { LanguageManager.Instance.Page_Toast_Section_Light_Style_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = LightStyleAnchorId
        };
        AddSnippet(LightStyleSection, """
            ToastManager.Show(
                new Toast("Light style message"),
                NotificationType.Success,
                classes: ["Light"]);
            """);

        ClickCallbackSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Click_Callback_Header,
            Descriptions = { LanguageManager.Instance.Page_Toast_Section_Click_Callback_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ClickCallbackAnchorId
        };
        AddSnippet(ClickCallbackSection, """
            ToastManager.Show(new Toast(
                "Click to open the update page",
                onClick: () => OpenUpdatePage()));
            """);

        CloseReasonSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Toast_Section_Close_Reason_Header,
            Descriptions = { LanguageManager.Instance.Page_Toast_Section_Close_Reason_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = CloseReasonAnchorId
        };
        AddSnippet(CloseReasonSection, """
            ToastManager.Show(new Toast(
                "Close me",
                expiration: TimeSpan.Zero,
                onClose: reason => CloseReason = reason));
            """);
    }

    [RelayCommand]
    public void ShowType(object obj)
    {
        if (obj is string value && Enum.TryParse<NotificationType>(value, out var type))
        {
            ToastManager?.Show(new Toast("This is message"), type);
        }
    }

    [RelayCommand]
    public void ShowWithIcon()
    {
        ToastManager?.Show(
            new Toast("This is message"),
            NotificationType.Information,
            showIcon: ShowIcon);
    }

    [RelayCommand]
    public void ShowWithCloseButton()
    {
        ToastManager?.Show(
            new Toast("This is message"),
            NotificationType.Information,
            showClose: ShowClose);
    }

    [RelayCommand]
    public void ShowTransient()
    {
        ToastManager?.Show(
            new Toast("This toast closes after five seconds"),
            NotificationType.Information,
            expiration: TimeSpan.FromSeconds(5));
    }

    [RelayCommand]
    public void ShowPersistent()
    {
        ToastManager?.Show(
            new Toast("Close me manually"),
            NotificationType.Information,
            expiration: TimeSpan.Zero);
    }

    [RelayCommand]
    public void ShowLight(object obj)
    {
        if (obj is string value && Enum.TryParse<NotificationType>(value, out var type))
        {
            ToastManager?.Show(
                new Toast("This is message"),
                type,
                showIcon: true,
                showClose: true,
                classes: ["Light"]);
        }
    }

    [RelayCommand]
    public void ShowClickCallback()
    {
        ClickCount = 0;
        ToastManager?.Show(new Toast(
            "Click this toast",
            onClick: () => ClickCount++));
    }

    [RelayCommand]
    public void ShowCloseReason()
    {
        Reason = null;
        ToastManager?.Show(new Toast(
            "Close me",
            expiration: TimeSpan.Zero,
            onClose: OnClose));
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
