using System;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class TimeOnlyRangePickerDemoViewModel : ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "TimeOnlyRangePicker",
        Description = "TimeOnlyRangePicker selects a time range using the TimeOnly type.",
        Breadcrumbs = ["Date & Time", "Time Only Range Picker"],
        Tags = ["TimeOnlyRangePicker", "Time", "Range"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimeOnlyRangePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/TimeOnlyRangePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] private TimeOnly? _startTime;
    [ObservableProperty] private TimeOnly? _endTime;

    public TimeOnlyRangePickerDemoViewModel()
    {
        StartTime = new TimeOnly(8, 21, 0);
        EndTime = new TimeOnly(18, 22, 0);
    }
}
