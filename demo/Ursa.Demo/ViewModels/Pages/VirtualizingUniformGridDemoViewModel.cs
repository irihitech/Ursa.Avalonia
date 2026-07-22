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

    [ObservableProperty] public partial int Columns { get; set; } = 4;
    [ObservableProperty] public partial double CacheLength { get; set; } = 0.5;
    [ObservableProperty] public partial double ItemWidth { get; set; } = double.NaN;
    [ObservableProperty] public partial double ItemHeight { get; set; } = double.NaN;
    [ObservableProperty] public partial bool UniformItemHeight { get; set; } = true;
    [ObservableProperty] public partial int ItemCount { get; set; } = 100000;

    [ObservableProperty] public partial bool AutoWidth { get; set; } = true;
    [ObservableProperty] public partial bool AutoHeight { get; set; } = true;

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
    [ObservableProperty] public partial int Index { get; set; }

    [ObservableProperty] public partial string Label { get; set; } = string.Empty;

    [ObservableProperty] public partial string Color { get; set; } = "#E0E0E0";
}
