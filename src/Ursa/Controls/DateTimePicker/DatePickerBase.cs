using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

namespace Ursa.Controls;

public class DatePickerBase: TemplatedControl
{
    public static readonly StyledProperty<AvaloniaList<DateRange>> BlackoutDatesProperty =
        AvaloniaProperty.Register<DatePickerBase, AvaloniaList<DateRange>>(nameof(BlackoutDates));

    public AvaloniaList<DateRange> BlackoutDates
    {
        get => GetValue(BlackoutDatesProperty);
        set => SetValue(BlackoutDatesProperty, value);
    }

    public static readonly StyledProperty<IDateSelector?> BlackoutDateRuleProperty =
        AvaloniaProperty.Register<DatePickerBase, IDateSelector?>(nameof(BlackoutDateRule));

    public IDateSelector? BlackoutDateRule
    {
        get => GetValue(BlackoutDateRuleProperty);
        set => SetValue(BlackoutDateRuleProperty, value);
    }

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        AvaloniaProperty.Register<DatePickerBase, DayOfWeek>(
            nameof(FirstDayOfWeek), DateTimeHelper.GetCurrentDateTimeFormatInfo().FirstDayOfWeek);

    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }
    
    public static readonly StyledProperty<bool> IsTodayHighlightedProperty =
        AvaloniaProperty.Register<DatePickerBase, bool>(nameof(IsTodayHighlighted), true);
    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }
    
}