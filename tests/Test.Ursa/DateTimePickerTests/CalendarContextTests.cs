using Ursa.Controls;

namespace Test.Ursa.DateTimePicker;

public class CalendarContextTests
{
    [Fact]
    public void Clone_ReturnsExactCopy()
    {
        var context = new CalendarContext(2023, 5, 2000, 2030);
        var clone = context.Clone();
        Assert.Equal(context.Year, clone.Year);
        Assert.Equal(context.Month, clone.Month);
        Assert.Equal(context.StartYear, clone.StartYear);
        Assert.Equal(context.EndYear, clone.EndYear);
    }

    [Fact]
    public void Today_ReturnsCurrentDate()
    {
        var today = CalendarContext.Today();
        Assert.Equal(DateTime.Today.Year, today.Year);
        Assert.Equal(DateTime.Today.Month, today.Month);
    }

    [Fact]
    public void With_UpdatesSpecifiedFields()
    {
        var context = new CalendarContext(2023, 5, 2000, 2030);
        var updated = context.With(month: 6);
        Assert.Equal(2023, updated.Year);
        Assert.Equal(6, updated.Month);
        Assert.Equal(2000, updated.StartYear);
        Assert.Equal(2030, updated.EndYear);
    }

    [Fact]
    public void NextMonth_UpdatesToNextMonth()
    {
        var context = new CalendarContext(2023, 5);
        var nextMonth = context.NextMonth();
        Assert.Equal(2023, nextMonth.Year);
        Assert.Equal(6, nextMonth.Month);
    }

    [Fact]
    public void NextMonth_UpdatesToNextYear()
    {
        var context = new CalendarContext(2023, 12);
        var nextMonth = context.NextMonth();
        Assert.Equal(2024, nextMonth.Year);
        Assert.Equal(1, nextMonth.Month);
    }

    [Fact]
    public void PreviousMonth_UpdatesToPreviousMonth()
    {
        var context = new CalendarContext(2023, 5);
        var previousMonth = context.PreviousMonth();
        Assert.Equal(2023, previousMonth.Year);
        Assert.Equal(4, previousMonth.Month);
    }

    [Fact]
    public void PreviousMonth_UpdatesToPreviousYear()
    {
        var context = new CalendarContext(2023, 1);
        var previousMonth = context.PreviousMonth();
        Assert.Equal(2022, previousMonth.Year);
        Assert.Equal(12, previousMonth.Month);
    }

    [Fact]
    public void NextYear_UpdatesToNextYear()
    {
        var context = new CalendarContext(2023, 5);
        var nextYear = context.NextYear();
        Assert.Equal(2024, nextYear.Year);
        Assert.Equal(5, nextYear.Month);
    }

    [Fact]
    public void PreviousYear_UpdatesToPreviousYear()
    {
        var context = new CalendarContext(2023, 5);
        var previousYear = context.PreviousYear();
        Assert.Equal(2022, previousYear.Year);
        Assert.Equal(5, previousYear.Month);
    }

    [Fact]
    public void CompareTo_SameContext_ReturnsZero()
    {
        var context1 = new CalendarContext(2023, 5);
        var context2 = new CalendarContext(2023, 5);
        Assert.Equal(0, context1.CompareTo(context2));
    }

    [Fact]
    public void CompareTo_DifferentYear_ReturnsNonZero()
    {
        var context1 = new CalendarContext(2023, 5);
        var context2 = new CalendarContext(2024, 5);
        Assert.NotEqual(0, context1.CompareTo(context2));
    }

    [Fact]
    public void CompareTo_DifferentMonth_ReturnsNonZero()
    {
        var context1 = new CalendarContext(2023, 5);
        var context2 = new CalendarContext(2023, 6);
        Assert.NotEqual(0, context1.CompareTo(context2));
    }

    [Fact]
    public void ToString_ReturnsCorrectFormat()
    {
        var context = new CalendarContext(2023, 5, 2000, 2030);
        var expected = "Start: 2000, End: 2030, Year: 2023, Month: 5";
        Assert.Equal(expected, context.ToString());
    }
}