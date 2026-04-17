namespace Ursa.Controls;

/// <summary>
/// Defines the minimal granularity for CalendarView selection.
/// </summary>
public enum CalendarMinimalGranularity
{
    /// <summary>
    /// Select individual days (default behavior).
    /// </summary>
    Day,
    
    /// <summary>
    /// Select months (for Month Picker).
    /// </summary>
    Month,
    
    /// <summary>
    /// Select years (for Year Picker).
    /// </summary>
    Year
}
