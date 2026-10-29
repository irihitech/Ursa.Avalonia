using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Presenters;
using Avalonia.Media;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using Ursa.Controls;
using Ursa.Fluent2.Demo;
using Xunit;

namespace Ursa.Themes.Fluent2.Tests;

public class WindowTitleBarTests
{
    [AvaloniaTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void Ursa_title_bar_has_one_title_and_honors_content_and_layout_properties(bool custom)
    {
        var title = "Ursa title";
        var window = new UrsaWindow { Title = title, Width = 600, Height = 300 };
        var customTitle = new TextBlock { Text = "Custom title" };
        if (custom) window.TitleBarContent = customTitle;
        try
        {
            window.Show();
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            var titleBar = window.GetVisualDescendants().OfType<TitleBar>().Single();
            var presenter = titleBar.GetVisualDescendants().OfType<ContentPresenter>()
                .Single(control => control.Name == "PART_ContentPresenter");
            Assert.True(titleBar.Bounds.Height >= 30);
            Assert.Equal(new Thickness(12, 0), titleBar.Padding);
            Assert.Equal(Avalonia.Layout.VerticalAlignment.Center, presenter.VerticalContentAlignment);
            if (custom)
                Assert.Same(customTitle, presenter.Content);
            else
            {
                Assert.Equal(title, presenter.Content);
                window.Title = "Updated title";
                Assert.Equal("Updated title", presenter.Content);
            }

            window.TitleBarPadding = new Thickness(20, 2);
            window.TitleBarBackground = Brushes.Red;
            window.TitleBarMargin = new Thickness(4);
            Assert.Equal(window.TitleBarPadding, titleBar.Padding);
            Assert.Equal(window.TitleBarBackground, titleBar.Background);
            Assert.Equal(window.TitleBarMargin, titleBar.Margin);
            window.IsTitleBarVisible = false;
            Assert.False(titleBar.IsVisible);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void Drawn_decorations_hide_only_the_duplicate_title_not_caption_buttons()
    {
        var decorations = new WindowDrawnDecorations
        {
            Theme = (Avalonia.Styling.ControlTheme)Application.Current!.FindResource(UrsaWindow.KEY_URSAWINDOW_DRAWN_DECORATIONS)!,
            Title = "Window title"
        };
        var window = new DecorationsTestWindow(decorations) { Width = 500, Height = 200 };
        try
        {
            window.Show();
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            // The headless platform does not create drawn decorations.
            typeof(WindowDrawnDecorations).GetMethod("ApplyTemplate",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.Invoke(decorations, null);
            var titles = decorations.GetLogicalDescendants().OfType<Panel>()
                .Where(panel => panel.Name == "PART_TitleTextPanel").ToArray();
            Assert.NotEmpty(titles);
            Assert.All(titles, panel => Assert.False(panel.IsVisible));
            Assert.Contains(decorations.GetLogicalDescendants().OfType<Button>(), button => button.Name == "PART_CloseButton");
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void Demo_custom_title_is_padded_and_vertically_centered()
    {
        var window = new MainWindow();
        try
        {
            window.Show();
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            var titleBar = window.GetVisualDescendants().OfType<TitleBar>().Single();
            var title = Assert.IsType<TextBlock>(window.TitleBarContent);
            var position = title.TranslatePoint(default, titleBar)!.Value;
            Assert.Equal(12, position.X, 2);
            Assert.InRange(Math.Abs((titleBar.Bounds.Height - title.Bounds.Height) / 2 - position.Y), 0, 0.5);
        }
        finally
        {
            window.Close();
        }

    }

    private sealed class DecorationsTestWindow : Window
    {
        public DecorationsTestWindow(WindowDrawnDecorations decorations) => LogicalChildren.Add(decorations);
    }
}
