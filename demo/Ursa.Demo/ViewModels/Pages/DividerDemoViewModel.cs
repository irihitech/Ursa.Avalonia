using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class DividerDemoViewModel: ViewModelBase
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "Divider",
        Description = "Divider renders a horizontal or vertical separator line between content.",
        Breadcrumbs = ["Layout & Display", "Divider"],
        Tags = ["Divider", "Separator", "Layout"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DividerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/DividerDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}