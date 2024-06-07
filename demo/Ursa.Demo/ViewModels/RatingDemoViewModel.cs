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

    [ObservableProperty] private double _defaultValue = 2.3;
    [ObservableProperty] private int _count = 5;
    // [ObservableProperty] private object _character;

    public ObservableCollection<string> Tooltips { get; set; } = ["1", "2", "3", "4", "5"];
}