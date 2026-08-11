using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.TimeBoxDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DateAndTimePage.Category_Key)]
[DocPage(Menu_Header, View = typeof(TimeBoxDemo))]
public partial class TimeBoxDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "TimeBox";
    public const string Menu_Header = "Menu_Header_TimeBox";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_TimeBox,
        Description = LanguageManager.Instance.Page_Description_TimeBox,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_TimeBox)],
        Tags = ["TimeBox", "Time", "Input"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimeBoxDemo/TimeBoxDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimeBoxDemo/TimeBoxDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial TimeSpan? TimeSpan { get; set; }

    [RelayCommand]
    private void ChangeRandomTime()
    {
        TimeSpan = new TimeSpan(Random.Shared.NextInt64(0x00000000FFFFFFFF));
    }
    
    public TimeBoxDemoViewModel()
    {
        TimeSpan = new TimeSpan(0, 21, 11, 36, 54);
    }
}