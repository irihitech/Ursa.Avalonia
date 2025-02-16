using Avalonia.Input;
using Irihi.Avalonia.Shared.Common;
using CalendarDayButton = Ursa.Controls.CalendarDayButton;

namespace Test.Ursa.DateTimePicker;

public class CalendarDayButtonTests
{
    [Fact]
    public void IsToday_SetsPseudoClass()
    {
        var button = new CalendarDayButton();
        button.IsToday = true;
        Assert.Contains(CalendarDayButton.PC_Today, button.Classes);
    }

    [Fact]
    public void IsStartDate_SetsPseudoClass()
    {
        var button = new CalendarDayButton();
        button.IsStartDate = true;
        Assert.Contains(CalendarDayButton.PC_StartDate, button.Classes);
    }

    [Fact]
    public void IsEndDate_SetsPseudoClass()
    {
        var button = new CalendarDayButton();
        button.IsEndDate = true;
        Assert.Contains(CalendarDayButton.PC_EndDate, button.Classes);
    }

    [Fact]
    public void IsPreviewStartDate_SetsPseudoClass()
    {
        var button = new CalendarDayButton();
        button.IsPreviewStartDate = true;
        Assert.Contains(CalendarDayButton.PC_PreviewStartDate, button.Classes);
    }

    [Fact]
    public void IsPreviewEndDate_SetsPseudoClass()
    {
        var button = new CalendarDayButton();
        button.IsPreviewEndDate = true;
        Assert.Contains(CalendarDayButton.PC_PreviewEndDate, button.Classes);
    }

    [Fact]
    public void IsInRange_SetsPseudoClass()
    {
        var button = new CalendarDayButton();
        button.IsInRange = true;
        Assert.Contains(CalendarDayButton.PC_InRange, button.Classes);
    }

    [Fact]
    public void IsSelected_SetsPseudoClass()
    {
        var button = new CalendarDayButton();
        button.IsSelected = true;
        Assert.Contains(PseudoClassName.PC_Selected, button.Classes);
    }

    [Fact]
    public void IsBlackout_SetsPseudoClass()
    {
        var button = new CalendarDayButton();
        button.IsBlackout = true;
        Assert.Contains(CalendarDayButton.PC_Blackout, button.Classes);
    }

    [Fact]
    public void IsNotCurrentMonth_SetsPseudoClass()
    {
        var button = new CalendarDayButton();
        button.IsNotCurrentMonth = true;
        Assert.Contains(CalendarDayButton.PC_NotCurrentMonth, button.Classes);
    }

    [Fact]
    public void ResetSelection_ClearsPseudoClasses()
    {
        var button = new CalendarDayButton();
        button.IsSelected = true;
        button.IsStartDate = true;
        button.ResetSelection();
        Assert.DoesNotContain(PseudoClassName.PC_Selected, button.Classes);
        Assert.DoesNotContain(CalendarDayButton.PC_StartDate, button.Classes);
    }
}