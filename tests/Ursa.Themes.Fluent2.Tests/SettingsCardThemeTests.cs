using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Themes.Fluent2;
using Avalonia.Threading;
using Ursa.Controls;
using Ursa.Themes.Fluent2;
using Xunit;
using static Ursa.Controls.SettingsCard;

namespace Ursa.Themes.Fluent2.Tests;

public class SettingsCardThemeTests
{
    private static T Part<T>(SettingsCard card, string name) where T : Control =>
        card.GetTemplateDescendants().OfType<T>().Single(part => part.Name == name);

    private static Color ResolveColor(Window window, string key)
    {
        Assert.True(Application.Current!.TryGetResource(key, window.ActualThemeVariant, out var value), key);
        return Assert.IsType<Color>(value);
    }

    [AvaloniaTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void Template_parts_and_expansion_track_state(bool dark)
    {
        var card = new SettingsCard
        {
            Content = "Row",
            Description = "Description",
            Detail = new TextBlock { Text = "Detail" },
        };
        var window = new Window
        {
            Content = new SettingsCardGroup { Items = { card } },
            RequestedThemeVariant = dark ? ThemeVariant.Dark : ThemeVariant.Light,
        };
        try
        {
            window.Show();
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();

            Assert.NotNull(Part<Border>(card, "PART_Root"));
            var chevron = Part<PathIcon>(card, "PART_Chevron");
            Assert.True(chevron.IsVisible);
            Assert.True(Application.Current!.TryGetResource("UrsaFluent2IconChevronDown", window.ActualThemeVariant, out var down));
            Assert.Same(down, chevron.Data);

            var host = Part<ContentPresenter>(card, PART_DetailHost);
            Assert.False(host.IsVisible);
            card.IsExpanded = true;
            window.UpdateLayout();
            Assert.True(host.IsVisible);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void Nested_cards_render_flat_with_separators(bool dark)
    {
        var first = new SettingsCard { Content = "First" };
        var last = new SettingsCard { Content = "Last" };
        var panel = new StackPanel { Children = { first, last } };
        var outer = new SettingsCard { Content = "Outer", Detail = panel, IsExpanded = true };
        var window = new Window
        {
            Content = new SettingsCardGroup { Items = { outer } },
            RequestedThemeVariant = dark ? ThemeVariant.Dark : ThemeVariant.Light,
        };
        try
        {
            window.Show();
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();

            Assert.Equal(Brushes.Transparent, first.Background);
            Assert.Equal(new Thickness(0), first.BorderThickness);
            Assert.Equal(new Thickness(0, 1, 0, 0), last.BorderThickness);
            Assert.Equal(new CornerRadius(0), first.CornerRadius);
            Assert.Equal(new CornerRadius(0), last.CornerRadius);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void Interactive_hover_uses_quiet_subtle_fill_and_yields_over_detail(bool dark)
    {
        var card = new SettingsCard { Content = "Row", Detail = new TextBlock { Text = "Detail" } };
        var window = new Window
        {
            Content = card,
            RequestedThemeVariant = dark ? ThemeVariant.Dark : ThemeVariant.Light,
        };
        try
        {
            window.Show();
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();

            var root = Part<Border>(card, "PART_Root");
            var quietFill = ResolveColor(window, "SubtleFillColorSecondary");
            var cardFill = ResolveColor(window, "CardBackgroundFillColorDefault");
            Assert.Equal(cardFill, Assert.IsAssignableFrom<ISolidColorBrush>(root.Background!).Color);

            ((IPseudoClasses)card.Classes).Set(":pointerover", true);
            Dispatcher.UIThread.RunJobs();
            Assert.Equal(quietFill, Assert.IsAssignableFrom<ISolidColorBrush>(root.Background!).Color);

            // Hovering the expanded detail area must not keep tinting the header row.
            ((IPseudoClasses)card.Classes).Set(":over-detail", true);
            Dispatcher.UIThread.RunJobs();
            Assert.Equal(cardFill, Assert.IsAssignableFrom<ISolidColorBrush>(root.Background!).Color);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void Keyboard_focus_draws_outer_focus_stroke(bool dark)
    {
        var card = new SettingsCard { Content = "Row", ShowChevron = true };
        var window = new Window
        {
            Content = card,
            RequestedThemeVariant = dark ? ThemeVariant.Dark : ThemeVariant.Light,
        };
        try
        {
            window.Show();
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();

            var outer = Part<Border>(card, "PART_Outer");
            Assert.Equal(
                ResolveColor(window, "CardStrokeColorDefault"),
                Assert.IsAssignableFrom<ISolidColorBrush>(outer.BorderBrush!).Color);

            ((IPseudoClasses)card.Classes).Set(":focus-visible", true);
            Dispatcher.UIThread.RunJobs();
            Assert.Equal(
                ResolveColor(window, "FocusStrokeColorOuter"),
                Assert.IsAssignableFrom<ISolidColorBrush>(outer.BorderBrush!).Color);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void Compact_density_shrinks_rows()
    {
        var card = new SettingsCard { Content = "Row" };
        var window = new Window { Content = card };
        var theme = Application.Current!.Styles.OfType<UrsaFluent2Theme>().Single();
        var previous = theme.DensityStyle;
        try
        {
            window.Show();
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            Assert.Equal(44, card.MinHeight);

            theme.DensityStyle = DensityStyle.Compact;
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            Assert.Equal(36, card.MinHeight);
            Assert.Equal(new Thickness(12, 6), card.Padding);
        }
        finally
        {
            theme.DensityStyle = previous;
            window.Close();
        }
    }
}
