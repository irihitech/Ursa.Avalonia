namespace Ursa.Controls;

public interface IDateSelector
{
    public bool Match(DateTime? date); 
}

public class WeekendDateSelector: IDateSelector
{
    public static WeekendDateSelector Instance { get; } = new WeekendDateSelector();
    
    public bool Match(DateTime? date)
    {
        if (date is null) return false;
        return date.Value.DayOfWeek == DayOfWeek.Saturday || date.Value.DayOfWeek == DayOfWeek.Sunday;
    }
}