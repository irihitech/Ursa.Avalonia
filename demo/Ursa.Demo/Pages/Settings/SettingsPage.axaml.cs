using Avalonia;
using Avalonia.Controls;
using Irihi.Lingua;
using Semi.Avalonia;
using Ursa.Themes.Semi;

namespace Ursa.Demo.Pages.Settings;

public partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private void CulturePicker_OnCultureChanged(object? sender, CultureChangedEventArgs e)
    {
        if (Application.Current is not null)
        {
            SemiTheme.OverrideLocaleResources(Application.Current, e.Culture?.Culture);
            UrsaSemiTheme.OverrideLocaleResources(Application.Current, e.Culture?.Culture);
        }
    }
}
