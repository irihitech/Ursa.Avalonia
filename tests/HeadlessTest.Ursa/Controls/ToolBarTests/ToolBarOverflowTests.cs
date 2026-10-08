using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.ToolBarTests;

/// <summary>
/// Regression tests for the toolbar overflow: items that do not fit must keep their content,
/// bindings and order while the overflow boundary moves, and only the open popup may host them.
/// </summary>
public class ToolBarOverflowTests
{
    private static (Window Window, ToolBar ToolBar, List<Button> Buttons, List<TextBlock> Labels) CreateWindow(
        double width,
        bool bindLabels)
    {
        var window = new Window { Width = width, Height = 140 };
        string[] model = ["A0", "A1", "A2", "A3", "A4", "A5"];
        window.DataContext = model;
        var toolbar = new ToolBar { HorizontalAlignment = HorizontalAlignment.Stretch };
        var buttons = new List<Button>();
        var labels = new List<TextBlock>();
        for (var i = 0; i < model.Length; i++)
        {
            var label = new TextBlock { Text = $"Item{i}" };
            if (bindLabels)
            {
                label.Bind(TextBlock.TextProperty, new Binding($"[{i}]"));
            }

            var button = new Button
            {
                Width = 70,
                Content = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Children = { new TextBlock { Text = "icon" }, label },
                },
            };
            ToolBar.SetOverflowMode(button, OverflowMode.AsNeeded);
            toolbar.Items.Add(button);
            buttons.Add(button);
            labels.Add(label);
        }

