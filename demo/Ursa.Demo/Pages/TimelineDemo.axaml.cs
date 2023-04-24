using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
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