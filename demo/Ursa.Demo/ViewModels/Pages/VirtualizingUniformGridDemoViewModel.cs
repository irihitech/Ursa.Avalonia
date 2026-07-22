using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Demo.ViewModels.Controls;

using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class VirtualizingUniformGridDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_VirtualizingUniformGrid,
        Description = LanguageManager.Instance.Page_Description_VirtualizingUniformGrid,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_LayoutAndDisplay), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_VirtualizingUniformGrid)],
        Tags = ["VirtualizingUniformGrid", "Panel", "Layout", "Virtualization"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/VirtualizingUniformGridDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/VirtualizingUniformGridDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    public ObservableCollection<GridItem> Items { get; } = [];

    public VirtualizingUniformGridDemoViewModel()
    {
        GenerateItems(100000);
    }

    private void GenerateItems(int count)
    {
        Items.Clear();
        for (int i = 0; i < count; i++)
        {
            Items.Add(new GridItem
            {
                Index = i,
                Label = $"Item {i:N0}",
                Color = GetColorForIndex(i)
            });
        }
    }
    
    private static string GetColorForIndex(int index)
    {
        // Cycle through a set of nice colours.
        var colors = new[] { "#E57373", "#81C784", "#64B5F6", "#FFB74D",
            "#BA68C8", "#4DB6AC", "#FFF176", "#A1887F" };
        return colors[index % colors.Length];
    }

    [ObservableProperty] private int _columns = 4;
    [ObservableProperty] private double _cacheLength = 0.5;
    [ObservableProperty] private double _itemWidth = double.NaN;
    [ObservableProperty] private double _itemHeight = double.NaN;
    [ObservableProperty] private bool _uniformItemHeight = true;
    [ObservableProperty] private int _itemCount = 100000;

    [ObservableProperty] private bool _autoWidth = true;
    [ObservableProperty] private bool _autoHeight = true;

    private double _oldItemWidth;
    private double _oldItemHeight;

    partial void OnAutoWidthChanged(bool value)
    {
        if (value)
        {
            _oldItemWidth = ItemWidth;
            ItemWidth = double.NaN;
        }
        else
        {
            ItemWidth = _oldItemWidth;
        }
    }

    partial void OnAutoHeightChanged(bool value)
    {
        if (value)
        {
            _oldItemHeight = ItemHeight;
            ItemHeight = double.NaN;
        }
        else
        {
            ItemHeight = _oldItemHeight;
        }
    }

    partial void OnItemCountChanged(int value)
    {
        GenerateItems(value);
    }
}

public partial class GridItem : ObservableObject
{
    [ObservableProperty]
    private int _index;

    [ObservableProperty]
    private string _label = string.Empty;

    [ObservableProperty]
    private string _color = "#E0E0E0";
}
