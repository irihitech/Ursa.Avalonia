using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Xunit;

namespace Ursa.Themes.Fluent2.Tests;

public class ButtonFlyoutDemoTests
{
    [AvaloniaTheory]
    [InlineData("IconDropDownButton", "NativeDropDownButton")]
    [InlineData("IconDropDownButton", "IconDropDownButton")]
    [InlineData("IconSplitButton", "NativeSplitButton")]
    [InlineData("IconSplitButton", "IconSplitButton")]
    [InlineData("IconToggleSplitButton", "IconToggleSplitButton")]
    public void Demo_button_opens_a_flyout_with_selectable_options(string pageId, string buttonName)
    {
        var page = Ursa.Fluent2.Demo.DemoCatalog.GetPage(pageId).CreateView();
        var window = new Window { Content = page };
        try
        {
            window.Show();
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            var button = page.FindControl<Control>(buttonName)!;
            var split = button as SplitButton;
            var flyout = Assert.IsType<Flyout>(split?.Flyout ?? Assert.IsAssignableFrom<Button>(button).Flyout);
            var target = split is null ? button : button.GetVisualDescendants().OfType<Button>()
                .Single(element => element.Name == "PART_SecondaryButton");
            Click(target);
            Assert.True(flyout.IsOpen);
            var options = Assert.IsType<StackPanel>(flyout.Content).GetLogicalChildren().OfType<RadioButton>().ToArray();
            Assert.Equal(3, options.Length);
            Assert.True(options[0].IsChecked);
            Click(options[1]);
            Assert.True(options[1].IsChecked);
            Assert.False(options[0].IsChecked);
            flyout.Hide();
            Assert.False(flyout.IsOpen);
        }
        finally
        {
            window.Close();
        }
    }

    private static void Click(Control control)
    {
        var root = TopLevel.GetTopLevel(control)!;
        var point = control.TranslatePoint(new Point(control.Bounds.Width / 2, control.Bounds.Height / 2), root)!.Value;
        root.MouseDown(point, MouseButton.Left);
        root.MouseUp(point, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
    }
}
