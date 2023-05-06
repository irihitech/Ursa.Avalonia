using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class PaginationDemo : UserControl
{
    public PaginationDemo()
    {
        InitializeComponent();
        this.DataContext = new PaginationDemoViewModel();
    }
}