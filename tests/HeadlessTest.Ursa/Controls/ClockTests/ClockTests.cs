using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using HeadlessTest.Ursa.TestHelpers;
using UrsaControls = Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.ClockTests;

public class ClockTests
{
    [AvaloniaFact]
    public void Clock_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var clock = new UrsaControls.Clock();

        // Assert
        Assert.Equal(default(DateTime), clock.Time);
        Assert.True(clock.ShowHourTicks);
        Assert.True(clock.ShowMinuteTicks);
        Assert.Null(clock.HandBrush);
        Assert.True(clock.ShowHourHand);
        Assert.True(clock.ShowMinuteHand);
        Assert.True(clock.ShowSecondHand);
        Assert.False(clock.IsSmooth);
        Assert.Equal(0.0, clock.HourAngle);
        Assert.Equal(0.0, clock.MinuteAngle);
        Assert.Equal(0.0, clock.SecondAngle);
    }

    [AvaloniaFact]
    public void Clock_Should_Set_Time_Property()
    {
        // Arrange
        var window = new Window();
        var clock = new UrsaControls.Clock();
        var testTime = new DateTime(2023, 12, 25, 15, 30, 45);
        window.Content = clock;
        window.Show();

        // Act
        clock.Time = testTime;

        // Assert
        Assert.Equal(testTime, clock.Time);
    }

    [AvaloniaFact]
    public void Clock_Should_Calculate_Hour_Angle_Correctly()
    {
        // Arrange
        var window = new Window();
        var clock = new UrsaControls.Clock();
        var testTime = new DateTime(2023, 12, 25, 3, 0, 0); // 3:00
        window.Content = clock;
        window.Show();

        // Act
        clock.Time = testTime;

        // Assert - 3:00 should be 90 degrees (3 * 30 degrees per hour)
        Assert.Equal(90.0, clock.HourAngle);
    }

    [AvaloniaFact]
    public void Clock_Should_Calculate_Minute_Angle_Correctly()
    {
        // Arrange
        var window = new Window();
        var clock = new UrsaControls.Clock();
        var testTime = new DateTime(2023, 12, 25, 0, 15, 0); // 15 minutes
        window.Content = clock;
        window.Show();

        // Act
        clock.Time = testTime;

        // Assert - 15 minutes should be 90 degrees (15 * 6 degrees per minute)
        Assert.Equal(90.0, clock.MinuteAngle);
    }

    [AvaloniaFact]
    public void Clock_Should_Calculate_Second_Angle_Correctly()
    {
        // Arrange
        var window = new Window();
        var clock = new UrsaControls.Clock();
        var testTime = new DateTime(2023, 12, 25, 0, 0, 15); // 15 seconds
        window.Content = clock;
        window.Show();

        // Act
        clock.Time = testTime;

        // Assert - 15 seconds should be 90 degrees (15 * 6 degrees per second)
        Assert.Equal(90.0, clock.SecondAngle);
    }

    [AvaloniaFact]
    public void Clock_Should_Set_ShowHourTicks_Property()
    {
        // Arrange
        var window = new Window();
        var clock = new UrsaControls.Clock();
        window.Content = clock;
        window.Show();

        // Act
        clock.ShowHourTicks = false;

        // Assert
        Assert.False(clock.ShowHourTicks);
    }

    [AvaloniaFact]
    public void Clock_Should_Set_ShowMinuteTicks_Property()
    {
        // Arrange
        var window = new Window();
        var clock = new UrsaControls.Clock();
        window.Content = clock;
        window.Show();

        // Act
        clock.ShowMinuteTicks = false;

        // Assert
        Assert.False(clock.ShowMinuteTicks);
    }

    [AvaloniaFact]
    public void Clock_Should_Set_HandBrush_Property()
    {
        // Arrange
        var window = new Window();
        var clock = new UrsaControls.Clock();
        var brush = Brushes.Red;
        window.Content = clock;
        window.Show();

        // Act
        clock.HandBrush = brush;

        // Assert
        Assert.Equal(brush, clock.HandBrush);
    }

    [AvaloniaFact]
    public void Clock_Should_Set_ShowHourHand_Property()
    {
        // Arrange
        var window = new Window();
        var clock = new UrsaControls.Clock();
        window.Content = clock;
        window.Show();

        // Act
        clock.ShowHourHand = false;

        // Assert
        Assert.False(clock.ShowHourHand);
    }

    [AvaloniaFact]
    public void Clock_Should_Set_ShowMinuteHand_Property()
    {
        // Arrange
        var window = new Window();
        var clock = new UrsaControls.Clock();
        window.Content = clock;
        window.Show();

        // Act
        clock.ShowMinuteHand = false;

        // Assert
        Assert.False(clock.ShowMinuteHand);
    }

    [AvaloniaFact]
    public void Clock_Should_Set_ShowSecondHand_Property()
    {
        // Arrange
        var window = new Window();
        var clock = new UrsaControls.Clock();
        window.Content = clock;
        window.Show();

        // Act
        clock.ShowSecondHand = false;

        // Assert
        Assert.False(clock.ShowSecondHand);
    }

    [AvaloniaFact]
    public void Clock_Should_Set_IsSmooth_Property()
    {
        // Arrange
        var window = new Window();
        var clock = new UrsaControls.Clock();
        window.Content = clock;
        window.Show();

        // Act
        clock.IsSmooth = true;

        // Assert
        Assert.True(clock.IsSmooth);
    }

    [AvaloniaFact]
    public void Clock_Should_Be_Visible_When_Added_To_Window()
    {
        // Arrange
        var window = new Window();
        var clock = new UrsaControls.Clock();

        // Act
        window.Content = clock;
        window.Show();

        // Assert
        Assert.True(clock.IsVisible);
    }

    [AvaloniaFact]
    public void Clock_Should_Inherit_From_TemplatedControl()
    {
        // Arrange & Act
        var clock = new UrsaControls.Clock();

        // Assert
        Assert.IsAssignableFrom<TemplatedControl>(clock);
    }

    [AvaloniaFact]
    public void Clock_Should_Calculate_Complex_Time_Angles()
    {
        // Arrange
        var window = new Window();
        var clock = new UrsaControls.Clock();
        var testTime = new DateTime(2023, 12, 25, 6, 30, 30); // 6:30:30
        window.Content = clock;
        window.Show();

        // Act
        clock.Time = testTime;

        // Assert
        // Hour: 6.5 hours * 30 degrees = 195 degrees
        Assert.Equal(195.0, clock.HourAngle);
        // Minute: 30.5 minutes * 6 degrees = 183 degrees
        Assert.Equal(183.0, clock.MinuteAngle);
        // Second: 30 seconds * 6 degrees = 180 degrees
        Assert.Equal(180.0, clock.SecondAngle);
    }
}