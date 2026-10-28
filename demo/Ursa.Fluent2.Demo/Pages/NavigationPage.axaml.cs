using Avalonia.Controls;
using Avalonia.Interactivity;
using Ursa.Controls;

namespace Ursa.Fluent2.Demo.Pages;

public partial class NavigationPage : UserControl
{
    public NavigationPage() => InitializeComponent();

    private void OpenSample(object? sender, RoutedEventArgs e)
    {
        var button = (Button)sender!;
        var name = (string)button.Tag!;
        Window window = name == "SplashWindow" ? new DemoSplashWindow() : new UrsaWindow();
        window.Title = $"{name} · Fluent 2";
        window.Width = 480;
        window.Height = 320;
        window.Content = new SampleMessage { DataContext = $"{name} theme sample." };
        if (TopLevel.GetTopLevel(button) is Window owner)
            window.Show(owner);
        else
            window.Show();
    }

    private sealed class DemoSplashWindow : SplashWindow
    {
        protected override Task<Window?> CreateNextWindow() => Task.FromResult<Window?>(null);
    }
}
