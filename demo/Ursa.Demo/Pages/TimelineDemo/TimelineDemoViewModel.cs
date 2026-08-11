using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Controls;

using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.TimelineDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(TimelineDemo))]
public class TimelineDemoViewModel: ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "Timeline";
    public const string Menu_Header = "Menu_Header_Timeline";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Timeline,
        Description = LanguageManager.Instance.Page_Description_Timeline,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Timeline)],
        Tags = ["Timeline", "History", "Steps"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimelineDemo/TimelineDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimelineDemo/TimelineDemoViewModel.cs",
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