using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;
using Xunit;

namespace Ursa.Themes.Fluent2.Tests;

public class ThemeToggleButtonTests
{
    [AvaloniaFact]
    public void Two_state_toggle_switches_away_from_default_theme()
    {
        var toggle = new ThemeToggleButton();
        var window = new Window { RequestedThemeVariant = ThemeVariant.Default, Content = toggle };
        try
        {
            window.Show();
            Dispatcher.UIThread.RunJobs();
            var button = toggle.GetVisualDescendants().OfType<Button>().Single(b => b.Name == ThemeToggleButton.PART_ThemeButton);
            var expected = toggle.ActualThemeVariant == ThemeVariant.Dark ? ThemeVariant.Light : ThemeVariant.Dark;

            button.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Button.ClickEvent));

            Assert.Equal(expected, toggle.SelectedTheme);
        }
        finally
        {
            window.Close();
        }
    }
}
