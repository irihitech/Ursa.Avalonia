using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class DateOnlyRangePickerDemoViewModel : ObservableValidator, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateOnlyRangePicker,
        Description = LanguageManager.Instance.Page_Description_DateOnlyRangePicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateOnlyRangePicker)],
        Tags = ["DateOnlyRangePicker", "Date", "Range"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOnlyRangePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/DateOnlyRangePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateOnly? StartDate { get; set; }
    [ObservableProperty] public partial DateOnly? EndDate { get; set; }
    
    public ValidatedDateOnlyRange ValidatedRange { get; } = new();

    public DateOnlyRangePickerDemoViewModel()
    {
        StartDate = DateOnly.FromDateTime(DateTime.Today);
        EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7));
    }
}

public partial class ValidatedDateOnlyRange : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Start date is required")]
    public partial DateOnly? Start { get; set; }
    
    [ObservableProperty]
    [Required(ErrorMessage = "End date is required")]
    public partial DateOnly? End { get; set; }

    public ValidatedDateOnlyRange()
    {
        Start = DateOnly.FromDateTime(DateTime.Today);
        End = DateOnly.FromDateTime(DateTime.Today.AddDays(7));
    }
}
