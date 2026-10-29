using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.LogicalTree;
using Avalonia.Styling;
using Avalonia.Themes.Fluent2;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;
using Ursa.Fluent2.Demo;
using Ursa.Themes.Fluent2;
using Xunit;

namespace Ursa.Themes.Fluent2.Tests;

public class DemoNavigationTests
{
    [AvaloniaFact]
    public void Search_navigates_to_one_control_and_recovers_from_no_results()
    {
        var window = new MainWindow();
        try
        {
            window.Show();
            var search = window.FindControl<TextBox>("ControlSearch")!;
            var menu = window.FindControl<NavMenu>("NavigationMenu")!;
            var model = window.ViewModel;
            Assert.Equal("Overview", model.CurrentPage!.Id);

            search.Text = "  nUmErIcUpDoWn  ";
            Dispatcher.UIThread.RunJobs();
            Assert.Single(model.Categories);
            Assert.Equal("NumericUpDown", model.CurrentPage!.Id);
            Assert.Same(model.SelectedItem, menu.SelectedItem);
            Assert.Equal("NumericUpDown", window.FindControl<TextBlock>("PageTitle")!.Text);

            search.Text = "no-such-control";
            Dispatcher.UIThread.RunJobs();
            Assert.True(model.NoResults);
            Assert.False(model.HasPage);
            Assert.Null(model.Content);
            Assert.Null(menu.SelectedItem);

            model.ClearSearchCommand.Execute(null);
            Assert.False(model.NoResults);
            Assert.Equal("NumericUpDown", model.CurrentPage!.Id);
            search.Text = "no-such-control";
            search.Text = null;
            Dispatcher.UIThread.RunJobs();
            Assert.False(model.NoResults);
            Assert.Equal(6, model.Categories.Count);
            Assert.Equal("NumericUpDown", model.CurrentPage!.Id);
            Assert.Same(model.SelectedItem, menu.SelectedItem);

            model.IsCollapsed = true;
            window.UpdateLayout();
            Assert.True(menu.IsHorizontalCollapsed);
            Assert.Equal(56, menu.Bounds.Width);
            model.IsCollapsed = false;
            window.UpdateLayout();
            Assert.Equal(272, menu.Bounds.Width);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void Settings_update_both_themes_and_localize_without_losing_the_active_page()
    {
        var app = Application.Current!;
        var fluent = app.Styles.OfType<Fluent2Theme>().Single();
        var ursa = app.Styles.OfType<UrsaFluent2Theme>().Single();
        var previousTheme = app.RequestedThemeVariant;
        var previousFluentDensity = fluent.DensityStyle;
        var previousUrsaDensity = ursa.DensityStyle;
        var previousLocale = ursa.Locale;
        var window = new MainWindow();
        try
        {
            window.Show();
            var model = window.ViewModel;
            model.SelectedItem = model.Categories.SelectMany(category => category.Children)
                .Single(item => item.PageId == "NumericUpDown");
            window.UpdateLayout();
            var page = model.Content;
            var view = Assert.Single(window.GetVisualDescendants().OfType<Ursa.Fluent2.Demo.Pages.DemoPageView>());
            var input = window.GetVisualDescendants().OfType<NumericIntUpDown>().First();
            var appearance = window.FindControl<Button>("AppearanceButton")!;
            var flyout = Assert.IsType<Flyout>(appearance.Flyout);
            flyout.ShowAt(appearance);
            Dispatcher.UIThread.RunJobs();
            var settings = Assert.IsType<StackPanel>(flyout.Content).GetLogicalDescendants()
                .OfType<ComboBox>().ToDictionary(combo => combo.Name!);

            settings["ThemeChoice"].SelectedIndex = 1;
            Assert.Equal(ThemeVariant.Light, app.RequestedThemeVariant);
            settings["ThemeChoice"].SelectedIndex = 2;
            Assert.Equal(ThemeVariant.Dark, app.RequestedThemeVariant);
            settings["DensityChoice"].SelectedIndex = 1;
            Assert.Equal(DensityStyle.Compact, fluent.DensityStyle);
            Assert.Equal(DensityStyle.Compact, ursa.DensityStyle);
            Assert.Equal(24, input.MinHeight);

            settings["LanguageChoice"].SelectedIndex = 0;
            Assert.Equal("en-US", ursa.Locale!.Name);
            Assert.Equal("Inputs and selection", model.PageCategory);
            settings["LanguageChoice"].SelectedIndex = 1;
            Assert.Equal("zh-CN", ursa.Locale!.Name);
            Assert.Equal("输入与选择", model.PageCategory);
            Assert.Equal(2, model.ThemeIndex);
            Assert.Equal(1, model.DensityIndex);
            Assert.Equal(ThemeVariant.Dark, app.RequestedThemeVariant);
            Assert.Same(page, model.Content);
            Assert.Same(view, Assert.Single(window.GetVisualDescendants().OfType<Ursa.Fluent2.Demo.Pages.DemoPageView>()));
            Assert.Same(model.SelectedItem, window.FindControl<NavMenu>("NavigationMenu")!.SelectedItem);
            Assert.Equal(2, settings["ThemeChoice"].SelectedIndex);
            Assert.Equal(1, settings["DensityChoice"].SelectedIndex);
            app.RequestedThemeVariant = ThemeVariant.Light;
            Assert.Equal(1, settings["ThemeChoice"].SelectedIndex);
            flyout.Hide();
        }
        finally
        {
            window.Close();
            app.RequestedThemeVariant = previousTheme;
            fluent.DensityStyle = previousFluentDensity;
            ursa.DensityStyle = previousUrsaDensity;
            ursa.Locale = previousLocale;
        }
    }
}
