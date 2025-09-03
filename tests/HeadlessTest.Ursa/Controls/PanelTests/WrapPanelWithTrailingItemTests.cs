using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.PanelTests;

public class WrapPanelWithTrailingItemTests
{
    [Fact]
    public void Visual_Children_Correct()
    {
        var panel = new WrapPanelWithTrailingItem();
        var child1 = new Button { Content = "Button 1"};
        var child2 = new Button { Content = "Button 2"};
        var trailing = new Button { Content = "Trailing"};
        
        panel.Children.Add(child1);
        panel.Children.Add(child2);
        panel.TrailingItem = trailing;
        
        var visualChildren = panel.GetVisualChildren().ToList();
        Assert.Equal(3, visualChildren.Count);
        Assert.Equal(child1, visualChildren[0]);
        Assert.Equal(child2, visualChildren[1]);
        Assert.Equal(trailing, visualChildren[2]);
        
        var child3 = new Button { Content = "Button 3"};
        panel.Children.Add(child3);
        
        visualChildren = panel.GetVisualChildren().ToList();
        Assert.Equal(4, visualChildren.Count);
        Assert.Equal(child1, visualChildren[0]);
        Assert.Equal(child2, visualChildren[1]);
        Assert.Equal(child3, visualChildren[2]);
        Assert.Equal(trailing, visualChildren[3]);
        
        var trailing2 = new Button { Content = "Trailing2"};
        panel.TrailingItem = trailing2;
        
        visualChildren = panel.GetVisualChildren().ToList();
        Assert.Equal(4, visualChildren.Count);
        Assert.Equal(child1, visualChildren[0]);
        Assert.Equal(child2, visualChildren[1]);
        Assert.Equal(child3, visualChildren[2]);
        Assert.Equal(trailing2, visualChildren[3]);

        panel.Children.Remove(child2);
        
        visualChildren = panel.GetVisualChildren().ToList();
        Assert.Equal(3, visualChildren.Count);
        Assert.Equal(child1, visualChildren[0]);
        Assert.Equal(child3, visualChildren[1]);
        Assert.Equal(trailing2, visualChildren[2]);
    }
    
    [Fact]
    // Items Appears in Logical Children because IsItemsHost is false for individual Panels
    public void Logical_Children_Correct()
    {
        var panel = new WrapPanelWithTrailingItem();
        var child1 = new Button { Content = "Button 1"};
        var child2 = new Button { Content = "Button 2"};
        var trailing = new Button { Content = "Trailing"};
        
        panel.Children.Add(child1);
        panel.Children.Add(child2);
        panel.TrailingItem = trailing;
        
        var logicalChildren = panel.GetLogicalChildren().ToList();
        Assert.Equal(3, logicalChildren.Count);
        Assert.Equal(child1, logicalChildren[0]);
        Assert.Equal(child2, logicalChildren[1]);
        Assert.Equal(trailing, logicalChildren[2]);
        
        var child3 = new Button { Content = "Button 3"};
        panel.Children.Add(child3);
        
        logicalChildren = panel.GetLogicalChildren().ToList();
        Assert.Equal(4, logicalChildren.Count);
        Assert.Equal(child1, logicalChildren[0]);
        Assert.Equal(child2, logicalChildren[1]);
        Assert.Equal(child3, logicalChildren[2]);
        Assert.Equal(trailing, logicalChildren[3]);
        
        var trailing2 = new Button { Content = "Trailing2"};
        panel.TrailingItem = trailing2;
        
        logicalChildren = panel.GetLogicalChildren().ToList();
        Assert.Equal(4, logicalChildren.Count);
        Assert.Equal(child1, logicalChildren[0]);
        Assert.Equal(child2, logicalChildren[1]);
        Assert.Equal(child3, logicalChildren[2]);
        Assert.Equal(trailing2, logicalChildren[3]);

        panel.Children.Remove(child2);
        
        logicalChildren = panel.GetLogicalChildren().ToList();
        Assert.Equal(3, logicalChildren.Count);
        Assert.Equal(child1, logicalChildren[0]);
        Assert.Equal(child3, logicalChildren[1]);
        Assert.Equal(trailing2, logicalChildren[2]);
    }

    [AvaloniaFact]
    public void Measure_Arrange_Children()
    {
        var window = new Window()
        {
            Height = 1000, Width = 1000, VerticalContentAlignment = VerticalAlignment.Stretch
        };
        var panel = new WrapPanelWithTrailingItem();
        var child1 = new Button { Content = "Button 1", Width = 200, Height = 100 };
        var child2 = new Button { Content = "Button 2", Width = 300, Height = 100 };
        var trailing = new Button
        {
            Content = "Trailing", 
            Height = 100, 
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        window.Content = panel;
        panel.Children.Add(child1);
        panel.Children.Add(child2);
        panel.TrailingItem = trailing;
        
        window.Show();
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(200, child1.Bounds.Width);
        Assert.Equal(300, child2.Bounds.Width);
        Assert.Equal(500, trailing.Bounds.Width);
        
        panel.Width = 600;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(200, child1.Bounds.Width);
        Assert.Equal(300, child2.Bounds.Width);
        Assert.Equal(100, trailing.Bounds.Width);

        panel.Width = 510;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(200, child1.Bounds.Width);
        Assert.Equal(300, child2.Bounds.Width);
        Assert.Equal(510, trailing.Bounds.Width);
        Assert.Equal(100, trailing.Bounds.Y);
        
        panel.Width = 300;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(200, child1.Bounds.Width);
        Assert.Equal(300, child2.Bounds.Width);
        Assert.Equal(300, trailing.Bounds.Width);
        Assert.Equal(200, trailing.Bounds.Y);
    }
}