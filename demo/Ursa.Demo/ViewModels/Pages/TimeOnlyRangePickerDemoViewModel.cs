using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class TimeOnlyRangePickerDemoViewModel : ObservableValidator, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_TimeOnlyRangePicker,
        Description = LanguageManager.Instance.Page_Description_TimeOnlyRangePicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_TimeOnlyRangePicker)],
        Tags = ["TimeOnlyRangePicker", "Time", "Range"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimeOnlyRangePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/TimeOnlyRangePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial TimeOnly? StartTime { get; set; }
    [ObservableProperty] public partial TimeOnly? EndTime { get; set; }
    
    public ValidatedTimeOnlyRange ValidatedRange { get; } = new();

    public TimeOnlyRangePickerDemoViewModel()
    {
        StartTime = new TimeOnly(8, 21, 0);
        EndTime = new TimeOnly(18, 22, 0);
    }
}

public partial class ValidatedTimeOnlyRange : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Start time is required")]
    public partial TimeOnly? Start { get; set; }
    
    [ObservableProperty]
    [Required(ErrorMessage = "End time is required")]
    public partial TimeOnly? End { get; set; }

    public ValidatedTimeOnlyRange()
    {
        Start = new TimeOnly(8, 21, 0);
        End = new TimeOnly(18, 22, 0);
    }
}
