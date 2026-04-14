using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_Popup, typeof(Popup))]
[TemplatePart(PART_StartCalendar, typeof(DatePickerCalendarView))]
[TemplatePart(PART_EndCalendar, typeof(DatePickerCalendarView))]
[TemplatePart(PART_StartTextBox, typeof(TextBox))]
[TemplatePart(PART_EndTextBox, typeof(TextBox))]
[PseudoClasses(PseudoClassName.PC_Empty)]
public abstract class DateRangePickerBase : TemplatedControl, IInnerContentControl, IPopupInnerContent
{
    public const string PART_Popup = "PART_Popup";
    public const string PART_StartCalendar = "PART_StartCalendar";
    public const string PART_EndCalendar = "PART_EndCalendar";
    public const string PART_StartTextBox = "PART_StartTextBox";
    public const string PART_EndTextBox = "PART_EndTextBox";

    protected const string DEFAULT_DATE_DISPLAY_FORMAT = "yyyy-MM-dd";

    protected DatePickerCalendarView? _startCalendar;
    protected DatePickerCalendarView? _endCalendar;
    protected TextBox? _startTextBox;
    protected TextBox? _endTextBox;
    protected Popup? _popup;
    private readonly RangePickerStatus _status = new();

    public static readonly StyledProperty<string?> DisplayFormatProperty =
        AvaloniaProperty.Register<DateRangePickerBase, string?>(
            nameof(DisplayFormat), DEFAULT_DATE_DISPLAY_FORMAT);

    public static readonly StyledProperty<AvaloniaList<DateRange>> BlackoutDatesProperty =
        AvaloniaProperty.Register<DateRangePickerBase, AvaloniaList<DateRange>>(nameof(BlackoutDates));

    public static readonly StyledProperty<IDateSelector?> BlackoutDateRuleProperty =
        AvaloniaProperty.Register<DateRangePickerBase, IDateSelector?>(nameof(BlackoutDateRule));

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        AvaloniaProperty.Register<DateRangePickerBase, DayOfWeek>(
            nameof(FirstDayOfWeek), DateTimeHelper.GetCurrentDateTimeFormatInfo().FirstDayOfWeek);

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty =
        AvaloniaProperty.Register<DateRangePickerBase, bool>(nameof(IsTodayHighlighted), true);

    public static readonly StyledProperty<object?> InnerLeftContentProperty =
        AvaloniaProperty.Register<DateRangePickerBase, object?>(nameof(InnerLeftContent));

    public static readonly StyledProperty<object?> InnerRightContentProperty =
        AvaloniaProperty.Register<DateRangePickerBase, object?>(nameof(InnerRightContent));

    public static readonly StyledProperty<object?> PopupInnerTopContentProperty =
        AvaloniaProperty.Register<DateRangePickerBase, object?>(nameof(PopupInnerTopContent));

    public static readonly StyledProperty<object?> PopupInnerBottomContentProperty =
        AvaloniaProperty.Register<DateRangePickerBase, object?>(nameof(PopupInnerBottomContent));

    public static readonly StyledProperty<bool> IsDropdownOpenProperty =
        AvaloniaProperty.Register<DateRangePickerBase, bool>(
            nameof(IsDropdownOpen), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> IsReadonlyProperty =
        AvaloniaProperty.Register<DateRangePickerBase, bool>(nameof(IsReadonly));

    public AvaloniaList<DateRange> BlackoutDates
    {
        get => GetValue(BlackoutDatesProperty);
        set => SetValue(BlackoutDatesProperty, value);
    }

    public IDateSelector? BlackoutDateRule
    {
        get => GetValue(BlackoutDateRuleProperty);
        set => SetValue(BlackoutDateRuleProperty, value);
    }

    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }

    public bool IsReadonly
    {
        get => GetValue(IsReadonlyProperty);
        set => SetValue(IsReadonlyProperty, value);
    }

    public bool IsDropdownOpen
    {
        get => GetValue(IsDropdownOpenProperty);
        set => SetValue(IsDropdownOpenProperty, value);
    }

