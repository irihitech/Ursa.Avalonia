using Avalonia.Controls;
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