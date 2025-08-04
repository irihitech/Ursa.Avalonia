using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using UrsaControls = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.AspectRatioLayoutItemTests;

public class AspectRatioLayoutItemTests
{
    [AvaloniaFact]
    public void AspectRatioLayoutItem_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var item = new UrsaControls.AspectRatioLayoutItem();

        // Assert
        Assert.Equal(UrsaControls.AspectRatioMode.None, item.AcceptAspectRatioMode);
        Assert.True(double.IsNaN(item.StartAspectRatioValue));
        Assert.True(double.IsNaN(item.EndAspectRatioValue));
        Assert.False(item.IsUseAspectRatioRange);
    }

    [AvaloniaFact]
    public void AspectRatioLayoutItem_Should_Set_AcceptAspectRatioMode_Property()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.AspectRatioLayoutItem();
        window.Content = item;
        window.Show();

        // Act
        item.AcceptAspectRatioMode = UrsaControls.AspectRatioMode.Square;

        // Assert
        Assert.Equal(UrsaControls.AspectRatioMode.Square, item.AcceptAspectRatioMode);
    }

    [AvaloniaFact]
    public void AspectRatioLayoutItem_Should_Set_StartAspectRatioValue_Property()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.AspectRatioLayoutItem();
        window.Content = item;
        window.Show();

        // Act
        item.StartAspectRatioValue = 1.5;

        // Assert
        Assert.Equal(1.5, item.StartAspectRatioValue);
    }

    [AvaloniaFact]
    public void AspectRatioLayoutItem_Should_Set_EndAspectRatioValue_Property()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.AspectRatioLayoutItem();
        window.Content = item;
        window.Show();

        // Act
        item.EndAspectRatioValue = 2.5;

        // Assert
        Assert.Equal(2.5, item.EndAspectRatioValue);
    }

    [AvaloniaFact]
    public void AspectRatioLayoutItem_Should_Calculate_IsUseAspectRatioRange_Correctly()
    {
        // Arrange
        var item = new UrsaControls.AspectRatioLayoutItem();

        // Act & Assert - Default values (NaN) should return false
        Assert.False(item.IsUseAspectRatioRange);

        // Act - Set only start value
        item.StartAspectRatioValue = 1.0;
        Assert.False(item.IsUseAspectRatioRange);

        // Act - Set only end value
        item.StartAspectRatioValue = double.NaN;
        item.EndAspectRatioValue = 2.0;
        Assert.False(item.IsUseAspectRatioRange);

        // Act - Set both values with start > end (invalid range)
        item.StartAspectRatioValue = 3.0;
        item.EndAspectRatioValue = 2.0;
        Assert.False(item.IsUseAspectRatioRange);

        // Act - Set valid range
        item.StartAspectRatioValue = 1.0;
        item.EndAspectRatioValue = 2.0;
        Assert.True(item.IsUseAspectRatioRange);
    }

    [AvaloniaFact]
    public void AspectRatioLayoutItem_Should_Support_Equal_Start_And_End_Values()
    {
        // Arrange
        var item = new UrsaControls.AspectRatioLayoutItem();

        // Act - Set equal start and end values
        item.StartAspectRatioValue = 1.5;
        item.EndAspectRatioValue = 1.5;

        // Assert
        Assert.True(item.IsUseAspectRatioRange);
    }

    [AvaloniaFact]
    public void AspectRatioLayoutItem_Should_Set_Content_Property()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.AspectRatioLayoutItem();
        var content = new Button { Content = "Test Content" };
        window.Content = item;
        window.Show();

        // Act
        item.Content = content;

        // Assert
        Assert.Equal(content, item.Content);
    }

    [AvaloniaFact]
    public void AspectRatioLayoutItem_Should_Be_Visible_When_Added_To_Window()
    {
        // Arrange
        var window = new Window();
        var item = new UrsaControls.AspectRatioLayoutItem();

        // Act
        window.Content = item;
        window.Show();

        // Assert
        Assert.True(item.IsVisible);
    }

    [AvaloniaFact]
    public void AspectRatioLayoutItem_Should_Inherit_From_ContentControl()
    {
        // Arrange & Act
        var item = new UrsaControls.AspectRatioLayoutItem();

        // Assert
        Assert.IsAssignableFrom<ContentControl>(item);
    }

    [AvaloniaFact]
    public void AspectRatioLayoutItem_Should_Handle_Zero_Values()
    {
        // Arrange
        var item = new UrsaControls.AspectRatioLayoutItem();

        // Act
        item.StartAspectRatioValue = 0.0;
        item.EndAspectRatioValue = 1.0;

        // Assert
        Assert.True(item.IsUseAspectRatioRange);
        Assert.Equal(0.0, item.StartAspectRatioValue);
        Assert.Equal(1.0, item.EndAspectRatioValue);
    }

    [AvaloniaFact]
    public void AspectRatioLayoutItem_Should_Handle_Negative_Values()
    {
        // Arrange
        var item = new UrsaControls.AspectRatioLayoutItem();

        // Act
        item.StartAspectRatioValue = -1.0;
        item.EndAspectRatioValue = 1.0;

        // Assert
        Assert.True(item.IsUseAspectRatioRange);
        Assert.Equal(-1.0, item.StartAspectRatioValue);
        Assert.Equal(1.0, item.EndAspectRatioValue);
    }

    [AvaloniaFact]
    public void AspectRatioLayoutItem_Should_Reset_To_NaN()
    {
        // Arrange
        var item = new UrsaControls.AspectRatioLayoutItem();
        item.StartAspectRatioValue = 1.0;
        item.EndAspectRatioValue = 2.0;

        // Act
        item.StartAspectRatioValue = double.NaN;

        // Assert
        Assert.False(item.IsUseAspectRatioRange);
        Assert.True(double.IsNaN(item.StartAspectRatioValue));
        Assert.Equal(2.0, item.EndAspectRatioValue);
    }
}