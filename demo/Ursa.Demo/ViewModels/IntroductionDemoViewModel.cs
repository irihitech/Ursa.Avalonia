using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class IntroductionDemoViewModel : ObservableObject
{
    public ObservableCollection<string> ButtonGroupItems { get; set; } = new()
    {
        "Avalonia", "WPF", "Xamarin"
    };

    [ObservableProperty] private double _ratingValue = 3.5;
    
    [ObservableProperty] private int _sliderValue = 50;
}