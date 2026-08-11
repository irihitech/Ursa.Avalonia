using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.DateTimeOffsetPickerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DateAndTimePage.Category_Key)]
[DocPage(Menu_Header, View = typeof(DateTimeOffsetPickerDemo))]
public partial class DateTimeOffsetPickerDemoViewModel : ObservableValidator, IPageMetadataProvider
{
    public const string Category_Key = "DateTimeOffsetPicker";
    public const string Menu_Header = "Menu_Header_DateTimeOffsetPicker";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_DateTimeOffsetPicker,
        Description = LanguageManager.Instance.Page_Description_DateTimeOffsetPicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_DateTimeOffsetPicker)],
        Tags = ["DateTimeOffsetPicker", "DateTime", "Offset"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateTimeOffsetPickerDemo/DateTimeOffsetPickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/DateTimeOffsetPickerDemo/DateTimeOffsetPickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial DateTimeOffset? SelectedDateTime { get; set; }

    [ObservableProperty]
    [Required(ErrorMessage = "Please select a date and time")]
    public partial DateTimeOffset? ValidatedDateTime { get; set; }

    public DateTimeOffsetPickerDemoViewModel()
    {
        SelectedDateTime = DateTimeOffset.Now;
        ValidatedDateTime = DateTimeOffset.Now;
    }
}
