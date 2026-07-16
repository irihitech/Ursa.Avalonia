using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless.XUnit;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.VirtualizingUniformGridTests;

public class Tests
{
    // =================================================================
    //  Property defaults
    // =================================================================

    [AvaloniaFact]
    public void Default_Columns_Is_Four()
    {
        var sut = new VirtualizingUniformGrid();
        Assert.Equal(4, sut.Columns);
    }

    [AvaloniaFact]
    public void Default_ItemWidth_Is_NaN()
    {
        var sut = new VirtualizingUniformGrid();
        Assert.True(double.IsNaN(sut.ItemWidth));
    }

    [AvaloniaFact]
    public void Default_ItemHeight_Is_NaN()
    {
        var sut = new VirtualizingUniformGrid();
        Assert.True(double.IsNaN(sut.ItemHeight));
    }

    [AvaloniaFact]
    public void Default_CacheLength_Is_Half()
    {
        var sut = new VirtualizingUniformGrid();
        Assert.Equal(0.5, sut.CacheLength);
    }

    // =================================================================
    //  Property validation
    // =================================================================

    [AvaloniaFact]
    public void Columns_Must_Be_Positive()
    {
        var sut = new VirtualizingUniformGrid();
        Assert.Throws<ArgumentException>(() => sut.Columns = 0);
        Assert.Throws<ArgumentException>(() => sut.Columns = -1);
    }

    [AvaloniaFact]
    public void CacheLength_Must_Be_In_Range()
    {
        var sut = new VirtualizingUniformGrid();
        Assert.Throws<ArgumentException>(() => sut.CacheLength = -0.1);
        Assert.Throws<ArgumentException>(() => sut.CacheLength = 2.1);
        sut.CacheLength = 0;
        sut.CacheLength = 2;
    }

    // =================================================================
    //  Property setters
    // =================================================================

    [AvaloniaFact]
    public void Columns_Can_Be_Set()
    {
        var sut = new VirtualizingUniformGrid();
        sut.Columns = 6;
        Assert.Equal(6, sut.Columns);
    }

    [AvaloniaFact]
    public void ItemWidth_Can_Be_Set()
    {
        var sut = new VirtualizingUniformGrid();
        sut.ItemWidth = 120;
        Assert.Equal(120, sut.ItemWidth);
    }

    [AvaloniaFact]
    public void ItemHeight_Can_Be_Set()
    {
        var sut = new VirtualizingUniformGrid();
        sut.ItemHeight = 80;
        Assert.Equal(80, sut.ItemHeight);
    }

    [AvaloniaFact]
    public void CacheLength_Can_Be_Set()
    {
        var sut = new VirtualizingUniformGrid();
        sut.CacheLength = 1.0;
        Assert.Equal(1.0, sut.CacheLength);
    }

    // =================================================================
    //  FirstRealizedIndex / LastRealizedIndex
    // =================================================================

    [AvaloniaFact]
    public void RealizedIndexes_Default_To_MinusOne()
    {
        var sut = new VirtualizingUniformGrid();
        Assert.Equal(-1, sut.FirstRealizedIndex);
        Assert.Equal(-1, sut.LastRealizedIndex);
    }

    // =================================================================
    //  Measure — standalone (no ItemsControl)
    // =================================================================

    [AvaloniaFact]
    public void Measure_With_No_Children_Returns_Zero_Height()
    {
        var sut = new VirtualizingUniformGrid { Columns = 4 };
        sut.Measure(new Size(400, 300));
        Assert.Equal(0, sut.DesiredSize.Height);
    }

    [AvaloniaFact]
    public void Measure_With_Finite_Width_Uses_AvailableWidth()
    {
        var sut = new VirtualizingUniformGrid { Columns = 2 };
        sut.Children.Add(new Border());
        sut.Measure(new Size(600, 400));
        Assert.Equal(600, sut.DesiredSize.Width);
    }

