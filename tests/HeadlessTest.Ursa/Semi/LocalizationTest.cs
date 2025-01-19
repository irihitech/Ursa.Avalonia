using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Semi.Avalonia.Locale;
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
        MessageBox.ShowOverlayAsync("Hello World", button: MessageBoxButton.YesNo, toplevelHashCode: window.GetHashCode());
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
        MessageBox.ShowOverlayAsync("Hello World", button: MessageBoxButton.YesNo, toplevelHashCode: window.GetHashCode());
        Task.Delay(100).Wait();
        Dispatcher.UIThread.RunJobs();
        var dialog = window.GetVisualDescendants().OfType<MessageBoxControl>().SingleOrDefault();
        var yesButton = dialog?.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.Name == "PART_YesButton");
        Assert.Equal("是", yesButton?.Content?.ToString());
        Assert.NotNull(Application.Current);
        SemiTheme.OverrideLocaleResources(Application.Current, new CultureInfo("en-US"));
        Assert.Equal("Yes", yesButton?.Content?.ToString());
    }
    
    [AvaloniaFact]
    public void Set_NonExisting_Culture_Does_Nothing()
    {
        var window = new UrsaWindow();
        window.Show();
        MessageBox.ShowOverlayAsync("Hello World", button: MessageBoxButton.YesNo, toplevelHashCode: window.GetHashCode());
        Task.Delay(100).Wait();
        Dispatcher.UIThread.RunJobs();
        var dialog = window.GetVisualDescendants().OfType<MessageBoxControl>().SingleOrDefault();
        var yesButton = dialog?.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.Name == "PART_YesButton");
        Assert.Equal("是", yesButton?.Content?.ToString());
        Assert.NotNull(Application.Current);
        // We expect there won't be anyone adding Armenian localization... Subject to change.
        SemiTheme.OverrideLocaleResources(Application.Current, new CultureInfo("hy-AM"));
        Assert.Equal("是", yesButton?.Content?.ToString());
        SemiTheme.OverrideLocaleResources(Application.Current, null);
        Assert.Equal("是", yesButton?.Content?.ToString());
    }
    
    [AvaloniaFact]
    public void Set_English_To_Control_Works()
    {
        var window = new UrsaWindow();
        window.Show();
        MessageBox.ShowOverlayAsync("Hello World", button: MessageBoxButton.YesNo, toplevelHashCode: window.GetHashCode());
        Task.Delay(100).Wait();
        Dispatcher.UIThread.RunJobs();
        var dialog = window.GetVisualDescendants().OfType<MessageBoxControl>().SingleOrDefault();
        var yesButton = dialog?.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.Name == "PART_YesButton");
        Assert.Equal("是", yesButton?.Content?.ToString());
        Assert.NotNull(Application.Current);
        SemiTheme.OverrideLocaleResources(window, new CultureInfo("en-US"));
        Assert.Equal("Yes", yesButton?.Content?.ToString());
    }
    
    [AvaloniaFact]
    public void SemiTheme_Localization_Behavior()
    {
        var theme = new SemiTheme();
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
}