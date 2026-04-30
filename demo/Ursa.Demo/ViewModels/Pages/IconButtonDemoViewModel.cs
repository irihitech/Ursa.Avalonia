using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Common;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class IconButtonDemoViewModel : ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "IconButton",
        Description = "IconButton is a button that displays an icon alongside optional text.",
        Breadcrumbs = ["Buttons & Inputs", "Icon Button"],
        Tags = ["IconButton", "Button", "Icon"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/IconButtonDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/IconButtonDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private bool _isLoading2;
    [ObservableProperty] private Position _selectedPosition;
}