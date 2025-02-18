using Ursa.Controls;

namespace Test.Ursa.DateTimePicker;

public class DateRangeExtensionTests
{
    [Fact]
    public void Contains_DateInAnyRange_ShouldReturnTrue()
    {
        var ranges = new List<DateRange>
        {
            new(new DateTime(2023, 10, 1), new DateTime(2023, 10, 5)),
            new(new DateTime(2023, 10, 10), new DateTime(2023, 10, 15))
        };
        var date = new DateTime(2023, 10, 3);

        Assert.True(ranges.Contains(date));
    }

    [Fact]
    public void Contains_DateNotInAnyRange_ShouldReturnFalse()
    {
        var ranges = new List<DateRange>
        {
            new(new DateTime(2023, 10, 1), new DateTime(2023, 10, 5)),
            new(new DateTime(2023, 10, 10), new DateTime(2023, 10, 15))
        };
        var date = new DateTime(2023, 10, 6);

        Assert.False(ranges.Contains(date));
    }

    [Fact]
    public void Contains_NullDate_ShouldReturnFalse()
    {
        var ranges = new List<DateRange>
        {
            new(new DateTime(2023, 10, 1), new DateTime(2023, 10, 5)),
            new(new DateTime(2023, 10, 10), new DateTime(2023, 10, 15))
        };

        Assert.False(ranges.Contains((DateTime?)null));
    }

    [Fact]
    public void Contains_NullRanges_ShouldReturnFalse()
    {
        IEnumerable<DateRange>? ranges = null;
        var date = new DateTime(2023, 10, 3);

        Assert.False(ranges.Contains(date));
    }
}