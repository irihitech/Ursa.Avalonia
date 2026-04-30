using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class ImageViewerDemoViewModel: ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "ImageViewer",
        Description = "ImageViewer displays images with pan, zoom, and fit-to-window capabilities.",
        Breadcrumbs = ["Layout & Display", "ImageViewer"],
        Tags = ["ImageViewer", "Image", "Zoom"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ImageViewerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/ImageViewerDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}