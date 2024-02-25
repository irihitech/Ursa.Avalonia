using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Ursa.Demo.ViewModels;

public partial class NumberDisplayerDemoViewModel: ObservableObject
{
    [ObservableProperty] private int _value;
    [ObservableProperty] private double _doubleValue;
    [ObservableProperty] private DateTime _dateValue;
    public ICommand IncreaseCommand { get; }
    public NumberDisplayerDemoViewModel()
    {
        IncreaseCommand = new RelayCommand(OnChange);
        Value = 0;
        DoubleValue = 0d;
        DateValue = DateTime.Now;
    }

    private void OnChange()
    {
        Random r = new Random();
        Value = r.Next(int.MaxValue);
        DoubleValue = r.NextDouble() * 100000;
        DateValue = DateTime.Today.AddDays(r.Next(1000));
    }
}