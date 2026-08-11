using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.QrCodeDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(QrCodeDemo))]
public class QrCodeDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "QrCode";
    public const string Menu_Header = "Menu_Header_QrCode";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_QrCode,
        Description = LanguageManager.Instance.Page_Description_QrCode,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_QrCode)],
        Tags = ["QrCode", "Code", "Image"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/QrCodeDemo/QrCodeDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/QrCodeDemo/QrCodeDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}