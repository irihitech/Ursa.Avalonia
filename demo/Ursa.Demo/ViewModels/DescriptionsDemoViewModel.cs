using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class DescriptionsDemoViewModel: ObservableObject
{
    public ObservableCollection<DescriptionItemViewModel> Items { get; set; }
    public ObservableCollection<DescriptionItemViewModel> Items2 { get; set; }

    public DescriptionsDemoViewModel()
    {
        Items = new ObservableCollection<DescriptionItemViewModel>()
        {
            new()
            {
                Label = "Actual Users",
                Description = "1,480,000"
            },
            new()
            {
                Label = "7-day Retention",
                Description = "98%"
            },
            new()
            {
                Label = "Security Level",
                Description = "III"
            }
        };
        Items2 = new ObservableCollection<DescriptionItemViewModel>()
        {
            new() { Label = "抖音号", Description = "SemiDesign" },
            new() { Label = "主播类型", Description = "自由主播" },
            new() { Label = "安全等级", Description = "3级" },
            new() { Label = "垂类标签", Description = "编程" },
            new() { Label = "作品数量", Description = "88888888" },
            new() { Label = "认证状态", Description = "这是一个很长很长很长很长很长很长很长很长很长的值" },
            new() { Label = "上次直播时间", Description = "2024-05-01 12:00:00"}
        };
    }
}

public partial class DescriptionItemViewModel : ObservableObject
{
    [ObservableProperty] private string? _label;
    [ObservableProperty] private object? _description;
}