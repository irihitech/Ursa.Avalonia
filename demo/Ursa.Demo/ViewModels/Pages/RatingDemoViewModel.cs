using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class RatingDemoViewModel : ViewModelBase
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "Rating",
        Description = "Rating displays and collects star-based rating values from users.",
        Breadcrumbs = ["Buttons & Inputs", "Rating"],
        Tags = ["Rating", "Star", "Input"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/RatingDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/RatingDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] private bool _allowClear = true;
    [ObservableProperty] private bool _allowHalf = true;
    [ObservableProperty] private bool _isEnabled = true;
    [ObservableProperty] private double _value;
    [ObservableProperty] private double _defaultValue = 2.3;
    [ObservableProperty] private int _count = 5;
}