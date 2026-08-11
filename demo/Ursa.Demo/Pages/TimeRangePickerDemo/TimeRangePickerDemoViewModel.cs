using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class TimeRangePickerDemoViewModel: ObservableValidator, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_TimeRangePicker,
        Description = LanguageManager.Instance.Page_Description_TimeRangePicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_TimeRangePicker)],
        Tags = ["TimeRangePicker", "Time", "Range"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimeRangePickerDemo/TimeRangePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimeRangePickerDemo/TimeRangePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial TimeSpan? StartTime { get; set; }
    [ObservableProperty] public partial TimeSpan? EndTime { get; set; }
    
    public ValidatedTimeRange ValidatedRange { get; } = new();

    public TimeRangePickerDemoViewModel()
    {
        StartTime = new TimeSpan(8, 21, 0);
        EndTime = new TimeSpan(18, 22, 0);
    }
}

public partial class ValidatedTimeRange : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Start time is required")]
    public partial TimeSpan? Start { get; set; }
    
    [ObservableProperty]
    [Required(ErrorMessage = "End time is required")]
    public partial TimeSpan? End { get; set; }

    public ValidatedTimeRange()
    {
        Start = new TimeSpan(8, 21, 0);
        End = new TimeSpan(18, 22, 0);
    }
}