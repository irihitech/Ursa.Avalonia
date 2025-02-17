using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.DateTimePicker;

public class CalendarYearButtonTests
{
    [AvaloniaFact]
    public void SetContext_YearMode_SetsContentToAbbreviatedMonthName()
    {
        var button = new CalendarYearButton();
        var context = new CalendarContext(null, 5);
        button.SetContext(CalendarViewMode.Year, context);
        Assert.Equal("May", button.Content);
    }
    
    [AvaloniaFact]
    public void SetContext_YearMode_SetsContentToAbbreviatedMonthName_Chinese()
    {
        CultureInfo.CurrentCulture = new CultureInfo("zh-CN");
        var button = new CalendarYearButton();
        var context = new CalendarContext(null, 5);
        button.SetContext(CalendarViewMode.Year, context);
        Assert.Equal("5月", button.Content);
    }

    [AvaloniaFact]
    public void SetContext_DecadeMode_SetsContentToYear()
    {
        var button = new CalendarYearButton();
        var context = new CalendarContext(2023);
        button.SetContext(CalendarViewMode.Decade, context);
        Assert.Equal("2023", button.Content);
    }

    [AvaloniaFact]
    public void SetContext_CenturyMode_SetsContentToYearRange()
    {
        var button = new CalendarYearButton();
        var context = new CalendarContext(null, null, 1900, 1999);
        button.SetContext(CalendarViewMode.Century, context);
        Assert.Equal("1900-1999", button.Content);
    }

    [AvaloniaFact]
    public void SetContext_InvalidYear_DisablesButton()
    {
        var button = new CalendarYearButton();
        var context = new CalendarContext(null, null, -1);
        button.SetContext(CalendarViewMode.Decade, context);
        Assert.False(button.IsEnabled);
    }
    
    [AvaloniaFact]
    public void SetContext_Month_Mode_DisablesButton()
    {
        var button = new CalendarYearButton();
        var context = new CalendarContext(1, 1);
        button.SetContext(CalendarViewMode.Month, context);
        Assert.False(button.IsEnabled);
    }
    
    [AvaloniaFact]
    public void OnPointerReleased_RaisesItemSelectedEvent()
    {

        var button = new CalendarYearButton();
        var context = new CalendarContext(2023, 5);
        button.SetContext(CalendarViewMode.Year, context);
        int eventRaised = 0;
        CalendarContext? eventContext = null;
        void OnItemSelected( object? sender, CalendarYearButtonEventArgs args)
        {
            eventRaised++;
            eventContext = args.Context;
        }
        button.ItemSelected += OnItemSelected;
        Window window = new Window();
        window.Content = button;
        window.Show();
        window.MouseUp(new Point(10, 10), MouseButton.Left);
        Assert.Equal(1, eventRaised);
        Assert.Equal(context, eventContext);
        button.ItemSelected -= OnItemSelected;
        eventContext = null;
        window.MouseUp(new Point(10, 10), MouseButton.Left);
        Assert.Null(eventContext);
        Assert.Equal(1, eventRaised);

    }
}