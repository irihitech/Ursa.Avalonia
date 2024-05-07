using System.Globalization;

namespace Ursa.Controls;

internal static class DateTimeHelper
{
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
}