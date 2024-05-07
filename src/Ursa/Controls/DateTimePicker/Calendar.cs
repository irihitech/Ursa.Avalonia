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

    public static readonly StyledProperty<AvaloniaList<DateRange>?> DisabledDatesProperty =
        AvaloniaProperty.Register<Calendar, AvaloniaList<DateRange>?>(
            nameof(DisabledDates));

    public AvaloniaList<DateRange>? DisabledDates
    {
        get => GetValue(DisabledDatesProperty);
        set => SetValue(DisabledDatesProperty, value);
    }

    public static readonly StyledProperty<IDateSelector?> DisabledDateRuleProperty = AvaloniaProperty.Register<Calendar, IDateSelector?>(
        nameof(DisabledDateRule));

    public IDateSelector? DisabledDateRule
    {
        get => GetValue(DisabledDateRuleProperty);
        set => SetValue(DisabledDateRuleProperty, value);
    }
}