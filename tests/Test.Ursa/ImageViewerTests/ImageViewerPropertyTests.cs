using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Ursa.Controls;

namespace Test.Ursa.Controls.ImageViewerTests;

public class ImageViewerPropertyTests
{
    // ── Default values ──────────────────────────────────────────────────────────

    [Fact]
    public void Default_Values_Should_Be_Correct()
    {
        var viewer = new ImageViewer();

        Assert.Null(viewer.Source);
        Assert.Equal(Stretch.Uniform, viewer.Stretch);
        Assert.Equal(StretchDirection.Both, viewer.StretchDirection);
        Assert.Equal(default(BitmapBlendingMode), viewer.BlendMode);
        Assert.Equal(1.0, viewer.Zoom);
        Assert.Equal(0.1, viewer.MinZoom);
        Assert.Equal(10.0, viewer.MaxZoom);
        Assert.Equal(0.0, viewer.OffsetX);
        Assert.Equal(0.0, viewer.OffsetY);
        Assert.Equal(1.0, viewer.SmallChange);
        Assert.Equal(10.0, viewer.LargeChange);
        Assert.Null(viewer.Overlayer);
    }

    // ── Zoom coercion ──────────────────────────────────────────────────────────

    [Fact]
    public void Zoom_Should_Be_Clamped_By_MinZoom_And_MaxZoom()
    {
        var viewer = new ImageViewer();

        viewer.Zoom = 100.0;
        Assert.Equal(10.0, viewer.Zoom);

        viewer.Zoom = -1.0;
        Assert.Equal(0.1, viewer.Zoom);

        viewer.Zoom = 5.0;
        Assert.Equal(5.0, viewer.Zoom);
    }

    [Fact]
    public void Zoom_Should_ReCoerce_When_MinZoom_Changes_Above_Current()
    {
        var viewer = new ImageViewer();
        viewer.Zoom = 0.5;
        viewer.MinZoom = 2.0;
        Assert.Equal(2.0, viewer.Zoom);
    }

    [Fact]
    public void Zoom_Should_ReCoerce_When_MaxZoom_Changes_Below_Current()
    {
        var viewer = new ImageViewer();
        viewer.Zoom = 8.0;
        viewer.MaxZoom = 5.0;
        Assert.Equal(5.0, viewer.Zoom);
    }

    [Fact]
    public void Zoom_Should_ReCoerce_When_MaxZoom_Reduced_Within_Valid_Range()
    {
        // Must keep MinZoom ≤ MaxZoom at all times.
        var viewer = new ImageViewer();
        viewer.MinZoom = 0.5;
        viewer.MaxZoom = 5.0;
        viewer.Zoom = 3.0;

        // Reduce MaxZoom below current Zoom but still ≥ MinZoom
        viewer.MaxZoom = 2.0;
        Assert.Equal(2.0, viewer.Zoom);
    }

    // ── Source / Stretch / StretchDirection change side effects ────────────────

    [Fact]
    public void Setting_Source_When_Already_Null_Does_Not_Reset_Offsets()
    {
        // Setting Source to null when it's already null does not trigger
        // OnPropertyChanged because the effective value hasn't changed.
        // The reset behavior only occurs when Source actually changes to a new value.
        var viewer = new ImageViewer();
        viewer.OffsetX = 100;
        viewer.OffsetY = 200;

        viewer.Source = null; // Source is already null, no change event
        Assert.Equal(100.0, viewer.OffsetX);
        Assert.Equal(200.0, viewer.OffsetY);
    }

    [Fact]
    public void Setting_Stretch_Should_Reset_Offsets()
    {
        var viewer = new ImageViewer();
        viewer.OffsetX = 50;
        viewer.OffsetY = 75;

        viewer.Stretch = Stretch.Fill;
        Assert.Equal(0.0, viewer.OffsetX);
        Assert.Equal(0.0, viewer.OffsetY);
    }

    [Fact]
    public void Setting_StretchDirection_Should_Reset_Offsets()
    {
        var viewer = new ImageViewer();
        viewer.OffsetX = 30;
        viewer.OffsetY = 40;

        viewer.StretchDirection = StretchDirection.DownOnly;
        Assert.Equal(0.0, viewer.OffsetX);
        Assert.Equal(0.0, viewer.OffsetY);
    }

