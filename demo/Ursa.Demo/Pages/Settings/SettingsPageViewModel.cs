using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Docs;
using Semi.Avalonia;

namespace Ursa.Demo.Pages.Settings;

[DocCategory(Category_Key)]
[DocPage(Menu_Header, View = typeof(SettingsPage))]
public partial class SettingsPageViewModel : ObservableObject
{
    public const string Category_Key = "Settings";
    public const string Menu_Header = "Menu_Header_Settings";

    public IReadOnlyList<ThemeItem> Themes { get; } =
    [
        new("Default", ThemeVariant.Default),
        new("Light", ThemeVariant.Light),
        new("Dark", ThemeVariant.Dark),
        new("Aquatic", SemiTheme.Aquatic),
        new("Desert", SemiTheme.Desert),
        new("Dusk", SemiTheme.Dusk),
        new("NightSky", SemiTheme.NightSky)
    ];

    [ObservableProperty] public partial ThemeItem? SelectedTheme { get; set; }

    public SettingsPageViewModel()
    {
        var current = Application.Current?.RequestedThemeVariant;
        SelectedTheme = Themes.FirstOrDefault(t => t.Theme == current) ?? Themes[0];
    }

    partial void OnSelectedThemeChanged(ThemeItem? oldValue, ThemeItem? newValue)
    {
        if (newValue is null || Application.Current is null) return;
        Application.Current.RequestedThemeVariant = newValue.Theme;
    }
}

public class ThemeItem(string name, ThemeVariant theme)
{
    public string Name { get; set; } = name;
    public ThemeVariant Theme { get; set; } = theme;
}
