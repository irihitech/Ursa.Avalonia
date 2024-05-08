using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace Ursa.Controls;

public class Calendar: TemplatedControl
{
    public static readonly StyledProperty<DateTime> SelectedDateProperty = AvaloniaProperty.Register<Calendar, DateTime>(nameof(SelectedDate), DateTime.Now);
    public DateTime SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        AvaloniaProperty.Register<Calendar, DayOfWeek>(nameof(FirstDayOfWeek),
            defaultValue: DateTimeHelper.GetCurrentDateTimeFormatInfo().FirstDayOfWeek);
    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty = AvaloniaProperty.Register<Calendar, bool>(nameof(IsTodayHighlighted), true);
    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }

    public static readonly StyledProperty<AvaloniaList<DateRange>?> BlackoutDatesProperty =
        AvaloniaProperty.Register<Calendar, AvaloniaList<DateRange>?>(
            nameof(BlackoutDates));

    public AvaloniaList<DateRange>? BlackoutDates
    {
        get => GetValue(BlackoutDatesProperty);
        set => SetValue(BlackoutDatesProperty, value);
    }

    public static readonly StyledProperty<IDateSelector?> BlackoutDateRuleProperty = AvaloniaProperty.Register<Calendar, IDateSelector?>(
        nameof(BlackoutDateRule));

    public IDateSelector? BlackoutDateRule
    {
        get => GetValue(BlackoutDateRuleProperty);
        set => SetValue(BlackoutDateRuleProperty, value);
    }
}