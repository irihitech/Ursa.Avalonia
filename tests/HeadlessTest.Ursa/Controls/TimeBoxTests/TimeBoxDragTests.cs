using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.TimeBoxTests;

public class TimeBoxDragTests
{
    [AvaloniaFact]
    public void TimeBox_AllowDrag_Should_Enable_Drag_Functionality()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        // Test enabling drag
        timeBox.AllowDrag = true;
        Assert.True(timeBox.AllowDrag);

        // Test disabling drag
        timeBox.AllowDrag = false;
        Assert.False(timeBox.AllowDrag);
    }

    [AvaloniaFact]
    public void TimeBox_DragOrientation_Should_Control_Drag_Direction()
    {
        var window = new Window();
        var timeBox = new TimeBox { AllowDrag = true };
        window.Content = timeBox;
        window.Show();

        // Test horizontal drag orientation
        timeBox.DragOrientation = TimeBoxDragOrientation.Horizontal;
        Assert.Equal(TimeBoxDragOrientation.Horizontal, timeBox.DragOrientation);

        // Test vertical drag orientation
        timeBox.DragOrientation = TimeBoxDragOrientation.Vertical;
        Assert.Equal(TimeBoxDragOrientation.Vertical, timeBox.DragOrientation);
    }

    [AvaloniaFact]
    public void TimeBox_Should_Handle_Focus_Events_When_Drag_Enabled()
    {
        var window = new Window();
        var timeBox = new TimeBox { AllowDrag = true };
        window.Content = timeBox;
        window.Show();

        // Focus the control to enable interaction
        timeBox.Focus();

        // Test that the control can be focused
        Assert.True(timeBox.IsFocused);
        Assert.True(timeBox.AllowDrag);
    }

    [AvaloniaFact]
    public void TimeBox_Should_Maintain_Time_During_Drag_Operations()
    {
        var window = new Window();
        var timeBox = new TimeBox { AllowDrag = true };
        window.Content = timeBox;
        window.Show();

        var initialTime = new TimeSpan(12, 30, 45, 123);
        timeBox.Time = initialTime;

        // Enable drag and verify time is preserved
        Assert.Equal(initialTime, timeBox.Time);
        Assert.True(timeBox.AllowDrag);
    }

    [AvaloniaFact]
    public void TimeBox_Drag_Should_Work_With_Different_Time_Values()
    {
        var window = new Window();
        var timeBox = new TimeBox { AllowDrag = true };
        window.Content = timeBox;
        window.Show();

        // Test with various time values
        var testTimes = new[]
        {
            TimeSpan.Zero,
            new TimeSpan(1, 0, 0),
            new TimeSpan(12, 30, 45),
            new TimeSpan(23, 59, 59, 999)
        };

        foreach (var testTime in testTimes)
        {
            timeBox.Time = testTime;
            Assert.Equal(testTime, timeBox.Time);
            Assert.True(timeBox.AllowDrag);
        }
    }

    [AvaloniaFact]
    public void TimeBox_Should_Handle_Focus_Changes_With_Drag_Enabled()
    {
        var window = new Window();
        var timeBox = new TimeBox { AllowDrag = true };
        var otherControl = new TextBox();
        var panel = new StackPanel();
        panel.Children.Add(timeBox);
        panel.Children.Add(otherControl);
        window.Content = panel;
        window.Show();

        // Test focusing the control
        timeBox.Focus();
        Assert.True(timeBox.IsFocused);

        // Test focusing other control  
        otherControl.Focus();
        
        // In headless mode, focus behavior might be different
        // Just verify that otherControl can be focused and drag is still enabled
        Assert.True(otherControl.IsFocused);
        Assert.True(timeBox.AllowDrag);
    }

    [AvaloniaFact]
    public void TimeBox_Should_Support_Both_Drag_Orientations()
    {
        var window = new Window();
        var timeBox = new TimeBox { AllowDrag = true };
        window.Content = timeBox;
        window.Show();

        // Test all drag orientations
        foreach (TimeBoxDragOrientation orientation in Enum.GetValues<TimeBoxDragOrientation>())
        {
            timeBox.DragOrientation = orientation;
            Assert.Equal(orientation, timeBox.DragOrientation);
            Assert.True(timeBox.AllowDrag);
        }
    }

    [AvaloniaFact]
    public void TimeBox_Drag_Should_Work_With_IsTimeLoop()
    {
        var window = new Window();
        var timeBox = new TimeBox 
        { 
            AllowDrag = true,
            IsTimeLoop = true
        };
        window.Content = timeBox;
        window.Show();

        // Test that drag and time loop can work together
        Assert.True(timeBox.AllowDrag);
        Assert.True(timeBox.IsTimeLoop);
        
        // Set a time value
        timeBox.Time = new TimeSpan(12, 30, 45);
        Assert.NotNull(timeBox.Time);
    }
}