    public object? InnerLeftContent
    {
        get => GetValue(InnerLeftContentProperty);
        set => SetValue(InnerLeftContentProperty, value);
    }

    public object? InnerRightContent
    {
        get => GetValue(InnerRightContentProperty);
        set => SetValue(InnerRightContentProperty, value);
    }

    public object? PopupInnerTopContent
    {
        get => GetValue(PopupInnerTopContentProperty);
        set => SetValue(PopupInnerTopContentProperty, value);
    }

    public object? PopupInnerBottomContent
    {
        get => GetValue(PopupInnerBottomContentProperty);
        set => SetValue(PopupInnerBottomContentProperty, value);
    }

    public string? DisplayFormat
    {
        get => GetValue(DisplayFormatProperty);
        set => SetValue(DisplayFormatProperty, value);
    }

    /// <summary>Exposes <see cref="RangePickerStatus.Reset"/> so the generic base can call it from <c>Clear()</c>.</summary>
    protected void ResetStatus() => _status.Reset();

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DatePickerCalendarView.DateSelectedEvent.RemoveHandler(OnDateSelected, _startCalendar, _endCalendar);
        DatePickerCalendarView.DatePreviewedEvent.RemoveHandler(OnDatePreviewed, _startCalendar, _endCalendar);
        GotFocusEvent.RemoveHandler(OnTextBoxGotFocus, _startTextBox, _endTextBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPressed, _startTextBox, _endTextBox);
        LostFocusEvent.RemoveHandler(OnTextBoxLostFocus, _startTextBox, _endTextBox);

        _startCalendar = e.NameScope.Find<DatePickerCalendarView>(PART_StartCalendar);
        _endCalendar = e.NameScope.Find<DatePickerCalendarView>(PART_EndCalendar);
        _startTextBox = e.NameScope.Find<TextBox>(PART_StartTextBox);
        _endTextBox = e.NameScope.Find<TextBox>(PART_EndTextBox);
        _popup = e.NameScope.Find<Popup>(PART_Popup);

        DatePickerCalendarView.DateSelectedEvent.AddHandler(OnDateSelected, _startCalendar, _endCalendar);
        DatePickerCalendarView.DatePreviewedEvent.AddHandler(OnDatePreviewed, _startCalendar, _endCalendar);
        GotFocusEvent.AddHandler(OnTextBoxGotFocus, _startTextBox, _endTextBox);
        PointerPressedEvent.AddHandler(OnTextBoxPressed,
            strategies: RoutingStrategies.Tunnel, handledEventsToo: true, _startTextBox, _endTextBox);
        LostFocusEvent.AddHandler(OnTextBoxLostFocus, _startTextBox, _endTextBox);

