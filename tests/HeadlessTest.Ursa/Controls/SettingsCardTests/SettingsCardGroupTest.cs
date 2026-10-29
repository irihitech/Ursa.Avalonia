using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Headless.XUnit;
using HeadlessTest.Ursa.TestHelpers;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.SettingsCardTests;

public partial class SettingsCardGroupTest
{
    [AvaloniaFact]
    public void SettingsCardGroup_ModelItems_AutoWrappedInCards()
    {
        var group = new SettingsCardGroup
        {
            ItemsSource = new ObservableCollection<string> { "First", "Second", "Third" },
        };
        var window = new Window { Content = group };
        window.Show();

        Assert.Equal(3, group.ItemCount);
        var first = group.ContainerFromIndex(0);
        var card = Assert.IsType<SettingsCard>(first);
        Assert.Equal("First", card.Content);
    }

    [AvaloniaFact]
    public void SettingsCardGroup_WithItemTemplate_UsesPresenterContainer()
    {
        var template = new FuncDataTemplate<object>((_, _) => new TextBlock());
        var group = new SettingsCardGroup
        {
            ItemTemplate = template,
            ItemsSource = new ObservableCollection<string> { "A" },
        };
        var window = new Window { Content = group };
        window.Show();

        var container = group.ContainerFromIndex(0);
        var presenter = Assert.IsType<ContentPresenter>(container);
        Assert.Equal("A", presenter.Content);
        Assert.Equal(template, presenter.ContentTemplate);
    }

    [AvaloniaFact]
    public void SettingsCardGroup_ControlItems_UsedDirectly()
    {
        var card = new SettingsCard { Content = "Direct" };
        var group = new SettingsCardGroup();
        var window = new Window { Content = group };
        window.Show();

        group.Items.Add(card);

        Assert.Equal(1, group.ItemCount);
        Assert.Same(card, group.ContainerFromIndex(0));
    }

    [AvaloniaFact]
    public void SettingsCardGroup_HeaderAndDescription_PropertiesWork()
    {
        var group = new SettingsCardGroup
        {
            Header = "Group header",
            Description = "Group description",
        };
        Assert.Equal("Group header", group.Header);
        Assert.Equal("Group description", group.Description);
    }

    [AvaloniaFact]
    public void SettingsCardGroup_ItemTemplate_ExpandableRows_BindToModel()
    {
        var model = new RowModel("Row");
        var template = new FuncDataTemplate<RowModel>((m, _) =>
        {
            var card = new SettingsCard { Content = m?.Name, Detail = new StackPanel() };
            card.Bind(SettingsCard.IsExpandedProperty, new Avalonia.Data.Binding(nameof(RowModel.IsExpanded)));
            return card;
        });
        var group = new SettingsCardGroup
        {
            ItemTemplate = template,
            ItemsSource = new System.Collections.ObjectModel.ObservableCollection<RowModel> { model },
        };
        var window = new Window { Content = group };
        window.Show();
        window.UpdateLayout();

        var presenter = Assert.IsType<ContentPresenter>(group.ContainerFromIndex(0));
        var card = Assert.IsType<SettingsCard>(presenter.Child);

        // Model drives the card, and clicking the card drives the model back —
        // the expanded state lives in the model and survives container regeneration.
        model.IsExpanded = true;
        Assert.True(card.IsExpanded);
        card.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Button.ClickEvent));
        Assert.False(model.IsExpanded);
    }

    [AvaloniaFact]
    public void SettingsCardGroup_ItemsSource_SeparatorRules()
    {
        var template = new FuncDataTemplate<string>((s, _) => new SettingsCard { Content = s });
        var group = new SettingsCardGroup
        {
            ItemTemplate = template,
            ItemsSource = new System.Collections.ObjectModel.ObservableCollection<string> { "A", "B" },
        };
        var window = new Window { Content = group };
        window.Show();
        window.UpdateLayout();

        var firstCard = Assert.IsType<SettingsCard>(Assert.IsType<ContentPresenter>(group.ContainerFromIndex(0)).Child);
        var lastCard = Assert.IsType<SettingsCard>(Assert.IsType<ContentPresenter>(group.ContainerFromIndex(1)).Child);

        // Separator between generated rows, none after the last one (the group card border
        // closes the card — a visible separator here would draw a double line at the bottom).
        Assert.True(firstCard.GetTemplateChildOfType<Border>("PART_Separator")!.IsVisible);
        Assert.False(lastCard.GetTemplateChildOfType<Border>("PART_Separator")!.IsVisible);

        // Expanding the last generated row: separators between nested detail rows stay intact
        // (the presenter rules must not reach cards nested inside a row's Detail).
        var nestedFirst = new SettingsCard { Content = "NF" };
        var nestedLast = new SettingsCard { Content = "NL" };
        lastCard.Detail = new StackPanel();
        ((StackPanel)lastCard.Detail).Children.Add(nestedFirst);
        ((StackPanel)lastCard.Detail).Children.Add(nestedLast);
        lastCard.IsExpanded = true;
        window.UpdateLayout();

        Assert.True(nestedFirst.GetTemplateChildOfType<Border>("PART_Separator")!.IsVisible);
        Assert.False(nestedLast.GetTemplateChildOfType<Border>("PART_Separator")!.IsVisible);
    }

    private partial class RowModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        public RowModel(string name) => Name = name;

        public string Name { get; }

        [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
        public partial bool IsExpanded { get; set; }
    }
}
