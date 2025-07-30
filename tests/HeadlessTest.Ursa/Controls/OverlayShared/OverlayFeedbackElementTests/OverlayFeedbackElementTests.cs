using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.EventArgs;

namespace HeadlessTest.Ursa.Controls.OverlayShared.OverlayFeedbackElementTests;

public class OverlayFeedbackElementTests
{
    [AvaloniaFact]
    public void IsClosed_Should_Default_To_True()
    {
        var element = new TestOverlayFeedbackElement();
        Assert.True(element.IsClosed);
    }

    [AvaloniaFact]
    public void IsClosed_Can_Be_Set_And_Get()
    {
        var element = new TestOverlayFeedbackElement();
        
        element.IsClosed = false;
        Assert.False(element.IsClosed);
        
        element.IsClosed = true;
        Assert.True(element.IsClosed);
    }

    [AvaloniaFact]
    public void Close_Method_Should_Be_Called_And_Raise_Event()
    {
        var element = new TestOverlayFeedbackElement();
        var eventRaised = false;
        object? eventResult = null;

        element.Closed += (sender, args) =>
        {
            eventRaised = true;
            eventResult = args.Result;
        };

        element.Close();

        Assert.True(element.CloseWasCalled);
        Assert.True(eventRaised);
        Assert.Equal("test_close_result", eventResult);
    }

    [AvaloniaFact]
    public void OnElementClosing_Should_Raise_Closed_Event()
    {
        var element = new TestOverlayFeedbackElement();
        var eventRaised = false;
        object? eventResult = null;

        element.Closed += (sender, args) =>
        {
            eventRaised = true;
            eventResult = args.Result;
        };

        element.TestOnElementClosing(element, "test_result");

        Assert.True(eventRaised);
        Assert.Equal("test_result", eventResult);
    }

    [AvaloniaFact]
    public void Closed_Event_Should_Update_IsClosed_Property()
    {
        var element = new TestOverlayFeedbackElement();
        element.IsClosed = false;

        element.TestOnElementClosing(element, "test_result");

        Assert.True(element.IsClosed);
    }

    [AvaloniaFact]
    public void DataContext_Change_Should_Subscribe_To_IDialogContext()
    {
        var element = new TestOverlayFeedbackElement();
        var mockContext = new MockDialogContext();
        var eventRaised = false;

        element.Closed += (sender, args) =>
        {
            eventRaised = true;
        };

        element.DataContext = mockContext;
        mockContext.TriggerRequestClose("context_result");

        Assert.True(eventRaised);
    }

    [AvaloniaFact]
    public void DataContext_Change_Should_Unsubscribe_From_Old_IDialogContext()
    {
        var element = new TestOverlayFeedbackElement();
        var oldContext = new MockDialogContext();
        var newContext = new MockDialogContext();
        var eventRaisedCount = 0;

        element.Closed += (sender, args) =>
        {
            eventRaisedCount++;
        };

        // Set first context
        element.DataContext = oldContext;
        
        // Change to new context
        element.DataContext = newContext;
        
        // Trigger on old context - should not raise event
        oldContext.TriggerRequestClose("old_result");
        Assert.Equal(0, eventRaisedCount);
        
        // Trigger on new context - should raise event
        newContext.TriggerRequestClose("new_result");
        Assert.Equal(1, eventRaisedCount);
    }

    [AvaloniaFact]
    public async Task ShowAsync_Should_Return_Task_With_Result()
    {
        var element = new TestOverlayFeedbackElement();
        
        // Start the async operation
        var task = element.ShowAsync<string>();
        
        // Simulate closing with result
        await Task.Delay(10); // Small delay to ensure task setup
        element.TestOnElementClosing(element, "async_result");
        
        var result = await task;
        Assert.Equal("async_result", result);
    }

    [AvaloniaFact]
    public async Task ShowAsync_Should_Return_Default_When_No_Result()
    {
        var element = new TestOverlayFeedbackElement();
        
        var task = element.ShowAsync<string>();
        
        await Task.Delay(10);
        element.TestOnElementClosing(element, null);
        
        var result = await task;
        Assert.Null(result);
    }

    [AvaloniaFact]
    public async Task ShowAsync_Should_Return_Default_When_Wrong_Type()
    {
        var element = new TestOverlayFeedbackElement();
        
        var task = element.ShowAsync<int>();
        
        await Task.Delay(10);
        element.TestOnElementClosing(element, "string_result");
        
        var result = await task;
        Assert.Equal(0, result);
    }

    [AvaloniaFact]
    public async Task ShowAsync_Should_Handle_Cancellation()
    {
        var element = new TestOverlayFeedbackElement();
        var cancellationTokenSource = new CancellationTokenSource();
        
        var task = element.ShowAsync<string>(cancellationTokenSource.Token);
        
        // Cancel the token
        cancellationTokenSource.Cancel();
        
        // Wait a bit for cancellation to process
        await Task.Delay(100);
        
        var result = await task;
        // When cancelled, the Close() method is called which provides "test_close_result"
        // This is the expected behavior - cancellation triggers close
        Assert.Equal("test_close_result", result);
        Assert.True(element.CloseWasCalled); // Should have called close
    }

    [AvaloniaFact]
    public void OnDetachedFromLogicalTree_Should_Clear_Content()
    {
        var element = new TestOverlayFeedbackElement();
        var contentControl = new Button { Content = "Test Content" };
        element.Content = contentControl;
        
        Assert.NotNull(element.Content);
        
        // Create a window and panel to simulate logical tree
        var window = new Window();
        var panel = new Panel();
        panel.Children.Add(element);
        window.Content = panel;
        
        // Show and then remove to trigger detachment
        window.Show();
        Dispatcher.UIThread.RunJobs();
        
        panel.Children.Remove(element);
        Dispatcher.UIThread.RunJobs();
        
        Assert.Null(element.Content);
    }

    [AvaloniaFact]
    public void OnAttachedToVisualTree_Should_Find_Container_Panel()
    {
        var element = new TestOverlayFeedbackElement();
        var panel = new Panel();
        var window = new Window();
        
        panel.Children.Add(element);
        window.Content = panel;
        window.Show();
        
        Dispatcher.UIThread.RunJobs();
        
        // ContainerPanel should be set (it's protected, but we can test the effect)
        // We can't directly access ContainerPanel, but we can test that attachment worked
        Assert.True(element.IsAttachedToVisualTree());
    }

    [AvaloniaFact]
    public void AnchorAndUpdatePositionInfo_Should_Be_Called_When_Invoked()
    {
        var element = new TestOverlayFeedbackElement();
        
        Assert.False(element.AnchorAndUpdatePositionInfoWasCalled);
        
        // Call the abstract method directly through our test implementation
        element.AnchorAndUpdatePositionInfo();
        
        Assert.True(element.AnchorAndUpdatePositionInfoWasCalled);
    }
}