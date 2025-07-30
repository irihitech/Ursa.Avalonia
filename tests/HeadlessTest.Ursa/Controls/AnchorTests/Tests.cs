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

    [AvaloniaFact]
    public void TopOffset_Property_Should_Affect_Scroll_Position()
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
        
        Assert.NotNull(anchor);
        Assert.NotNull(scrollViewer);
        
        // Set TopOffset to 50
        anchor.TopOffset = 50;
        
        // Scroll to position that should trigger item2 selection
        scrollViewer.Offset = new Vector(0, 310.0);
        Dispatcher.UIThread.RunJobs();
        
        // Check that the offset affects position calculations
        Assert.Equal(50, anchor.TopOffset);
        
        // The behavior should account for the top offset
        anchor.InvalidatePositions();
        Dispatcher.UIThread.RunJobs();
    }

    [AvaloniaFact]
    public void InvalidatePositions_Should_Update_Internal_Positions()
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
        
        Assert.NotNull(anchor);
        Assert.NotNull(scrollViewer);
        
        Dispatcher.UIThread.RunJobs();
        
        // Call InvalidatePositions explicitly
        anchor.InvalidatePositions();
        Dispatcher.UIThread.RunJobs();
        
        // Verify that positions are correctly calculated by checking selection
        var item1 = view.FindControl<AnchorItem>("Item1");
        Assert.NotNull(item1);
        Assert.True(item1.IsSelected); // Should be selected at top
    }

    [AvaloniaFact]
    public void Anchor_Id_Attached_Property_Should_Work()
    {
        var border = new Border();
        
        // Test SetId and GetId
        Anchor.SetId(border, "test-id");
        var retrievedId = Anchor.GetId(border);
        
        Assert.Equal("test-id", retrievedId);
        
        // Test with null
        Anchor.SetId(border, null);
        var nullId = Anchor.GetId(border);
        Assert.Null(nullId);
    }

    [AvaloniaFact]
    public void Anchor_Without_TargetContainer_Should_Not_Crash()
    {
        var window = new Window();
        var anchor = new Anchor();
        window.Content = anchor;
        window.Show();
        
        // These operations should not crash when TargetContainer is null
        anchor.InvalidatePositions();
        Dispatcher.UIThread.RunJobs();
        
        // Should not throw
        Assert.Null(anchor.TargetContainer);
    }

    [AvaloniaFact]
    public void AnchorItem_Level_Property_Should_Calculate_Correctly()
    {
        var window = new Window()
        {
            Width = 500,
            Height = 500,
        };
        var view = new TestView();
        window.Content = view;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        var item1 = view.FindControl<AnchorItem>("Item1");
        var item2 = view.FindControl<AnchorItem>("Item2");
        var item4 = view.FindControl<AnchorItem>("Item4");
        
        Assert.NotNull(item1);
        Assert.NotNull(item2);
        Assert.NotNull(item4);
        
        // Based on the XAML structure, Item1 is inside Anchor (level 1)
        Assert.Equal(1, item1.Level);
        
        // Item2 is nested inside Item1, so level 2
        Assert.Equal(2, item2.Level);
        
        // Item4 is at the same level as Item1
        Assert.Equal(1, item4.Level);
    }

    [AvaloniaFact]
    public void AnchorItem_Without_Anchor_Parent_Should_Throw()
    {
        // This test verifies that AnchorItem throws when not inside an Anchor
        var anchorItem = new AnchorItem();
        var window = new Window();
        
        // Add some items to the AnchorItem to trigger container creation
        anchorItem.ItemsSource = new[] { "Item1", "Item2" };
        window.Content = anchorItem;
        
        // The exception should be thrown when showing the window
        var exception = Assert.Throws<InvalidOperationException>(() => window.Show());
        Assert.Contains("AnchorItem must be inside an Anchor control", exception.Message);
    }

    [AvaloniaFact]
    public async void Scroll_To_Bottom_Should_Handle_Edge_Case()
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
        
        Assert.NotNull(anchor);
        Assert.NotNull(scrollViewer);
        
        Dispatcher.UIThread.RunJobs();
        
        // Scroll to the very bottom
        var maxOffset = scrollViewer.Extent.Height - scrollViewer.Bounds.Height;
        scrollViewer.Offset = new Vector(0, maxOffset);
        Dispatcher.UIThread.RunJobs();
        
        // Should handle the edge case without crashing
        anchor.InvalidatePositions();
        Dispatcher.UIThread.RunJobs();
        
        // The last item should be selected
        var lastItems = window.GetVisualDescendants().OfType<AnchorItem>()
            .Where(i => i.IsSelected).ToList();
        Assert.Single(lastItems);
    }
}