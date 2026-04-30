using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class DescriptionsDemoViewModel : ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "Descriptions",
        Description = "Descriptions presents key-value data in a structured label-content layout.",
        Breadcrumbs = ["Layout & Display", "Descriptions"],
        Tags = ["Descriptions", "Label", "Value"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DescriptionsDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/DescriptionsDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public ObservableCollection<DescriptionItemViewModel> Items { get; set; }
    public ObservableCollection<DescriptionItemViewModel> Items2 { get; set; }

    public DescriptionsDemoViewModel()
    {
        Items = new ObservableCollection<DescriptionItemViewModel>()
        {
            new() { Label = "Actual Users", Description = "1,480,000" },
            new() { Label = "7-day Retention", Description = "98%" },
            new() { Label = "Security Level", Description = "III" },
            new() { Label = "Category Tag", Description = "E-commerce" },
            new() { Label = "Authorized State", Description = "Unauthorized" },
        };
        Items2 = new ObservableCollection<DescriptionItemViewModel>()
        {
            new() { Label = "抖音号", Description = "SemiDesign" },
            new() { Label = "主播类型", Description = "自由主播" },
            new() { Label = "安全等级", Description = "3级" },
            new() { Label = "垂类标签", Description = "编程" },
            new() { Label = "作品数量", Description = "88888888" },
            new() { Label = "认证状态", Description = "这是一个很长很长很长很长很长很长很长很长很长的值" },
            new() { Label = "上次直播时间", Description = "2024-05-01 12:00:00" }
        };
    }
}

public partial class DescriptionItemViewModel : ObservableObject
{
    [ObservableProperty] private string? _label;
    [ObservableProperty] private object? _description;
}