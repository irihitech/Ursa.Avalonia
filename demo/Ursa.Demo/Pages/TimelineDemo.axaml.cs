using Avalonia.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class TimelineDemo : UserControl
{
    public TimelineDemo()
    {
        InitializeComponent();
        this.DataContext = new TimelineDemoViewModel();
    }
}