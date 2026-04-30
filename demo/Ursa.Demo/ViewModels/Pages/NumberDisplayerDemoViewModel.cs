using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class NumberDisplayerDemoViewModel: ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "NumberDisplayer",
        Description = "NumberDisplayer animates number transitions with smooth digit rolling effects.",
        Breadcrumbs = ["Layout & Display", "Number Displayer"],
        Tags = ["NumberDisplayer", "Number", "Animation"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/NumberDisplayerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/NumberDisplayerDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    [ObservableProperty] private int _value;
    [ObservableProperty] private long _longValue;
    [ObservableProperty] private double _doubleValue;
    [ObservableProperty] private DateTime _dateValue;
    public ICommand IncreaseCommand { get; }
    public NumberDisplayerDemoViewModel()
    {
        IncreaseCommand = new RelayCommand(OnChange);
        Value = 0;
        LongValue = 0L;
        DoubleValue = 0d;
        DateValue = DateTime.Now;
    }

    private void OnChange()
    {
        Random r = new Random();
        Value = r.Next(int.MaxValue);
        LongValue = ((long)r.Next(int.MaxValue)) * 1000 + r.Next(1000);
        DoubleValue = r.NextDouble() * 100000;
        DateValue = DateTime.Today.AddDays(r.Next(1000));
    }
}