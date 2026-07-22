using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Demo.ViewModels.Controls;

using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class ProportionalCanvasDemoViewModel: ViewModelBase, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_ProportionalCanvas,
        Description = LanguageManager.Instance.Page_Description_ProportionalCanvas,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_ProportionalCanvas)],
        Tags = ["ProportionalCanvas", "Canvas", "Layout", "Proportional"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ProportionalCanvasDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/ProportionalCanvasDemoViewModel.cs",
        InlineXamlSupport = true,
    };
    
    [ObservableProperty]
    private double _canvasWidth = 500;

    [ObservableProperty]
    private double _canvasHeight = 400;
}
