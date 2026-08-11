using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Ursa.Demo.Pages.AboutUsDemo;

public partial class AboutUsDemo : UserControl
{
    public AboutUsDemo()
    {
        InitializeComponent();
    }
    
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (DataContext is AboutUsDemoViewModel vm)
        {
            var launcher = TopLevel.GetTopLevel(this)?.Launcher;
            vm.Launcher = launcher;
        }
    }
}