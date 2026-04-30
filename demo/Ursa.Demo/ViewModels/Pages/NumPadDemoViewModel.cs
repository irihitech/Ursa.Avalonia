using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class NumPadDemoViewModel
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "NumPad",
        Description = "NumPad is a numeric keypad control for entering numbers.",
        Breadcrumbs = ["Buttons & Inputs", "NumPad"],
        Tags = ["NumPad", "Input", "Number"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/NumPadDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/NumPadDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}