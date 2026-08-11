using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.ThemeVariantMapperDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(ThemeVariantMapperDemo))]
public class ThemeVariantMapperDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "ThemeVariantMapper";
    public const string Menu_Header = "Menu_Header_ThemeVariantMapper";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_ThemeVariantMapper,
        Description = LanguageManager.Instance.Page_Description_ThemeVariantMapper,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_ThemeVariantMapper)],
        Tags = ["ThemeVariantMapper", "Theme", "Color"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ThemeVariantMapperDemo/ThemeVariantMapperDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ThemeVariantMapperDemo/ThemeVariantMapperDemoViewModel.cs",
        InlineXamlSupport = true,
    };

}

