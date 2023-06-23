using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Ursa.Demo.Pages;

public partial class LoadingDemo : UserControl
{
    public LoadingDemo()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}