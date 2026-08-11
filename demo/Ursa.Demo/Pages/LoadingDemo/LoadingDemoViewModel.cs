using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.LoadingDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DialogAndFeedbacksPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(LoadingDemo))]
public class LoadingDemoViewModel: ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "Loading";
    public const string Menu_Header = "Menu_Header_Loading";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Loading,
        Description = LanguageManager.Instance.Page_Description_Loading,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Loading)],
        Tags = ["Loading", "Spinner", "Progress"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/LoadingDemo/LoadingDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/LoadingDemo/LoadingDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}