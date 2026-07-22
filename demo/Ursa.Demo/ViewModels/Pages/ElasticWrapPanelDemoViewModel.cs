using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class ElasticWrapPanelDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_ElasticWrapPanel,
        Description = LanguageManager.Instance.Page_Description_ElasticWrapPanel,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_ElasticWrapPanel)],
        Tags = ["ElasticWrapPanel", "Panel", "Layout"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ElasticWrapPanelDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/ElasticWrapPanelDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    [ObservableProperty] public partial Orientation SelectedOrientation { get; set; } = Orientation.Horizontal;
    [ObservableProperty] public partial ScrollBarVisibility HorizontalVisibility { get; set; } = ScrollBarVisibility.Auto;
    [ObservableProperty] public partial ScrollBarVisibility VerticalVisibility { get; set; } = ScrollBarVisibility.Auto;

    [ObservableProperty] public partial bool IsFillHorizontal { get; set; }
    [ObservableProperty] public partial bool IsFillVertical { get; set; }
    [ObservableProperty] public partial double ItemWidth { get; set; } = 40d;
    [ObservableProperty] public partial double ItemHeight { get; set; } = 40d;
    [ObservableProperty] public partial double ItemSpacing { get; set; }
    [ObservableProperty] public partial double LineSpacing { get; set; }

    [ObservableProperty] public partial bool AutoWidth { get; set; } = true;
    [ObservableProperty] public partial bool AutoHeight { get; set; } = true;
    [ObservableProperty] public partial double ItemSelfWidth { get; set; } = double.NaN;
    [ObservableProperty] public partial double ItemSelfHeight { get; set; } = double.NaN;

    [ObservableProperty] public partial HorizontalAlignment CmbHAlign { get; set; } = HorizontalAlignment.Left;
    [ObservableProperty] public partial VerticalAlignment CmbVAlign { get; set; } = VerticalAlignment.Stretch;

    private double _oldItemSelfWidth;
    private double _oldItemSelfHeight;

    partial void OnAutoWidthChanged(bool value)
    {
        if (value)
        {
            _oldItemSelfWidth = ItemSelfWidth;
            ItemSelfWidth = double.NaN;
        }
        else
        {
            ItemSelfWidth = _oldItemSelfWidth;
        }
    }

    partial void OnAutoHeightChanged(bool value)
    {
        if (value)
        {
            _oldItemSelfHeight = ItemSelfHeight;
            ItemSelfHeight = double.NaN;
        }
        else
        {
            ItemSelfHeight = _oldItemSelfHeight;
        }
    }
}