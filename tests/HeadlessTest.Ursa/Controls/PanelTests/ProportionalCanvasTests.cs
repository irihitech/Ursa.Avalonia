using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Ursa.Controls;


namespace HeadlessTest.Ursa.Controls.PanelTests;

/// <summary>
/// Headless tests for <see cref="ProportionalCanvas"/>.
/// </summary>
public class ProportionalCanvasTests
{
    // ── Sentinel ─────────────────────────────────────────────────────────

    [AvaloniaFact]
    public void UnsetValue_Has_NaN_Scalar()
    {
        Assert.True(double.IsNaN(ProportionalCanvas.UnsetValue.Scalar));
    }

    // ── Default attached property values ──────────────────────────────────

    [AvaloniaFact]
    public void Default_Left_Is_Unset()
    {
        var child = new Border();
        var val = ProportionalCanvas.GetLeft(child);
        Assert.True(double.IsNaN(val.Scalar));
    }

    [AvaloniaFact]
    public void Default_Top_Is_Unset()
    {
        var child = new Border();
        var val = ProportionalCanvas.GetTop(child);
        Assert.True(double.IsNaN(val.Scalar));
    }

    [AvaloniaFact]
    public void Default_Right_Is_Unset()
    {
        var child = new Border();
        var val = ProportionalCanvas.GetRight(child);
        Assert.True(double.IsNaN(val.Scalar));
    }

    [AvaloniaFact]
    public void Default_Bottom_Is_Unset()
    {
        var child = new Border();
        var val = ProportionalCanvas.GetBottom(child);
        Assert.True(double.IsNaN(val.Scalar));
    }

    // ── Attached property round-trips ────────────────────────────────────

    [AvaloniaFact]
    public void Setting_Left_RoundTrips()
    {
        var child = new Border();
        var scalar = new RelativeScalar(0.5, RelativeUnit.Relative);
        ProportionalCanvas.SetLeft(child, scalar);
        var result = ProportionalCanvas.GetLeft(child);
        Assert.Equal(scalar.Scalar, result.Scalar);
        Assert.Equal(scalar.Unit, result.Unit);
    }

    [AvaloniaFact]
    public void Setting_Top_RoundTrips()
    {
        var child = new Border();
        var scalar = new RelativeScalar(100, RelativeUnit.Absolute);
        ProportionalCanvas.SetTop(child, scalar);
        var result = ProportionalCanvas.GetTop(child);
        Assert.Equal(scalar.Scalar, result.Scalar);
        Assert.Equal(scalar.Unit, result.Unit);
    }

    [AvaloniaFact]
    public void Setting_Right_RoundTrips()
    {
        var child = new Border();
        var scalar = new RelativeScalar(0.25, RelativeUnit.Relative);
        ProportionalCanvas.SetRight(child, scalar);
        var result = ProportionalCanvas.GetRight(child);
        Assert.Equal(scalar.Scalar, result.Scalar);
        Assert.Equal(scalar.Unit, result.Unit);
    }

    [AvaloniaFact]
    public void Setting_Bottom_RoundTrips()
    {
        var child = new Border();
        var scalar = new RelativeScalar(50, RelativeUnit.Absolute);
        ProportionalCanvas.SetBottom(child, scalar);
        var result = ProportionalCanvas.GetBottom(child);
        Assert.Equal(scalar.Scalar, result.Scalar);
        Assert.Equal(scalar.Unit, result.Unit);
    }

    // ── Layout: absolute positioning ─────────────────────────────────────

