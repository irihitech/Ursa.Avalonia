using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class AspectRatioLayoutDemoViewModel : ViewModelBase
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "AspectRatioLayout",
        Description = "AspectRatioLayout maintains a fixed aspect ratio for its child content.",
        Breadcrumbs = ["Layout & Display", "AspectRatioLayout"],
        Tags = ["AspectRatioLayout", "Layout", "Ratio"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/AspectRatioLayoutDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/AspectRatioLayoutDemoViewModel.cs",
        InlineXamlSupport = true,
    };

}