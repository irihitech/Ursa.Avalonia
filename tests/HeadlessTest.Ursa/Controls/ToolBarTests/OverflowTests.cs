using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;
using UrsaToolBar = Ursa.Controls.ToolBar;

namespace HeadlessTest.Ursa.Controls.ToolBarTests;

public class OverflowTests
{
    [AvaloniaFact]
    public void Button_Text_Remains_Visible_After_Overflow_And_Return()
    {
        var toolbar = new UrsaToolBar { Width = 400 };
        var first = new Button { Width = 100, Content = "First" };
        var second = new Button { Width = 100, Content = "Second" };
        var third = new Button { Width = 100, Content = "Third" };
        UrsaToolBar.SetOverflowMode(first, OverflowMode.Never);
        UrsaToolBar.SetOverflowMode(second, OverflowMode.AsNeeded);
        UrsaToolBar.SetOverflowMode(third, OverflowMode.AsNeeded);
        toolbar.Items.Add(first);
        toolbar.Items.Add(second);
        toolbar.Items.Add(third);

        var window = new Window { Width = 500, Height = 200, Content = toolbar };
        window.Show();
        window.UpdateLayout();

        var panel = Assert.Single(toolbar.GetVisualDescendants().OfType<ToolBarPanel>());
        var overflowButton = Assert.Single(toolbar.GetVisualDescendants().OfType<ToggleButton>());
        Assert.Contains(second, panel.Children);

        for (var i = 0; i < 3; i++)
        {
            toolbar.Width = 150;
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            Assert.DoesNotContain(second, panel.Children);
            overflowButton.IsChecked = true;
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            AssertHeaderVisible(second);
            overflowButton.IsChecked = false;

            toolbar.Width = 400;
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();
            Assert.Contains(second, panel.Children);
            AssertHeaderVisible(second);
        }

        static void AssertHeaderVisible(Button button)
        {
            var text = Assert.Single(button.GetVisualDescendants().OfType<TextBlock>());
            Assert.Equal("Second", text.Text);
            Assert.True(text.IsEffectivelyVisible);
            Assert.True(text.Bounds.Width > 0);
        }
    }
}
