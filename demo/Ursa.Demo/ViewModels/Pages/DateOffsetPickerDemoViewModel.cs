using System;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class DateOffsetPickerDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateOffsetPicker,
        Description = LanguageManager.Instance.Page_Description_DateOffsetPicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateOffsetPicker)],
        Tags = ["DateOffsetPicker", "Date", "Offset"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOffsetPickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/DateOffsetPickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateTimeOffset? SelectedDate { get; set; }

    public DateOffsetPickerDemoViewModel()
    {
        SelectedDate = DateTimeOffset.Now;
    }
}
