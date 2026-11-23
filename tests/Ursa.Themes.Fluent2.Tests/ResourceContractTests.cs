using System.Text.RegularExpressions;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using Ursa.Controls;
using Xunit;

namespace Ursa.Themes.Fluent2.Tests;

public class ResourceContractTests
{
    private static readonly XNamespace Xaml = "http://schemas.microsoft.com/winfx/2006/xaml";
    private static readonly Regex DynamicResource = new(@"\{DynamicResource\s+([^{}]+)\}");
    private static readonly (string Path, XDocument Document)[] Sources =
        typeof(ResourceContractTests).Assembly.GetManifestResourceNames()
            .Where(name => name.StartsWith("Fluent2/", StringComparison.Ordinal) ||
                           name.StartsWith("Semi/", StringComparison.Ordinal))
            .Select(name =>
            {
                using var stream = typeof(ResourceContractTests).Assembly.GetManifestResourceStream(name)!;
                return (name.Replace('\\', '/'), XDocument.Load(stream));
            }).ToArray();

    [Fact]
    public void Only_the_theme_entry_point_is_public_axaml()
    {
        foreach (var (path, document) in Sources.Where(source =>
                     source.Path.StartsWith("Fluent2/", StringComparison.Ordinal) &&
                     source.Path != "Fluent2/UrsaFluent2Theme.axaml"))
        {
            Assert.True((string?)document.Root!.Attribute(Xaml + "ClassModifier") == "internal", path);
        }

        Assert.DoesNotContain(typeof(UrsaFluent2Theme).Assembly.GetExportedTypes(),
            type => type.Namespace == "Ursa.Themes.Fluent2.Locale");
    }

    [Fact]
    public void Fluent2_covers_the_current_Semi_control_theme_types()
    {
        static string[] ThemeKeys(string prefix) => Sources
            .Where(source => source.Path.StartsWith(prefix, StringComparison.Ordinal))
            .SelectMany(source => source.Document.Descendants())
            .Where(element => element.Name.LocalName == "ControlTheme")
            .Select(element => (string?)element.Attribute(Xaml + "Key"))
            .OfType<string>()
            .Where(key => key.StartsWith("{x:Type ", StringComparison.Ordinal))
            .Distinct()
            .Order(StringComparer.Ordinal)
            .ToArray();

        var expected = ThemeKeys("Semi/");
        Assert.NotEmpty(expected);
        Assert.Equal(expected, ThemeKeys("Fluent2/"));
    }

    [Fact]
    public void Light_and_dark_define_the_same_resource_keys()
    {
        static string[] Keys(string directory) => Sources
            .Where(source => source.Path.StartsWith($"Fluent2/Themes/{directory}/", StringComparison.Ordinal))
            .SelectMany(source => source.Document.Descendants())
            .Select(element => (string?)element.Attribute(Xaml + "Key"))
            .OfType<string>()
            .Distinct()
            .Order(StringComparer.Ordinal)
            .ToArray();

        var light = Keys("Light");
        Assert.NotEmpty(light);
        Assert.Equal(light, Keys("Dark"));
    }

    [AvaloniaTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void Dynamic_resources_resolve_without_the_Semi_theme(bool dark)
    {
        var sources = Sources.Where(source => source.Path.StartsWith("Fluent2/", StringComparison.Ordinal)).ToArray();
        var scopedKeys = sources.SelectMany(source => source.Document.Descendants())
            .Where(element => element.Ancestors().Any(parent => parent.Name.LocalName == "ControlTheme"))
            .Select(element => (string?)element.Attribute(Xaml + "Key"))
            .OfType<string>()
            .ToHashSet(StringComparer.Ordinal);
        var variant = dark ? ThemeVariant.Dark : ThemeVariant.Light;
        var missing = new HashSet<string>(StringComparer.Ordinal);
        foreach (var (path, document) in sources)
        {
            foreach (var attribute in document.Descendants().Attributes())
            {
                foreach (Match match in DynamicResource.Matches(attribute.Value))
                {
                    var key = match.Groups[1].Value.Trim();
                    if (!scopedKeys.Contains(key) &&
                        !Application.Current!.TryGetResource(key, variant, out _))
                    {
                        missing.Add($"{path}: {key}");
                    }
                }
            }
        }

        Assert.True(missing.Count == 0, string.Join(Environment.NewLine, missing.Order(StringComparer.Ordinal)));
        Assert.DoesNotContain(Application.Current!.Styles,
            style => style.GetType().Assembly.GetName().Name == "Ursa.Themes.Semi");
    }

    [AvaloniaFact]
    public void Theme_entry_point_can_be_loaded_by_avares_uri()
    {
        var theme = AvaloniaXamlLoader.Load(new Uri(
            "avares://Ursa.Themes.Fluent2/UrsaFluent2Theme.axaml"));
        Assert.IsType<UrsaFluent2Theme>(theme);
    }

    [AvaloniaTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void Component_overrides_are_dynamic_scoped_and_convert_colors_to_brushes(bool dark)
    {
        var button = new IconButton { Content = "Scoped override" };
        var window = new Window
        {
            RequestedThemeVariant = dark ? ThemeVariant.Dark : ThemeVariant.Light,
            Content = button,
            Resources = { ["ButtonDefaultBackground"] = Colors.Red }
        };
        try
        {
            window.Show();
            Assert.Equal(Colors.Red, Assert.IsAssignableFrom<ISolidColorBrush>(button.Background).Color);

            window.Resources["ButtonDefaultBackground"] = Colors.Blue;
            Assert.Equal(Colors.Blue, Assert.IsAssignableFrom<ISolidColorBrush>(button.Background).Color);

            button.Resources["ButtonDefaultBackground"] = Brushes.Green;
            Assert.Same(Brushes.Green, button.Background);
            window.RequestedThemeVariant = dark ? ThemeVariant.Light : ThemeVariant.Dark;
            Assert.Same(Brushes.Green, button.Background);

            button.Resources.Remove("ButtonDefaultBackground");
            Assert.Equal(Colors.Blue, Assert.IsAssignableFrom<ISolidColorBrush>(button.Background).Color);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void Newly_added_controls_use_fluent_resources(bool dark)
    {
        var selector = new ClassSelector
        {
            Items =
            {
                new ClassSelectorGroup
                {
                    Header = "Appearance",
                    Items = { new ClassSelectorItem { Content = "Accent", ClassName = "accent" } }
                }
            }
        };
        var shimmer = new ShimmerText { Text = "Loading" };
        var selectable = new ShimmerSelectableText { Text = "Selectable loading" };
        var window = new Window
        {
            RequestedThemeVariant = dark ? ThemeVariant.Dark : ThemeVariant.Light,
            Content = new StackPanel { Children = { selector, shimmer, selectable } }
        };
        try
        {
            window.Show();
            window.UpdateLayout();
            Assert.NotNull(selector.Template);
            Assert.True(Application.Current!.TryGetResource("TextFillColorSecondary", window.ActualThemeVariant, out var color));
            Assert.Equal(color, shimmer.BaseColor);
            Assert.Equal(color, selectable.BaseColor);
            Assert.NotNull(selectable.SelectionBrush);
        }
        finally
        {
            window.Close();
        }
    }
}
