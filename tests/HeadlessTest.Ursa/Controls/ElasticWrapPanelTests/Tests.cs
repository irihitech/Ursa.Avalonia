using System.Security.Cryptography.X509Certificates;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.ElasticWrapPanelTests;

public class Tests
{
    [AvaloniaFact]
    public void LineCount_Correct()
    {
        var window = new Window() { };
        var panel = new ElasticWrapPanel
        {
            Width = 200,
            Height = 200,
            Orientation = Orientation.Horizontal,
        };
        window.Content = panel;
        window.Show();
        Assert.Equal(0, panel.LineCount);
    }
    
    [AvaloniaFact]
    public void LineCount_Correct_1()
    {
        var window = new Window() { };
        var panel = new ElasticWrapPanel
        {
            Width = 200,
            Height = 200,
            Orientation = Orientation.Horizontal,
        };
        var rect = new Rectangle
        {
            Width = 100,
            Height = 100,
        };
        panel.Children.Add(rect);
        window.Content = panel;
        window.Show();
        Assert.Equal(1, panel.LineCount);
    }
    
    [AvaloniaFact]
    public void LineCount_Correct_2()
    {
        var window = new Window() { };
        var panel = new ElasticWrapPanel
        {
            Width = 200,
            Height = 200,
            Orientation = Orientation.Horizontal,
        };
        for (int i = 0; i < 3; i++)
        {
            var rect = new Rectangle
            {
                Width = 100,
                Height = 100,
            };
            panel.Children.Add(rect);
        }
        window.Content = panel;
        window.Show();
        Assert.Equal(2, panel.LineCount);
    }

    [AvaloniaFact]
    public void ItemSpacing_Applied_Correctly()
    {
        var window = new Window() { };
        var panel = new ElasticWrapPanel
        {
            Width = 400,
            Height = 200,
            Orientation = Orientation.Horizontal,
            ItemSpacing = 10,
        };
        
        // Add 3 rectangles of 100x100 with 10px spacing
        // Total width should be: 100 + 10 + 100 + 10 + 100 = 320
        for (int i = 0; i < 3; i++)
        {
            var rect = new Rectangle
            {
                Width = 100,
                Height = 100,
            };
            panel.Children.Add(rect);
        }
        
        window.Content = panel;
        window.Show();
        
        // All 3 should fit on one line
        Assert.Equal(1, panel.LineCount);
        
        // Check that the desired size accounts for spacing
        Assert.True(panel.DesiredSize.Width >= 320);
    }

    [AvaloniaFact]
    public void LineSpacing_Applied_Correctly()
    {
        var window = new Window() { };
        var panel = new ElasticWrapPanel
        {
            Width = 200,
            Height = 400,
            Orientation = Orientation.Horizontal,
            LineSpacing = 20,
        };
        
        // Add 4 rectangles of 100x100, should wrap to 2 lines
        for (int i = 0; i < 4; i++)
        {
            var rect = new Rectangle
            {
                Width = 100,
                Height = 100,
            };
            panel.Children.Add(rect);
        }
        
        window.Content = panel;
        window.Show();
        
        // Should have 2 lines
        Assert.Equal(2, panel.LineCount);
        
        // Total height should be: 100 + 20 + 100 = 220
        Assert.True(panel.DesiredSize.Height >= 220);
    }

    [AvaloniaFact]
    public void ItemSpacing_And_LineSpacing_Together()
    {
        var window = new Window() { };
        var panel = new ElasticWrapPanel
        {
            Width = 250,
            Height = 400,
            Orientation = Orientation.Horizontal,
            ItemSpacing = 10,
            LineSpacing = 20,
        };
        
        // Add 4 rectangles of 100x100
        // With ItemSpacing=10, two items per line need 100 + 10 + 100 = 210 width
        // With LineSpacing=20, two lines need 100 + 20 + 100 = 220 height
        for (int i = 0; i < 4; i++)
        {
            var rect = new Rectangle
            {
                Width = 100,
                Height = 100,
            };
            panel.Children.Add(rect);
        }
        
        window.Content = panel;
        window.Show();
        
        // Should have 2 lines
        Assert.Equal(2, panel.LineCount);
        
        // Check dimensions include spacing
        Assert.True(panel.DesiredSize.Width >= 210);
        Assert.True(panel.DesiredSize.Height >= 220);
    }

    [AvaloniaFact]
    public void ItemSpacing_Does_Not_Affect_LineCount()
    {
        var window = new Window() { };
        
        // Panel without spacing - should fit 2 items per line (200 width / 100 per item = 2)
        var panel1 = new ElasticWrapPanel
        {
            Width = 200,
            Height = 400,
            Orientation = Orientation.Horizontal,
            ItemSpacing = 0,
        };
        
        for (int i = 0; i < 4; i++)
        {
            panel1.Children.Add(new Rectangle { Width = 100, Height = 100 });
        }
        
        // Panel with spacing - items with spacing should fit fewer per line
        // 100 + 10 + 100 = 210 > 200, so only 1 item per line
        var panel2 = new ElasticWrapPanel
        {
            Width = 200,
            Height = 400,
            Orientation = Orientation.Horizontal,
            ItemSpacing = 10,
        };
        
        for (int i = 0; i < 4; i++)
        {
            panel2.Children.Add(new Rectangle { Width = 100, Height = 100 });
        }
        
        window.Content = panel1;
        window.Show();
        Assert.Equal(2, panel1.LineCount); // 2 items per line, 4 items total = 2 lines
        
        window.Content = panel2;
        window.UpdateLayout();
        Assert.Equal(4, panel2.LineCount); // 1 item per line, 4 items total = 4 lines
    }
}