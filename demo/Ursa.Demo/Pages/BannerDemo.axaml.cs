using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Ursa.Demo.Pages;

public partial class BannerDemo : UserControl
{
    public BannerDemo()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}