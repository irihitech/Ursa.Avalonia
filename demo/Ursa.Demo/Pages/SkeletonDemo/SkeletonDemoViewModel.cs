using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.SkeletonDemo
{
[DocCategory(Category_Key, IsClickable = false, Parent = DialogAndFeedbacksPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(SkeletonDemo))]
    public class SkeletonDemoViewModel : ViewModelBase, IPageMetadataProvider
    {
    public const string Category_Key = "Skeleton";
    public const string Menu_Header = "Menu_Header_Skeleton";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Skeleton,
        Description = LanguageManager.Instance.Page_Description_Skeleton,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Skeleton)],
        Tags = ["Skeleton", "Loading", "Placeholder"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/SkeletonDemo/SkeletonDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/SkeletonDemo/SkeletonDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    }
}
