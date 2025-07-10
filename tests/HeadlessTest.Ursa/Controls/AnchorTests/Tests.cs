using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.AnchorTests;

public class Tests
{
    [AvaloniaFact]
    public async void Click_Anchor_With_Mouse_Should_Update_Scroll_Offset()
    {
        var window = new Window()
        {
            Width = 500,
            Height = 500,
        };
        var view = new TestView();
        window.Content = view;
        window.Show();
        
        var anchor = view.FindControl<Anchor>("Anchor");
        var scrollViewer = view.FindControl<ScrollViewer>("ScrollViewer");
        var item4 = view.FindControl<AnchorItem>("Item4");
        
        Assert.NotNull(anchor);
        Assert.NotNull(scrollViewer);
        Assert.NotNull(item4);

        var transltion = item4.TranslatePoint(new Point(0, 0), window);
        
        Assert.Equal(0, scrollViewer.Offset.Y);
        
        // Simulate a click on the anchor
        window.MouseDown(new Point(10, transltion.Value.Y+10), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        await Task.Delay(800);
        Assert.True(item4.IsSelected);
        Assert.Equal(300.0 * 3, scrollViewer.Offset.Y, 50.0);
    }
    
    [AvaloniaFact]
    public async void Change_Scroll_Offset_Should_Update_Selected_Item()
    {
        var window = new Window()
        {
            Width = 500,
            Height = 500,
        };
        var view = new TestView();
        window.Content = view;
        window.Show();
        
        var anchor = view.FindControl<Anchor>("Anchor");
        var scrollViewer = view.FindControl<ScrollViewer>("ScrollViewer");
        var item1 = view.FindControl<AnchorItem>("Item1");
        var item2 = view.FindControl<AnchorItem>("Item2");
        var item4 = view.FindControl<AnchorItem>("Item4");
        
        Assert.NotNull(anchor);
        Assert.NotNull(scrollViewer);
        Assert.NotNull(item1);
        Assert.NotNull(item2);
        Assert.NotNull(item4);
        
        Dispatcher.UIThread.RunJobs();
        
        Assert.True(item1.IsSelected);
        Assert.False(item2.IsSelected);

        // Change the scroll offset
        scrollViewer.Offset = new Vector(0, 310.0);
        Dispatcher.UIThread.RunJobs();
        
        // Check if the second item is selected
        Assert.True(item2.IsSelected);
        Assert.False(item4.IsSelected);
    }

    [AvaloniaFact]
    public void MVVM_Support()
    {
        var window = new Window();
        var view = new TestView2();
        window.Content = view;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var items = window.GetVisualDescendants().OfType<AnchorItem>().ToList();
        Assert.NotEmpty(items);
        Assert.Equal(10, items.Count);

    }
}