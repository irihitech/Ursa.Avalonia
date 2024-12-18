using System.Collections.ObjectModel;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class ThemeTogglerDemoViewModel: ObservableObject
{
    [ObservableProperty] private ObservableCollection<ThemeVariant> _themeSource;

    public ThemeTogglerDemoViewModel()
    {
        ThemeSource = new ObservableCollection<ThemeVariant>
        {
            ThemeVariant.Light,
            ThemeVariant.Dark,
            ThemeVariant.Default
        };
    }
}