    [AvaloniaFact]
    public void Measure_With_Fixed_ItemWidth_Uses_It()
    {
        var sut = new VirtualizingUniformGrid { Columns = 3, ItemWidth = 50 };
        sut.Children.Add(new Border());
        sut.Children.Add(new Border());
        sut.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        // 2 items / 3 cols = 1 row. cellW = 50, cellH = 50 (auto). total = 3*50 wide, 1*50 high.
        Assert.Equal(150, sut.DesiredSize.Width);
        Assert.Equal(50, sut.DesiredSize.Height);
    }

    [AvaloniaFact]
    public void Measure_With_Fixed_ItemHeight_Uses_It()
    {
        var sut = new VirtualizingUniformGrid { Columns = 2, ItemHeight = 60 };
        for (int i = 0; i < 4; i++)
            sut.Children.Add(new Border());
        sut.Measure(new Size(300, double.PositiveInfinity));
        // 4 items / 2 cols = 2 rows. cellH = 60. total height = 120.
        Assert.Equal(120, sut.DesiredSize.Height);
    }

    [AvaloniaFact]
    public void Measure_With_Both_ItemWidth_And_ItemHeight_Uses_Both()
    {
        var sut = new VirtualizingUniformGrid { Columns = 2, ItemWidth = 80, ItemHeight = 40 };
        sut.Children.Add(new Border());
        sut.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        Assert.Equal(160, sut.DesiredSize.Width);  // 2 cols * 80
        Assert.Equal(40, sut.DesiredSize.Height);   // 1 row * 40
    }

    [AvaloniaFact]
    public void Measure_Zero_Items_Zero_Height()
    {
        var sut = new VirtualizingUniformGrid { Columns = 5 };
        sut.Measure(new Size(500, 500));
        Assert.Equal(0, sut.DesiredSize.Height);
    }

    // =================================================================
    //  Arrange — standalone
    // =================================================================

    [AvaloniaFact]
    public void Arrange_No_Children_Completes()
    {
        var sut = new VirtualizingUniformGrid { Columns = 3 };
        sut.Measure(new Size(300, 300));
        sut.Arrange(new Rect(0, 0, 300, 300));
        // Should not throw.
    }

    [AvaloniaFact]
    public void Arrange_Children_Positioned_In_Grid()
    {
        var sut = new VirtualizingUniformGrid { Columns = 2 };
        var b0 = new Border();
        var b1 = new Border();
        var b2 = new Border();
        sut.Children.Add(b0);
        sut.Children.Add(b1);
        sut.Children.Add(b2);

        sut.Measure(new Size(200, 200));
        sut.Arrange(new Rect(0, 0, 200, 200));

        // 3 items / 2 cols = 2 rows.
        // cellW = 100, cellH = 100.
        // b0 at (0, 0), b1 at (100, 0), b2 at (0, 100).
        Assert.Equal(new Rect(0, 0, 100, 100), b0.Bounds);
        Assert.Equal(new Rect(100, 0, 100, 100), b1.Bounds);
        Assert.Equal(new Rect(0, 100, 100, 100), b2.Bounds);
    }

    [AvaloniaFact]
    public void Arrange_With_Fixed_ItemWidth_ItemHeight()
    {
        var sut = new VirtualizingUniformGrid { Columns = 3, ItemWidth = 50, ItemHeight = 30 };
        var b = new Border();
        sut.Children.Add(b);
        sut.Children.Add(new Border());
        sut.Children.Add(new Border());
        sut.Children.Add(new Border()); // 4 items = 2 rows

        sut.Measure(new Size(300, 200));
        sut.Arrange(new Rect(0, 0, 300, 200));

        // cellW = 50, cellH = 30. b at (0, 0, 50, 30).
        Assert.Equal(new Rect(0, 0, 50, 30), b.Bounds);
    }

    // =================================================================
    //  IScrollSnapPointsInfo
    // =================================================================

