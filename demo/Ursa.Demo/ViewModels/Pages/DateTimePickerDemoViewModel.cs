using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public class DateTimePickerDemoViewModel
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "DateTimePicker",
        Description = "DateTimePicker allows selecting both date and time values.",
        Breadcrumbs = ["Date & Time", "Date Time Picker"],
        Tags = ["DateTimePicker", "Date", "Time"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateTimePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/DateTimePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    
}