using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class TimePickerDemoViewModel: ObservableValidator, IPageMetadataProvider
{
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