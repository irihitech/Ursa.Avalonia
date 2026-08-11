using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.MarqueeDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(MarqueeDemo))]
public class MarqueeDemoViewModel: ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "Marquee";
    public const string Menu_Header = "Menu_Header_Marquee";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Marquee,
        Description = LanguageManager.Instance.Page_Description_Marquee,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Marquee)],
        Tags = ["Marquee", "Scroll", "Text"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/MarqueeDemo/MarqueeDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/MarqueeDemo/MarqueeDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}