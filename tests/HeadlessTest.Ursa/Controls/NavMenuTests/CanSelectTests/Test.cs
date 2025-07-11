using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.LogicalTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.NavMenuTests.CanSelectTests;

public class Test
{
    [AvaloniaFact]
    public void CanSelect_Blocks_Selection_Change_Inline_Xaml()
    {
        Window window = new Window
        {
            Width = 400,
            Height = 400,
        };

        var view = new TestView1();
        
        window.Content = view;
        
        window.Show();

        var menu = view.FindControl<NavMenu>("Menu");
        var item1 = view.FindControl<NavMenuItem>("MenuItem1");
        var item2 = view.FindControl<NavMenuItem>("MenuItem2");
        var item3 = view.FindControl<NavMenuItem>("MenuItem3");

        Assert.NotNull(menu);
        Assert.NotNull(item1);
        Assert.NotNull(item2);
        Assert.NotNull(item3);

        var point1 = item1.TranslatePoint(new Point(0, 0), window);
        var point2 = item2.TranslatePoint(new Point(0, 0), window);
        var point3 = item3.TranslatePoint(new Point(0, 0), window);
        Assert.NotNull(point1);
        Assert.NotNull(point2);
        Assert.NotNull(point3);
        
        window.MouseDown(new Point(point1.Value.X+10, point1.Value.Y+10), Avalonia.Input.MouseButton.Left);
        window.MouseUp(new Point(point1.Value.X+10, point1.Value.Y+10), Avalonia.Input.MouseButton.Left);
        Assert.Equal(item1, menu.SelectedItem);
        
        window.MouseDown(new Point(point2.Value.X+10, point2.Value.Y+10), Avalonia.Input.MouseButton.Left);
        window.MouseUp(new Point(point2.Value.X+10, point2.Value.Y+10), Avalonia.Input.MouseButton.Left);
        Assert.Equal(item1, menu.SelectedItem); // Should not change selection due to CanSelect being false
        
        window.MouseDown(new Point(point3.Value.X+10, point3.Value.Y+10), Avalonia.Input.MouseButton.Left);
        window.MouseUp(new Point(point3.Value.X+10, point3.Value.Y+10), Avalonia.Input.MouseButton.Left);
        Assert.Equal(item3, menu.SelectedItem); // Should change selection to item3

    }
    
    [AvaloniaFact]
    public void CanSelect_Blocks_Selection_Change_Inline_Code()
    {
        Window window = new Window
        {
            Width = 400,
            Height = 400,
        };

        var view = new TestView2();
        
        window.Content = view;
        
        window.Show();

        var menu = view.FindControl<NavMenu>("Menu");
        var items = menu.GetLogicalDescendants().OfType<NavMenuItem>().ToList();
        var item1 = items[0];
        var item2 = items[1];
        var item3 = items[2];

        Assert.NotNull(menu);
        Assert.NotNull(item1);
        Assert.NotNull(item2);
        Assert.NotNull(item3);

        var point1 = item1.TranslatePoint(new Point(0, 0), window);
        var point2 = item2.TranslatePoint(new Point(0, 0), window);
        var point3 = item3.TranslatePoint(new Point(0, 0), window);
        Assert.NotNull(point1);
        Assert.NotNull(point2);
        Assert.NotNull(point3);
        
        window.MouseDown(new Point(point1.Value.X+10, point1.Value.Y+10), Avalonia.Input.MouseButton.Left);
        window.MouseUp(new Point(point1.Value.X+10, point1.Value.Y+10), Avalonia.Input.MouseButton.Left);
        Assert.Equal(item1.DataContext, menu.SelectedItem);
        
        window.MouseDown(new Point(point2.Value.X+10, point2.Value.Y+10), Avalonia.Input.MouseButton.Left);
        window.MouseUp(new Point(point2.Value.X+10, point2.Value.Y+10), Avalonia.Input.MouseButton.Left);
        Assert.Equal(item1.DataContext, menu.SelectedItem); // Should not change selection due to CanSelect being false
        
        window.MouseDown(new Point(point3.Value.X+10, point3.Value.Y+10), Avalonia.Input.MouseButton.Left);
        window.MouseUp(new Point(point3.Value.X+10, point3.Value.Y+10), Avalonia.Input.MouseButton.Left);
        Assert.Equal(item3.DataContext, menu.SelectedItem); // Should change selection to item3
    }
}