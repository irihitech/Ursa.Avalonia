using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class RatingDemoViewModel : ViewModelBase
{
    [ObservableProperty] private bool _allowClear = true;
    [ObservableProperty] private bool _allowHalf = true;
    [ObservableProperty] private bool _allowFocus;
    [ObservableProperty] private bool _isEnabled = true;

    [ObservableProperty] private double _value;

    // [ObservableProperty] private object _character;
    [ObservableProperty] private int _count = 10;
    [ObservableProperty] private double _defaultValue = 5;

    public ObservableCollection<string> Tooltips { get; set; } = ["1", "2", "3", "4", "5"];
}