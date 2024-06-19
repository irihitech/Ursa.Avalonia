using System.Diagnostics;
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

    public event EventHandler<CalendarDayButtonEventArgs> ItemSelected
    {
        add => AddHandler(ItemSelectedEvent, value);
        remove => RemoveHandler(ItemSelectedEvent, value);
    }
    
    internal void SetContext(CalendarViewMode mode, CalendarContext context)
    {
        CalendarContext = context.Clone();
        CalendarContext.Month = context.Month;
        CalendarContext.Year = context.Year;
        CalendarContext.StartYear = context.StartYear;
        CalendarContext.EndYear = context.EndYear;
        this.Mode = mode;
        Content = Mode switch
        {
            CalendarViewMode.Year => DateTimeHelper.GetCurrentDateTimeFormatInfo().AbbreviatedMonthNames[ CalendarContext.Month ?? 0 ],
            CalendarViewMode.Decade => CalendarContext.Year?.ToString(),
            CalendarViewMode.Century => CalendarContext.StartYear + "-" + CalendarContext.EndYear,
            // CalendarViewMode.Century => CalendarContext.StartYear + "-" + CalendarContext.EndYear,
            _ => Content
        };
        IsEnabled = Content != null;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        RaiseEvent(new CalendarYearButtonEventArgs(Mode, this.CalendarContext.Clone())
            { RoutedEvent = ItemSelectedEvent, Source = this });
    }
}