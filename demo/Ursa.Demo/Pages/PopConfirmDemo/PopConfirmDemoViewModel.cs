using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.PopConfirmDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DialogAndFeedbacksPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(PopConfirmDemo))]
public class PopConfirmDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "PopConfirm";
    public const string Menu_Header = "Menu_Header_PopConfirm";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_PopConfirm,
        Description = LanguageManager.Instance.Page_Description_PopConfirm,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_PopConfirm)],
        Tags = ["PopConfirm", "Confirm", "Popup"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/PopConfirmDemo/PopConfirmDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/PopConfirmDemo/PopConfirmDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    public PopConfirmDemoViewModel()
    {
        AsyncConfirmCommand = new AsyncRelayCommand(OnConfirmAsync);
        AsyncCancelCommand = new RelayCommand(OnCancelAsync);
        ConfirmCommand = new RelayCommand(OnConfirm);
        CancelCommand = new RelayCommand(OnCancel);
    }

    internal WindowToastManager? ToastManager { get; set; }

    public ICommand ConfirmCommand { get; }
    public ICommand CancelCommand { get; }

    public ICommand AsyncConfirmCommand { get; }
    public ICommand AsyncCancelCommand { get; }

    private void OnCancel()
    {
        ToastManager?.Show(new Toast("Canceled"), NotificationType.Error, classes: ["Light"]);
    }

    private void OnConfirm()
    {
        ToastManager?.Show(new Toast("Confirmed"), NotificationType.Success, classes: ["Light"]);
    }

    private async Task OnConfirmAsync()
    {
        await Task.Delay(3000);
        ToastManager?.Show(new Toast("Async Confirmed"), NotificationType.Success, classes: ["Light"]);
    }

    private void OnCancelAsync()
    {
        ToastManager?.Show(new Toast("Async Canceled"), NotificationType.Error, classes: ["Light"]);
    }
}