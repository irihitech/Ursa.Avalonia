using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.DateTimePickerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DateAndTimePage.Category_Key)]
[DocPage(Menu_Header, View = typeof(DateTimePickerDemo))]
public partial class DateTimePickerDemoViewModel : ObservableValidator, IPageMetadataProvider
{
    public const string Category_Key = "DateTimePicker";
    public const string Menu_Header = "Menu_Header_DateTimePicker";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateTimePicker,
        Description = LanguageManager.Instance.Page_Description_DateTimePicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateTimePicker)],
        Tags = ["DateTimePicker", "Date", "Time"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateTimePickerDemo/DateTimePickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateTimePickerDemo/DateTimePickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty]
    [Required(ErrorMessage = "Please select a date and time")]
    public partial DateTime? ValidatedDateTime { get; set; }

    public DateTimePickerDemoViewModel()
    {
        ValidatedDateTime = DateTime.Now;
    }
}