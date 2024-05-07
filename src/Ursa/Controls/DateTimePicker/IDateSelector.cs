namespace Ursa.Controls;

public interface IDateSelector
{
    public bool IsValid(DateTime? date); 
}

public class WeekendDateSelector: IDateSelector
{
    public static WeekendDateSelector Instance { get; } = new WeekendDateSelector();
    
    public bool IsValid(DateTime? date)
    {
        if (date is null) return false;
        return date.Value.DayOfWeek == DayOfWeek.Saturday || date.Value.DayOfWeek == DayOfWeek.Sunday;
    }
}