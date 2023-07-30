using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Ursa.Demo.Pages;

public partial class KeyGestureInputDemo : UserControl
{
    public KeyGestureInputDemo()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}