    [AvaloniaFact]
    public void Arrange_Absolute_Left_Top_Positions_Correctly()
    {
        var child = new Border { Width = 100, Height = 50 };
        ProportionalCanvas.SetLeft(child, new RelativeScalar(30, RelativeUnit.Absolute));
        ProportionalCanvas.SetTop(child, new RelativeScalar(40, RelativeUnit.Absolute));

        var sut = new ProportionalCanvas { Width = 400, Height = 300 };
        sut.Children.Add(child);

        sut.Measure(new Size(400, 300));
        sut.Arrange(new Rect(0, 0, 400, 300));

        Assert.Equal(30, child.Bounds.X, precision: 1);
        Assert.Equal(40, child.Bounds.Y, precision: 1);
        Assert.Equal(100, child.Bounds.Width, precision: 1);
        Assert.Equal(50, child.Bounds.Height, precision: 1);
    }

    // ── Layout: relative positioning ─────────────────────────────────────

    [AvaloniaFact]
    public void Arrange_Relative_Left_Top_Positions_Correctly()
    {
        var child = new Border { Width = 100, Height = 50 };
        ProportionalCanvas.SetLeft(child, new RelativeScalar(0.25, RelativeUnit.Relative));
        ProportionalCanvas.SetTop(child, new RelativeScalar(0.5, RelativeUnit.Relative));

        var sut = new ProportionalCanvas { Width = 400, Height = 300 };
        sut.Children.Add(child);

        sut.Measure(new Size(400, 300));
        sut.Arrange(new Rect(0, 0, 400, 300));

        // 0.25 * 400 = 100, 0.5 * 300 = 150
        Assert.Equal(100, child.Bounds.X, precision: 1);
        Assert.Equal(150, child.Bounds.Y, precision: 1);
    }

    [AvaloniaFact]
    public void Arrange_Relative_Right_Bottom_Positions_Correctly()
    {
        var child = new Border { Width = 100, Height = 50 };
        ProportionalCanvas.SetRight(child, new RelativeScalar(0.25, RelativeUnit.Relative));
        ProportionalCanvas.SetBottom(child, new RelativeScalar(0.1, RelativeUnit.Relative));

        var sut = new ProportionalCanvas { Width = 400, Height = 300 };
        sut.Children.Add(child);

        sut.Measure(new Size(400, 300));
        sut.Arrange(new Rect(0, 0, 400, 300));

        // Right edge = 0.25 * 400 = 100 → X = 400 - 100 - 100 = 200
        // Bottom edge = 0.1 * 300 = 30 → Y = 300 - 30 - 50 = 220
        Assert.Equal(200, child.Bounds.X, precision: 1);
        Assert.Equal(220, child.Bounds.Y, precision: 1);
    }

    // ── Layout: stretch with Left + Right ────────────────────────────────

    [AvaloniaFact]
    public void Arrange_Left_And_Right_Stretches_Width()
    {
        var child = new Border { Height = 50 };
        ProportionalCanvas.SetLeft(child, new RelativeScalar(0.1, RelativeUnit.Relative));
        ProportionalCanvas.SetRight(child, new RelativeScalar(0.2, RelativeUnit.Relative));
        ProportionalCanvas.SetTop(child, new RelativeScalar(30, RelativeUnit.Absolute));

        var sut = new ProportionalCanvas { Width = 400, Height = 300 };
        sut.Children.Add(child);

        sut.Measure(new Size(400, 300));
        sut.Arrange(new Rect(0, 0, 400, 300));

        // X = 0.1 * 400 = 40
        // Width = 400 - 40 - (0.2 * 400) = 400 - 40 - 80 = 280
        Assert.Equal(40, child.Bounds.X, precision: 1);
        Assert.Equal(280, child.Bounds.Width, precision: 1);
        Assert.Equal(30, child.Bounds.Y, precision: 1);
        Assert.Equal(50, child.Bounds.Height, precision: 1);
    }

    // ── Layout: stretch with Top + Bottom ────────────────────────────────

