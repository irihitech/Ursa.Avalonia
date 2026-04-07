namespace Ursa.Controls;

internal sealed class DatePickerCalendarContext(int? year = null, int? month = null, int? startYear = null, int? endYear = null): IComparable<DatePickerCalendarContext>
{
    public int? Year { get; } = year;
    public int? Month { get; } = month;
    public int? StartYear { get; } = startYear;
    public int? EndYear { get; } = endYear;


    public DatePickerCalendarContext Clone()
    {
        return new DatePickerCalendarContext(Year, Month, StartYear, EndYear);
    }

    public static DatePickerCalendarContext Today()
    {
        return new DatePickerCalendarContext(DateTime.Today.Year, DateTime.Today.Month);
    }

    public DatePickerCalendarContext With(int? year = null, int? month = null, int? startYear = null, int? endYear = null)
    {
        return new DatePickerCalendarContext(year ?? Year, month ?? Month, startYear ?? StartYear, endYear ?? EndYear);
    }

    public DatePickerCalendarContext NextMonth()
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
        return new DatePickerCalendarContext(year, month, StartYear, EndYear);
    }

    public DatePickerCalendarContext PreviousMonth()
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
        return new DatePickerCalendarContext(year, month, StartYear, EndYear);
    }

    public DatePickerCalendarContext NextYear()
    {
        return new DatePickerCalendarContext(Year + 1, Month, StartYear, EndYear);
    }

    public DatePickerCalendarContext PreviousYear()
    {
        return new DatePickerCalendarContext(Year - 1, Month, StartYear, EndYear);
    }

    public int CompareTo(DatePickerCalendarContext? other)
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