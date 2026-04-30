using System;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class TimeOnlyPickerDemoViewModel : ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "TimeOnlyPicker",
        Description = "TimeOnlyPicker selects time values using the TimeOnly type.",
        Breadcrumbs = ["Date & Time", "Time Only Picker"],
        Tags = ["TimeOnlyPicker", "Time", "Input"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TimeOnlyPickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/TimeOnlyPickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] private TimeOnly? _time;

    public TimeOnlyPickerDemoViewModel()
    {
        Time = new TimeOnly(12, 20, 0);
    }
}