    [AvaloniaFact]
    public void Snap_Points_Are_Regular()
    {
        var sut = new VirtualizingUniformGrid();
        Assert.True(sut.AreHorizontalSnapPointsRegular);
        Assert.True(sut.AreVerticalSnapPointsRegular);
    }

    [AvaloniaFact]
    public void Irregular_Snap_Points_Is_Empty()
    {
        var sut = new VirtualizingUniformGrid();
        var points = sut.GetIrregularSnapPoints(
            Orientation.Vertical,
            SnapPointsAlignment.Near);
        Assert.Empty(points);
    }

    [AvaloniaFact]
    public void RegularSnapPoints_Vertical_Returns_CellHeight()
    {
        var sut = new VirtualizingUniformGrid { Columns = 4, ItemHeight = 80 };
        sut.Children.Add(new Border());
        sut.Measure(new Size(400, 400));

        var snap = sut.GetRegularSnapPoints(Orientation.Vertical, SnapPointsAlignment.Near, out var offset);
        Assert.Equal(80, snap);
        Assert.Equal(0, offset);
    }

    [AvaloniaFact]
    public void RegularSnapPoints_Horizontal_Returns_CellWidth()
    {
        var sut = new VirtualizingUniformGrid { Columns = 4, ItemWidth = 75 };
        sut.Children.Add(new Border());
        sut.Measure(new Size(400, 400));

        var snap = sut.GetRegularSnapPoints(Orientation.Horizontal, SnapPointsAlignment.Near, out var offset);
        Assert.Equal(75, snap);
        Assert.Equal(0, offset);
    }

    [AvaloniaFact]
    public void RegularSnapPoints_Defaults_When_No_Measure()
    {
        var sut = new VirtualizingUniformGrid();
        // Before any measure pass, cell dimensions default to 100.
        var snap = sut.GetRegularSnapPoints(Orientation.Vertical, SnapPointsAlignment.Near, out var offset);
        Assert.Equal(100, snap);
    }

    // =================================================================
    //  Snap-point events (add/remove handlers — smoke test)
    // =================================================================

    [AvaloniaFact]
    public void HorizontalSnapPointsChanged_AddRemove_NoException()
    {
        var sut = new VirtualizingUniformGrid();
        void Handler(object? s, RoutedEventArgs e) { }
        sut.HorizontalSnapPointsChanged += Handler;
        sut.HorizontalSnapPointsChanged -= Handler;
    }

    [AvaloniaFact]
    public void VerticalSnapPointsChanged_AddRemove_NoException()
    {
        var sut = new VirtualizingUniformGrid();
        void Handler(object? s, RoutedEventArgs e) { }
        sut.VerticalSnapPointsChanged += Handler;
        sut.VerticalSnapPointsChanged -= Handler;
    }

    // =================================================================
    //  Edge cases
    // =================================================================

    [AvaloniaFact]
    public void Columns_One_Works()
    {
        var sut = new VirtualizingUniformGrid { Columns = 1 };
        for (int i = 0; i < 5; i++)
            sut.Children.Add(new Border());
        sut.Measure(new Size(200, double.PositiveInfinity));
        // 5 items / 1 col = 5 rows. cellW = 200, cellH = 200.
        Assert.Equal(200 * 5, sut.DesiredSize.Height);
    }

    [AvaloniaFact]
    public void Single_Item_Fills_One_Cell()
    {
        var sut = new VirtualizingUniformGrid { Columns = 3 };
        sut.Children.Add(new Border());
        sut.Measure(new Size(300, 300));
        // 1 item / 3 cols = 1 row. cellW = 100, cellH = 100.
        Assert.Equal(300, sut.DesiredSize.Width);
        Assert.Equal(100, sut.DesiredSize.Height);
    }

