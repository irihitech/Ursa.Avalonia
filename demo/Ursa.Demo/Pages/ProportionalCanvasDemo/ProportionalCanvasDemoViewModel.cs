using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;

using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.ProportionalCanvasDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = LayoutAndDisplayPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(ProportionalCanvasDemo))]
public partial class ProportionalCanvasDemoViewModel: ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "Proportional Canvas";
    public const string Menu_Header = "Menu_Header_ProportionalCanvas";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_ProportionalCanvas,
        Description = LanguageManager.Instance.Page_Description_ProportionalCanvas,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_ProportionalCanvas)],
        Tags = ["ProportionalCanvas", "Canvas", "Layout", "Proportional"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ProportionalCanvasDemo/ProportionalCanvasDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ProportionalCanvasDemo/ProportionalCanvasDemoViewModel.cs",
        InlineXamlSupport = true,
    };
    
    [ObservableProperty] public partial double CanvasWidth { get; set; } = 500;

    [ObservableProperty] public partial double CanvasHeight { get; set; } = 400;
}
