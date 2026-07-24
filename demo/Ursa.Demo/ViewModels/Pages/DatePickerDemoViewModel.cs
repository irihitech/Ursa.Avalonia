using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class DatePickerDemoViewModel: ObservableValidator, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DatePicker,
        Description = LanguageManager.Instance.Page_Description_DatePicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DatePicker)],
        Tags = ["DatePicker", "Date", "Input"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DatePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/DatePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateTime? SelectedDate { get; set; }

    [ObservableProperty]
    [Required(ErrorMessage = "Please select a date")]
    public partial DateTime? ValidatedDate { get; set; }

    public DatePickerDemoViewModel()
    {
        SelectedDate = DateTime.Today;
        ValidatedDate = DateTime.Today;
    }
}