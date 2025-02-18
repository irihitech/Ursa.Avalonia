using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Common;

namespace Ursa.Controls;

[PseudoClasses(PC_Range, PseudoClassName.PC_Selected)]
public class CalendarYearButton : ContentControl
{
    public const string PC_Range = ":range";

    public static readonly RoutedEvent<CalendarYearButtonEventArgs> ItemSelectedEvent =
        RoutedEvent.Register<CalendarYearButton, CalendarYearButtonEventArgs>(
            nameof(ItemSelected), RoutingStrategies.Bubble);

    static CalendarYearButton()
    {
        PressedMixin.Attach<CalendarYearButton>();
    }

    internal CalendarContext CalendarContext { get; set; } = new ();

    internal CalendarViewMode Mode { get; private set; }

    public event EventHandler<CalendarYearButtonEventArgs> ItemSelected
    {
        add => AddHandler(ItemSelectedEvent, value);
        remove => RemoveHandler(ItemSelectedEvent, value);
    }
    
    internal void SetContext(CalendarViewMode mode, CalendarContext context)
    {
        CalendarContext = context.Clone();
        Mode = mode;
        switch (Mode)
        {
            case CalendarViewMode.Year:
                Content = DateTimeHelper.GetCurrentDateTimeFormatInfo()
                    .AbbreviatedMonthNames[(CalendarContext.Month - 1) ?? 0];
                break;
            case CalendarViewMode.Decade:
                Content = CalendarContext.Year <= 0 || CalendarContext.Year > 9999
                    ? null
                    : CalendarContext.Year?.ToString();
                break;
            case CalendarViewMode.Century:
                Content = CalendarContext.EndYear <= 0 || CalendarContext.StartYear > 9999
                    ? null
                    : CalendarContext.StartYear + "-" + CalendarContext.EndYear;
                break;
            default:
                Content = null;
                break;
        }
        IsEnabled = Content != null;
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        RaiseEvent(new CalendarYearButtonEventArgs(Mode, this.CalendarContext.Clone())
            { RoutedEvent = ItemSelectedEvent, Source = this });
    }
}