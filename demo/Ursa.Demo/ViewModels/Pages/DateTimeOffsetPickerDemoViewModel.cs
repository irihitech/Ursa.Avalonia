using System;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class DateTimeOffsetPickerDemoViewModel : ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "DateTimeOffsetPicker",
        Description = "DateTimeOffsetPicker selects a DateTimeOffset including timezone information.",
        Breadcrumbs = ["Date & Time", "DateTime Offset Picker"],
        Tags = ["DateTimeOffsetPicker", "DateTime", "Offset"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateTimeOffsetPickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/DateTimeOffsetPickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] private DateTimeOffset? _selectedDateTime;

    public DateTimeOffsetPickerDemoViewModel()
    {
        SelectedDateTime = DateTimeOffset.Now;
    }
}
