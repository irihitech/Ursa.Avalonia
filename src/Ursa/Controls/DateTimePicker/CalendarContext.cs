namespace Ursa.Controls;

internal sealed class CalendarContext(int? year = null, int? month = null, int? startYear = null, int? endYear = null): IComparable<CalendarContext>
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

        if (month is null)
        {
            month = 1;
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
        if (month is null)
        {
            month = 1;
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

    public int CompareTo(CalendarContext? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var yearComparison = Nullable.Compare(Year, other.Year);
        if (yearComparison != 0) return yearComparison;
        return Nullable.Compare(Month, other.Month);
    }

    public override string ToString()
    {
        return
            $"Start: {StartYear?.ToString() ?? "null"}, End: {EndYear?.ToString() ?? "null"}, Year: {Year?.ToString() ?? "null"}, Month: {Month?.ToString() ?? "null"}";
    }
}