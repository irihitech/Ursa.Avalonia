using System.Collections.ObjectModel;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class ElasticWrapPanelDemoViewModel : ObservableObject
{
    [ObservableProperty] private Orientation _selectedOrientation = Orientation.Horizontal;
    [ObservableProperty] private ScrollBarVisibility _horizontalVisibility = ScrollBarVisibility.Auto;
    [ObservableProperty] private ScrollBarVisibility _verticalVisibility = ScrollBarVisibility.Auto;

    [ObservableProperty] private bool _isFillHorizontal;
    [ObservableProperty] private bool _isFillVertical;
    [ObservableProperty] private double _itemWidth = 40d;
    [ObservableProperty] private double _itemHeight = 40d;

    [ObservableProperty] private bool _autoWidth = true;
    [ObservableProperty] private bool _autoHeight = true;
    [ObservableProperty] private double _itemSelfWidth = double.NaN;
    [ObservableProperty] private double _itemSelfHeight = double.NaN;

    [ObservableProperty] private HorizontalAlignment _cmbHAlign = HorizontalAlignment.Left;
    [ObservableProperty] private VerticalAlignment _cmbVAlign = VerticalAlignment.Stretch;

    public ObservableCollection<Orientation> Orientations =>
    [
        Orientation.Horizontal,
        Orientation.Vertical,
    ];

    public ObservableCollection<ScrollBarVisibility> ScrollBarVisibilities =>
    [
        ScrollBarVisibility.Auto,
        ScrollBarVisibility.Hidden,
        ScrollBarVisibility.Visible,
    ];

    public ObservableCollection<HorizontalAlignment> HorizontalAlignments =>
    [
        HorizontalAlignment.Left,
        HorizontalAlignment.Center,
        HorizontalAlignment.Right,
        HorizontalAlignment.Stretch,
    ];

    public ObservableCollection<VerticalAlignment> VerticalAlignments =>
    [
        VerticalAlignment.Top,
        VerticalAlignment.Center,
        VerticalAlignment.Bottom,
        VerticalAlignment.Stretch,
    ];

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