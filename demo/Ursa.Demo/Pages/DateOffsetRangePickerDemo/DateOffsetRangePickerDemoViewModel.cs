using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.Pages.DateOffsetRangePickerDemo;

public partial class DateOffsetRangePickerDemoViewModel : ObservableValidator, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateOffsetRangePicker,
        Description = LanguageManager.Instance.Page_Description_DateOffsetRangePicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateOffsetRangePicker)],
        Tags = ["DateOffsetRangePicker", "Date", "Range"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOffsetRangePickerDemo/DateOffsetRangePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOffsetRangePickerDemo/DateOffsetRangePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateTimeOffset? StartDate { get; set; }
    [ObservableProperty] public partial DateTimeOffset? EndDate { get; set; }
    
    public ValidatedDateTimeOffsetRange ValidatedRange { get; } = new();

    public DateOffsetRangePickerDemoViewModel()
    {
        StartDate = DateTimeOffset.Now;
        EndDate = DateTimeOffset.Now.AddDays(7);
    }
}

public partial class ValidatedDateTimeOffsetRange : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Start date is required")]
    public partial DateTimeOffset? Start { get; set; }
    
    [ObservableProperty]
    [Required(ErrorMessage = "End date is required")]
    public partial DateTimeOffset? End { get; set; }

    public ValidatedDateTimeOffsetRange()
    {
        Start = DateTimeOffset.Now;
        End = DateTimeOffset.Now.AddDays(7);
    }
}
