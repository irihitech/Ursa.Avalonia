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
}