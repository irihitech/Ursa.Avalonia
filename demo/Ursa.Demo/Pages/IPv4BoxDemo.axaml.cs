using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Ursa.Demo.Pages;

public partial class IPv4BoxDemo : UserControl
{
    public IPv4BoxDemo()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}