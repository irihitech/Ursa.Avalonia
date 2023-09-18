using Avalonia.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class ElasticWrapPanelDemo : UserControl
{
    public ElasticWrapPanelDemo()
    {
        InitializeComponent();
        DataContext = new ElasticWrapPanelDemoViewModel();
    }
}