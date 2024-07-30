using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Common;

namespace Ursa.Controls;

[PseudoClasses(PseudoClassName.PC_Pressed, PseudoClassName.PC_Selected,
    PC_StartDate, PC_EndDate, PC_PreviewStartDate, PC_PreviewEndDate, PC_InRange, PC_Today, PC_Blackout,
    PC_NotCurrentMonth)]
public class CalendarDayButton : ContentControl
{
    public const string PC_StartDate = ":start-date";
    public const string PC_EndDate = ":end-date";
    public const string PC_PreviewStartDate = ":preview-start-date";
    public const string PC_PreviewEndDate = ":preview-end-date";
    public const string PC_InRange = ":in-range";
    public const string PC_Today = ":today";
    public const string PC_NotCurrentMonth = ":not-current-month";
    public const string PC_Blackout = ":blackout";

    private static HashSet<string> _pseudoClasses =
    [
        PseudoClassName.PC_Selected, PC_StartDate, PC_EndDate, PC_PreviewStartDate,  PC_PreviewEndDate, PC_InRange
    ];

    public static readonly RoutedEvent<CalendarDayButtonEventArgs> DateSelectedEvent =
        RoutedEvent.Register<CalendarDayButton, CalendarDayButtonEventArgs>(
            nameof(DateSelected), RoutingStrategies.Bubble);

    public static readonly RoutedEvent<CalendarDayButtonEventArgs> DatePreviewedEvent =
        RoutedEvent.Register<CalendarDayButton, CalendarDayButtonEventArgs>(
            nameof(DatePreviewed), RoutingStrategies.Bubble);

    private bool _isBlackout;

    private bool _isEndDate;

    private bool _isInRange;

    private bool _isNotCurrentMonth;

    private bool _isPreviewEndDate;

    private bool _isPreviewStartDate;

    private bool _isSelected;

    private bool _isStartDate;

    private bool _isToday;

    static CalendarDayButton()
    {
        PressedMixin.Attach<CalendarDayButton>();
    }

    // internal CalendarDisplayControl? Owner { get; set; }

    public bool IsToday
    {
        get => _isToday;
        set
        {
            _isToday = value;
            PseudoClasses.Set(PC_Today, value);
        }
    }

    public bool IsStartDate
    {
        get => _isStartDate;
        set
        {
            _isStartDate = value;
            SetPseudoClass(PC_StartDate, value);
        }
    }

    public bool IsEndDate
    {
        get => _isEndDate;
        set
        {
            _isEndDate = value;
            SetPseudoClass(PC_EndDate, value);
        }
    }

    public bool IsPreviewStartDate
    {
        get => _isPreviewStartDate;
        set
        {
            _isPreviewStartDate = value;
            SetPseudoClass(PC_PreviewStartDate, value);
        }
    }

    public bool IsPreviewEndDate
    {
        get => _isPreviewEndDate;
        set
        {
            _isPreviewEndDate = value;
            SetPseudoClass(PC_PreviewEndDate, value);
        }
    }

    public bool IsInRange
    {
        get => _isInRange;
        set
        {
            _isInRange = value;
            SetPseudoClass(PC_InRange, value);
        }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            SetPseudoClass(PseudoClassName.PC_Selected, value);
        }
    }

    /// <summary>
    ///     Notice: IsBlackout is not equivalent to not IsEnabled. Blackout dates still react to pointerover actions.
    /// </summary>
    public bool IsBlackout
    {
        get => _isBlackout;
        set
        {
            _isBlackout = value;
            PseudoClasses.Set(PC_Blackout, value);
        }
    }

    /// <summary>
    ///     Notice: IsNotCurrentMonth is not equivalent to not IsEnabled. Not current month dates still react to pointerover
    ///     and press action.
    /// </summary>
    public bool IsNotCurrentMonth
    {
        get => _isNotCurrentMonth;
        set
        {
            _isNotCurrentMonth = value;
            PseudoClasses.Set(PC_NotCurrentMonth, value);
        }
    }

    public event EventHandler<CalendarDayButtonEventArgs> DateSelected
    {
        add => AddHandler(DateSelectedEvent, value);
        remove => RemoveHandler(DateSelectedEvent, value);
    }

    public event EventHandler<CalendarDayButtonEventArgs> DatePreviewed
    {
        add => AddHandler(DatePreviewedEvent, value);
        remove => RemoveHandler(DatePreviewedEvent, value);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (DataContext is DateTime d)
            RaiseEvent(new CalendarDayButtonEventArgs(d) { RoutedEvent = DateSelectedEvent, Source = this });
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);
        if (DataContext is DateTime d)
            RaiseEvent(new CalendarDayButtonEventArgs(d) { RoutedEvent = DatePreviewedEvent, Source = this });
    }

    internal void ResetSelection()
    {
        foreach (var pc in _pseudoClasses)
        {
            PseudoClasses.Set(pc, false);
        }
    }

    private void SetPseudoClass(string s, bool value)
    {
        if (_pseudoClasses.Contains(s) && value)
        {
            foreach (var pc in _pseudoClasses)
            {
                PseudoClasses.Set(pc, false);
            }
        }
        PseudoClasses.Set(s, value);
    }
}