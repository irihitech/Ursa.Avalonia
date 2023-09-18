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
    [ObservableProperty] private double _itemWidth;
    [ObservableProperty] private double _itemHeight;


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
        ItemWidth = 100d;
        ItemHeight = double.NaN;
    }
}