    // ── Obsolete property mapping ──────────────────────────────────────────────

    [Fact]
    public void Scale_Should_Map_To_Zoom()
    {
        var viewer = new ImageViewer();

        viewer.Scale = 3.0;
        Assert.Equal(3.0, viewer.Zoom);
        Assert.Equal(3.0, viewer.Scale);

        viewer.Zoom = 5.0;
        Assert.Equal(5.0, viewer.Scale);
    }

    [Fact]
    public void MinScale_Should_Map_To_MinZoom()
    {
        var viewer = new ImageViewer();

        viewer.MinScale = 0.5;
        Assert.Equal(0.5, viewer.MinZoom);
        Assert.Equal(0.5, viewer.MinScale);

        viewer.MinZoom = 2.0;
        Assert.Equal(2.0, viewer.MinScale);
    }

    [Fact]
    public void TranslateX_Should_Map_To_OffsetX()
    {
        var viewer = new ImageViewer();

        viewer.TranslateX = 50.0;
        Assert.Equal(50.0, viewer.OffsetX);
        Assert.Equal(50.0, viewer.TranslateX);

        viewer.OffsetX = -10.0;
        Assert.Equal(-10.0, viewer.TranslateX);
    }

    [Fact]
    public void TranslateY_Should_Map_To_OffsetY()
    {
        var viewer = new ImageViewer();

        viewer.TranslateY = 75.0;
        Assert.Equal(75.0, viewer.OffsetY);
        Assert.Equal(75.0, viewer.TranslateY);

        viewer.OffsetY = -20.0;
        Assert.Equal(-20.0, viewer.TranslateY);
    }

    [Fact]
    public void Obsolete_Scale_Should_Still_Be_Clamped()
    {
        var viewer = new ImageViewer();

        viewer.Scale = 100.0;
        Assert.Equal(10.0, viewer.Scale);

        viewer.Scale = -1.0;
        Assert.Equal(0.1, viewer.Scale);
    }

    // ── SmallChange / LargeChange ──────────────────────────────────────────────

    [Fact]
    public void SmallChange_Default_Should_Be_One()
    {
        var viewer = new ImageViewer();
        Assert.Equal(1.0, viewer.SmallChange);
    }

    [Fact]
    public void SmallChange_Should_Be_Settable()
    {
        var viewer = new ImageViewer();
        viewer.SmallChange = 5.0;
        Assert.Equal(5.0, viewer.SmallChange);
    }

    [Fact]
    public void LargeChange_Default_Should_Be_Ten()
    {
        var viewer = new ImageViewer();
        Assert.Equal(10.0, viewer.LargeChange);
    }

    [Fact]
    public void LargeChange_Should_Be_Settable()
    {
        var viewer = new ImageViewer();
        viewer.LargeChange = 20.0;
        Assert.Equal(20.0, viewer.LargeChange);
    }

    // ── Overlayer property ─────────────────────────────────────────────────────

    [Fact]
    public void Overlayer_Should_Be_Null_By_Default()
    {
        var viewer = new ImageViewer();
        Assert.Null(viewer.Overlayer);
    }

    [Fact]
    public void Overlayer_Should_Be_Settable()
    {
        var viewer = new ImageViewer();
        var border = new Border();
        viewer.Overlayer = border;
        Assert.Same(border, viewer.Overlayer);
    }

    [Fact]
    public void Overlayer_Should_Be_Clearable()
    {
        var viewer = new ImageViewer();
        var border = new Border();
        viewer.Overlayer = border;
        viewer.Overlayer = null;
        Assert.Null(viewer.Overlayer);
    }

    // ── Stretch property edge cases ────────────────────────────────────────────

    [Fact]
    public void Stretch_All_Values_Should_Be_Settable()
    {
        var viewer = new ImageViewer();

        viewer.Stretch = Stretch.None;
        Assert.Equal(Stretch.None, viewer.Stretch);

        viewer.Stretch = Stretch.Fill;
        Assert.Equal(Stretch.Fill, viewer.Stretch);

        viewer.Stretch = Stretch.Uniform;
        Assert.Equal(Stretch.Uniform, viewer.Stretch);

        viewer.Stretch = Stretch.UniformToFill;
        Assert.Equal(Stretch.UniformToFill, viewer.Stretch);
    }

