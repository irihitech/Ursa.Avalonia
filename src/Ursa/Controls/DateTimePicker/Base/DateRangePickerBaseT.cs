using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public abstract class DateRangePickerBase<T> : DateRangePickerBase where T : struct
{
    public static readonly StyledProperty<T?> SelectedStartDateProperty =
        AvaloniaProperty.Register<DateRangePickerBase<T>, T?>(
            nameof(SelectedStartDate), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    public static readonly StyledProperty<T?> SelectedEndDateProperty =
        AvaloniaProperty.Register<DateRangePickerBase<T>, T?>(
            nameof(SelectedEndDate), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    public T? SelectedStartDate
    {
        get => GetValue(SelectedStartDateProperty);
        set => SetValue(SelectedStartDateProperty, value);
    }

    public T? SelectedEndDate
    {
        get => GetValue(SelectedEndDateProperty);
        set => SetValue(SelectedEndDateProperty, value);
    }

    private DatePickerCalendarView? _startCalendar;
    private DatePickerCalendarView? _endCalendar;
    private TextBox? _startTextBox;
    private TextBox? _endTextBox;
    private Popup? _popup;
    private readonly RangePickerStatus _status = new();

    /// <summary>Converts a <typeparamref name="T"/> value to <see cref="DateOnly"/> for calendar operations.</summary>
    protected abstract DateOnly? ToDateOnly(T? value);

    /// <summary>Creates a <typeparamref name="T"/> value from a <see cref="DateOnly"/> (at day start).</summary>
    protected abstract T FromDateOnly(DateOnly date);

    /// <summary>Parses a text string with the given format into a <typeparamref name="T"/> value, or <see langword="null"/> on failure.</summary>
    protected abstract T? Parse(string? text, string? format);

    /// <summary>Formats a <typeparamref name="T"/> value to a display string using the given format.</summary>
    protected abstract string? Format(T? value, string? format);

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

        SyncDateToText();
        PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedStartDate is null && SelectedEndDate is null);
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
        _startCalendar?.MarkDates(ToDateOnly(SelectedStartDate), ToDateOnly(SelectedEndDate));
        _endCalendar?.MarkDates(ToDateOnly(SelectedStartDate), ToDateOnly(SelectedEndDate));
    }

    private void OnDatePreviewed(object? sender, DatePickerCalendarDayButtonEventArgs e)
    {
        if (_status.Current == Status.Start)
        {
            _startCalendar?.MarkDates(ToDateOnly(SelectedStartDate), ToDateOnly(SelectedEndDate), e.Date);
            _endCalendar?.MarkDates(ToDateOnly(SelectedStartDate), ToDateOnly(SelectedEndDate), e.Date);
        }
        else if (_status.Current == Status.End)
        {
            _startCalendar?.MarkDates(ToDateOnly(SelectedStartDate), ToDateOnly(SelectedEndDate), null, e.Date);
            _endCalendar?.MarkDates(ToDateOnly(SelectedStartDate), ToDateOnly(SelectedEndDate), null, e.Date);
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
            SetCurrentValue(SelectedStartDateProperty,
                e.Date.HasValue ? (T?)FromDateOnly(e.Date.Value) : null);
            if (ToDateOnly(SelectedEndDate) is { } endDate && e.Date is { } start && start > endDate)
                SetCurrentValue(SelectedEndDateProperty, (T?)null);
            _status.Push(Status.End);
            _endTextBox?.Focus();
        }
        else if (_status is { Current: Status.Start, Previous: Status.End })
        {
            SetCurrentValue(SelectedStartDateProperty,
                e.Date.HasValue ? (T?)FromDateOnly(e.Date.Value) : null);
            _status.Reset();
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
        else if (_status is { Current: Status.End, Previous: Status.None })
        {
            SetCurrentValue(SelectedEndDateProperty,
                e.Date.HasValue ? (T?)FromDateOnly(e.Date.Value) : null);
            if (ToDateOnly(SelectedStartDate) is { } startDate && e.Date is { } end && end < startDate)
                SetCurrentValue(SelectedStartDateProperty, (T?)null);
            _status.Push(Status.Start);
            _startTextBox?.Focus();
        }
        else if (_status is { Current: Status.End, Previous: Status.Start })
        {
            SetCurrentValue(SelectedEndDateProperty,
                e.Date.HasValue ? (T?)FromDateOnly(e.Date.Value) : null);
            _status.Reset();
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
    }

    private void SetCalendarContextDate()
    {
        var startDateOnly = ToDateOnly(SelectedStartDate) ?? DateOnly.FromDateTime(DateTime.Today);
        _startCalendar?.SyncContextDate(new DatePickerCalendarContext(startDateOnly.Year, startDateOnly.Month));
        var endDateOnly = ToDateOnly(SelectedEndDate) ?? startDateOnly;
        if (endDateOnly.Year == startDateOnly.Year && endDateOnly.Month == startDateOnly.Month)
            endDateOnly = endDateOnly.AddMonths(1);
        _endCalendar?.SyncContextDate(new DatePickerCalendarContext(endDateOnly.Year, endDateOnly.Month));
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            SetCurrentValue(IsDropdownOpenProperty, false);
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Down)
        {
            SetCurrentValue(IsDropdownOpenProperty, true);
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Tab)
        {
            if (Equals(e.Source, _endTextBox)) SetCurrentValue(IsDropdownOpenProperty, false);
            return;
        }

        if (e.Key == Key.Enter)
        {
            SetCurrentValue(IsDropdownOpenProperty, false);
            CommitInput();
            e.Handled = true;
            return;
        }

        base.OnKeyDown(e);
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
        SetCurrentValue(IsDropdownOpenProperty, false);
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

    private void SyncDateToText()
    {
        _startTextBox?.SetCurrentValue(TextBox.TextProperty,
            Format(SelectedStartDate, DisplayFormat ?? DEFAULT_DATE_DISPLAY_FORMAT) ?? string.Empty);
        _endTextBox?.SetCurrentValue(TextBox.TextProperty,
            Format(SelectedEndDate, DisplayFormat ?? DEFAULT_DATE_DISPLAY_FORMAT) ?? string.Empty);
    }

    private void CommitInput()
    {
        var format = DisplayFormat ?? DEFAULT_DATE_DISPLAY_FORMAT;

        if (string.IsNullOrWhiteSpace(_startTextBox?.Text))
            SetCurrentValue(SelectedStartDateProperty, (T?)null);
        else
            SetCurrentValue(SelectedStartDateProperty, Parse(_startTextBox?.Text, format));

        if (string.IsNullOrWhiteSpace(_endTextBox?.Text))
            SetCurrentValue(SelectedEndDateProperty, (T?)null);
        else
            SetCurrentValue(SelectedEndDateProperty, Parse(_endTextBox?.Text, format));

        _startCalendar?.MarkDates(ToDateOnly(SelectedStartDate), ToDateOnly(SelectedEndDate));
        _endCalendar?.MarkDates(ToDateOnly(SelectedStartDate), ToDateOnly(SelectedEndDate));
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == SelectedStartDateProperty || change.Property == SelectedEndDateProperty)
        {
            SyncDateToText();
            _startCalendar?.MarkDates(ToDateOnly(SelectedStartDate), ToDateOnly(SelectedEndDate));
            _endCalendar?.MarkDates(ToDateOnly(SelectedStartDate), ToDateOnly(SelectedEndDate));
            PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedStartDate is null && SelectedEndDate is null);
        }
    }

    public override void Clear()
    {
        SetCurrentValue(SelectedStartDateProperty, (T?)null);
        SetCurrentValue(SelectedEndDateProperty, (T?)null);
        _status.Reset();
    }
}
