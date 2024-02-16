using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Ursa.Demo.ViewModels;

public partial class NumberDisplayerDemoViewModel: ObservableObject
{
    [ObservableProperty] private int _value;
    [ObservableProperty] private double _doubleValue;
    public ICommand IncreaseCommand { get; }
    public NumberDisplayerDemoViewModel()
    {
        IncreaseCommand = new RelayCommand(OnChange);
        Value = 0;
        DoubleValue = 0d;
    }

    private void OnChange()
    {
        Random r = new Random();
        Value = r.Next(int.MaxValue);
        DoubleValue = r.NextDouble() * 100000;
    }
}