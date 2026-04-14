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
[TemplatePart(PART_TextBox, typeof(TextBox))]
[TemplatePart(PART_Calendar, typeof(DatePickerCalendarView))]
[TemplatePart(PART_TimePicker, typeof(TimePickerPresenter))]
[PseudoClasses(PseudoClassName.PC_Empty)]
public abstract class DateTimePickerBase : TemplatedControl, IInnerContentControl, IPopupInnerContent
{
    public const string PART_Popup = "PART_Popup";
    public const string PART_TextBox = "PART_TextBox";
    public const string PART_Calendar = "PART_Calendar";
    public const string PART_TimePicker = "PART_TimePicker";

    protected const string DEFAULT_DATETIME_DISPLAY_FORMAT = "yyyy-MM-dd HH:mm:ss";

    public static readonly StyledProperty<string?> DisplayFormatProperty =
        AvaloniaProperty.Register<DateTimePickerBase, string?>(
            nameof(DisplayFormat), DEFAULT_DATETIME_DISPLAY_FORMAT);

    public static readonly StyledProperty<AvaloniaList<DateRange>> BlackoutDatesProperty =
        AvaloniaProperty.Register<DateTimePickerBase, AvaloniaList<DateRange>>(nameof(BlackoutDates));

    public static readonly StyledProperty<IDateSelector?> BlackoutDateRuleProperty =
        AvaloniaProperty.Register<DateTimePickerBase, IDateSelector?>(nameof(BlackoutDateRule));

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        AvaloniaProperty.Register<DateTimePickerBase, DayOfWeek>(
            nameof(FirstDayOfWeek), DateTimeHelper.GetCurrentDateTimeFormatInfo().FirstDayOfWeek);

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty =
        AvaloniaProperty.Register<DateTimePickerBase, bool>(nameof(IsTodayHighlighted), true);

    public static readonly StyledProperty<object?> InnerLeftContentProperty =
        AvaloniaProperty.Register<DateTimePickerBase, object?>(nameof(InnerLeftContent));

    public static readonly StyledProperty<object?> InnerRightContentProperty =
        AvaloniaProperty.Register<DateTimePickerBase, object?>(nameof(InnerRightContent));

    public static readonly StyledProperty<object?> PopupInnerTopContentProperty =
        AvaloniaProperty.Register<DateTimePickerBase, object?>(nameof(PopupInnerTopContent));

    public static readonly StyledProperty<object?> PopupInnerBottomContentProperty =
        AvaloniaProperty.Register<DateTimePickerBase, object?>(nameof(PopupInnerBottomContent));

    public static readonly StyledProperty<bool> IsDropdownOpenProperty =
        AvaloniaProperty.Register<DateTimePickerBase, bool>(
            nameof(IsDropdownOpen), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> IsReadonlyProperty =
        AvaloniaProperty.Register<DateTimePickerBase, bool>(nameof(IsReadonly));

    protected TextBox? _textBox;
    protected DatePickerCalendarView? _calendar;
    protected Popup? _popup;
    protected TimePickerPresenter? _timePickerPresenter;

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

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        TimePickerPresenter.SelectedTimeChangedEvent.RemoveHandler(OnTimeSelected, _timePickerPresenter);
        DatePickerCalendarView.DateSelectedEvent.RemoveHandler(OnDateSelected, _calendar);
        GotFocusEvent.RemoveHandler(OnTextBoxGotFocus, _textBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPressed, _textBox);
        LostFocusEvent.RemoveHandler(OnTextBoxLostFocus, _textBox);

        _popup = e.NameScope.Find<Popup>(PART_Popup);
        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        _calendar = e.NameScope.Find<DatePickerCalendarView>(PART_Calendar);
        _timePickerPresenter = e.NameScope.Find<TimePickerPresenter>(PART_TimePicker);

        TimePickerPresenter.SelectedTimeChangedEvent.AddHandler(OnTimeSelected, _timePickerPresenter);
        DatePickerCalendarView.DateSelectedEvent.AddHandler(OnDateSelected, _calendar);
        GotFocusEvent.AddHandler(OnTextBoxGotFocus, _textBox);
        PointerPressedEvent.AddHandler(OnTextBoxPressed, RoutingStrategies.Tunnel, true, _textBox);
        LostFocusEvent.AddHandler(OnTextBoxLostFocus, _textBox);

        SyncToUI();
    }

    private void OnTextBoxLostFocus(object? sender, FocusChangedEventArgs e) => CommitInput();
    private void OnTextBoxGotFocus(object? sender, FocusChangedEventArgs e) => InitializePopupOpen();
    private void OnTextBoxPressed(object? sender, PointerPressedEventArgs e) => InitializePopupOpen();

    private void InitializePopupOpen()
    {
        SetCurrentValue(IsDropdownOpenProperty, true);
        var date = GetCalendarContextDate();
        _calendar?.SyncContextDate(new DatePickerCalendarContext(date.Year, date.Month));
        _timePickerPresenter?.SyncTime(GetSelectedTimeOnly());
    }

    private void OnDateSelected(object? sender, DatePickerCalendarDayButtonEventArgs e)
    {
        if (e.Date is null) return;
        OnCalendarDateSelected(e.Date.Value);
    }

    private void OnTimeSelected(object? sender, TimeChangedEventArgs e)
    {
        if (e.NewTime is null) return;
        OnTimePickerTimeSelected(e.NewTime.Value);
    }

    /// <summary>Synchronises the current selected value to the text box, calendar, and time picker UI.</summary>
    protected abstract void SyncToUI();

    /// <summary>Parses the text box content and commits the result to the selected-date property.</summary>
    protected abstract void CommitInput();

    /// <summary>Returns the date component of the currently selected value, for calendar marking.</summary>
    protected abstract DateOnly? GetSelectedDateOnly();

    /// <summary>Returns the time component of the currently selected value, for time picker sync.</summary>
    protected abstract TimeOnly? GetSelectedTimeOnly();

    /// <summary>Returns the date to show in the calendar when it opens.</summary>
    protected abstract DateOnly GetCalendarContextDate();

    /// <summary>Called when the user picks a day on the calendar popup.</summary>
    protected abstract void OnCalendarDateSelected(DateOnly date);

    /// <summary>Called when the user picks a time in the time picker popup.</summary>
    protected abstract void OnTimePickerTimeSelected(TimeOnly time);

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (!e.Handled && e.Source is Visual source)
        {
            if (_popup?.IsInsidePopup(source) == true)
                e.Handled = true;
            else
                InitializePopupOpen();
        }
    }

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        base.OnLostFocus(e);
        var newItem = e.NewFocusedElement;
        if (Equals(newItem, _textBox)) return;
        if (newItem is Visual visual)
        {
            if (_popup?.IsInsidePopup(visual) == true) return;
            CommitInput();
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
    }
}
