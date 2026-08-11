using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.RatingDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(RatingDemo))]
public partial class RatingDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "Rating";
    public const string Menu_Header = "Menu_Header_Rating";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Rating,
        Description = LanguageManager.Instance.Page_Description_Rating,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Rating)],
        Tags = ["Rating", "Star", "Input"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/RatingDemo/RatingDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/RatingDemo/RatingDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial bool AllowClear { get; set; } = true;
    [ObservableProperty] public partial bool AllowHalf { get; set; } = true;
    [ObservableProperty] public partial bool IsEnabled { get; set; } = true;
    [ObservableProperty] public partial double Value { get; set; }
    [ObservableProperty] public partial double DefaultValue { get; set; } = 2.3;
    [ObservableProperty] public partial int Count { get; set; } = 5;
}