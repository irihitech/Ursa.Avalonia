namespace Ursa.Controls;

public class DateContext(int? year = null, int? month = null, int? day = null)
{
    public int? Year = year;
    public int? Month = month;
    public int? Day = day;
}