using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;
using Ursa.Fluent2.Demo;
using Ursa.Fluent2.Demo.Pages;
using Ursa.Fluent2.Demo.ViewModels;
using Xunit;

namespace Ursa.Themes.Fluent2.Tests;

public class OverviewDemoTests
{
    [AvaloniaTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void Overview_is_the_landing_page_with_responsive_live_controls(bool dark)
    {
        var window = new MainWindow
        {
            RequestedThemeVariant = dark ? Avalonia.Styling.ThemeVariant.Dark : Avalonia.Styling.ThemeVariant.Light
        };
        var language = window.ViewModel.LanguageIndex;
        try
        {
            window.Show();
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            var model = Assert.IsType<OverviewPageViewModel>(window.ViewModel.Content);
            Assert.Equal("Overview", window.ViewModel.SelectedItem!.PageId);
            Assert.Equal("Overview", window.ViewModel.Categories[0].Children[0].PageId);
            var page = Assert.Single(window.GetVisualDescendants().OfType<OverviewPage>());
            var cards = page.FindControl<WrapPanel>("OverviewCards")!;
            Assert.Equal(6, cards.Children.Count);
            Assert.True(cards.Children[1].Bounds.X > cards.Children[0].Bounds.X);
            var input = page.GetVisualDescendants().OfType<MultiAutoCompleteBox>().Single();
            Assert.Same(model.Technologies, input.ItemsSource);
            Assert.Same(model.SelectedTechnologies, input.SelectedItems);
            Assert.Same(model.Tags, page.GetVisualDescendants().OfType<TagInput>().Single().Tags);
            model.SelectedTechnologies.Add("Ursa");
            window.ViewModel.LanguageIndex = 1 - language;
            Dispatcher.UIThread.RunJobs();
            Assert.Same(model, window.ViewModel.Content);
            Assert.Same(page, window.GetVisualDescendants().OfType<OverviewPage>().Single());
            Assert.Equal(window.ViewModel.Text.PageTitle("Overview"), window.FindControl<TextBlock>("PageTitle")!.Text);

            window.Width = 760;
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            Assert.Equal(cards.Children[0].Bounds.X, cards.Children[1].Bounds.X);
            Assert.True(cards.Children[1].Bounds.Y > cards.Children[0].Bounds.Y);
            Assert.All(cards.Children, card => Assert.True(card.Bounds.Right <= cards.Bounds.Width));

            window.ViewModel.SearchText = "MultiAutoCompleteBox";
            Assert.Equal("MultiAutoCompleteBox", window.ViewModel.CurrentPage!.Id);
            window.ViewModel.SearchText = "Overview";
            Assert.IsType<OverviewPageViewModel>(window.ViewModel.Content);
        }
        finally
        {
            window.ViewModel.LanguageIndex = language;
            window.Close();
        }
    }
}
