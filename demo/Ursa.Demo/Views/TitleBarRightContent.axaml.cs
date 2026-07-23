using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Irihi.Lingua;
using Semi.Avalonia;
using Ursa.Themes.Semi;

namespace Ursa.Demo.Views;

public partial class TitleBarRightContent : UserControl
{
    public TitleBarRightContent()
    {
        InitializeComponent();
    }

    private async void OpenRepository(object? sender, RoutedEventArgs e)
    {
        var top = TopLevel.GetTopLevel(this);
        if (top is null) return;
        var launcher = top.Launcher;
        await launcher.LaunchUriAsync(new Uri("https://github.com/irihitech/Ursa.Avalonia"));
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
