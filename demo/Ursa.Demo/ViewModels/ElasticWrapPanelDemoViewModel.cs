using System.Collections.ObjectModel;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class ElasticWrapPanelDemoViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Orientation> _orientations = [Orientation.Horizontal, Orientation.Vertical];

    [ObservableProperty] private Orientation _selectedOrientation = Orientation.Horizontal;

    [ObservableProperty] private ObservableCollection<ScrollBarVisibility> _hScrollBarVisibilities =
    [
        ScrollBarVisibility.Disabled, ScrollBarVisibility.Auto, ScrollBarVisibility.Hidden, ScrollBarVisibility.Visible
    ];

    [ObservableProperty] private ObservableCollection<ScrollBarVisibility> _vScrollBarVisibilities =
    [
        ScrollBarVisibility.Disabled, ScrollBarVisibility.Auto, ScrollBarVisibility.Hidden, ScrollBarVisibility.Visible
    ];

    [ObservableProperty] private ScrollBarVisibility _horizontalVisibility = ScrollBarVisibility.Auto;
    [ObservableProperty] private ScrollBarVisibility _verticalVisibility = ScrollBarVisibility.Auto;

    [ObservableProperty] private bool _isFillHorizontal = true;
    [ObservableProperty] private bool _isFillVertical = false;
    [ObservableProperty] private double _itemWidth = 40d;
    [ObservableProperty] private double _itemHeight = 40d;

    [ObservableProperty] private bool _autoWidth;
    [ObservableProperty] private bool _autoHeight;
    [ObservableProperty] private double _itemSelfWidth = double.NaN;
    [ObservableProperty] private double _itemSelfHeight = double.NaN;

    [ObservableProperty] private ObservableCollection<HorizontalAlignment> _cmbHAligns =
    [
        HorizontalAlignment.Stretch,
        HorizontalAlignment.Left,
        HorizontalAlignment.Center,
        HorizontalAlignment.Right
    ];

    [ObservableProperty] private ObservableCollection<VerticalAlignment> _cmbVAligns =
    [
        VerticalAlignment.Stretch,
        VerticalAlignment.Top,
        VerticalAlignment.Center,
        VerticalAlignment.Bottom
    ];

    [ObservableProperty] private HorizontalAlignment _cmbHAlign = HorizontalAlignment.Stretch;
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