using Ursa.Controls;

namespace Test.Ursa.DateTimePicker;

public class DateTimeHelperTests
{
    [Fact]
    public void GetFirstDayOfMonth_ReturnsFirstDay()
    {
        var date = new DateTime(2023, 5, 15);
        var firstDay = date.GetFirstDayOfMonth();
        Assert.Equal(new DateTime(2023, 5, 1), firstDay);
    }

    [Fact]
    public void GetLastDayOfMonth_ReturnsLastDay()
    {
        var date = new DateTime(2023, 5, 15);
        var lastDay = date.GetLastDayOfMonth();
        Assert.Equal(new DateTime(2023, 5, 31), lastDay);
    }

    [Fact]
    public void CompareYearMonth_SameYearMonth_ReturnsZero()
    {
        var date1 = new DateTime(2023, 5, 15);
        var date2 = new DateTime(2023, 5, 20);
        var result = DateTimeHelper.CompareYearMonth(date1, date2);
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareYearMonth_DifferentYearMonth_ReturnsDifference()
    {
        var date1 = new DateTime(2023, 5, 15);
        var date2 = new DateTime(2024, 6, 20);
        var result = DateTimeHelper.CompareYearMonth(date1, date2);
        Assert.Equal(-13, result);
    }

    [Fact]
    public void Min_ReturnsMinimumDate()
    {
        var date1 = new DateTime(2023, 5, 15);
        var date2 = new DateTime(2024, 6, 20);
        var result = DateTimeHelper.Min(date1, date2);
        Assert.Equal(date1, result);
    }

    [Fact]
    public void Max_ReturnsMaximumDate()
    {
        var date1 = new DateTime(2023, 5, 15);
        var date2 = new DateTime(2024, 6, 20);
        var result = DateTimeHelper.Max(date1, date2);
        Assert.Equal(date2, result);
    }

    [Fact]
    public void GetDecadeViewRangeByYear_ReturnsCorrectRange()
    {
        var year = 2023;
        var result = DateTimeHelper.GetDecadeViewRangeByYear(year);
        Assert.Equal((2020, 2029), result);
    }

    [Fact]
    public void GetCenturyViewRangeByYear_ReturnsCorrectRange()
    {
        var year = 2023;
        var result = DateTimeHelper.GetCenturyViewRangeByYear(year);
        Assert.Equal((2000, 2100), result);
    }
}