using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels
{
    public class SkeletonDemoViewModel : ViewModelBase
    {
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "Skeleton",
        Description = "Skeleton shows placeholder loading animations for content areas.",
        Breadcrumbs = ["Dialog & Feedbacks", "Skeleton"],
        Tags = ["Skeleton", "Loading", "Placeholder"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/SkeletonDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/SkeletonDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    }
}
