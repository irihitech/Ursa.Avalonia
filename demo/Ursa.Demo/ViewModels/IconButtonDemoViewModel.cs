using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Common;

namespace Ursa.Demo.ViewModels;

public partial class IconButtonDemoViewModel : ObservableObject
{
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private Position _selectedPosition;
}