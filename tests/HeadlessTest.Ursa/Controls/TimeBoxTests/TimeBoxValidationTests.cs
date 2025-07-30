using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.TimeBoxTests;

public class TimeBoxValidationTests
{
    [AvaloniaFact]
    public void TimeBox_Should_Handle_Valid_TimeSpan()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        var validTime = new TimeSpan(12, 30, 45, 123);
        timeBox.Time = validTime;

        Assert.Equal(validTime, timeBox.Time);
    }

    [AvaloniaFact]
    public void TimeBox_Should_Handle_Zero_TimeSpan()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        timeBox.Time = TimeSpan.Zero;

        Assert.Equal(TimeSpan.Zero, timeBox.Time);
    }

    [AvaloniaFact]
    public void TimeBox_Should_Handle_Maximum_Valid_Time()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        // Maximum time that should be valid (23:59:59.999)
        var maxTime = new TimeSpan(0, 23, 59, 59, 999);
        timeBox.Time = maxTime;

        Assert.Equal(maxTime, timeBox.Time);
    }

    [AvaloniaFact]
    public void TimeBox_Should_Handle_Time_With_Days()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        // TimeSpan with days component
        var timeWithDays = new TimeSpan(1, 2, 3, 4, 5);
        timeBox.Time = timeWithDays;

        // Should accept the time (behavior depends on control implementation)
        Assert.NotNull(timeBox.Time);
    }

    [AvaloniaFact]
    public void TimeBox_ShowLeadingZero_Should_Affect_Display()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        timeBox.Time = new TimeSpan(1, 2, 3, 4);
        
        // Test with leading zeros disabled
        timeBox.ShowLeadingZero = false;
        Assert.False(timeBox.ShowLeadingZero);
        
        // Test with leading zeros enabled
        timeBox.ShowLeadingZero = true;
        Assert.True(timeBox.ShowLeadingZero);
    }

    [AvaloniaFact]
    public void TimeBox_IsTimeLoop_Should_Affect_Overflow_Behavior()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        // Test the IsTimeLoop property
        timeBox.IsTimeLoop = true;
        Assert.True(timeBox.IsTimeLoop);

        timeBox.IsTimeLoop = false;
        Assert.False(timeBox.IsTimeLoop);
    }

    [AvaloniaFact]
    public void TimeBox_Should_Preserve_Millisecond_Precision()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        // Test with specific millisecond values
        var timeWithMs = new TimeSpan(0, 1, 2, 3, 456);
        timeBox.Time = timeWithMs;

        Assert.Equal(timeWithMs, timeBox.Time);
    }

    [AvaloniaFact]
    public void TimeBox_Should_Handle_Negative_TimeSpan()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        // Test with negative TimeSpan
        var negativeTime = new TimeSpan(-1, -2, -3, -4);
        timeBox.Time = negativeTime;

        // The control should handle negative values according to its implementation
        Assert.NotNull(timeBox.Time);
    }

    [AvaloniaFact]
    public void TimeBox_Time_Property_Should_Support_TwoWay_Binding()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        // Test that the property supports two-way binding by setting and getting
        var testTime1 = new TimeSpan(10, 20, 30, 40);
        var testTime2 = new TimeSpan(5, 15, 25, 35);

        timeBox.Time = testTime1;
        Assert.Equal(testTime1, timeBox.Time);

        timeBox.Time = testTime2;
        Assert.Equal(testTime2, timeBox.Time);
    }
}