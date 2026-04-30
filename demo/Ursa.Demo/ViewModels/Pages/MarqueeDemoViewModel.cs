using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class MarqueeDemoViewModel: ViewModelBase
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "Marquee",
        Description = "Marquee scrolls text or content continuously in a horizontal direction.",
        Breadcrumbs = ["Layout & Display", "Marquee"],
        Tags = ["Marquee", "Scroll", "Text"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/MarqueeDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/MarqueeDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}