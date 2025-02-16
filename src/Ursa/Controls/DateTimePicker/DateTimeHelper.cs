using System.Globalization;

namespace Ursa.Controls;

internal static class DateTimeHelper
{
    public const int NumberOfDaysPerWeek = 7;
    public const int NumberOfWeeksPerMonth = 6;
    
    public static DateTimeFormatInfo GetCurrentDateTimeFormatInfo()
    {
        if (CultureInfo.CurrentCulture.Calendar is GregorianCalendar) return CultureInfo.CurrentCulture.DateTimeFormat;
        Calendar? calendar =
            CultureInfo.CurrentCulture.OptionalCalendars.OfType<GregorianCalendar>().FirstOrDefault();
        string cultureName = calendar is null ? CultureInfo.InvariantCulture.Name : CultureInfo.CurrentCulture.Name;
        var dt = new CultureInfo(cultureName).DateTimeFormat;
        dt.Calendar = calendar ?? new GregorianCalendar();
        return dt;
    }
    
    public static DateTime GetFirstDayOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1);
    }
    
    public static DateTime GetLastDayOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
    }
    
    public static int CompareYearMonth(DateTime dt1, DateTime dt2)
    {
        return (dt1.Year - dt2.Year) * 12 + dt1.Month - dt2.Month;
    }

    public static DateTime Min(DateTime d1, DateTime d2)
    {
        return d1.Ticks > d2.Ticks ? d2 : d1;
    }

    public static DateTime Max(DateTime d1, DateTime d2)
    {
        return d1.Ticks < d2.Ticks ? d2 : d1;
    }
    
    public static (int start, int end) GetDecadeViewRangeByYear(int year)
    {
        int start = year / 10 * 10;
        return new ValueTuple<int, int>(start, start + 10 - 1);
    }
    
    public static (int start, int end) GetCenturyViewRangeByYear(int year)
    {
        int start = year / 100 * 100;
        return new ValueTuple<int, int>(start, start + 100);
    }
}