using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.ViewModels;

public partial class ProportionalCanvasDemoViewModel: ViewModelBase
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "Proportional Canvas",
        Description = "ProportionalCanvas is a layout panel that arranges child elements proportionally within the available space.",
        Breadcrumbs = ["Layout & Display", "Proportional Canvas"],
        Tags = ["ProportionalCanvas", "Canvas", "Layout", "Proportional"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ProportionalCanvasDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/ProportionalCanvasDemoViewModel.cs",
        InlineXamlSupport = true,
    };
    
    [ObservableProperty]
    private double _canvasWidth = 500;

    [ObservableProperty]
    private double _canvasHeight = 400;
}
