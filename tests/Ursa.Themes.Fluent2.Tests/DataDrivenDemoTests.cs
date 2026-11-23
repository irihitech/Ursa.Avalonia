using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;
using Ursa.Fluent2.Demo;
using Ursa.Fluent2.Demo.Pages;
using Ursa.Fluent2.Demo.ViewModels;
using Xunit;
using AutoCompleteBox = Ursa.Controls.AutoCompleteBox;

namespace Ursa.Themes.Fluent2.Tests;

public class DataDrivenDemoTests
{
    [AvaloniaFact]
    public void Multi_auto_complete_filters_suggestions_and_updates_the_bound_selection()
    {
        WithPage<MultiAutoCompleteBoxPageViewModel>("MultiAutoCompleteBox", (window, page, model) =>
        {
            var input = page.FindControl<MultiAutoCompleteBox>("MultiAutoCompleteInput")!;
            Assert.Same(model.Items, input.ItemsSource);
            Assert.Same(model.SelectedItems, input.SelectedItems);
            Assert.Equal(["Avalonia"], model.SelectedItems);

            var tags = input.GetVisualDescendants().OfType<MultiComboBoxSelectedItemList>().Single();
            var panel = Assert.IsType<WrapPanelWithTrailingItem>(tags.ItemsPanelRoot);
            var textBox = Assert.IsType<TextBox>(panel.TrailingItem);
            textBox.Focus();
            textBox.Text = "Fl";
            Dispatcher.UIThread.RunJobs();
            Assert.Equal("Fl", model.SearchText);
            Assert.True(input.IsDropDownOpen);

            var popup = input.GetVisualDescendants().OfType<Popup>().Single();
            var suggestions = popup.Child!.GetVisualDescendants().OfType<ListBox>().Single();
            Assert.Equal("Fluent 2", Assert.Single(suggestions.Items));
            suggestions.SelectedItem = model.Items[2];
            textBox.RaiseEvent(new KeyEventArgs { RoutedEvent = InputElement.KeyDownEvent, Key = Key.Enter });
            Dispatcher.UIThread.RunJobs();
            Assert.Equal(["Avalonia", "Fluent 2"], model.SelectedItems);
            Assert.Equal("Selected items: 2", page.FindControl<TextBlock>("SelectionCount")!.Text);
            Assert.Equal(2, tags.ItemCount);

            var tag = Assert.IsType<ClosableTag>(tags.ContainerFromItem("Fluent 2"));
            tag.Command!.Execute(tag);
            Assert.Equal(["Avalonia"], model.SelectedItems);
            model.SelectedItems.Add("Ursa");
            Dispatcher.UIThread.RunJobs();
            Assert.Equal(2, tags.ItemCount);

            var language = window.ViewModel.LanguageIndex;
            try
            {
                window.ViewModel.LanguageIndex = 1 - language;
                Dispatcher.UIThread.RunJobs();
                Assert.Same(model, window.ViewModel.Content);
                Assert.Same(page, window.GetVisualDescendants().OfType<DemoPageView>().Single());
                Assert.Equal(["Avalonia", "Ursa"], model.SelectedItems);
            }
            finally
            {
                window.ViewModel.LanguageIndex = language;
            }

            model.ClearSelectionCommand.Execute(null);
            Dispatcher.UIThread.RunJobs();
            Assert.Empty(input.SelectedItems!);
            Assert.Equal("Selected items: 0", page.FindControl<TextBlock>("SelectionCount")!.Text);
            Assert.Equal(string.Empty, textBox.Text);
        });
    }

    [AvaloniaFact]
    public void Auto_complete_binds_suggestions_search_and_single_selection()
    {
        WithPage<AutoCompleteBoxPageViewModel>("AutoCompleteBox", (_, page, model) =>
        {
            var input = page.FindControl<AutoCompleteBox>("AutoCompleteInput")!;
            Assert.Same(model.Items, input.ItemsSource);
            model.Items.Add("Oslo");
            Assert.Contains("Oslo", input.ItemsSource!.Cast<string>());
            model.SelectedItem = "Tokyo";
            Assert.Equal("Tokyo", input.SelectedItem);
            input.SetCurrentValue(AutoCompleteBox.SelectedItemProperty, "Berlin");
            Assert.Equal("Berlin", model.SelectedItem);
            Assert.Equal("Selected: Berlin", page.FindControl<TextBlock>("SelectionValue")!.Text);
            input.SetCurrentValue(AutoCompleteBox.TextProperty, "Par");
            Assert.Equal("Par", model.SearchText);
            model.ClearSelectionCommand.Execute(null);
            Assert.Null(input.SelectedItem);
            Assert.Equal(string.Empty, input.Text);
        });
    }

    [AvaloniaFact]
    public void Multi_combo_box_shares_observable_selection_with_its_tag_list()
    {
        WithPage<MultiComboBoxPageViewModel>("MultiComboBox", (_, page, model) =>
        {
            var input = page.FindControl<MultiComboBox>("MultiComboBoxInput")!;
            var tags = page.FindControl<MultiComboBoxSelectedItemList>("SelectedRoles")!;
            Assert.Same(model.Items, input.ItemsSource);
            Assert.Same(model.SelectedItems, input.SelectedItems);
            Assert.Same(model.SelectedItems, tags.ItemsSource);
            input.IsDropDownOpen = true;
            Dispatcher.UIThread.RunJobs();
            var item = Assert.IsType<MultiComboBoxItem>(input.ContainerFromItem("Design"));
            item.IsSelected = true;
            Assert.Contains("Design", model.SelectedItems);
            var tag = Assert.IsType<ClosableTag>(tags.ContainerFromItem("Development"));
            tag.Command!.Execute(tag);
            Assert.DoesNotContain("Development", model.SelectedItems);
            Assert.False(Assert.IsType<MultiComboBoxItem>(input.ContainerFromItem("Development")).IsSelected);
            model.SelectedItems.Add("Support");
            Dispatcher.UIThread.RunJobs();
            Assert.True(Assert.IsType<MultiComboBoxItem>(input.ContainerFromItem("Support")).IsSelected);
            Assert.Equal("Selected items: 3", page.FindControl<TextBlock>("SelectionCount")!.Text);
            model.ClearSelectionCommand.Execute(null);
            Dispatcher.UIThread.RunJobs();
            Assert.Empty(input.SelectedItems!);
            Assert.Empty(tags.Items);
        });
    }