        SyncToUI();
    }

    private void OnTextBoxLostFocus(object? sender, FocusChangedEventArgs e)
    {
        CommitInput();
        if (_status is { Current: Status.End, Previous: Status.Start }
            && Equals(sender, _endTextBox) && _endTextBox?.IsFocused == true)
        {
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
    }

    private void OnTextBoxPressed(object? sender, PointerPressedEventArgs e) =>
        InitializePopupOpen(sender as TextBox);

    private void OnTextBoxGotFocus(object? sender, FocusChangedEventArgs e) =>
        InitializePopupOpen(sender as TextBox);

    private void InitializePopupOpen(TextBox? sender)
    {
        if (sender is null) return;
        SetCurrentValue(IsDropdownOpenProperty, true);
        SetCalendarContextDate();
        if (Equals(sender, _startTextBox))
            _status.Push(Status.Start);
        else if (Equals(sender, _endTextBox))
            _status.Push(Status.End);
        _startCalendar?.MarkDates(GetStartDateOnly(), GetEndDateOnly());
        _endCalendar?.MarkDates(GetStartDateOnly(), GetEndDateOnly());
    }

    private void OnDatePreviewed(object? sender, DatePickerCalendarDayButtonEventArgs e)
    {
        if (_status.Current == Status.Start)
        {
            _startCalendar?.MarkDates(GetStartDateOnly(), GetEndDateOnly(), e.Date);
            _endCalendar?.MarkDates(GetStartDateOnly(), GetEndDateOnly(), e.Date);
        }
        else if (_status.Current == Status.End)
        {
            _startCalendar?.MarkDates(GetStartDateOnly(), GetEndDateOnly(), null, e.Date);
            _endCalendar?.MarkDates(GetStartDateOnly(), GetEndDateOnly(), null, e.Date);
        }
        else
        {
            _startCalendar?.ClearSelection();
            _endCalendar?.ClearSelection();
        }
    }

    private void OnDateSelected(object? sender, DatePickerCalendarDayButtonEventArgs e)
    {
        if (_status is { Current: Status.Start, Previous: Status.None })
        {
            SetSelectedStartDate(e.Date);
            if (GetEndDateOnly() is { } endDate && e.Date is { } start && start > endDate)
                SetSelectedEndDate(null);
            _status.Push(Status.End);
            _endTextBox?.Focus();
        }
        else if (_status is { Current: Status.Start, Previous: Status.End })
        {
            SetSelectedStartDate(e.Date);
            _status.Reset();
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
        else if (_status is { Current: Status.End, Previous: Status.None })
        {
            SetSelectedEndDate(e.Date);
            if (GetStartDateOnly() is { } startDate && e.Date is { } end && end < startDate)
                SetSelectedStartDate(null);
            _status.Push(Status.Start);
            _startTextBox?.Focus();
        }
        else if (_status is { Current: Status.End, Previous: Status.Start })
        {
            SetSelectedEndDate(e.Date);
            _status.Reset();
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
    }

    private void SetCalendarContextDate()
    {
        var startDateOnly = GetStartDateOnly() ?? DateOnly.FromDateTime(DateTime.Today);
        _startCalendar?.SyncContextDate(new DatePickerCalendarContext(startDateOnly.Year, startDateOnly.Month));
        var endDateOnly = GetEndDateOnly() ?? startDateOnly;
        if (endDateOnly.Year == startDateOnly.Year && endDateOnly.Month == startDateOnly.Month)
            endDateOnly = endDateOnly.AddMonths(1);
        _endCalendar?.SyncContextDate(new DatePickerCalendarContext(endDateOnly.Year, endDateOnly.Month));
    }

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        base.OnLostFocus(e);
        var newItem = e.NewFocusedElement;
        if (Equals(newItem, _endTextBox) || Equals(newItem, _startTextBox)) return;
        if (newItem is Visual visual)
        {
            if (_popup?.IsInsidePopup(visual) == true) return;
        }
        else
        {
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
        CommitInput();
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (e.Source is Visual source && _popup?.IsInsidePopup(source) == true) return;
        if (_startTextBox?.IsFocused == false)
            _startTextBox?.Focus();
        else
        {
            SetCurrentValue(IsDropdownOpenProperty, true);
            SetCalendarContextDate();
        }
    }

    /// <summary>Synchronises the selected start/end values to the text boxes and calendars.</summary>
    protected abstract void SyncToUI();

    /// <summary>Parses text box content and commits results to the selected start/end date properties.</summary>
    protected abstract void CommitInput();

    /// <summary>Returns the <see cref="DateOnly"/> representation of the currently selected start date.</summary>
    protected abstract DateOnly? GetStartDateOnly();

    /// <summary>Returns the <see cref="DateOnly"/> representation of the currently selected end date.</summary>
    protected abstract DateOnly? GetEndDateOnly();

    /// <summary>Sets the selected start date from a calendar-picked <see cref="DateOnly"/> (or clears it when <see langword="null"/>).</summary>
    protected abstract void SetSelectedStartDate(DateOnly? date);

    /// <summary>Sets the selected end date from a calendar-picked <see cref="DateOnly"/> (or clears it when <see langword="null"/>).</summary>
    protected abstract void SetSelectedEndDate(DateOnly? date);
}
