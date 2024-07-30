using Avalonia.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class NavMenuDemo : UserControl
{
    public NavMenuDemo()
    {
        InitializeComponent();
        DataContext = new NavMenuDemoViewModel();
    }
}