    // ── StretchDirection edge cases ────────────────────────────────────────────

    [Fact]
    public void StretchDirection_All_Values_Should_Be_Settable()
    {
        var viewer = new ImageViewer();

        viewer.StretchDirection = StretchDirection.Both;
        Assert.Equal(StretchDirection.Both, viewer.StretchDirection);

        viewer.StretchDirection = StretchDirection.UpOnly;
        Assert.Equal(StretchDirection.UpOnly, viewer.StretchDirection);

        viewer.StretchDirection = StretchDirection.DownOnly;
        Assert.Equal(StretchDirection.DownOnly, viewer.StretchDirection);
    }

    // ── Offset property range ──────────────────────────────────────────────────

    [Fact]
    public void OffsetX_Should_Accept_Negative_Values()
    {
        var viewer = new ImageViewer();
        viewer.OffsetX = -500.0;
        Assert.Equal(-500.0, viewer.OffsetX);
    }

    [Fact]
    public void OffsetY_Should_Accept_Negative_Values()
    {
        var viewer = new ImageViewer();
        viewer.OffsetY = -500.0;
        Assert.Equal(-500.0, viewer.OffsetY);
    }

    [Fact]
    public void OffsetX_Should_Accept_Large_Values()
    {
        var viewer = new ImageViewer();
        viewer.OffsetX = 10000.0;
        Assert.Equal(10000.0, viewer.OffsetX);
    }

    [Fact]
    public void OffsetY_Should_Accept_Large_Values()
    {
        var viewer = new ImageViewer();
        viewer.OffsetY = 10000.0;
        Assert.Equal(10000.0, viewer.OffsetY);
    }

    // ── BlendMode ──────────────────────────────────────────────────────────────

    [Fact]
    public void BlendMode_Should_Be_Settable()
    {
        var viewer = new ImageViewer();
        viewer.BlendMode = BitmapBlendingMode.Plus;
        Assert.Equal(BitmapBlendingMode.Plus, viewer.BlendMode);

        viewer.BlendMode = BitmapBlendingMode.Screen;
        Assert.Equal(BitmapBlendingMode.Screen, viewer.BlendMode);
    }

    // ── CoerceZoom edge cases ──────────────────────────────────────────────────

    [Fact]
    public void CoerceZoom_Should_Work_When_MinZoom_Equals_MaxZoom()
    {
        var viewer = new ImageViewer();
        viewer.MinZoom = 3.0;
        viewer.MaxZoom = 3.0;
        viewer.Zoom = 5.0;
        Assert.Equal(3.0, viewer.Zoom);

        viewer.Zoom = 1.0;
        Assert.Equal(3.0, viewer.Zoom);
    }

    [Fact]
    public void CoerceZoom_Should_Work_With_Floating_Point_Precision()
    {
        var viewer = new ImageViewer();
        viewer.MinZoom = 0.1;
        viewer.MaxZoom = 10.0;
        viewer.Zoom = 1.000000001;
        Assert.Equal(1.000000001, viewer.Zoom);
    }

    [Fact]
    public void Zoom_Should_Not_Accept_Negative_Values_Below_MinZoom()
    {
        var viewer = new ImageViewer();
        viewer.MinZoom = 0.1;
        viewer.Zoom = -5.0;
        Assert.Equal(0.1, viewer.Zoom);
    }

    // ── FitToView parameter validation ─────────────────────────────────────────

    [Fact]
    public void FitToView_Should_Not_Throw_When_Source_Is_Null()
    {
        var viewer = new ImageViewer();
        viewer.FitToView(); // Should not throw
    }

    // ── Static constants ───────────────────────────────────────────────────────

    [Fact]
    public void Static_Part_Names_Should_Be_Defined()
    {
        Assert.Equal("PART_Image", ImageViewer.PART_Image);
        Assert.Equal("PART_Layer", ImageViewer.PART_Layer);
        Assert.Equal(":moving", ImageViewer.PC_Moving);
    }
}
