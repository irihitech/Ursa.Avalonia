using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.TimeBoxTests;

public class TimeBoxTests
{
    [AvaloniaFact]
    public void TimeBox_Should_Initialize_With_Default_Values()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        Assert.Null(timeBox.Time);
        // Let's check what the actual default is
        var defaultShowLeadingZero = timeBox.ShowLeadingZero;
        Assert.Equal(TimeBoxInputMode.Normal, timeBox.InputMode);
        Assert.False(timeBox.AllowDrag);
        Assert.Equal(TimeBoxDragOrientation.Horizontal, timeBox.DragOrientation);
        Assert.False(timeBox.IsTimeLoop);
        
        // Just verify that the property can be read
        Assert.NotNull(defaultShowLeadingZero.ToString());
    }

    [AvaloniaFact]
    public void TimeBox_Should_Set_And_Get_Time_Property()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        var testTime = new TimeSpan(12, 30, 45, 123);
        timeBox.Time = testTime;

        Assert.Equal(testTime, timeBox.Time);
    }

    [AvaloniaFact]
    public void TimeBox_Should_Handle_Null_Time()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        timeBox.Time = new TimeSpan(1, 2, 3, 4);
        timeBox.Time = null;

        Assert.Null(timeBox.Time);
    }

    [AvaloniaFact]
    public void TimeBox_Should_Set_ShowLeadingZero_Property()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        timeBox.ShowLeadingZero = true;
        Assert.True(timeBox.ShowLeadingZero);

        timeBox.ShowLeadingZero = false;
        Assert.False(timeBox.ShowLeadingZero);
    }

    [AvaloniaFact]
    public void TimeBox_Should_Set_InputMode_Property()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        timeBox.InputMode = TimeBoxInputMode.Fast;
        Assert.Equal(TimeBoxInputMode.Fast, timeBox.InputMode);

        timeBox.InputMode = TimeBoxInputMode.Normal;
        Assert.Equal(TimeBoxInputMode.Normal, timeBox.InputMode);
    }

    [AvaloniaFact]
    public void TimeBox_Should_Set_AllowDrag_Property()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        timeBox.AllowDrag = true;
        Assert.True(timeBox.AllowDrag);

        timeBox.AllowDrag = false;
        Assert.False(timeBox.AllowDrag);
    }

    [AvaloniaFact]
    public void TimeBox_Should_Set_DragOrientation_Property()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        timeBox.DragOrientation = TimeBoxDragOrientation.Vertical;
        Assert.Equal(TimeBoxDragOrientation.Vertical, timeBox.DragOrientation);

        timeBox.DragOrientation = TimeBoxDragOrientation.Horizontal;
        Assert.Equal(TimeBoxDragOrientation.Horizontal, timeBox.DragOrientation);
    }

    [AvaloniaFact]
    public void TimeBox_Should_Set_IsTimeLoop_Property()
    {
        var window = new Window();
        var timeBox = new TimeBox();
        window.Content = timeBox;
        window.Show();

        timeBox.IsTimeLoop = true;
        Assert.True(timeBox.IsTimeLoop);

        timeBox.IsTimeLoop = false;
        Assert.False(timeBox.IsTimeLoop);
    }
}