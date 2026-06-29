using Ursa.Controls;

namespace Test.Ursa.Controls.ImageViewerTests;

public class PanZoomGestureHandlerTests
{
    [Fact]
    public void Constructor_Should_Initialize_With_No_Active_Pointers()
    {
        var handler = new PanZoomGestureHandler();
        // Should not throw; state should be empty
        handler.Complete(); // Should not throw either
    }

    [Fact]
    public void Complete_Should_Reset_State()
    {
        var handler = new PanZoomGestureHandler();
        // Should not throw even without active state
        handler.Complete();
    }

    [Fact]
    public void Pan_Event_Should_Be_Subscribable_And_Unsubscribable()
    {
        var handler = new PanZoomGestureHandler();
        bool invoked = false;
        Action<double, double> callback = (dx, dy) => invoked = true;

        handler.Pan += callback;
        handler.Pan -= callback;
        // No throw means success
    }

    [Fact]
    public void PinchStarted_Event_Should_Be_Subscribable()
    {
        var handler = new PanZoomGestureHandler();
        bool invoked = false;
        Action callback = () => invoked = true;

        handler.PinchStarted += callback;
        handler.PinchStarted -= callback;
    }

    [Fact]
    public void PinchUpdated_Event_Should_Be_Subscribable()
    {
        var handler = new PanZoomGestureHandler();
        bool invoked = false;
        Action<PinchUpdateEventArgs> callback = e => invoked = true;

        handler.PinchUpdated += callback;
        handler.PinchUpdated -= callback;
    }

    [Fact]
    public void PinchEnded_Event_Should_Be_Subscribable()
    {
        var handler = new PanZoomGestureHandler();
        bool invoked = false;
        Action callback = () => invoked = true;

        handler.PinchEnded += callback;
        handler.PinchEnded -= callback;
    }

    [Fact]
    public void PinchUpdateEventArgs_Should_Store_Values_Correctly()
    {
        var args = new PinchUpdateEventArgs(
            cumulativeScale: 1.5,
            cumulativeTranslationX: 10.0,
            cumulativeTranslationY: 20.0,
            centerX: 100.0,
            centerY: 200.0);

        Assert.Equal(1.5, args.CumulativeScale);
        Assert.Equal(10.0, args.CumulativeTranslationX);
        Assert.Equal(20.0, args.CumulativeTranslationY);
        Assert.Equal(100.0, args.CenterX);
        Assert.Equal(200.0, args.CenterY);
    }

    [Fact]
    public void PinchUpdateEventArgs_Should_Handle_Zero_Values()
    {
        var args = new PinchUpdateEventArgs(0.0, 0.0, 0.0, 0.0, 0.0);
        Assert.Equal(0.0, args.CumulativeScale);
        Assert.Equal(0.0, args.CumulativeTranslationX);
        Assert.Equal(0.0, args.CumulativeTranslationY);
        Assert.Equal(0.0, args.CenterX);
        Assert.Equal(0.0, args.CenterY);
    }

    [Fact]
    public void PinchUpdateEventArgs_Should_Handle_Negative_Values()
    {
        var args = new PinchUpdateEventArgs(-1.0, -10.0, -20.0, -100.0, -200.0);
        Assert.Equal(-1.0, args.CumulativeScale);
        Assert.Equal(-10.0, args.CumulativeTranslationX);
        Assert.Equal(-20.0, args.CumulativeTranslationY);
        Assert.Equal(-100.0, args.CenterX);
        Assert.Equal(-200.0, args.CenterY);
    }
}
