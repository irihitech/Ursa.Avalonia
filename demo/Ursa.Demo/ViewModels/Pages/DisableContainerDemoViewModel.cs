using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class DisableContainerDemoViewModel: ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "DisableContainer",
        Description = "DisableContainer wraps content to visually and functionally disable child elements.",
        Breadcrumbs = ["Layout & Display", "Disable Container"],
        Tags = ["DisableContainer", "Container", "Disable"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DisableContainerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/DisableContainerDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}