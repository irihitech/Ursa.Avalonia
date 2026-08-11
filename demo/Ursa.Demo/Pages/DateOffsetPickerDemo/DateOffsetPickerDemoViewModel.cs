using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.DateOffsetPickerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DateAndTimePage.Category_Key)]
[DocPage(Menu_Header, View = typeof(DateOffsetPickerDemo))]
public partial class DateOffsetPickerDemoViewModel : ObservableValidator, IPageMetadataProvider
{
    public const string Category_Key = "DateOffsetPicker";
    public const string Menu_Header = "Menu_Header_DateOffsetPicker";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateOffsetPicker,
        Description = LanguageManager.Instance.Page_Description_DateOffsetPicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateOffsetPicker)],
        Tags = ["DateOffsetPicker", "Date", "Offset"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOffsetPickerDemo/DateOffsetPickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateOffsetPickerDemo/DateOffsetPickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateTimeOffset? SelectedDate { get; set; }

    [ObservableProperty]
    [Required(ErrorMessage = "Please select a date")]
    public partial DateTimeOffset? ValidatedDate { get; set; }

    public DateOffsetPickerDemoViewModel()
    {
        SelectedDate = DateTimeOffset.Now;
        ValidatedDate = DateTimeOffset.Now;
    }
}
