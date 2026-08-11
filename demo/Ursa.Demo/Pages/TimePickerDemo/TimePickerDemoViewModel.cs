using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.TimePickerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DateAndTimePage.Category_Key)]
[DocPage(Menu_Header, View = typeof(TimePickerDemo))]
public partial class TimePickerDemoViewModel: ObservableValidator, IPageMetadataProvider
{
    public const string Category_Key = "TimePicker";
    public const string Menu_Header = "Menu_Header_TimePicker";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_TimePicker,
        Description = LanguageManager.Instance.Page_Description_TimePicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_TimePicker)],
        Tags = ["TimePicker", "Time", "Input"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimePickerDemo/TimePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimePickerDemo/TimePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial TimeSpan? Time { get; set; }

    [ObservableProperty]
    [Required(ErrorMessage = "Please select a time")]
    public partial TimeSpan? ValidatedTime { get; set; }
    
    public TimePickerDemoViewModel()
    {
        Time = new TimeSpan(12, 20, 0);
        ValidatedTime = new TimeSpan(12, 20, 0);
    }
}