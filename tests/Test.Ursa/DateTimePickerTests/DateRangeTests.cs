using Ursa.Controls;

namespace Test.Ursa.DateTimePicker;

public class DateRangeTests
{
    [Fact]
    public void DateRange_SingleDayRange_ShouldHaveSameStartAndEnd()
    {
        var date = new DateTime(2023, 10, 1);
        var range = new DateRange(date);

        Assert.Equal(date.Date, range.Start);
        Assert.Equal(date.Date, range.End);
    }

    [Fact]
    public void DateRange_ValidRange_ShouldSetStartAndEndCorrectly()
    {
        var start = new DateTime(2023, 10, 1);
        var end = new DateTime(2023, 10, 5);
        var range = new DateRange(start, end);

        Assert.Equal(start.Date, range.Start);
        Assert.Equal(end.Date, range.End);
    }

    [Fact]
    public void DateRange_EndBeforeStart_ShouldSetEndToStart()
    {
        var start = new DateTime(2023, 10, 5);
        var end = new DateTime(2023, 10, 1);
        var range = new DateRange(start, end);

        Assert.Equal(start.Date, range.Start);
        Assert.Equal(start.Date, range.End);
    }

    [Fact]
    public void Contains_DateWithinRange_ShouldReturnTrue()
    {
        var range = new DateRange(new DateTime(2023, 10, 1), new DateTime(2023, 10, 5));
        var date = new DateTime(2023, 10, 3);

        Assert.True(range.Contains(date));
    }

    [Fact]
    public void Contains_DateOutsideRange_ShouldReturnFalse()
    {
        var range = new DateRange(new DateTime(2023, 10, 1), new DateTime(2023, 10, 5));
        var date = new DateTime(2023, 10, 6);

        Assert.False(range.Contains(date));
    }

    [Fact]
    public void Contains_NullDate_ShouldReturnFalse()
    {
        var range = new DateRange(new DateTime(2023, 10, 1), new DateTime(2023, 10, 5));

        Assert.False(range.Contains(null));
    }
}