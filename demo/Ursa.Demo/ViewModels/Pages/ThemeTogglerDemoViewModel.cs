using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class ThemeTogglerDemoViewModel
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "ThemeToggler",
        Description = "ThemeToggler provides a control to switch between light and dark themes.",
        Breadcrumbs = ["Buttons & Inputs", "Theme Toggler"],
        Tags = ["ThemeToggler", "Theme", "Toggle"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ThemeTogglerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/ThemeTogglerDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}