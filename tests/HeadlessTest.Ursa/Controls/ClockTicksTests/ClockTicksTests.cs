using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using UrsaControls = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.ClockTicksTests;

public class ClockTicksTests
{
    [AvaloniaFact]
    public void ClockTicks_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var clockTicks = new UrsaControls.ClockTicks();

        // Assert
        Assert.True(clockTicks.ShowHourTicks);
        Assert.True(clockTicks.ShowMinuteTicks);
        Assert.Null(clockTicks.HourTickForeground);
        Assert.Null(clockTicks.MinuteTickForeground);
        Assert.Equal(10.0, clockTicks.HourTickLength);
        Assert.Equal(5.0, clockTicks.MinuteTickLength);
        Assert.Equal(2.0, clockTicks.HourTickWidth);
        Assert.Equal(1.0, clockTicks.MinuteTickWidth);
    }

    [AvaloniaFact]
    public void ClockTicks_Should_Set_ShowHourTicks_Property()
    {
        // Arrange
        var window = new Window();
        var clockTicks = new UrsaControls.ClockTicks();
        window.Content = clockTicks;
        window.Show();

        // Act
        clockTicks.ShowHourTicks = false;

        // Assert
        Assert.False(clockTicks.ShowHourTicks);
    }

    [AvaloniaFact]
    public void ClockTicks_Should_Set_ShowMinuteTicks_Property()
    {
        // Arrange
        var window = new Window();
        var clockTicks = new UrsaControls.ClockTicks();
        window.Content = clockTicks;
        window.Show();

        // Act
        clockTicks.ShowMinuteTicks = false;

        // Assert
        Assert.False(clockTicks.ShowMinuteTicks);
    }

    [AvaloniaFact]
    public void ClockTicks_Should_Set_HourTickForeground_Property()
    {
        // Arrange
        var window = new Window();
        var clockTicks = new UrsaControls.ClockTicks();
        var brush = Brushes.Red;
        window.Content = clockTicks;
        window.Show();

        // Act
        clockTicks.HourTickForeground = brush;

        // Assert
        Assert.Equal(brush, clockTicks.HourTickForeground);
    }

    [AvaloniaFact]
    public void ClockTicks_Should_Set_MinuteTickForeground_Property()
    {
        // Arrange
        var window = new Window();
        var clockTicks = new UrsaControls.ClockTicks();
        var brush = Brushes.Blue;
        window.Content = clockTicks;
        window.Show();

        // Act
        clockTicks.MinuteTickForeground = brush;

        // Assert
        Assert.Equal(brush, clockTicks.MinuteTickForeground);
    }

    [AvaloniaFact]
    public void ClockTicks_Should_Set_HourTickLength_Property()
    {
        // Arrange
        var window = new Window();
        var clockTicks = new UrsaControls.ClockTicks();
        window.Content = clockTicks;
        window.Show();

        // Act
        clockTicks.HourTickLength = 15.0;

        // Assert
        Assert.Equal(15.0, clockTicks.HourTickLength);
    }

    [AvaloniaFact]
    public void ClockTicks_Should_Set_MinuteTickLength_Property()
    {
        // Arrange
        var window = new Window();
        var clockTicks = new UrsaControls.ClockTicks();
        window.Content = clockTicks;
        window.Show();

        // Act
        clockTicks.MinuteTickLength = 8.0;

        // Assert
        Assert.Equal(8.0, clockTicks.MinuteTickLength);
    }

    [AvaloniaFact]
    public void ClockTicks_Should_Set_HourTickWidth_Property()
    {
        // Arrange
        var window = new Window();
        var clockTicks = new UrsaControls.ClockTicks();
        window.Content = clockTicks;
        window.Show();

        // Act
        clockTicks.HourTickWidth = 3.0;

        // Assert
        Assert.Equal(3.0, clockTicks.HourTickWidth);
    }

    [AvaloniaFact]
    public void ClockTicks_Should_Set_MinuteTickWidth_Property()
    {
        // Arrange
        var window = new Window();
        var clockTicks = new UrsaControls.ClockTicks();
        window.Content = clockTicks;
        window.Show();

        // Act
        clockTicks.MinuteTickWidth = 1.5;

        // Assert
        Assert.Equal(1.5, clockTicks.MinuteTickWidth);
    }

    [AvaloniaFact]
    public void ClockTicks_Should_Be_Visible_When_Added_To_Window()
    {
        // Arrange
        var window = new Window();
        var clockTicks = new UrsaControls.ClockTicks();

        // Act
        window.Content = clockTicks;
        window.Show();

        // Assert
        Assert.True(clockTicks.IsVisible);
    }

    [AvaloniaFact]
    public void ClockTicks_Should_Inherit_From_Control()
    {
        // Arrange & Act
        var clockTicks = new UrsaControls.ClockTicks();

        // Assert
        Assert.IsAssignableFrom<Control>(clockTicks);
    }
}