    [AvaloniaFact]
    public void Arrange_Top_And_Bottom_Stretches_Height()
    {
        var child = new Border { Width = 100 };
        ProportionalCanvas.SetLeft(child, new RelativeScalar(50, RelativeUnit.Absolute));
        ProportionalCanvas.SetTop(child, new RelativeScalar(0.1, RelativeUnit.Relative));
        ProportionalCanvas.SetBottom(child, new RelativeScalar(0.1, RelativeUnit.Relative));

        var sut = new ProportionalCanvas { Width = 400, Height = 300 };
        sut.Children.Add(child);

        sut.Measure(new Size(400, 300));
        sut.Arrange(new Rect(0, 0, 400, 300));

        // Y = 0.1 * 300 = 30
        // Height = 300 - 30 - (0.1 * 300) = 300 - 30 - 30 = 240
        Assert.Equal(30, child.Bounds.Y, precision: 1);
        Assert.Equal(240, child.Bounds.Height, precision: 1);
    }

    // ── Layout: all four sides (fill defined rectangle) ──────────────────

    [AvaloniaFact]
    public void Arrange_All_Four_Sides_Fills_Rectangle()
    {
        var child = new Border();
        ProportionalCanvas.SetLeft(child, new RelativeScalar(0.2, RelativeUnit.Relative));
        ProportionalCanvas.SetTop(child, new RelativeScalar(0.2, RelativeUnit.Relative));
        ProportionalCanvas.SetRight(child, new RelativeScalar(0.2, RelativeUnit.Relative));
        ProportionalCanvas.SetBottom(child, new RelativeScalar(0.2, RelativeUnit.Relative));

        var sut = new ProportionalCanvas { Width = 400, Height = 300 };
        sut.Children.Add(child);

        sut.Measure(new Size(400, 300));
        sut.Arrange(new Rect(0, 0, 400, 300));

        Assert.Equal(80, child.Bounds.X, precision: 1);
        Assert.Equal(60, child.Bounds.Y, precision: 1);
        Assert.Equal(240, child.Bounds.Width, precision: 1);
        Assert.Equal(180, child.Bounds.Height, precision: 1);
    }

    // ── Layout: unset properties default to 0 ────────────────────────────

    [AvaloniaFact]
    public void Arrange_Unset_Defaults_To_TopLeft_Corner()
    {
        var child = new Border { Width = 100, Height = 50 };

        var sut = new ProportionalCanvas { Width = 400, Height = 300 };
        sut.Children.Add(child);

        sut.Measure(new Size(400, 300));
        sut.Arrange(new Rect(0, 0, 400, 300));

        Assert.Equal(0, child.Bounds.X, precision: 1);
        Assert.Equal(0, child.Bounds.Y, precision: 1);
        Assert.Equal(100, child.Bounds.Width, precision: 1);
        Assert.Equal(50, child.Bounds.Height, precision: 1);
    }

    // ── Resize preserves relative positioning ────────────────────────────

    [AvaloniaFact]
    public void Resize_Preserves_Relative_Position()
    {
        var child = new Border { Width = 50, Height = 40 };
        ProportionalCanvas.SetLeft(child, new RelativeScalar(0.5, RelativeUnit.Relative));
        ProportionalCanvas.SetTop(child, new RelativeScalar(0.5, RelativeUnit.Relative));

        var sut = new ProportionalCanvas { Width = 200, Height = 200 };
        sut.Children.Add(child);

        sut.Measure(new Size(200, 200));
        sut.Arrange(new Rect(0, 0, 200, 200));

        // At 200×200: X = 100, Y = 100
        Assert.Equal(100, child.Bounds.X, precision: 1);
        Assert.Equal(100, child.Bounds.Y, precision: 1);

        // Resize to 400×300
        sut.Width = 400;
        sut.Height = 300;
        sut.Measure(new Size(400, 300));
        sut.Arrange(new Rect(0, 0, 400, 300));

        // At 400×300: X = 0.5*400 = 200, Y = 0.5*300 = 150
        Assert.Equal(200, child.Bounds.X, precision: 1);
        Assert.Equal(150, child.Bounds.Y, precision: 1);
    }

