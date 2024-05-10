using System.Globalization;

namespace Ursa.Controls;

internal static class DateTimeHelper
{
    public const int NumberOfDaysPerWeek = 7;
    public const int NumberOfWeeksPerMonth = 6;
    
    public static DateTimeFormatInfo GetCurrentDateTimeFormatInfo()
    {
        if (CultureInfo.CurrentCulture.Calendar is GregorianCalendar) return CultureInfo.CurrentCulture.DateTimeFormat;
        System.Globalization.Calendar? calendar =
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
    
}