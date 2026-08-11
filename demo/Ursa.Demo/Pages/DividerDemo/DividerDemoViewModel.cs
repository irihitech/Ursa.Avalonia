using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.DividerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(DividerDemo))]
public class DividerDemoViewModel: ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "Divider";
    public const string Menu_Header = "Menu_Header_Divider";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Divider,
        Description = LanguageManager.Instance.Page_Description_Divider,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Divider)],
        Tags = ["Divider", "Separator", "Layout"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DividerDemo/DividerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DividerDemo/DividerDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}