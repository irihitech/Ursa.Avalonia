using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Ursa.Demo.Localizations;

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

    private void LangSwitchButton_Clicked(object? sender, RoutedEventArgs e)
    {
        var condition = string.Equals(LanguageManager.Instance.CurrentCulture.TwoLetterISOLanguageName, "zh");
        LanguageManager.Instance.UpdateCulture(condition ? CultureInfo.InvariantCulture : new CultureInfo("zh-Hans"));
    }
}
