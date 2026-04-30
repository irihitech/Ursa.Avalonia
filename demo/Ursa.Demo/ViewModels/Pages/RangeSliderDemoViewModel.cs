using System.Collections.ObjectModel;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class RangeSliderDemoViewModel: ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "RangeSlider",
        Description = "RangeSlider allows selecting a value range with two draggable thumbs.",
        Breadcrumbs = ["Buttons & Inputs", "RangeSlider"],
        Tags = ["RangeSlider", "Slider", "Range"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/RangeSliderDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/RangeSliderDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public ObservableCollection<Orientation> Orientations { get; set; } = new ObservableCollection<Orientation>()
    {
        Orientation.Horizontal,
        Orientation.Vertical
    };

    [ObservableProperty] private Orientation _orientation;
}