using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;

namespace Ursa.Demo.Pages;

public partial class ThemeVariantMapperDemo : UserControl
{
    public ThemeVariantMapperDemo()
    {
        InitializeComponent();
    }
    
    private void OnSetLight(object? sender, RoutedEventArgs e)
        => DynamicParentScope.RequestedThemeVariant = ThemeVariant.Light;

    private void OnSetDark(object? sender, RoutedEventArgs e)
        => DynamicParentScope.RequestedThemeVariant = ThemeVariant.Dark;

    private void OnSetDefault(object? sender, RoutedEventArgs e)
        => DynamicParentScope.RequestedThemeVariant = ThemeVariant.Default;
}

