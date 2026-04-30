using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Controls;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class TimelineDemoViewModel: ViewModelBase
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "Timeline",
        Description = "Timeline presents a sequence of events in chronological vertical order.",
        Breadcrumbs = ["Layout & Display", "Timeline"],
        Tags = ["Timeline", "History", "Steps"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimelineDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/TimelineDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public TimelineItemViewModel[] Items { get; } =
    {
        new()
        {
            Time = DateTime.Now,
            Description = "Item 1",
            Header = "审核中",
            ItemType = TimelineItemType.Success,
        },
        new()
        {
            Time = DateTime.Now,
            Description = "Item 2",
            Header = "发布成功",
            ItemType = TimelineItemType.Ongoing,
        },
        new()
        {
            Time = DateTime.Now,
            Description = "Item 3",
            Header = "审核失败",
            ItemType = TimelineItemType.Error,
        }
    };
}

public class TimelineItemViewModel: ObservableObject
{
    public DateTime Time { get; set; }
    public string? TimeFormat { get; set; }
    public string? Description { get; set; }    
    public string? Header { get; set; }
    public TimelineItemType ItemType { get; set; }
}