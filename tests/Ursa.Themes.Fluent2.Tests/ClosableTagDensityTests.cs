using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;
using Ursa.Themes.Fluent2;
using Xunit;
using FluentDensity = Avalonia.Themes.Fluent2.DensityStyle;

namespace Ursa.Themes.Fluent2.Tests;

public class ClosableTagDensityTests
{
    [AvaloniaFact]
    public void Closable_tags_and_hosts_follow_density()
    {
        var app = Application.Current!;
        var fluentTheme = app.Styles.OfType<Avalonia.Themes.Fluent2.Fluent2Theme>().Single();
        var ursaTheme = app.Styles.OfType<UrsaFluent2Theme>().Single();
        var items = new[] { "Avalonia", "Ursa" };
        var tagInput = new TagInput { Width = 360, Tags = new List<string> { "Avalonia", "Ursa" } };
        var combo = new MultiComboBox { Width = 360, ItemsSource = items };
        var auto = new MultiAutoCompleteBox { Width = 360, ItemsSource = items, SelectedItems = new System.Collections.ObjectModel.ObservableCollection<object>() };
        var window = new Window { Content = new StackPanel { Spacing = 8, Children = { tagInput, combo, auto } } };
        try
        {
            window.Show();
            combo.SelectedItems!.Add("Avalonia");
            auto.SelectedItems!.Add("Avalonia");
            foreach (var (density, hostHeight, tagHeight) in new[] { (FluentDensity.Normal, 32d, 26d), (FluentDensity.Compact, 24d, 20d) })
            {
                fluentTheme.DensityStyle = density;
                ursaTheme.DensityStyle = density;
                Dispatcher.UIThread.RunJobs();
                window.UpdateLayout();
                foreach (Control host in new Control[] { tagInput, combo, auto })
                {
                    var tag = host.GetVisualDescendants().OfType<ClosableTag>().First();
                    Assert.Equal(hostHeight, host.Bounds.Height, 0.5);
                    Assert.Equal(tagHeight, tag.Bounds.Height, 0.5);
                }
            }
        }
        finally
        {
            window.Close();
            fluentTheme.DensityStyle = FluentDensity.Normal;
            ursaTheme.DensityStyle = FluentDensity.Normal;
        }

    }
}
