using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using UrsaControls = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.TagInputTests;

public class TagInputPanelTests
{
    [AvaloniaFact]
    public void TagInputPanel_Should_Measure_Single_Child_Correctly()
    {
        // Arrange
        var window = new Window();
        var panel = new UrsaControls.TagInputPanel();
        var child = new Button { Content = "Test", Width = 100, Height = 30 };
        
        panel.Children.Add(child);
        window.Content = panel;
        window.Show();

        // Act
        var availableSize = new Size(200, double.PositiveInfinity);
        panel.Measure(availableSize);
        var measuredSize = panel.DesiredSize;

        // Assert
        Assert.True(measuredSize.Width > 0);
        Assert.True(measuredSize.Height > 0);
        Assert.True(measuredSize.Width <= availableSize.Width);
    }

    [AvaloniaFact]
    public void TagInputPanel_Should_Handle_Multiple_Children()
    {
        // Arrange
        var window = new Window();
        var panel = new UrsaControls.TagInputPanel();
        
        // Add multiple children (simulating tags + textbox)
        panel.Children.Add(new Button { Content = "Tag1", Width = 50, Height = 25 });
        panel.Children.Add(new Button { Content = "Tag2", Width = 50, Height = 25 });
        panel.Children.Add(new TextBox { Width = 100, Height = 25 }); // Last item (textbox)
        
        window.Content = panel;
        window.Show();

        // Act
        var availableSize = new Size(200, double.PositiveInfinity);
        panel.Measure(availableSize);
        var measuredSize = panel.DesiredSize;

        // Assert
        Assert.True(measuredSize.Width > 0);
        Assert.True(measuredSize.Height > 0);
        Assert.Equal(3, panel.Children.Count);
    }

    [AvaloniaFact]
    public void TagInputPanel_Should_Handle_Width_Overflow()
    {
        // Arrange
        var window = new Window();
        var panel = new UrsaControls.TagInputPanel();
        
        // Add children that exceed available width
        panel.Children.Add(new Button { Content = "VeryLongTag1", Width = 120, Height = 25 });
        panel.Children.Add(new Button { Content = "VeryLongTag2", Width = 120, Height = 25 });
        panel.Children.Add(new TextBox { Width = 100, Height = 25 });
        
        window.Content = panel;
        window.Show();

        // Act - Constrain width to force wrapping
        var availableSize = new Size(150, double.PositiveInfinity);
        panel.Measure(availableSize);
        var measuredSize = panel.DesiredSize;

        // Assert - Height should increase due to wrapping
        Assert.True(measuredSize.Height >= 50); // At least 2 rows worth of height
        Assert.True(measuredSize.Width <= availableSize.Width);
    }

    [AvaloniaFact]
    public void TagInputPanel_Should_Arrange_Children_In_Available_Space()
    {
        // Arrange
        var window = new Window();
        var panel = new UrsaControls.TagInputPanel();
        
        var child1 = new Button { Content = "Tag1", Width = 50, Height = 25 };
        var child2 = new Button { Content = "Tag2", Width = 50, Height = 25 };
        var textBox = new TextBox { Width = 80, Height = 25 };
        
        panel.Children.Add(child1);
        panel.Children.Add(child2);
        panel.Children.Add(textBox);
        
        window.Content = panel;
        window.Show();

        // Act
        var finalSize = new Size(200, 100);
        panel.Measure(finalSize);
        panel.Arrange(new Rect(finalSize));
        var arrangedSize = finalSize;

        // Assert
        Assert.Equal(finalSize.Width, arrangedSize.Width);
        Assert.True(arrangedSize.Height > 0);
        
        // Children should be arranged
        Assert.True(child1.Bounds.Width > 0);
        Assert.True(child2.Bounds.Width > 0);
        Assert.True(textBox.Bounds.Width > 0);
    }

    [AvaloniaFact]
    public void TagInputPanel_Should_Place_Last_Child_On_New_Line_When_Insufficient_Space()
    {
        // Arrange
        var window = new Window();
        var panel = new UrsaControls.TagInputPanel();
        
        // Add items that will fill most of the width
        panel.Children.Add(new Button { Content = "Tag1", Width = 80, Height = 25 });
        panel.Children.Add(new Button { Content = "Tag2", Width = 80, Height = 25 });
        panel.Children.Add(new TextBox { Width = 100, Height = 25 }); // Last item
        
        window.Content = panel;
        window.Show();

        // Act - Constrain width so last item doesn't fit
        var availableSize = new Size(180, double.PositiveInfinity); // Not enough for all items on one line
        panel.Measure(availableSize);
        var measuredSize = panel.DesiredSize;

        // Assert - Should wrap to new line
        Assert.True(measuredSize.Height >= 50); // Should be at least 2 rows high
    }

    [AvaloniaFact]
    public void TagInputPanel_Should_Handle_Single_Child()
    {
        // Arrange
        var window = new Window();
        var panel = new UrsaControls.TagInputPanel();
        
        // TagInputPanel expects at least one child (the TextBox)
        var textBox = new TextBox { Width = 100, Height = 25 };
        panel.Children.Add(textBox);
        
        window.Content = panel;
        window.Show();

        // Act & Assert - Should not throw
        var availableSize = new Size(200, 200);
        panel.Measure(availableSize);
        var measuredSize = panel.DesiredSize;
        
        // Should handle single child gracefully
        Assert.True(measuredSize.Width >= 0);
        Assert.True(measuredSize.Height >= 0);
        Assert.Single(panel.Children);
    }
}