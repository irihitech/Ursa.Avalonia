using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.AspectRatioLayoutDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(AspectRatioLayoutDemo))]
public class AspectRatioLayoutDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "AspectRatioLayout";
    public const string Menu_Header = "Menu_Header_AspectRatioLayout";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_AspectRatioLayout,
        Description = LanguageManager.Instance.Page_Description_AspectRatioLayout,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_AspectRatioLayout)],
        Tags = ["AspectRatioLayout", "Layout", "Ratio"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/AspectRatioLayoutDemo/AspectRatioLayoutDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/AspectRatioLayoutDemo/AspectRatioLayoutDemoViewModel.cs",
        InlineXamlSupport = true,
    };

}