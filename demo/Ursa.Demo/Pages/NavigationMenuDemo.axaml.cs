using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class NavigationMenuDemo : UserControl
{
    public NavigationMenuDemo()
    {
        InitializeComponent();
        this.DataContext = new NavigationMenuDemoViewModel();
    }
}