using Irihi.Avalonia.Shared.Contracts;
using Ursa.Controls.OverlayShared;

namespace Test.Ursa.OverlayFeedbackElementTests;

/// <summary>
/// Simple unit tests for OverlayFeedbackElement that don't require UI framework
/// </summary>
public class OverlayFeedbackElementUnitTests
{
    /// <summary>
    /// Minimal test implementation of OverlayFeedbackElement
    /// </summary>
    private class TestOverlayFeedbackElement : OverlayFeedbackElement
    {
        public bool CloseWasCalled { get; private set; }
        public bool AnchorWasCalled { get; private set; }

        public override void Close()
        {
            CloseWasCalled = true;
            OnElementClosing(this, "test_result");
        }

        protected internal override void AnchorAndUpdatePositionInfo()
        {
            AnchorWasCalled = true;
        }

        // Expose protected method for testing
        public void TestOnElementClosing(object? sender, object? args)
        {
            OnElementClosing(sender, args);
        }
    }

    private class MockDialogContext : IDialogContext
    {
        public event EventHandler<object?>? RequestClose;

        public void Close()
        {
            RequestClose?.Invoke(this, null);
        }

        public void TriggerRequestClose(object? result = null)
        {
            RequestClose?.Invoke(this, result);
        }
    }

    [Fact]
    public void IsClosed_Should_Default_To_True()
    {
        var element = new TestOverlayFeedbackElement();
        Assert.True(element.IsClosed);
    }

    [Fact]
    public void IsClosed_Can_Be_Set_And_Retrieved()
    {
        var element = new TestOverlayFeedbackElement();
        
        element.IsClosed = false;
        Assert.False(element.IsClosed);
        
        element.IsClosed = true;
        Assert.True(element.IsClosed);
    }

    [Fact]
    public void Close_Method_Should_Be_Called()
    {
        var element = new TestOverlayFeedbackElement();
        
        element.Close();
        
        Assert.True(element.CloseWasCalled);
    }

    [Fact]
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

    [Fact]
    public void Closed_Event_Should_Set_IsClosed_To_True()
    {
        var element = new TestOverlayFeedbackElement();
        element.IsClosed = false;
        
        element.TestOnElementClosing(element, "test_result");
        
        Assert.True(element.IsClosed);
    }

    [Fact]
    public void DataContext_With_IDialogContext_Should_Subscribe_To_RequestClose()
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

    [Fact]
    public void DataContext_Change_Should_Unsubscribe_From_Previous_Context()
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
        
        // Trigger on old context - should NOT raise event
        oldContext.TriggerRequestClose("old_result");
        Assert.Equal(0, eventRaisedCount);
        
        // Trigger on new context - should raise event
        newContext.TriggerRequestClose("new_result");
        Assert.Equal(1, eventRaisedCount);
    }

    [Fact]
    public void IsShowAsync_Property_Should_Be_Set_During_ShowAsync()
    {
        var element = new TestOverlayFeedbackElement();
        
        Assert.False(element.IsShowAsync);
        
        var task = element.ShowAsync<string>();
        Assert.True(element.IsShowAsync);
        
        // Complete the async operation
        element.TestOnElementClosing(element, "result");
        
        // Should be reset after completion
        Assert.False(element.IsShowAsync);
    }

    [Fact]
    public void AnchorAndUpdatePositionInfo_Should_Be_Callable()
    {
        var element = new TestOverlayFeedbackElement();
        
        Assert.False(element.AnchorWasCalled);
        
        element.AnchorAndUpdatePositionInfo();
        
        Assert.True(element.AnchorWasCalled);
    }
}