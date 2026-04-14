using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Common;

namespace Ursa.Controls;

[PseudoClasses(PC_Range, PseudoClassName.PC_Selected)]
public class DatePickerCalendarYearButton : ContentControl
{
    public const string PC_Range = ":range";

    public static readonly RoutedEvent<DatePickerCalendarYearButtonEventArgs> ItemSelectedEvent =
        RoutedEvent.Register<DatePickerCalendarYearButton, DatePickerCalendarYearButtonEventArgs>(
            nameof(ItemSelected), RoutingStrategies.Bubble);

    static DatePickerCalendarYearButton()
    {
        PressedMixin.Attach<DatePickerCalendarYearButton>();
    }

    internal DatePickerCalendarContext DatePickerCalendarContext { get; set; } = new ();

    internal DatePickerCalendarViewMode Mode { get; private set; }

    public event EventHandler<DatePickerCalendarYearButtonEventArgs> ItemSelected
    {
        add => AddHandler(ItemSelectedEvent, value);
        remove => RemoveHandler(ItemSelectedEvent, value);
    }
    
    internal void SetContext(DatePickerCalendarViewMode mode, DatePickerCalendarContext context)
    {
        DatePickerCalendarContext = context.Clone();
        Mode = mode;
        switch (Mode)
        {
            case DatePickerCalendarViewMode.Year:
                Content = DateTimeHelper.GetCurrentDateTimeFormatInfo()
                    .AbbreviatedMonthNames[(DatePickerCalendarContext.Month - 1) ?? 0];
                break;
            case DatePickerCalendarViewMode.Decade:
                Content = DatePickerCalendarContext.Year <= 0 || DatePickerCalendarContext.Year > 9999
                    ? null
                    : DatePickerCalendarContext.Year?.ToString();
                break;
            case DatePickerCalendarViewMode.Century:
                Content = DatePickerCalendarContext.EndYear <= 0 || DatePickerCalendarContext.StartYear > 9999
                    ? null
                    : DatePickerCalendarContext.StartYear + "-" + DatePickerCalendarContext.EndYear;
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
        RaiseEvent(new DatePickerCalendarYearButtonEventArgs(Mode, this.DatePickerCalendarContext.Clone())
            { RoutedEvent = ItemSelectedEvent, Source = this });
    }
}