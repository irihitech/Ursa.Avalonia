using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Controls;

namespace Ursa.Demo.ViewModels;

public class TimelineDemoViewModel: ViewModelBase
{
    public TimelineItemViewModel[] Items { get; } =
    {
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "yyyy-MM-dd HH:mm:ss",
            Description = "Item 1",
            Content = "First",
            ItemType = TimelineItemType.Success,
        },
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "HH:mm:ss",
            Description = "Item 2",
            Content = "Content 2",
            ItemType = TimelineItemType.Success,
        },
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "HH:mm:ss",
            Description = "Item 3",
            Content = "Content 3",
            ItemType = TimelineItemType.Ongoing,
        },
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "HH:mm:ss",
            Description = "Item 4",
            Content = "Content 4"
        },
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "HH:mm:ss",
            Description = "Item 5",
            Content = "Content 5"
        },
    };
}

public class TimelineItemViewModel: ObservableObject
{
    public DateTime Time { get; set; }
    public string? TimeFormat { get; set; }
    public string? Description { get; set; }    
    public string? Content { get; set; }
    public TimelineItemType ItemType { get; set; }
}