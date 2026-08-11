using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.DisableContainerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(DisableContainerDemo))]
public class DisableContainerDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "DisableContainer";
    public const string Menu_Header = "Menu_Header_DisableContainer";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DisableContainer,
        Description = LanguageManager.Instance.Page_Description_DisableContainer,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DisableContainer)],
        Tags = ["DisableContainer", "Container", "Disable"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DisableContainerDemo/DisableContainerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DisableContainerDemo/DisableContainerDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}