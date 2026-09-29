using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;
using Ursa.Themes.Semi;

namespace HeadlessTest.Ursa.Semi;

public class LocalizationTest
{
    [AvaloniaFact]
    public void Default_Locale_Is_Chinese()
    {
        var window = new UrsaWindow();
        window.Show();
        OverlayMessageBox.ShowAsync("Hello World", button: MessageBoxButton.YesNo, toplevelHashCode: window.GetHashCode());
        Task.Delay(100).Wait();
        Dispatcher.UIThread.RunJobs();
        var dialog = window.GetVisualDescendants().OfType<MessageBoxControl>().SingleOrDefault();
        var yesButton = dialog?.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.Name == "PART_YesButton");
        Assert.Equal("是", yesButton?.Content?.ToString());
    }
    
    [AvaloniaFact]
    public void Set_English_Works()
    {
        var window = new UrsaWindow();
        window.Show();
        OverlayMessageBox.ShowAsync("Hello World", button: MessageBoxButton.YesNo, toplevelHashCode: window.GetHashCode());
        Task.Delay(100).Wait();
        Dispatcher.UIThread.RunJobs();
        var dialog = window.GetVisualDescendants().OfType<MessageBoxControl>().SingleOrDefault();
        var yesButton = dialog?.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.Name == "PART_YesButton");
        Assert.Equal("是", yesButton?.Content?.ToString());
        Assert.NotNull(Application.Current);
        UrsaSemiTheme.OverrideLocaleResources(Application.Current, new CultureInfo("en-US"));
        Assert.Equal("Yes", yesButton?.Content?.ToString());
    }
    
    [AvaloniaFact]
    public void Set_NonExisting_Culture_Does_Nothing()
    {
        var window = new UrsaWindow();
        window.Show();
        OverlayMessageBox.ShowAsync("Hello World", button: MessageBoxButton.YesNo, toplevelHashCode: window.GetHashCode());
        Task.Delay(100).Wait();
        Dispatcher.UIThread.RunJobs();
        var dialog = window.GetVisualDescendants().OfType<MessageBoxControl>().SingleOrDefault();
        var yesButton = dialog?.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.Name == "PART_YesButton");
        Assert.Equal("是", yesButton?.Content?.ToString());
        Assert.NotNull(Application.Current);
        // We expect there won't be anyone adding Armenian localization... Subject to change.
        UrsaSemiTheme.OverrideLocaleResources(Application.Current, new CultureInfo("hy-AM"));
        Assert.Equal("是", yesButton?.Content?.ToString());
        UrsaSemiTheme.OverrideLocaleResources(Application.Current, null);
        Assert.Equal("是", yesButton?.Content?.ToString());
    }
    
    [AvaloniaFact]
    public void Set_English_To_Control_Works()
    {
        var window = new UrsaWindow();
        window.Show();
        OverlayMessageBox.ShowAsync("Hello World", button: MessageBoxButton.YesNo, toplevelHashCode: window.GetHashCode());
        Task.Delay(100).Wait();
        Dispatcher.UIThread.RunJobs();
        var dialog = window.GetVisualDescendants().OfType<MessageBoxControl>().SingleOrDefault();
        var yesButton = dialog?.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.Name == "PART_YesButton");
        Assert.Equal("是", yesButton?.Content?.ToString());
        Assert.NotNull(Application.Current);
        UrsaSemiTheme.OverrideLocaleResources(window, new CultureInfo("en-US"));
        Assert.Equal("Yes", yesButton?.Content?.ToString());
    }
    
    [AvaloniaFact]
    public void SemiTheme_Localization_Behavior()
    {
        var theme = new UrsaSemiTheme();
        Assert.Null(theme.Locale);
        theme.Locale = new CultureInfo("en-US");
        Assert.Equal(new CultureInfo("en-US"), theme.Locale);
        var yesText = theme.Resources["STRING_MENU_DIALOG_YES"];
        Assert.Equal("Yes", yesText);
        theme.Locale = new CultureInfo("zh-CN");
        Assert.Equal(new CultureInfo("zh-CN"), theme.Locale);
        yesText = theme.Resources["STRING_MENU_DIALOG_YES"];
        Assert.Equal("是", yesText);
        theme.Locale = new CultureInfo("hy-AM");
        Assert.Equal(new CultureInfo("zh-CN"), theme.Locale);
        yesText = theme.Resources["STRING_MENU_DIALOG_YES"];
        Assert.Equal("是", yesText);
        theme.Locale = null;
        Assert.Equal(new CultureInfo("zh-CN"), theme.Locale);
        yesText = theme.Resources["STRING_MENU_DIALOG_YES"];
        Assert.Equal("是", yesText);
    }

    [AvaloniaTheory]
    [InlineData("ko-KR", "예", "아니요")]
    [InlineData("ja-JP", "はい", "いいえ")]
    public void Korean_And_Japanese_Locales_Have_Complete_Resources(string cultureName, string yes, string no)
    {
        var culture = new CultureInfo(cultureName);
        var theme = new UrsaSemiTheme { Locale = culture };
        Assert.Equal(culture, theme.Locale);
        Assert.Equal(yes, theme.Resources["STRING_MENU_DIALOG_YES"]);
        Assert.Equal(no, theme.Resources["STRING_MENU_DIALOG_NO"]);

        var english = new global::Ursa.Themes.Semi.Locale.en_us();
        var localized = cultureName == "ko-KR"
            ? (ResourceDictionary)new global::Ursa.Themes.Semi.Locale.ko_kr()
            : new global::Ursa.Themes.Semi.Locale.ja_jp();
        Assert.Equal(english.Count, localized.Count);
        foreach (var key in english.Keys)
            Assert.True(localized.ContainsKey(key), $"Missing {key} in {cultureName}");

        var legacyTheme = new global::Ursa.Themes.Semi.Legacy.SemiTheme { Locale = culture };
        Assert.Equal(culture, legacyTheme.Locale);
        Assert.Equal(yes, legacyTheme.Resources["STRING_MENU_DIALOG_YES"]);

        Assert.NotNull(Application.Current);
        var window = new UrsaWindow();
        window.Show();
        OverlayMessageBox.ShowAsync("Hello World", button: MessageBoxButton.YesNo, toplevelHashCode: window.GetHashCode());
        Task.Delay(100).Wait();
        Dispatcher.UIThread.RunJobs();
        var dialog = window.GetVisualDescendants().OfType<MessageBoxControl>().SingleOrDefault();
        var yesButton = dialog?.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.Name == "PART_YesButton");
        UrsaSemiTheme.OverrideLocaleResources(Application.Current, culture);
        Assert.Equal(yes, Application.Current.Resources["STRING_MENU_DIALOG_YES"]);
        Assert.Equal(yes, yesButton?.Content?.ToString());
    }
}