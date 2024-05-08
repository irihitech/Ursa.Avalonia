namespace Ursa.Controls;

/// <summary>
///     Represents a date range. It can be a single day or a range of days. The range is inclusive.
/// </summary>
public sealed record DateRange
{
    public DateRange(DateTime day)
    {
        Start = day.Date;
        End = day.Date;
    }

    public DateRange(DateTime start, DateTime end)
    {
        if (DateTime.Compare(end, start) >= 0)
        {
            Start = start.Date;
            End = end.Date;
        }
        else
        {
            Start = start.Date;
            End = start.Date;
        }
    }

    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    public bool Contains(DateTime? date)
    {
        if (date is null) return false;
        return date >= Start && date <= End;
    }
}

internal static class DateRangeExtension
{
    public static bool Contains(this IEnumerable<DateRange>? ranges, DateTime? date)
    {
        if (date is null || ranges is null) return false;
        return ranges.Any(range => range.Contains(date));
    }
}