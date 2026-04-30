using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class ClassInputDemoViewModel: ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "ClassInput",
        Description = "ClassInput allows users to input CSS class names interactively.",
        Breadcrumbs = ["Buttons & Inputs", "Class Input"],
        Tags = ["ClassInput", "Input", "CSS"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ClassInputDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/ClassInputDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}