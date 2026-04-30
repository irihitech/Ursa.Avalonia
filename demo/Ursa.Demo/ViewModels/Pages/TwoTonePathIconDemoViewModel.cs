using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class TwoTonePathIconDemoViewModel:ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "TwoTonePathIcon",
        Description = "TwoTonePathIcon renders path icons with two distinct fill colors.",
        Breadcrumbs = ["Layout & Display", "TwoTonePathIcon"],
        Tags = ["TwoTonePathIcon", "Icon", "Path"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TwoTonePathIconDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/TwoTonePathIconDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}