    [AvaloniaFact]
    public void Partial_Last_Row_Does_Not_Affect_Cell_Size()
    {
        var sut = new VirtualizingUniformGrid { Columns = 3 };
        for (int i = 0; i < 5; i++)
            sut.Children.Add(new Border());
        sut.Measure(new Size(300, double.PositiveInfinity));
        // 5 items / 3 cols = 2 rows. cellW = 100, cellH = 100.
        Assert.Equal(200, sut.DesiredSize.Height); // 2 rows
    }

    [AvaloniaFact]
    public void Exactly_One_Row_Of_Items()
    {
        var sut = new VirtualizingUniformGrid { Columns = 5 };
        for (int i = 0; i < 5; i++)
            sut.Children.Add(new Border());
        sut.Measure(new Size(500, double.PositiveInfinity));
        Assert.Equal(100, sut.DesiredSize.Height); // 1 row, cellW=cellH=100
    }

    [AvaloniaFact]
    public void Large_Number_Of_Columns()
    {
        var sut = new VirtualizingUniformGrid { Columns = 20 };
        sut.Children.Add(new Border());
        sut.Measure(new Size(400, double.PositiveInfinity));
        // cellW = 400/20 = 20, cellH = 20. total = 20 high.
        Assert.Equal(400, sut.DesiredSize.Width);
        Assert.Equal(20, sut.DesiredSize.Height);
    }

    [AvaloniaFact]
    public void CacheLength_Property_Changed_Invalidates_Measure()
    {
        var sut = new VirtualizingUniformGrid();
        sut.Children.Add(new Border());
        sut.Measure(new Size(200, 200));
        Assert.True(sut.IsMeasureValid);

        sut.CacheLength = 2.0;
        // CacheLength change triggers InvalidateMeasure via class handler.
        Assert.False(sut.IsMeasureValid);
    }

    [AvaloniaFact]
    public void Columns_Property_Changed_Invalidates_Measure()
    {
        var sut = new VirtualizingUniformGrid { Columns = 2 };
        sut.Children.Add(new Border());
        sut.Measure(new Size(200, 200));
        Assert.True(sut.IsMeasureValid);

        sut.Columns = 3;
        Assert.False(sut.IsMeasureValid);
    }

    [AvaloniaFact]
    public void ItemWidth_Property_Changed_Invalidates_Measure()
    {
        var sut = new VirtualizingUniformGrid { Columns = 2 };
        sut.Children.Add(new Border());
        sut.Measure(new Size(200, 200));
        Assert.True(sut.IsMeasureValid);

        sut.ItemWidth = 50;
        Assert.False(sut.IsMeasureValid);
    }

    [AvaloniaFact]
    public void ItemHeight_Property_Changed_Invalidates_Measure()
    {
        var sut = new VirtualizingUniformGrid { Columns = 2 };
        sut.Children.Add(new Border());
        sut.Measure(new Size(200, 200));
        Assert.True(sut.IsMeasureValid);

        sut.ItemHeight = 80;
        Assert.False(sut.IsMeasureValid);
    }

    [AvaloniaFact]
    public void UniformItemHeight_Property_Changed_Invalidates_Measure()
    {
        var sut = new VirtualizingUniformGrid { Columns = 2 };
        sut.Children.Add(new Border());
        sut.Measure(new Size(200, 200));
        Assert.True(sut.IsMeasureValid);

        sut.UniformItemHeight = false;
        Assert.False(sut.IsMeasureValid);
    }

    [AvaloniaFact]
    public void Columns_Change_Affects_Layout()
    {
        var sut = new VirtualizingUniformGrid { Columns = 2 };
        var b0 = new Border();
        var b1 = new Border();
        var b2 = new Border();
        sut.Children.Add(b0);
        sut.Children.Add(b1);
        sut.Children.Add(b2);

        sut.Measure(new Size(300, 300));
        sut.Arrange(new Rect(0, 0, 300, 300));
        // 3 items / 2 cols = 2 rows. cellW=150, cellH=150.
        Assert.Equal(new Rect(0, 0, 150, 150), b0.Bounds);

        // Change columns to 3 and re-layout.
        sut.Columns = 3;
        sut.Measure(new Size(300, 300));
        sut.Arrange(new Rect(0, 0, 300, 300));
        // 3 items / 3 cols = 1 row. cellW=100, cellH=100.
        Assert.Equal(new Rect(0, 0, 100, 100), b0.Bounds);
        Assert.Equal(new Rect(100, 0, 100, 100), b1.Bounds);
        Assert.Equal(new Rect(200, 0, 100, 100), b2.Bounds);
    }

