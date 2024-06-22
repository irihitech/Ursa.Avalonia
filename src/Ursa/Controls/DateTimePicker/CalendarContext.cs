namespace Ursa.Controls;

public sealed class CalendarContext(int? year = null, int? month = null, int? startYear = null, int? endYear = null)
{
    public int? Year { get; } = year;
    public int? Month { get; } = month;
    public int? StartYear { get; } = startYear;
    public int? EndYear { get; } = endYear;


    public CalendarContext Clone()
    {
        return new CalendarContext(Year, Month, StartYear, EndYear);
    }

    public static CalendarContext Today()
    {
        return new CalendarContext(DateTime.Today.Year, DateTime.Today.Month);
    }

    public CalendarContext With(int? year = null, int? month = null, int? startYear = null, int? endYear = null)
    {
        return new CalendarContext(year ?? Year, month ?? Month, startYear ?? StartYear, endYear ?? EndYear);
    }

    public CalendarContext NextMonth()
    {
        var year = Year;
        var month = Month;
        if (month == 12)
        {
            year++;
            month = 1;
        }
        else
        {
            month++;
        }
        return new CalendarContext(year, month, StartYear, EndYear);
    }

    public CalendarContext PreviousMonth()
    {
        var year = Year;
        var month = Month;
        if (month == 1)
        {
            year--;
            month = 12;
        }
        else
        {
            month--;
        }
        return new CalendarContext(year, month, StartYear, EndYear);
    }

    public CalendarContext NextYear()
    {
        return new CalendarContext(Year + 1, Month, StartYear, EndYear);
    }

    public CalendarContext PreviousYear()
    {
        return new CalendarContext(Year - 1, Month, StartYear, EndYear);
    }
}