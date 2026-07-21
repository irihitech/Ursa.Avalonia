using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels
{
    public class SkeletonDemoViewModel : ViewModelBase, IPageMetadataProvider
    {
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Skeleton,
        Description = LanguageManager.Instance.Page_Description_Skeleton,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Skeleton)],
        Tags = ["Skeleton", "Loading", "Placeholder"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/SkeletonDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/SkeletonDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    }
}
