using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class DateRangePickerDemoViewModel: ObservableValidator, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateRangePicker,
        Description = LanguageManager.Instance.Page_Description_DateRangePicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateRangePicker)],
        Tags = ["DateRangePicker", "Date", "Range"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateRangePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/DateRangePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateTime? StartDate { get; set; }
    [ObservableProperty] public partial DateTime? EndDate { get; set; }
    
    public ValidatedDateRange ValidatedRange { get; } = new();

    public DateRangePickerDemoViewModel()
    {
        StartDate = DateTime.Today;
        EndDate = DateTime.Today.AddDays(7);
    }
}

public partial class ValidatedDateRange : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Start date is required")]
    public partial DateTime? Start { get; set; }
    
    [ObservableProperty]
    [Required(ErrorMessage = "End date is required")]
    public partial DateTime? End { get; set; }

    public ValidatedDateRange()
    {
        Start = DateTime.Today;
        End = DateTime.Today.AddDays(7);
    }
}