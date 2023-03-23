using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public class TimelineDemoViewModel: ObservableObject
{
    public TimelineItemViewModel[] Items { get; } =
    {
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "yyyy-MM-dd HH:mm:ss",
            Description = "Item 1",
            Content = "First"
        },
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "HH:mm:ss",
            Description = "Item 2",
            Content = "Content 2"
        },
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "HH:mm:ss",
            Description = "Item 3",
            Content = "Content 3"
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
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "HH:mm:ss",
            Description = "Item 6",
            Content = "Content 6"
        },
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "HH:mm:ss",
            Description = "Item 7",
            Content = "Content 71231"
        },
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "HH:mm:ss",
            Description = "Item 8",
            Content = "Content 8123123"
        },
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "HH:mm:ss",
            Description = "Item 9",
            Content = "Content 9123123"
        },
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "HH:mm:ss",
            Description = "Item 10",
            Content = "Content 1231231231231231231230"
        },
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "HH:mm:ss",
            Description = "Item 11",
            Content = "Content 11231231"
        },
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "HH:mm:ss",
            Description = "Item 12",
            Content = "Content 12123123123123"
        },
        new()
        {
            Time = DateTime.Now,
            TimeFormat = "HH:mm:ss",
            Description = "Item 13",
            Content = "Last"
        }
    };
}

public class TimelineItemViewModel: ObservableObject
{
    public DateTime Time { get; set; }
    public string? TimeFormat { get; set; }
    public string? Description { get; set; }    
    public string? Content { get; set; }
}