using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Ursa.Demo.Pages;

public partial class BadgeDemo : UserControl
{
    public BadgeDemo()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}