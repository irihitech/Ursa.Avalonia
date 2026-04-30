using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class ThemeVariantMapperDemoViewModel : ViewModelBase
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "ThemeVariantMapper",
        Description = "ThemeVariantMapper maps values between light and dark theme variants.",
        Breadcrumbs = ["Layout & Display", "Theme Variant Mapper"],
        Tags = ["ThemeVariantMapper", "Theme", "Color"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ThemeVariantMapperDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/ThemeVariantMapperDemoViewModel.cs",
        InlineXamlSupport = true,
    };

}

