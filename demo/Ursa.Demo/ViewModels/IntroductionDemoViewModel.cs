using System;
using System.Collections.ObjectModel;
using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Demo.Models;

namespace Ursa.Demo.ViewModels;

public partial class IntroductionDemoViewModel : ObservableObject
{
    public ObservableCollection<string> ButtonGroupItems { get; set; } = new()
    {
        "Avalonia", "WPF", "Xamarin"
    };

    public ObservableCollection<string> ComboBoxItems { get; set; } = new()
    {
        "Option 1", "Option 2", "Option 3", "Option 4", "Option 5"
    };

    public ObservableCollection<ControlData> AutoCompleteItems { get; set; } = new()
    {
        new() { MenuHeader = "Avatar", Chinese = "头像" },
        new() { MenuHeader = "Badge", Chinese = "徽标" },
        new() { MenuHeader = "Button", Chinese = "按钮" },
        new() { MenuHeader = "Dialog", Chinese = "对话框" },
    };

    public ObservableCollection<string> TagItems { get; set; } = new()
    {
        "Tag1", "Tag2", "Tag3"
    };

    [ObservableProperty] private double _ratingValue = 3.5;
    
    [ObservableProperty] private int _sliderValue = 50;

    [ObservableProperty] private IPAddress? _ipAddress = new IPAddress(new byte[] { 192, 168, 1, 1 });

    [ObservableProperty] private double _lowerValue = 20;
    
    [ObservableProperty] private double _upperValue = 80;

    [ObservableProperty] private DateTime _startDate = DateTime.Today;
    
    [ObservableProperty] private DateTime _endDate = DateTime.Today.AddDays(7);

    [ObservableProperty] private DateTime _dateTime = DateTime.Now;

    [ObservableProperty] private TimeSpan _startTime = new TimeSpan(9, 0, 0);
    
    [ObservableProperty] private TimeSpan _endTime = new TimeSpan(17, 0, 0);
}