    [AvaloniaFact]
    public void Resize_Preserves_Absolute_Position()
    {
        var child = new Border { Width = 50, Height = 40 };
        ProportionalCanvas.SetLeft(child, new RelativeScalar(30, RelativeUnit.Absolute));
        ProportionalCanvas.SetTop(child, new RelativeScalar(20, RelativeUnit.Absolute));

        var sut = new ProportionalCanvas { Width = 200, Height = 200 };
        sut.Children.Add(child);

        sut.Measure(new Size(200, 200));
        sut.Arrange(new Rect(0, 0, 200, 200));

        Assert.Equal(30, child.Bounds.X, precision: 1);
        Assert.Equal(20, child.Bounds.Y, precision: 1);

        // Resize — absolute values should stay the same.
        sut.Width = 400;
        sut.Height = 300;
        sut.Measure(new Size(400, 300));
        sut.Arrange(new Rect(0, 0, 400, 300));

        Assert.Equal(30, child.Bounds.X, precision: 1);
        Assert.Equal(20, child.Bounds.Y, precision: 1);
    }

    [AvaloniaFact]
    public void Resize_Preserves_Stretch_When_Left_And_Right_Are_Relative()
    {
        var child = new Border { Height = 50 };
        ProportionalCanvas.SetLeft(child, new RelativeScalar(0.25, RelativeUnit.Relative));
        ProportionalCanvas.SetRight(child, new RelativeScalar(0.25, RelativeUnit.Relative));
        ProportionalCanvas.SetTop(child, new RelativeScalar(20, RelativeUnit.Absolute));

        var sut = new ProportionalCanvas { Width = 200, Height = 200 };
        sut.Children.Add(child);

        sut.Measure(new Size(200, 200));
        sut.Arrange(new Rect(0, 0, 200, 200));

        // Width = 200 - 50 - 50 = 100
        Assert.Equal(50, child.Bounds.X, precision: 1);
        Assert.Equal(100, child.Bounds.Width, precision: 1);

        // Resize to 400
        sut.Width = 400;
        sut.Measure(new Size(400, 200));
        sut.Arrange(new Rect(0, 0, 400, 200));

        // Width = 400 - 100 - 100 = 200
        Assert.Equal(100, child.Bounds.X, precision: 1);
        Assert.Equal(200, child.Bounds.Width, precision: 1);
    }

    // ── Measure: children are measured ───────────────────────────────────

    [AvaloniaFact]
    public void Measure_Measures_Children()
    {
        var child = new Border { Width = 100, Height = 50 };
        ProportionalCanvas.SetLeft(child, new RelativeScalar(10, RelativeUnit.Absolute));

        var sut = new ProportionalCanvas { Width = 400, Height = 300 };
        sut.Children.Add(child);

        sut.Measure(new Size(400, 300));

        // Child should have been measured.
        Assert.Equal(100, child.DesiredSize.Width, precision: 1);
        Assert.Equal(50, child.DesiredSize.Height, precision: 1);
    }

    // ── Measure: unconstrained sizing ────────────────────────────────────

    [AvaloniaFact]
    public void Measure_Unconstrained_Sizes_To_Children()
    {
        var child = new Border { Width = 200, Height = 100 };
        ProportionalCanvas.SetLeft(child, new RelativeScalar(30, RelativeUnit.Absolute));
        ProportionalCanvas.SetTop(child, new RelativeScalar(20, RelativeUnit.Absolute));

        var sut = new ProportionalCanvas();
        sut.Children.Add(child);

        sut.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

        // Desired size should accommodate the furthest child edge.
        // X offset 30 + width 200 = 230
        // Y offset 20 + height 100 = 120
        Assert.True(sut.DesiredSize.Width >= 230);
        Assert.True(sut.DesiredSize.Height >= 120);
    }

    // ── Layout: Right-anchored (only Right set) ──────────────────────────

