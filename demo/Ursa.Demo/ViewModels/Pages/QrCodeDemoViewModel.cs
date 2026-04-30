using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class QrCodeDemoViewModel: ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "QrCode",
        Description = "QrCode generates and displays QR code images from text or URL input.",
        Breadcrumbs = ["Layout & Display", "Qr Code"],
        Tags = ["QrCode", "Code", "Image"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/QrCodeDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/QrCodeDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}