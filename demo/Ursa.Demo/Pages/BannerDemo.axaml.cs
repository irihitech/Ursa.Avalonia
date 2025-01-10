using Avalonia.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class BannerDemo : UserControl
{
    public BannerDemo()
    {
        InitializeComponent();
        this.DataContext = new BannerDemoViewModel();
    }
}