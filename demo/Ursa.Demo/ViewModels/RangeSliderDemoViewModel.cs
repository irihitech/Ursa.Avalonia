using System.Collections.ObjectModel;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class RangeSliderDemoViewModel: ObservableObject
{
    public ObservableCollection<Orientation> Orientations { get; set; } = new ObservableCollection<Orientation>()
    {
        Orientation.Horizontal,
        Orientation.Vertical
    };

    [ObservableProperty] private Orientation _orientation;
}