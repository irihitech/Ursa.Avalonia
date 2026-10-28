using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Ursa.Controls;
using Ursa.Themes.Fluent2;

namespace Ursa.Fluent2.Demo;

public partial class MainWindow : UrsaWindow
{
    private readonly UrsaFluent2Theme _ursaTheme;
    private readonly Avalonia.Themes.Fluent2.Fluent2Theme _avaloniaTheme;

    public MainWindow()
    {
        InitializeComponent();
        _ursaTheme = Application.Current!.Styles.OfType<UrsaFluent2Theme>().Single();
        _avaloniaTheme = Application.Current.Styles.OfType<Avalonia.Themes.Fluent2.Fluent2Theme>().Single();
    }

    private void UseSystemTheme(object? sender, RoutedEventArgs e) => Application.Current!.RequestedThemeVariant = ThemeVariant.Default;
    private void UseLightTheme(object? sender, RoutedEventArgs e) => Application.Current!.RequestedThemeVariant = ThemeVariant.Light;
    private void UseDarkTheme(object? sender, RoutedEventArgs e) => Application.Current!.RequestedThemeVariant = ThemeVariant.Dark;
    private void UseNormalDensity(object? sender, RoutedEventArgs e) => SetDensity(Avalonia.Themes.Fluent2.DensityStyle.Normal);
    private void UseCompactDensity(object? sender, RoutedEventArgs e) => SetDensity(Avalonia.Themes.Fluent2.DensityStyle.Compact);
    private void UseChinese(object? sender, RoutedEventArgs e) => _ursaTheme.Locale = CultureInfo.GetCultureInfo("zh-CN");
    private void UseEnglish(object? sender, RoutedEventArgs e) => _ursaTheme.Locale = CultureInfo.GetCultureInfo("en-US");

    private void SetDensity(Avalonia.Themes.Fluent2.DensityStyle value)
    {
        _avaloniaTheme.DensityStyle = value;
        _ursaTheme.DensityStyle = value;
    }
}
