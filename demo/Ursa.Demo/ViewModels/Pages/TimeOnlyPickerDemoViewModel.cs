using System;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class TimeOnlyPickerDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_TimeOnlyPicker,
        Description = LanguageManager.Instance.Page_Description_TimeOnlyPicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_TimeOnlyPicker)],
        Tags = ["TimeOnlyPicker", "Time", "Input"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimeOnlyPickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/TimeOnlyPickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial TimeOnly? Time { get; set; }

    public TimeOnlyPickerDemoViewModel()
    {
        Time = new TimeOnly(12, 20, 0);
    }
}
