using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class RatingDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Rating,
        Description = LanguageManager.Instance.Page_Description_Rating,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Rating)],
        Tags = ["Rating", "Star", "Input"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/RatingDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/RatingDemoViewModel.cs",
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