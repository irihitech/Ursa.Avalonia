using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class LoadingDemoViewModel: ViewModelBase
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "Loading",
        Description = "Loading displays a spinner or progress indicator during async operations.",
        Breadcrumbs = ["Dialog & Feedbacks", "Loading"],
        Tags = ["Loading", "Spinner", "Progress"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/LoadingDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/LoadingDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}