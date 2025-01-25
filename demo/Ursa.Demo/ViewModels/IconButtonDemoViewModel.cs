using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Common;

namespace Ursa.Demo.ViewModels;

public partial class IconButtonDemoViewModel : ObservableObject
{
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private bool _isLoading2;
    [ObservableProperty] private Position _selectedPosition;
}