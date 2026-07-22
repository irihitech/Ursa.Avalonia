using System;
using System.Collections.ObjectModel;
using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Demo.Models;

using Ursa.Demo.ViewModels.Controls;
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

    public ObservableCollection<ControlData> AutoCompleteSelectedItems { get; set; } = [];

    public ObservableCollection<string> TagItems { get; set; } = new()
    {
        "Tag1", "Tag2", "Tag3"
    };

    [ObservableProperty] public partial double RatingValue { get; set; } = 3.5;
    
    [ObservableProperty] public partial int SliderValue { get; set; } = 50;

    [ObservableProperty] public partial IPAddress? IpAddress { get; set; } = new IPAddress(new byte[] { 192, 168, 1, 1 });

    [ObservableProperty] public partial double LowerValue { get; set; } = 20;
    
    [ObservableProperty] public partial double UpperValue { get; set; } = 80;

    [ObservableProperty] public partial DateTime StartDate { get; set; } = DateTime.Today;
    
    [ObservableProperty] public partial DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);

    [ObservableProperty] public partial DateTime DateTime { get; set; } = DateTime.Now;

    [ObservableProperty] public partial TimeSpan StartTime { get; set; } = new TimeSpan(9, 0, 0);
    
    [ObservableProperty] public partial TimeSpan EndTime { get; set; } = new TimeSpan(17, 0, 0);
}