    [AvaloniaFact]
    public void Arrange_Only_Right_Set_Anchors_To_Right_Edge()
    {
        var child = new Border { Width = 100, Height = 50 };
        ProportionalCanvas.SetRight(child, new RelativeScalar(30, RelativeUnit.Absolute));

        var sut = new ProportionalCanvas { Width = 400, Height = 300 };
        sut.Children.Add(child);

        sut.Measure(new Size(400, 300));
        sut.Arrange(new Rect(0, 0, 400, 300));

        // Right edge = 30 pixels → X = 400 - 30 - 100 = 270
        Assert.Equal(270, child.Bounds.X, precision: 1);
        Assert.Equal(0, child.Bounds.Y, precision: 1); // Top defaults to 0
    }

    // ── Layout: Bottom-anchored (only Bottom set) ────────────────────────

    [AvaloniaFact]
    public void Arrange_Only_Bottom_Set_Anchors_To_Bottom_Edge()
    {
        var child = new Border { Width = 100, Height = 50 };
        ProportionalCanvas.SetBottom(child, new RelativeScalar(20, RelativeUnit.Absolute));

        var sut = new ProportionalCanvas { Width = 400, Height = 300 };
        sut.Children.Add(child);

        sut.Measure(new Size(400, 300));
        sut.Arrange(new Rect(0, 0, 400, 300));

        // Bottom edge = 20 pixels → Y = 300 - 20 - 50 = 230
        Assert.Equal(0, child.Bounds.X, precision: 1); // Left defaults to 0
        Assert.Equal(230, child.Bounds.Y, precision: 1);
    }

    // ── Layout: mixed Relative and Absolute ──────────────────────────────

    [AvaloniaFact]
    public void Arrange_Mixed_Relative_Absolute_Calculates_Correctly()
    {
        var child = new Border { Width = 80, Height = 60 };
        ProportionalCanvas.SetLeft(child, new RelativeScalar(0.5, RelativeUnit.Relative));   // 50% of width
        ProportionalCanvas.SetTop(child, new RelativeScalar(40, RelativeUnit.Absolute));      // 40px

        var sut = new ProportionalCanvas { Width = 400, Height = 300 };
        sut.Children.Add(child);

        sut.Measure(new Size(400, 300));
        sut.Arrange(new Rect(0, 0, 400, 300));

        // X = 0.5 * 400 = 200
        // Y = 40
        Assert.Equal(200, child.Bounds.X, precision: 1);
        Assert.Equal(40, child.Bounds.Y, precision: 1);
    }

    // ── Multiple children ────────────────────────────────────────────────

    [AvaloniaFact]
    public void Arrange_Multiple_Children_Positions_Correctly()
    {
        var child1 = new Border { Width = 50, Height = 50 };
        ProportionalCanvas.SetLeft(child1, new RelativeScalar(0.1, RelativeUnit.Relative));
        ProportionalCanvas.SetTop(child1, new RelativeScalar(0.1, RelativeUnit.Relative));

        var child2 = new Border { Width = 50, Height = 50 };
        ProportionalCanvas.SetLeft(child2, new RelativeScalar(0.9, RelativeUnit.Relative));
        ProportionalCanvas.SetTop(child2, new RelativeScalar(0.9, RelativeUnit.Relative));

        var sut = new ProportionalCanvas { Width = 400, Height = 300 };
        sut.Children.Add(child1);
        sut.Children.Add(child2);

        sut.Measure(new Size(400, 300));
        sut.Arrange(new Rect(0, 0, 400, 300));

        // child1 at (40, 30) — top-left area
        Assert.Equal(40, child1.Bounds.X, precision: 1);
        Assert.Equal(30, child1.Bounds.Y, precision: 1);

        // child2 at (360, 270) — only Left/Top set
        Assert.Equal(360, child2.Bounds.X, precision: 1);
        Assert.Equal(270, child2.Bounds.Y, precision: 1);
    }
}
