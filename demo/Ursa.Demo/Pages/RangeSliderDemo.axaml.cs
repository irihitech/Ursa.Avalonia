using Avalonia.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class RangeSliderDemo : UserControl
{
    public RangeSliderDemo()
    {
        InitializeComponent();
        this.DataContext = new RangeSliderDemoViewModel();
    }
}