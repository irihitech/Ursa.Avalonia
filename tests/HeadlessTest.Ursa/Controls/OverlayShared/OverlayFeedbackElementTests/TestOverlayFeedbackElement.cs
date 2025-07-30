using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Irihi.Avalonia.Shared.Contracts;
using Ursa.Controls.OverlayShared;

namespace HeadlessTest.Ursa.Controls.OverlayShared.OverlayFeedbackElementTests;

/// <summary>
/// Concrete test implementation of OverlayFeedbackElement for testing purposes
/// </summary>
public class TestOverlayFeedbackElement : OverlayFeedbackElement
{
    public bool CloseWasCalled { get; private set; }
    public bool AnchorAndUpdatePositionInfoWasCalled { get; private set; }

    public override void Close()
    {
        CloseWasCalled = true;
        OnElementClosing(this, "test_close_result");
    }

    protected internal override void AnchorAndUpdatePositionInfo()
    {
        AnchorAndUpdatePositionInfoWasCalled = true;
    }

    // Expose protected methods for testing
    public void TestOnElementClosing(object? sender, object? args)
    {
        OnElementClosing(sender, args);
    }

    public new void BeginResizeDrag(WindowEdge windowEdge, PointerPressedEventArgs e)
    {
        base.BeginResizeDrag(windowEdge, e);
    }

    public new void BeginMoveDrag(PointerPressedEventArgs e)
    {
        base.BeginMoveDrag(e);
    }
}

/// <summary>
/// Mock implementation of IDialogContext for testing
/// </summary>
public class MockDialogContext : IDialogContext
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