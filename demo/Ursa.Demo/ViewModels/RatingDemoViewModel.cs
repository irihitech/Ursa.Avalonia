using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class RatingDemoViewModel : ViewModelBase
{
    [ObservableProperty] private bool _allowClear = true;
    [ObservableProperty] private bool _allowHalf = true;
    [ObservableProperty] private bool _isEnabled = true;
    [ObservableProperty] private double _value;
    [ObservableProperty] private double _defaultValue = 2.3;
    [ObservableProperty] private int _count = 5;
}