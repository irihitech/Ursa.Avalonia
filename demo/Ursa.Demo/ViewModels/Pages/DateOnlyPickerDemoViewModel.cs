using System;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class DateOnlyPickerDemoViewModel : ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "DateOnlyPicker",
        Description = "DateOnlyPicker is a date picker that works with the DateOnly type.",
        Breadcrumbs = ["Date & Time", "Date Only Picker"],
        Tags = ["DateOnlyPicker", "Date", "Input"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOnlyPickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/DateOnlyPickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] private DateOnly? _selectedDate;

    public DateOnlyPickerDemoViewModel()
    {
        SelectedDate = DateOnly.FromDateTime(DateTime.Today);
    }
}
