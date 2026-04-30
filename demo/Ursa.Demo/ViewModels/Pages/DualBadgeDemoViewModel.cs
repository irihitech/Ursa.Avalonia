using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class DualBadgeDemoViewModel : ViewModelBase
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "DualBadge",
        Description = "DualBadge shows a two-part badge with header and value sections.",
        Breadcrumbs = ["Layout & Display", "DualBadge"],
        Tags = ["DualBadge", "Badge", "Label"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DualBadgeDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/DualBadgeDemoViewModel.cs",
        InlineXamlSupport = true,
    };

}