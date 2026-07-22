using System;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class DateTimeOffsetPickerDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateTimeOffsetPicker,
        Description = LanguageManager.Instance.Page_Description_DateTimeOffsetPicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateTimeOffsetPicker)],
        Tags = ["DateTimeOffsetPicker", "DateTime", "Offset"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateTimeOffsetPickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/DateTimeOffsetPickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateTimeOffset? SelectedDateTime { get; set; }

    public DateTimeOffsetPickerDemoViewModel()
    {
        SelectedDateTime = DateTimeOffset.Now;
    }
}