    [AvaloniaFact]
    public void Tree_combo_box_renders_hierarchical_data_and_binds_the_selected_node()
    {
        WithPage<TreeComboBoxPageViewModel>("TreeComboBox", (_, page, model) =>
        {
            var input = page.FindControl<TreeComboBox>("TreeComboBoxInput")!;
            Assert.Same(model.Items, input.ItemsSource);
            input.IsDropDownOpen = true;
            Dispatcher.UIThread.RunJobs();
            var parent = Assert.IsType<TreeComboBoxItem>(input.ContainerFromItem(model.Items[0]));
            parent.IsExpanded = true;
            Dispatcher.UIThread.RunJobs();
            var node = model.Items[0].Children[1];
            var child = Assert.IsType<TreeComboBoxItem>(parent.ContainerFromItem(node));
            var header = child.GetVisualDescendants().OfType<TextBlock>().Single(text => text.Text == node.Name);
            var root = TopLevel.GetTopLevel(header)!;
            var position = header.TranslatePoint(new Point(header.Bounds.Width / 2, header.Bounds.Height / 2), root)!.Value;
            root.MouseDown(position, MouseButton.Left);
            root.MouseUp(position, MouseButton.Left);
            Dispatcher.UIThread.RunJobs();
            Assert.Same(node, model.SelectedItem);
            Assert.Equal("Selected: Web", page.FindControl<TextBlock>("SelectionValue")!.Text);
            model.SelectedItem = model.Items[1];
            Assert.Same(model.Items[1], input.SelectedItem);
            model.ClearSelectionCommand.Execute(null);
            Assert.Null(input.SelectedItem);
        });
    }

    [AvaloniaFact]
    public void Selection_list_updates_selection_in_both_directions()
    {
        WithPage<SelectionListPageViewModel>("SelectionList", (_, page, model) =>
        {
            var input = page.FindControl<SelectionList>("SelectionListInput")!;
            Assert.Same(model.Items, input.ItemsSource);
            Assert.Equal("Overview", input.SelectedItem);
            model.SelectedItem = "Members";
            Assert.Equal("Members", input.SelectedItem);
            input.SetCurrentValue(SelectionList.SelectedItemProperty, "Activity");
            Assert.Equal("Activity", model.SelectedItem);
            Assert.Equal("Selected: Activity", page.FindControl<TextBlock>("SelectionValue")!.Text);
            model.ClearSelectionCommand.Execute(null);
            Assert.Null(input.SelectedItem);
        });
    }

    [AvaloniaFact]
    public void Tag_input_edits_the_view_model_collection_and_observes_collection_changes()
    {
        WithPage<TagInputPageViewModel>("TagInput", (_, page, model) =>
        {
            var input = page.FindControl<TagInput>("TagInput")!;
            Assert.Same(model.Tags, input.Tags);
            var tags = input.GetVisualDescendants().OfType<MultiComboBoxSelectedItemList>().Single();
            var panel = Assert.IsType<WrapPanelWithTrailingItem>(tags.ItemsPanelRoot);
            var textBox = Assert.IsType<TextBox>(panel.TrailingItem);
            textBox.Focus();
            textBox.Text = "MVVM";
            textBox.RaiseEvent(new KeyEventArgs { RoutedEvent = InputElement.KeyDownEvent, Key = Key.Enter });
            Dispatcher.UIThread.RunJobs();
            Assert.Equal(["Avalonia", "Fluent 2", "MVVM"], model.Tags);
            var tag = Assert.IsType<ClosableTag>(tags.ContainerFromItem("MVVM"));
            tag.Command!.Execute(tag);
            Assert.Equal(["Avalonia", "Fluent 2"], model.Tags);
            model.Tags.Add("Ursa");
            Dispatcher.UIThread.RunJobs();
            Assert.Equal(3, tags.ItemCount);
            Assert.Equal("Tags: 3", page.FindControl<TextBlock>("TagCount")!.Text);
            model.ClearTagsCommand.Execute(null);
            Dispatcher.UIThread.RunJobs();
            Assert.Empty(input.Tags!);
            Assert.Empty(tags.Items);
        });
    }

    private static void WithPage<T>(string pageId, Action<MainWindow, DemoPageView, T> action)
        where T : DemoPageViewModel
    {
        var window = new MainWindow();
        try
        {
            window.Show();
            window.ViewModel.SelectedItem = window.ViewModel.Categories.SelectMany(category => category.Children)
                .Single(item => item.PageId == pageId);
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            var model = Assert.IsType<T>(window.ViewModel.Content);
            var page = Assert.Single(window.GetVisualDescendants().OfType<DemoPageView>());
            Assert.Same(model, page.DataContext);
            action(window, page, model);
        }
        finally
        {
            window.Close();
        }
    }
}
