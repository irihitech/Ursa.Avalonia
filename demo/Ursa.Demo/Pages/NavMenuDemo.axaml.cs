using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class NavMenuDemo : UserControl
{
    public NavMenuDemo()
    {
        InitializeComponent();
        this.DataContext = new NavMenuDemoViewModel();
    }
}