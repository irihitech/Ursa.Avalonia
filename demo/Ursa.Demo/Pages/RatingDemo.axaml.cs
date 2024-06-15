using Avalonia.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class RatingDemo : UserControl
{
    public RatingDemo()
    {
        InitializeComponent();
        this.DataContext = new RatingDemoViewModel();
    }
}