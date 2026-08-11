using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.NumPadDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(NumPadDemo))]
public class NumPadDemoViewModel : IPageMetadataProvider
{
    public const string Category_Key = "NumPad";
    public const string Menu_Header = "Menu_Header_NumPad";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_NumPad,
        Description = LanguageManager.Instance.Page_Description_NumPad,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_NumPad)],
        Tags = ["NumPad", "Input", "Number"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/NumPadDemo/NumPadDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/NumPadDemo/NumPadDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}