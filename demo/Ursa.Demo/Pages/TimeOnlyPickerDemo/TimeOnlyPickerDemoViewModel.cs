using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.TimeOnlyPickerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DateAndTimePage.Category_Key)]
[DocPage(Menu_Header, View = typeof(TimeOnlyPickerDemo))]
public partial class TimeOnlyPickerDemoViewModel : ObservableValidator, IPageMetadataProvider
{
    public const string Category_Key = "TimeOnlyPicker";
    public const string Menu_Header = "Menu_Header_TimeOnlyPicker";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_TimeOnlyPicker,
        Description = LanguageManager.Instance.Page_Description_TimeOnlyPicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_TimeOnlyPicker)],
        Tags = ["TimeOnlyPicker", "Time", "Input"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimeOnlyPickerDemo/TimeOnlyPickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimeOnlyPickerDemo/TimeOnlyPickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial TimeOnly? Time { get; set; }

    [ObservableProperty]
    [Required(ErrorMessage = "Please select a time")]
    public partial TimeOnly? ValidatedTime { get; set; }

    public TimeOnlyPickerDemoViewModel()
    {
        Time = new TimeOnly(12, 20, 0);
        ValidatedTime = new TimeOnly(12, 20, 0);
    }
}