        window.Content = toolbar;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        return (window, toolbar, buttons, labels);
    }

    private static Panel GetMainPanel(ToolBar toolbar)
    {
        var presenter = toolbar.GetVisualDescendants().OfType<ItemsPresenter>().First();
        return (Panel)presenter.Panel!;
    }

    private static Panel? GetOverflowPanel(ToolBar toolbar)
    {
        var popup = toolbar.GetTemplateDescendants().OfType<Popup>().FirstOrDefault();
        return popup?.Child?.GetVisualDescendants().OfType<StackPanel>()
            .FirstOrDefault(panel => panel.Name == ToolBar.PART_OverflowPanel);
    }

    private static ToggleButton GetOverflowToggle(ToolBar toolbar)
    {
        var popup = toolbar.GetTemplateDescendants().OfType<Popup>().First();
        var host = popup.GetVisualParent();
        return host?.GetVisualDescendants().OfType<ToggleButton>().First()
               ?? toolbar.GetTemplateDescendants().OfType<ToggleButton>().First();
    }

    [AvaloniaFact]
    public void Narrow_ToolBar_Marks_Tail_Items_As_Overflow()
    {
        var (window, toolbar, buttons, _) = CreateWindow(240, bindLabels: false);

        Assert.Contains(ToolBar.PC_Overflow, toolbar.Classes);
        Assert.False(ToolBar.GetIsOverflowItem(buttons[0]));
        Assert.True(ToolBar.GetIsOverflowItem(buttons[^1]));

        // While the popup is closed the overflow items stay in the toolbar panel so their content
        // and bindings keep working; they are only arranged outside the visible area.
        var panel = GetMainPanel(toolbar);
        Assert.Contains(buttons[^1], panel.Children);
        Assert.True(buttons[^1].Bounds.X >= panel.Bounds.Width - 1,
            $"overflow item was laid out inside the visible area ({buttons[^1].Bounds} in {panel.Bounds})");
        window.Close();
    }

    [AvaloniaFact]
    public void Popup_Hosts_Overflow_Items_Only_While_It_Is_Open()
    {
        var (window, toolbar, buttons, labels) = CreateWindow(240, bindLabels: true);
        var panel = GetMainPanel(toolbar);
        var overflowPanel = GetOverflowPanel(toolbar);
        Assert.NotNull(overflowPanel);

        var toggle = GetOverflowToggle(toolbar);
        toggle.IsChecked = true;
        Dispatcher.UIThread.RunJobs();

        Assert.True(toolbar.IsOverflowPopupOpen);
        Assert.Contains(buttons[^1], overflowPanel!.Children);
        Assert.DoesNotContain(buttons[^1], panel.Children);
        Assert.Equal("A5", labels[^1].Text);

        toggle.IsChecked = false;
        Dispatcher.UIThread.RunJobs();

        Assert.False(toolbar.IsOverflowPopupOpen);
        Assert.DoesNotContain(buttons[^1], overflowPanel.Children);
        Assert.Contains(buttons[^1], panel.Children);
        Assert.Equal("A5", labels[^1].Text);
        window.Close();
    }

    [AvaloniaFact]
    public void Items_Return_To_The_Main_Panel_In_Order_When_The_ToolBar_Grows()
    {
        var (window, toolbar, buttons, labels) = CreateWindow(240, bindLabels: false);
        Assert.True(ToolBar.GetIsOverflowItem(buttons[^1]));

        window.Width = 900;
        Dispatcher.UIThread.RunJobs();

        Assert.DoesNotContain(ToolBar.PC_Overflow, toolbar.Classes);
        var panel = GetMainPanel(toolbar);
        for (var i = 0; i < buttons.Count; i++)
        {
            Assert.False(ToolBar.GetIsOverflowItem(buttons[i]), $"item {i} is still an overflow item");
            Assert.True(buttons[i].IsVisible);
            Assert.Same(buttons[i], panel.Children[i]);
            Assert.Equal($"Item{i}", labels[i].Text);
            Assert.True(labels[i].Bounds.Height > 0, $"label {i} was not laid out");
        }

        window.Close();
    }

    [AvaloniaFact]
    public void Bound_Item_Content_Survives_An_Overflow_Round_Trip()
    {
        var (window, _, buttons, labels) = CreateWindow(240, bindLabels: true);
        Assert.True(ToolBar.GetIsOverflowItem(buttons[^1]));

        window.Width = 900;
        Dispatcher.UIThread.RunJobs();

        for (var i = 0; i < buttons.Count; i++)
        {
            Assert.Equal($"A{i}", labels[i].Text);
        }

        window.Close();
    }

    /// <summary>
    /// Mirrors the consuming application switching language while part of the toolbar overflows:
    /// the items keep their bindings and must show the new text once they are visible again.
    /// </summary>
    [AvaloniaFact]
    public void Bound_Item_Content_Updates_When_The_Source_Changes_While_Overflowed()
    {
        var (window, _, buttons, labels) = CreateWindow(240, bindLabels: true);
        Assert.True(ToolBar.GetIsOverflowItem(buttons[^1]));

        window.DataContext = new[] { "B0", "B1", "B2", "B3", "B4", "B5" };
        Dispatcher.UIThread.RunJobs();

        window.Width = 900;
        Dispatcher.UIThread.RunJobs();

        for (var i = 0; i < buttons.Count; i++)
        {
            Assert.Equal($"B{i}", labels[i].Text);
            Assert.True(labels[i].Bounds.Width > 0, $"label {i} has no width");
        }

        window.Close();
    }

    [AvaloniaFact]
    public void Hidden_Item_Is_Not_Reported_As_Overflow()
    {
        var (window, toolbar, buttons, _) = CreateWindow(240, bindLabels: false);

        buttons[^1].IsVisible = false;
        Dispatcher.UIThread.RunJobs();

        Assert.False(ToolBar.GetIsOverflowItem(buttons[^1]));
        Assert.DoesNotContain(buttons[^1], GetOverflowPanel(toolbar)!.Children);
        window.Close();
    }

    [AvaloniaFact]
    public void Separators_Do_Not_Trail_The_Visible_Items()
    {
        var window = new Window { Width = 240, Height = 140 };
        var toolbar = new ToolBar { HorizontalAlignment = HorizontalAlignment.Stretch };
        for (var i = 0; i < 4; i++)
        {
            var button = new Button { Width = 70, Content = new TextBlock { Text = $"Item{i}" } };
            ToolBar.SetOverflowMode(button, OverflowMode.AsNeeded);
            toolbar.Items.Add(button);
            if (i == 1)
            {
                toolbar.Items.Add(new ToolBarSeparator());
            }
        }

        window.Content = toolbar;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var panel = GetMainPanel(toolbar);
        Assert.IsNotType<ToolBarSeparator>(panel.Children[^1]);
        var overflowPanel = GetOverflowPanel(toolbar);
        if (overflowPanel is { Children.Count: > 0 })
        {
            Assert.IsNotType<ToolBarSeparator>(overflowPanel.Children[0]);
        }

        window.Close();
    }
}
