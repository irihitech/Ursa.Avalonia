using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.ThemeTogglerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(ThemeTogglerDemo))]
public class ThemeTogglerDemoViewModel : IPageMetadataProvider
{
    public const string Category_Key = "ThemeToggler";
    public const string Menu_Header = "Menu_Header_ThemeToggler";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_ThemeToggler,
        Description = LanguageManager.Instance.Page_Description_ThemeToggler,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_ThemeToggler)],
        Tags = ["ThemeToggler", "Theme", "Toggle"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ThemeTogglerDemo/ThemeTogglerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ThemeTogglerDemo/ThemeTogglerDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}