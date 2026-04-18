using Avalonia.Input;
using Irihi.Avalonia.Shared.Common;
using Ursa.Controls;

namespace Test.Ursa.DateTimePicker;

public class DatePickerCalendarDayButtonTests
{
    [Fact]
    public void IsToday_SetsPseudoClass()
    {
        var button = new DatePickerCalendarDayButton();
        button.IsToday = true;
        Assert.Contains(DatePickerCalendarDayButton.PC_Today, button.Classes);
        Assert.True(button.IsToday);
    }

    [Fact]
    public void IsStartDate_SetsPseudoClass()
    {
        var button = new DatePickerCalendarDayButton();
        button.IsStartDate = true;
        Assert.Contains(DatePickerCalendarDayButton.PC_StartDate, button.Classes);
        Assert.True(button.IsStartDate);
    }

    [Fact]
    public void IsEndDate_SetsPseudoClass()
    {
        var button = new DatePickerCalendarDayButton();
        button.IsEndDate = true;
        Assert.Contains(DatePickerCalendarDayButton.PC_EndDate, button.Classes);
        Assert.True(button.IsEndDate);
    }

    [Fact]
    public void IsPreviewStartDate_SetsPseudoClass()
    {
        var button = new DatePickerCalendarDayButton();
        button.IsPreviewStartDate = true;
        Assert.Contains(DatePickerCalendarDayButton.PC_PreviewStartDate, button.Classes);
        Assert.True(button.IsPreviewStartDate);
    }

    [Fact]
    public void IsPreviewEndDate_SetsPseudoClass()
    {
        var button = new DatePickerCalendarDayButton();
        button.IsPreviewEndDate = true;
        Assert.Contains(DatePickerCalendarDayButton.PC_PreviewEndDate, button.Classes);
        Assert.True(button.IsPreviewEndDate);
    }

    [Fact]
    public void IsInRange_SetsPseudoClass()
    {
        var button = new DatePickerCalendarDayButton();
        button.IsInRange = true;
        Assert.Contains(DatePickerCalendarDayButton.PC_InRange, button.Classes);
        Assert.True(button.IsInRange);
    }

    [Fact]
    public void IsSelected_SetsPseudoClass()
    {
        var button = new DatePickerCalendarDayButton();
        button.IsSelected = true;
        Assert.Contains(PseudoClassName.PC_Selected, button.Classes);
        Assert.True(button.IsSelected);
    }

    [Fact]
    public void IsBlackout_SetsPseudoClass()
    {
        var button = new DatePickerCalendarDayButton();
        button.IsBlackout = true;
        Assert.Contains(DatePickerCalendarDayButton.PC_Blackout, button.Classes);
        Assert.True(button.IsBlackout);
    }

    [Fact]
    public void IsNotCurrentMonth_SetsPseudoClass()
    {
        var button = new DatePickerCalendarDayButton();
        button.IsNotCurrentMonth = true;
        Assert.Contains(DatePickerCalendarDayButton.PC_NotCurrentMonth, button.Classes);
        Assert.True(button.IsNotCurrentMonth);
    }

    [Fact]
    public void ResetSelection_ClearsPseudoClasses()
    {
        var button = new DatePickerCalendarDayButton();
        button.IsSelected = true;
        button.IsStartDate = true;
        button.ResetSelection();
        Assert.DoesNotContain(PseudoClassName.PC_Selected, button.Classes);
        Assert.DoesNotContain(DatePickerCalendarDayButton.PC_StartDate, button.Classes);
    }
    
    [Fact]
    public void IsToday_ClearsOtherPseudoClasses()
    {
        var button = new DatePickerCalendarDayButton();
        button.IsStartDate = true;
        button.IsEndDate = true;
        button.IsToday = true;
        Assert.Contains(DatePickerCalendarDayButton.PC_Today, button.Classes);
        Assert.Contains(DatePickerCalendarDayButton.PC_EndDate, button.Classes);
        Assert.DoesNotContain(DatePickerCalendarDayButton.PC_StartDate, button.Classes);
    }
}