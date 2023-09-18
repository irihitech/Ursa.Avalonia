using System.Collections.ObjectModel;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class ElasticWrapPanelDemoViewModel : ObservableObject
{
    [ObservableProperty] private Orientation _selectedOrientation;
    [ObservableProperty] private ScrollBarVisibility _horizontalVisibility;
    [ObservableProperty] private ScrollBarVisibility _verticalVisibility;
    [ObservableProperty] private ObservableCollection<Orientation> _orientations = null!;
    [ObservableProperty] private ObservableCollection<ScrollBarVisibility> _hScrollBarVisibilities = null!;
    [ObservableProperty] private ObservableCollection<ScrollBarVisibility> _vScrollBarVisibilities = null!;

    [ObservableProperty] private bool _isFillHorizontal;
    [ObservableProperty] private bool _isFillVertical;
    [ObservableProperty] private double? _itemWidth;
    [ObservableProperty] private double? _itemHeight;

    [ObservableProperty] private double? _itemSelfWidth;
    [ObservableProperty] private double? _itemSelfHeight;

    [ObservableProperty] private HorizontalAlignment _cmbHAlign;
    [ObservableProperty] private VerticalAlignment _cmbVAlign;
    [ObservableProperty] private ObservableCollection<HorizontalAlignment> _cmbHAligns = null!;
    [ObservableProperty] private ObservableCollection<VerticalAlignment> _cmbVAligns = null!;

    public ElasticWrapPanelDemoViewModel()
    {
        SelectedOrientation = Orientation.Horizontal;
        HorizontalVisibility = VerticalVisibility = ScrollBarVisibility.Auto;
        Orientations = new ObservableCollection<Orientation>() { Orientation.Horizontal, Orientation.Vertical };
        HScrollBarVisibilities = new ObservableCollection<ScrollBarVisibility>()
        {
            ScrollBarVisibility.Disabled,
            ScrollBarVisibility.Auto,
            ScrollBarVisibility.Hidden,
            ScrollBarVisibility.Visible
        };
        VScrollBarVisibilities = new ObservableCollection<ScrollBarVisibility>(HScrollBarVisibilities);

        IsFillHorizontal = true;
        IsFillVertical = false;
        ItemWidth = 40d;
        ItemHeight = 40d;

        ItemSelfWidth = double.NaN;
        ItemSelfHeight = double.NaN;

        CmbHAligns = new ObservableCollection<HorizontalAlignment>()
        {
            HorizontalAlignment.Stretch,
            HorizontalAlignment.Left,
            HorizontalAlignment.Center,
            HorizontalAlignment.Right
        };
        CmbVAligns = new ObservableCollection<VerticalAlignment>()
        {
            VerticalAlignment.Stretch,
            VerticalAlignment.Top,
            VerticalAlignment.Center,
            VerticalAlignment.Bottom
        };
        CmbHAlign = HorizontalAlignment.Stretch;
        CmbVAlign = VerticalAlignment.Stretch;
    }
}