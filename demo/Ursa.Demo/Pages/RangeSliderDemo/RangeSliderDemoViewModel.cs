using System.Collections.ObjectModel;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.RangeSliderDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(RangeSliderDemo))]
public partial class RangeSliderDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "RangeSlider";
    public const string Menu_Header = "Menu_Header_RangeSlider";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_RangeSlider,
        Description = LanguageManager.Instance.Page_Description_RangeSlider,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_RangeSlider)],
        Tags = ["RangeSlider", "Slider", "Range"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/RangeSliderDemo/RangeSliderDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/RangeSliderDemo/RangeSliderDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public ObservableCollection<Orientation> Orientations { get; set; } = new ObservableCollection<Orientation>()
    {
        Orientation.Horizontal,
        Orientation.Vertical
    };

    [ObservableProperty] public partial Orientation Orientation { get; set; }
}