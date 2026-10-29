using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Shapes;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;
using Ursa.Fluent2.Demo;
using Ursa.Themes.Fluent2;
using Xunit;

namespace Ursa.Themes.Fluent2.Tests;

public class ThemeSmokeTests
{
    [AvaloniaTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void Nav_menu_chevron_matches_fluent_dropdowns_and_preserves_state_directions(bool dark)
    {
        var leaf = new NavMenuItem { Header = "Leaf" };
        var nested = new NavMenuItem { Header = "Nested", Items = { leaf } };
        var parent = new NavMenuItem { Header = "Parent", Items = { nested } };
        var menu = new NavMenu { Items = { parent } };
        var combo = new MultiComboBox { Width = 200 };
        var window = new Window
        {
            Content = new StackPanel { Children = { menu, combo } },
            RequestedThemeVariant = dark ? ThemeVariant.Dark : ThemeVariant.Light
        };
        try
        {
            window.Show();
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            var icon = parent.GetVisualDescendants().OfType<PathIcon>()
                .First(element => element.Name == "PART_ExpanderIcon");
            var dropdown = combo.GetVisualDescendants().OfType<PathIcon>()
                .Single(element => element.Name == "DropDownGlyph");
            icon.Transitions = null;
            Assert.Same(dropdown.Data, icon.Data);
            Assert.Equal(dropdown.Width, icon.Width);
            Assert.Equal(dropdown.Height, icon.Height);
            Assert.Equal(8, icon.Width);
            Assert.Equal(8, icon.Height);

            parent.IsVerticalCollapsed = false;
            Assert.Equal(-1, icon.RenderTransform!.Value.M11, 6);
            parent.IsVerticalCollapsed = true;
            Assert.Equal(Matrix.Identity, icon.RenderTransform!.Value);
            parent.IsVerticalCollapsed = false;
            Assert.Equal(-1, icon.RenderTransform!.Value.M11, 6);

            var nestedIcon = nested.GetVisualDescendants().OfType<PathIcon>()
                .First(element => element.Name == "PART_ExpanderIcon");
            nestedIcon.Transitions = null;
            var leafIcon = leaf.GetVisualDescendants().OfType<PathIcon>()
                .Single(element => element.Name == "PART_ExpanderIcon");
            Assert.False(leafIcon.IsVisible);
            menu.IsHorizontalCollapsed = true;
            Assert.False(icon.IsVisible);
            Assert.Equal(-1, nestedIcon.RenderTransform!.Value.M12, 6);
            menu.IsHorizontalCollapsed = false;
            Assert.True(icon.IsVisible);

            var replacement = Geometry.Parse("M0 0 L4 4 L8 0");
            window.Resources["NavigationMenuItemExpandIconGlyph"] = replacement;
            window.Resources["NavigationMenuItemExpandIconWidth"] = 10d;
            Assert.Same(replacement, icon.Data);
            Assert.Equal(10, icon.Width);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaTheory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void Multi_auto_complete_box_renders_its_outer_border(bool dark, bool hasSelection)
    {
        var control = new MultiAutoCompleteBox
        {
            Width = 280,
            SelectedItems = new System.Collections.ObjectModel.ObservableCollection<string>(
                hasSelection ? ["Selected item"] : [])
        };
        var window = new Window
        {
            Content = control,
            RequestedThemeVariant = dark ? ThemeVariant.Dark : ThemeVariant.Light
        };
        try
        {
            window.Show();
            window.UpdateLayout();
            Dispatcher.UIThread.RunJobs();
            var border = control.GetVisualDescendants().OfType<Border>()
                .Single(element => element.Name == "PART_RootBorder");
            Assert.True(Application.Current!.TryGetResource("TextBoxBorderThickness", window.ActualThemeVariant, out var thickness));
            Assert.Equal(new Thickness(1), Assert.IsType<Thickness>(thickness));
            Assert.Equal(thickness, border.BorderThickness);
            Assert.IsAssignableFrom<IBrush>(border.BorderBrush);
            Assert.True(border.BorderBrush!.Opacity > 0);
            Assert.True(border.Bounds.Width > 0);
            Assert.True(border.Bounds.Height > 0);

            control.BorderThickness = new Thickness(2);
            Assert.Equal(new Thickness(2), border.BorderThickness);
            control.ClearValue(TemplatedControl.BorderThicknessProperty);
            Assert.Equal(thickness, border.BorderThickness);

            window.Resources["TextBoxBorderThickness"] = new Thickness(3);
            Assert.Equal(new Thickness(3), border.BorderThickness);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void Numeric_input_uses_fluent_spinner_border_without_nested_textbox_border(bool dark)
    {
        var native = new Avalonia.Controls.NumericUpDown { Value = 3, Width = 280 };
        var ursa = new NumericIntUpDown { Value = 3, Width = 280 };
        var window = new Window
        {
            Content = new StackPanel { Children = { native, ursa } },
            RequestedThemeVariant = dark ? ThemeVariant.Dark : ThemeVariant.Light
        };
        try
        {
            window.Show();
            window.UpdateLayout();
            Dispatcher.UIThread.RunJobs();
            var nativeText = native.GetVisualDescendants().OfType<TextBox>().Single();
            var ursaText = ursa.GetVisualDescendants().OfType<TextBox>().Single();
            var nativeSpinner = native.GetVisualDescendants().OfType<ButtonSpinner>().Single();
            var ursaSpinner = ursa.GetVisualDescendants().OfType<ButtonSpinner>().Single();

            Assert.Same(nativeText.Theme, ursaText.Theme);
            Assert.DoesNotContain(ursaText.GetVisualDescendants().OfType<Border>(), border => border.Name == "PART_BorderElement");
            Assert.Equal(nativeSpinner.BorderThickness, ursaSpinner.BorderThickness);
            Assert.Equal(nativeSpinner.CornerRadius, ursaSpinner.CornerRadius);
            Assert.Equal(nativeText.Padding, ursaText.Padding);
            Assert.Equal("3", ursaText.Text);

            ursaText.Focus();
            Dispatcher.UIThread.RunJobs();
            var outline = ursaSpinner.GetVisualDescendants().OfType<Border>().Single(border => border.Name == "BorderElement");
            Assert.True(Application.Current!.TryGetResource("TextControlBorderBrushFocused", window.ActualThemeVariant, out var focusedBorder));
            Assert.Equal(focusedBorder, outline.BorderBrush);

            ursa.IsReadOnly = true;
            Assert.True(ursaText.IsReadOnly);
            ursa.ShowButtonSpinner = false;
            Assert.False(ursaSpinner.ShowButtonSpinner);
            ursa.AllowDrag = true;
            Assert.True(ursa.GetVisualDescendants().OfType<Panel>().Single(panel => panel.Name == "PART_DragPanel").IsVisible);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void Gallery_loads_control_templates_and_fluent_resources(bool dark)
    {
        var window = new MainWindow { RequestedThemeVariant = dark ? ThemeVariant.Dark : ThemeVariant.Light };
        try
        {
            window.Show();
            window.UpdateLayout();

            var menu = window.FindControl<NavMenu>("NavigationMenu")!;
            var model = window.ViewModel;
            Assert.Equal(6, model.Categories.Count);
            Assert.Equal(73, DemoCatalog.Pages.Count);
            Assert.Equal(73, DemoCatalog.Pages.Select(page => page.Id).Distinct().Count());
            var examples = 0;
            foreach (var item in model.Categories.SelectMany(category => category.Children))
            {
                menu.SelectedItem = item;
                window.UpdateLayout();
                Dispatcher.UIThread.RunJobs();
                Assert.Equal(item.PageId, model.CurrentPage!.Id);
                Assert.Equal(item.PageId, model.Content!.PageId);
                Assert.Same(model.Content, window.FindControl<ContentControl>("PageContent")!.Content);
                var page = Assert.Single(window.GetVisualDescendants().OfType<Ursa.Fluent2.Demo.Pages.DemoPageView>());
                Assert.Equal($"{item.PageId}Page", page.GetType().Name);
                Assert.Same(model.Content, page.DataContext);
                foreach (var launch in page.GetVisualDescendants().OfType<Button>()
                             .Where(button => button.CommandParameter is string))
                {
                    Assert.NotNull(launch.Command);
                    Assert.Same(model.Content.OpenSampleCommand, launch.Command);
                    Assert.True(launch.Command.CanExecute(launch.CommandParameter));
                }
                var sampleLabels = window.GetVisualDescendants().OfType<TextBlock>()
                    .Where(text => text.Classes.Contains("gallery-label")).ToArray();
                Assert.NotEmpty(sampleLabels);
                Assert.All(sampleLabels, label => Assert.Equal(16, label.FontSize));
                Assert.Equal(ScrollBarVisibility.Auto,
                    Assert.IsType<ScrollViewer>(page.Content).HorizontalScrollBarVisibility);
                examples += sampleLabels.Length;
                if (item.PageId == "ScrollToButton")
                {
                    var scrollButton = window.GetVisualDescendants().OfType<ScrollToButton>().Single();
                    Assert.IsType<ScrollViewer>(scrollButton.Target);
                }
                if (item.PageId == "ClassSelector")
                {
                    var selector = window.GetVisualDescendants().OfType<ClassSelector>().Single();
                    var target = window.GetVisualDescendants().OfType<Button>()
                        .Single(button => button.Name == "ClassSelectorTarget");
                    Assert.Same(target, selector.Target);
                }
            }
            Assert.Equal(115, examples);
            menu.SelectedItem = model.Categories.SelectMany(category => category.Children)
                .Single(item => item.PageId == "UrsaGroupBox");
            window.UpdateLayout();

            var group = window.GetVisualDescendants().OfType<UrsaGroupBox>().First();
            Assert.NotNull(group.Template);
            Assert.IsAssignableFrom<IBrush>(group.BorderBrush);
            Assert.IsAssignableFrom<IBrush>(group.Foreground);
            Assert.Contains(group.GetVisualDescendants(), child => child is GroupBoxBorder);

            menu.SelectedItem = model.Categories.SelectMany(category => category.Children)
                .Single(item => item.PageId == "Divider");
            window.UpdateLayout();
            var dividers = window.GetVisualDescendants().OfType<Divider>().ToArray();
            Assert.NotEmpty(dividers);
            Assert.All(dividers, divider =>
            {
                Assert.NotNull(divider.Template);
                Assert.IsAssignableFrom<IBrush>(divider.BorderBrush);
                Assert.True(divider.Bounds.Width > 0);
                Assert.True(divider.Bounds.Height > 0);
            });
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void Switching_theme_updates_inherited_fluent_brushes()
    {
        var divider = new Divider { Content = "Theme switch" };
        var window = new Window { Content = divider, RequestedThemeVariant = ThemeVariant.Light };
        try
        {
            window.Show();
            var light = Assert.IsAssignableFrom<ISolidColorBrush>(divider.BorderBrush).Color;
            window.RequestedThemeVariant = ThemeVariant.Dark;
            var dark = Assert.IsAssignableFrom<ISolidColorBrush>(divider.BorderBrush).Color;
            Assert.NotEqual(light, dark);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void Icon_button_uses_fluent_button_colors_in_both_theme_variants()
    {
        var native = new Button { Content = "Save" };
        var ursa = new IconButton { Content = "Save" };
        var window = new UrsaWindow { Content = new StackPanel { Children = { native, ursa } }, RequestedThemeVariant = ThemeVariant.Light };
        try
        {
            window.Show();
            window.UpdateLayout();
            var lightWindow = Assert.IsAssignableFrom<ISolidColorBrush>(window.Background).Color;
            var lightButton = Assert.IsAssignableFrom<ISolidColorBrush>(native.Background).Color;
            Assert.Equal(lightButton, Assert.IsAssignableFrom<ISolidColorBrush>(ursa.Background).Color);
            Assert.Equal(Assert.IsAssignableFrom<ISolidColorBrush>(native.Foreground).Color,
                Assert.IsAssignableFrom<ISolidColorBrush>(ursa.Foreground).Color);
            Assert.Equal(native.CornerRadius, ursa.CornerRadius);

            window.RequestedThemeVariant = ThemeVariant.Dark;
            window.UpdateLayout();
            var darkWindow = Assert.IsAssignableFrom<ISolidColorBrush>(window.Background).Color;
            var darkButton = Assert.IsAssignableFrom<ISolidColorBrush>(native.Background).Color;
            Assert.NotEqual(lightWindow, darkWindow);
            Assert.NotEqual(lightButton, darkButton);
            Assert.Equal(darkButton, Assert.IsAssignableFrom<ISolidColorBrush>(ursa.Background).Color);
            Assert.Equal(Assert.IsAssignableFrom<ISolidColorBrush>(native.Foreground).Color,
                Assert.IsAssignableFrom<ISolidColorBrush>(ursa.Foreground).Color);

            native.IsEnabled = false;
            ursa.IsEnabled = false;
            window.UpdateLayout();
            var nativePresenter = native.GetVisualDescendants().OfType<ContentPresenter>()
                .First(element => element.Name == "PART_ContentPresenter");
            var ursaBorder = ursa.GetVisualDescendants().OfType<Border>()
                .First(element => element.Name == "PART_Background");
            Assert.Equal(Assert.IsAssignableFrom<ISolidColorBrush>(nativePresenter.Background).Color,
                Assert.IsAssignableFrom<ISolidColorBrush>(ursaBorder.Background).Color);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void Range_slider_uses_fluent_slider_thumbs()
    {
        var range = new RangeSlider { Width = 280, Minimum = 0, Maximum = 100, LowerValue = 25, UpperValue = 75 };
        var window = new Window { Content = range };
        try
        {
            window.Show();
            window.UpdateLayout();
            var thumbs = range.GetVisualDescendants().OfType<Thumb>().ToArray();
            Assert.Equal(2, thumbs.Length);
            Assert.All(thumbs, thumb =>
            {
                Assert.Equal(22, thumb.Width);
                Assert.Equal(22, thumb.Height);
                Assert.Contains(thumb.GetVisualDescendants().OfType<Ellipse>(), ellipse => ellipse.Name == "SliderInnerThumb");
            });
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void Selection_placeholders_match_fluent_combo_box_contrast()
    {
        var native = new ComboBox { PlaceholderText = "Select" };
        var multi = new MultiComboBox { PlaceholderText = "Select" };
        var tree = new TreeComboBox { PlaceholderText = "Select" };
        var window = new Window { Content = new StackPanel { Children = { native, multi, tree } }, RequestedThemeVariant = ThemeVariant.Light };
        try
        {
            window.Show();
            window.UpdateLayout();
            var expected = Assert.IsAssignableFrom<ISolidColorBrush>(native.PlaceholderForeground).Color;
            foreach (var control in new Control[] { multi, tree })
            {
                var placeholder = control.GetVisualDescendants().OfType<TextBlock>().First(text => text.Name == "PlaceholderTextBlock");
                Assert.Equal(1, placeholder.Opacity);
                Assert.Equal(expected, Assert.IsAssignableFrom<ISolidColorBrush>(placeholder.Foreground).Color);
            }
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void Overlay_and_divider_resources_follow_the_active_fluent_palette(bool dark)
    {
        var variant = dark ? ThemeVariant.Dark : ThemeVariant.Light;
        var app = Application.Current!;
        Assert.True(app.TryGetResource("DialogBackground", variant, out var background));
        Assert.Equal(255, Assert.IsAssignableFrom<ISolidColorBrush>(background).Color.A);
        Assert.True(app.TryGetResource("DividerBorderBrush", variant, out var divider));
        Assert.True(app.TryGetResource("DividerStrokeColorDefault", variant, out var fluentDivider));
        Assert.Equal(fluentDivider, divider);
    }

    [AvaloniaTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void Accent_roles_use_the_same_palette_as_fluent_controls(bool dark)
    {
        var variant = dark ? ThemeVariant.Dark : ThemeVariant.Light;
        var app = Application.Current!;
        Assert.True(app.TryGetResource("AccentFillColorDefaultBrush", variant, out var accent));
        var expected = Assert.IsAssignableFrom<ISolidColorBrush>(accent).Color;
        foreach (var key in new[] { "BadgePrimaryBadgeBackground", "ButtonGroupSolidPrimaryBackground", "CalenderDaySelectedBackground" })
        {
            Assert.True(app.TryGetResource(key, variant, out var actual), key);
            Assert.Equal(expected, Assert.IsAssignableFrom<ISolidColorBrush>(actual).Color);
        }
    }

    [AvaloniaFact]
    public void Overlay_and_tag_close_buttons_render_their_geometry_as_icons()
    {
        Assert.True(Application.Current!.TryGetResource("OverlayCloseButton", ThemeVariant.Light, out var theme));
        var close = new Button { Theme = Assert.IsType<ControlTheme>(theme) };
        var tag = new ClosableTag { Content = "Example" };
        var window = new Window { Content = new StackPanel { Children = { close, tag } }, RequestedThemeVariant = ThemeVariant.Light };
        try
        {
            window.Show();
            window.UpdateLayout();
            Assert.Contains(close.GetVisualDescendants().OfType<PathIcon>(), icon => icon.Data is not null);
            var tagClose = tag.GetVisualDescendants().OfType<Button>().First(button => button.Name == ClosableTag.PART_CloseButton);
            Assert.Contains(tagClose.GetVisualDescendants().OfType<PathIcon>(), icon => icon.Data is not null);
            Assert.Equal(24, close.Bounds.Width);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void Density_and_locale_follow_the_fluent_theme_configuration()
    {
        var app = Application.Current!;
        var fluentTheme = app.Styles.OfType<Avalonia.Themes.Fluent2.Fluent2Theme>().Single();
        var ursaTheme = app.Styles.OfType<UrsaFluent2Theme>().Single();
        var previousFluentDensity = fluentTheme.DensityStyle;
        var previousUrsaDensity = ursaTheme.DensityStyle;
        var previousLocale = ursaTheme.Locale;
        var window = new MainWindow();
        try
        {
            window.Show();
            var model = window.ViewModel;
            model.SelectedItem = model.Categories.SelectMany(category => category.Children)
                .Single(item => item.PageId == "NumericUpDown");
            window.UpdateLayout();
            var numberInput = window.GetVisualDescendants().OfType<NumericIntUpDown>().First();
            fluentTheme.DensityStyle = Avalonia.Themes.Fluent2.DensityStyle.Normal;
            ursaTheme.DensityStyle = Avalonia.Themes.Fluent2.DensityStyle.Normal;
            Assert.Equal(32, numberInput.MinHeight);

            fluentTheme.DensityStyle = Avalonia.Themes.Fluent2.DensityStyle.Compact;
            ursaTheme.DensityStyle = Avalonia.Themes.Fluent2.DensityStyle.Compact;
            Assert.Equal(24, numberInput.MinHeight);

            ursaTheme.Locale = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            Assert.True(ursaTheme.Resources.TryGetResource("STRING_MENU_DIALOG_OK", ThemeVariant.Default, out var english));
            Assert.Equal("OK", english);

            ursaTheme.Locale = System.Globalization.CultureInfo.GetCultureInfo("zh-CN");
            Assert.True(ursaTheme.Resources.TryGetResource("STRING_MENU_DIALOG_OK", ThemeVariant.Default, out var chinese));
            Assert.Equal("确定", chinese);

            foreach (var name in new[] { "cs-CZ", "de-DE", "en-US", "fr-FR", "pl-PL", "ru-RU", "zh-CN", "ja-JP", "ko-KR" })
            {
                ursaTheme.Locale = System.Globalization.CultureInfo.GetCultureInfo(name);
                Assert.True(ursaTheme.Resources.TryGetResource("STRING_MENU_DIALOG_OK", ThemeVariant.Default, out var localized));
                Assert.IsType<string>(localized);
                Assert.NotEmpty((string)localized!);
            }

            ursaTheme.Locale = System.Globalization.CultureInfo.GetCultureInfo("es-ES");
            Assert.Equal("zh-CN", ursaTheme.Locale!.Name);
            Assert.True(ursaTheme.Resources.TryGetResource("STRING_MENU_DIALOG_OK", ThemeVariant.Default, out var fallback));
            Assert.Equal("确定", fallback);
        }
        finally
        {
            window.Close();
            fluentTheme.DensityStyle = previousFluentDensity;
            ursaTheme.DensityStyle = previousUrsaDensity;
            ursaTheme.Locale = previousLocale;
        }
    }
}
