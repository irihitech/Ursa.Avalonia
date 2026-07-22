using System;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class DateOffsetRangePickerDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateOffsetRangePicker,
        Description = LanguageManager.Instance.Page_Description_DateOffsetRangePicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateOffsetRangePicker)],
        Tags = ["DateOffsetRangePicker", "Date", "Range"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOffsetRangePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/DateOffsetRangePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateTimeOffset? StartDate { get; set; }
    [ObservableProperty] public partial DateTimeOffset? EndDate { get; set; }

    public DateOffsetRangePickerDemoViewModel()
    {
        StartDate = DateTimeOffset.Now;
        EndDate = DateTimeOffset.Now.AddDays(7);
    }
}
