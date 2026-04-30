using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class GroupBoxDemoViewModel : ViewModelBase
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "GroupBox",
        Description = "GroupBox groups related controls together with an optional title border.",
        Breadcrumbs = ["Layout & Display", "GroupBox"],
        Tags = ["GroupBox", "Container", "Layout"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/GroupBoxDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/GroupBoxDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

}

