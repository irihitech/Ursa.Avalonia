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

    [ObservableProperty] private Orientation _selectedOrientation = Orientation.Horizontal;
    [ObservableProperty] private ScrollBarVisibility _horizontalVisibility = ScrollBarVisibility.Auto;
    [ObservableProperty] private ScrollBarVisibility _verticalVisibility = ScrollBarVisibility.Auto;

    [ObservableProperty] private bool _isFillHorizontal;
    [ObservableProperty] private bool _isFillVertical;
    [ObservableProperty] private double _itemWidth = 40d;
    [ObservableProperty] private double _itemHeight = 40d;
    [ObservableProperty] private double _itemSpacing;
    [ObservableProperty] private double _lineSpacing;

    [ObservableProperty] private bool _autoWidth = true;
    [ObservableProperty] private bool _autoHeight = true;
    [ObservableProperty] private double _itemSelfWidth = double.NaN;
    [ObservableProperty] private double _itemSelfHeight = double.NaN;

    [ObservableProperty] private HorizontalAlignment _cmbHAlign = HorizontalAlignment.Left;
    [ObservableProperty] private VerticalAlignment _cmbVAlign = VerticalAlignment.Stretch;

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