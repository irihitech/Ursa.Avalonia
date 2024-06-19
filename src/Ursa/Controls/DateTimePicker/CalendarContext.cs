namespace Ursa.Controls;

public class CalendarContext(int? year = null, int? month = null, int? day = null)
{
    public int? Year = year;
    public int? Month = month;
    public int? Day = day;
    public int? StartYear;
    public int? EndYear;


    public CalendarContext Clone()
    {
        return new CalendarContext(Year, Month, Day) { StartYear = StartYear, EndYear = EndYear };
    }
}