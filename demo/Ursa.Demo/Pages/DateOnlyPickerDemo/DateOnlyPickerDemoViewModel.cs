using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.Pages.DateOnlyPickerDemo;

public partial class DateOnlyPickerDemoViewModel : ObservableValidator, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateOnlyPicker,
        Description = LanguageManager.Instance.Page_Description_DateOnlyPicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateOnlyPicker)],
        Tags = ["DateOnlyPicker", "Date", "Input"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOnlyPickerDemo/DateOnlyPickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOnlyPickerDemo/DateOnlyPickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateOnly? SelectedDate { get; set; }

    [ObservableProperty]
    [Required(ErrorMessage = "Please select a date")]
    public partial DateOnly? ValidatedDate { get; set; }

    public DateOnlyPickerDemoViewModel()
    {
        SelectedDate = DateOnly.FromDateTime(DateTime.Today);
        ValidatedDate = DateOnly.FromDateTime(DateTime.Today);
    }
}
