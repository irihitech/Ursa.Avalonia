using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Headless.XUnit;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.SettingsCardTests;

public class SettingsCardGroupTest
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
}
