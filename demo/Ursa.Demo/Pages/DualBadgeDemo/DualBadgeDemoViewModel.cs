using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.DualBadgeDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(DualBadgeDemo))]
public class DualBadgeDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "DualBadge";
    public const string Menu_Header = "Menu_Header_DualBadge";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DualBadge,
        Description = LanguageManager.Instance.Page_Description_DualBadge,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DualBadge)],
        Tags = ["DualBadge", "Badge", "Label"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DualBadgeDemo/DualBadgeDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DualBadgeDemo/DualBadgeDemoViewModel.cs",
        InlineXamlSupport = true,
    };

}