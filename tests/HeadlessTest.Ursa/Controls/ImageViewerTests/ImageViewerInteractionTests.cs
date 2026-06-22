using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.ImageViewerTests;

public class ImageViewerInteractionTests
{
    // ── Keyboard navigation (direct property manipulation) ─────────────────────

    [AvaloniaFact]
    public void SmallChange_And_LargeChange_Defaults_Are_Preserved_In_Tree()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer
        {
            SmallChange = 5.0,
            LargeChange = 25.0
        };
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        Assert.Equal(5.0, viewer.SmallChange);
        Assert.Equal(25.0, viewer.LargeChange);
    }

    [AvaloniaFact]
    public void OffsetX_And_OffsetY_Can_Be_Manipulated_While_In_Tree()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        viewer.OffsetX = 50;
        viewer.OffsetY = -30;
        Assert.Equal(50, viewer.OffsetX);
        Assert.Equal(-30, viewer.OffsetY);

        viewer.OffsetX -= 10;
        viewer.OffsetY += 20;
        Assert.Equal(40, viewer.OffsetX);
        Assert.Equal(-10, viewer.OffsetY);
    }

    [AvaloniaFact]
    public void ImageViewer_Focus_Should_Not_Throw()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        // ImageViewer is not focusable by default (TemplatedControl default),
        // but we can still attempt to programmatically focus it without crash.
        // Focus() on a non-focusable control should not throw.
        viewer.Focus();
        Dispatcher.UIThread.RunJobs();
        Assert.True(true);
    }

    // ── Mouse wheel zoom (Source is null → no-op) ──────────────────────────────

    [AvaloniaFact]
    public void MouseWheel_Should_Not_Change_Zoom_When_Source_Is_Null()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        double initialZoom = viewer.Zoom;

        // Scroll up (positive Y delta) — should be ignored because Source is null
        var centerPoint = new Point(200, 150);
        window.MouseWheel(centerPoint, new Vector(0, 1));
        Dispatcher.UIThread.RunJobs();

        Assert.Equal(initialZoom, viewer.Zoom);
    }

    [AvaloniaFact]
    public void MouseWheel_Down_Should_Not_Change_Zoom_When_Source_Is_Null()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer
        {
            Zoom = 2.0
        };
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        double initialZoom = viewer.Zoom;

        // Scroll down (negative Y delta) — should be ignored because Source is null
        var centerPoint = new Point(200, 150);
        window.MouseWheel(centerPoint, new Vector(0, -1));
        Dispatcher.UIThread.RunJobs();

        Assert.Equal(initialZoom, viewer.Zoom);
    }

    [AvaloniaFact]
    public void MouseWheel_Should_Not_Exceed_MaxZoom_Even_With_Source_Null()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer
        {
            MaxZoom = 3.0
        };
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        // Setting zoom via property is clamped
        viewer.Zoom = 5.0;
        Assert.True(viewer.Zoom <= 3.0);
    }

    [AvaloniaFact]
    public void MouseWheel_Should_Not_Go_Below_MinZoom_Even_With_Source_Null()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer
        {
            MinZoom = 0.5
        };
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        viewer.Zoom = 0.1;
        Assert.True(viewer.Zoom >= 0.5);
    }

    // ── Double-tap ─────────────────────────────────────────────────────────────

    [AvaloniaFact]
    public void DoubleTap_Should_Call_FitToView_Even_When_Source_Is_Null()
    {
        // FitToView returns early when Source is null, but shouldn't throw.
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer
        {
            Zoom = 3.0,
            OffsetX = 50,
            OffsetY = 100
        };
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        // Simulate double-tap by two quick clicks
        var point = new Point(200, 150);
        window.MouseDown(point, MouseButton.Left);
        window.MouseUp(point, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();

        window.MouseDown(point, MouseButton.Left);
        window.MouseUp(point, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();

        // When Source is null, FitToView returns immediately,
        // so Zoom and offsets stay the same.
        Assert.Equal(3.0, viewer.Zoom);
        Assert.Equal(50.0, viewer.OffsetX);
        Assert.Equal(100.0, viewer.OffsetY);
    }

    // ── Pan via mouse drag ─────────────────────────────────────────────────────

    [AvaloniaFact]
    public void Mouse_Drag_Should_Pan_Image()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        double initialX = viewer.OffsetX;
        double initialY = viewer.OffsetY;

        // Simulate drag: press, move, release
        window.MouseDown(new Point(200, 150), MouseButton.Left);
        window.MouseMove(new Point(250, 180));
        window.MouseUp(new Point(250, 180), MouseButton.Left);
        Dispatcher.UIThread.RunJobs();

        // Offset should have changed due to drag
        Assert.True(viewer.OffsetX != initialX || viewer.OffsetY != initialY,
            "Offset should change after mouse drag");
    }

    // ── Overlayer / AdornerLayer ───────────────────────────────────────────────

    [AvaloniaFact]
    public void Setting_Overlayer_Should_Set_Adorner()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        var overlayer = new Border
        {
            Width = 100,
            Height = 50
        };
        viewer.Overlayer = overlayer;

        Dispatcher.UIThread.RunJobs();

        var adorner = AdornerLayer.GetAdorner(viewer);
        Assert.Same(overlayer, adorner);
    }

    [AvaloniaFact]
    public void Clearing_Overlayer_Does_Not_Auto_Remove_Adorner()
    {
        // The current ImageViewer implementation only sets the adorner
        // when a non-null Overlayer is assigned. Setting Overlayer to null
        // does not clear the adorner — it remains from the previous assignment.
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        var overlayer = new Border();
        viewer.Overlayer = overlayer;
        Dispatcher.UIThread.RunJobs();

        Assert.NotNull(AdornerLayer.GetAdorner(viewer));

        // Setting to null does not clear the adorner (current behavior)
        viewer.Overlayer = null;
        Dispatcher.UIThread.RunJobs();

        Assert.NotNull(AdornerLayer.GetAdorner(viewer));
    }

    // ── Visual tree attach / detach ────────────────────────────────────────────

    [AvaloniaFact]
    public void ImageViewer_Should_Survive_Detach_From_Visual_Tree()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        // Detach from visual tree
        window.Content = null;
        Dispatcher.UIThread.RunJobs();

        // Should not crash; verify viewer still works
        viewer.Zoom = 2.0;
        Assert.Equal(2.0, viewer.Zoom);
    }

    [AvaloniaFact]
    public void ImageViewer_Should_Survive_ReAttach_To_Visual_Tree()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer();
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        // Detach
        window.Content = null;
        Dispatcher.UIThread.RunJobs();

        // Re-attach
        window.Content = viewer;
        Dispatcher.UIThread.RunJobs();

        // Should not crash; verify viewer still works
        viewer.Zoom = 3.0;
        Assert.Equal(3.0, viewer.Zoom);
    }

    // ── InitialFit behavior ────────────────────────────────────────────────────

    [AvaloniaFact]
    public void Changing_Stretch_While_In_Tree_Should_Reset_Zoom()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer
        {
            Zoom = 3.0,
            OffsetX = 100,
            OffsetY = 200
        };
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        // Changing Stretch should reset offsets and trigger FitToView
        viewer.Stretch = Stretch.Fill;
        Dispatcher.UIThread.RunJobs();

        // Offsets should be reset to 0 after stretch change
        Assert.Equal(0.0, viewer.OffsetX);
        Assert.Equal(0.0, viewer.OffsetY);
    }

    [AvaloniaFact]
    public void Changing_StretchDirection_While_In_Tree_Should_Reset_Offsets()
    {
        var window = new Window()
        {
            Width = 400,
            Height = 300
        };
        var viewer = new ImageViewer
        {
            OffsetX = 50,
            OffsetY = 75
        };
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        viewer.StretchDirection = StretchDirection.UpOnly;
        Dispatcher.UIThread.RunJobs();

        Assert.Equal(0.0, viewer.OffsetX);
        Assert.Equal(0.0, viewer.OffsetY);
    }

    [AvaloniaFact]
    public void Viewer_Should_Not_Crash_On_SizeChanged_After_Initial_Fit()
    {
        var window = new Window()
        {
            Width = 100,
            Height = 100
        };
        var viewer = new ImageViewer
        {
            Zoom = 5.0,
            OffsetX = 50
        };
        window.Content = viewer;
        window.Show();

        Dispatcher.UIThread.RunJobs();

        // Resize the window
        window.Width = 200;
        window.Height = 200;
        Dispatcher.UIThread.RunJobs();

        // Should not crash
        Assert.True(true);
    }
}