    // =================================================================
    //  Non-uniform item height (UniformItemHeight = false)
    // =================================================================

    [AvaloniaFact]
    public void UniformItemHeight_Default_Is_True()
    {
        var sut = new VirtualizingUniformGrid();
        Assert.True(sut.UniformItemHeight);
    }

    [AvaloniaFact]
    public void UniformItemHeight_Can_Be_Set_To_False()
    {
        var sut = new VirtualizingUniformGrid { UniformItemHeight = false };
        Assert.False(sut.UniformItemHeight);
    }

    [AvaloniaFact]
    public void NonUniform_Measure_Uses_Actual_Child_Height()
    {
        var sut = new VirtualizingUniformGrid { Columns = 2, UniformItemHeight = false };
        sut.Children.Add(new Border { MinHeight = 30, Width = 50, Child = new TextBlock { Text = "A" } });
        sut.Children.Add(new Border { MinHeight = 80, Width = 50, Child = new TextBlock { Text = "BB" } });
        sut.Children.Add(new Border { MinHeight = 20, Width = 50, Child = new TextBlock { Text = "C" } });

        sut.Measure(new Size(200, double.PositiveInfinity));
        // 3 items / 2 cols = 2 rows.
        // Row 0 max height = 80, Row 1 height = 20.
        Assert.True(sut.DesiredSize.Height > 90,
            $"Expected >90, got {sut.DesiredSize.Height}");
    }

    [AvaloniaFact]
    public void NonUniform_Arrange_Positions_At_Row_Offsets()
    {
        var sut = new VirtualizingUniformGrid { Columns = 2, UniformItemHeight = false };
        var b0 = new Border { MinHeight = 30, Child = new TextBlock { Text = "A" } };
        var b1 = new Border { MinHeight = 80, Child = new TextBlock { Text = "BB" } };
        var b2 = new Border { MinHeight = 20, Child = new TextBlock { Text = "C" } };
        sut.Children.Add(b0);
        sut.Children.Add(b1);
        sut.Children.Add(b2);

        sut.Measure(new Size(200, double.PositiveInfinity));
        sut.Arrange(new Rect(0, 0, 200, 200));

        // Row 0: (0,0,100,80) and (100,0,100,80). Row 1: (0,80,100,20).
        Assert.Equal(new Rect(0, 0, 100, 80), b0.Bounds);
        Assert.Equal(new Rect(100, 0, 100, 80), b1.Bounds);
        Assert.Equal(new Rect(0, 80, 100, 20), b2.Bounds);
    }

    [AvaloniaFact]
    public void IntegrityTests()
    {
        var window = new Window()
        {
            Height = 800,
            Width = 600,
        };
        
        var testView = new TestView();
        window.Content = testView;
        
        window.Show();
        
        Dispatcher.UIThread.RunJobs();

        var children = testView.list.GetLogicalChildren().ToList();
        
        Assert.True(children.Count < 100);
        
        testView.list.Scroll.Offset = new Vector(0, 1000);
        
        Dispatcher.UIThread.RunJobs();
        
        testView.list.Scroll.Offset = new Vector(0, 2000);
        
        Dispatcher.UIThread.RunJobs();
        
        testView.list.Scroll.Offset = new Vector(0, 3000);
        
        Dispatcher.UIThread.RunJobs();
        
        Assert.True(children.Count < 100);
        
    }
}
