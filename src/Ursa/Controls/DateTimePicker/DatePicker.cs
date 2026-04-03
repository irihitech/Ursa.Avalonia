using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_Popup, typeof(Popup))]
[TemplatePart(PART_TextBox, typeof(TextBox))]
[TemplatePart(PART_Calendar, typeof(CalendarView))]
public class DatePicker : DatePickerBase, IClearControl
{
    public const string PART_Popup = "PART_Popup";
    public const string PART_TextBox = "PART_TextBox";
    public const string PART_Calendar = "PART_Calendar";
    private TextBox? _textBox;
    private CalendarView? _calendar;
    private Popup? _popup;

    public static readonly StyledProperty<DateTime?> SelectedDateProperty =
        AvaloniaProperty.Register<DatePicker, DateTime?>(
            nameof(SelectedDate), defaultBindingMode: BindingMode.TwoWay);

    public DateTime? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public static readonly StyledProperty<string?> PlaceholderTextProperty =
        TextBox.PlaceholderTextProperty.AddOwner<DatePicker>();

    public string? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty =
        TextBox.PlaceholderForegroundProperty.AddOwner<DatePicker>();

    public IBrush? PlaceholderForeground
    {
        get => GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
    }

    [Obsolete("Use PlaceholderTextProperty instead.")]
    public static readonly StyledProperty<string?> WatermarkProperty = PlaceholderTextProperty;

    [Obsolete("Use PlaceholderText instead.")]
    public string? Watermark
    {
        get => PlaceholderText;
        set => PlaceholderText = value;
    }

    static DatePicker()
    {
        SelectedDateProperty.Changed.AddClassHandler<DatePicker, DateTime?>((picker, args) =>
            picker.OnSelectionChanged(args));
    }

    private void OnSelectionChanged(AvaloniaPropertyChangedEventArgs<DateTime?> args)
    {
        SyncSelectedDateToText(args.NewValue.Value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        CalendarView.DateSelectedEvent.RemoveHandler(OnDateSelected, _calendar);
        GotFocusEvent.RemoveHandler(OnTextBoxGotFocus, _textBox);
        LostFocusEvent.RemoveHandler(OnTextBoxLostFocus, _textBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPressed, _textBox);

        _popup = e.NameScope.Find<Popup>(PART_Popup);
        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        _calendar = e.NameScope.Find<CalendarView>(PART_Calendar);
        
        CalendarView.DateSelectedEvent.AddHandler(OnDateSelected, RoutingStrategies.Bubble, true, _calendar);
        GotFocusEvent.AddHandler(OnTextBoxGotFocus, _textBox);
        LostFocusEvent.AddHandler(OnTextBoxLostFocus, _textBox);
        PointerPressedEvent.AddHandler(OnTextBoxPressed, RoutingStrategies.Tunnel, true, _textBox);
        SyncSelectedDateToText(SelectedDate);
    }

    private void OnTextBoxPressed(object? sender, PointerPressedEventArgs e)
    {
        InitializePopupOpen();
    }

    private void InitializePopupOpen()
    {
        SetCurrentValue(IsDropdownOpenProperty, true);
        SetCalendarContextDate();
        _calendar?.MarkDates(SelectedDate?.Date, SelectedDate?.Date);
    }

    private void OnTextBoxLostFocus(object? sender, FocusChangedEventArgs e)
    {
        CommitInput();
    }

    private void OnTextBoxGotFocus(object? sender, FocusChangedEventArgs e)
    {
        InitializePopupOpen();
    }

    private void SetCalendarContextDate()
    {
        var date = SelectedDate ?? DateTime.Today;
        _calendar?.SyncContextDate(new CalendarContext(date.Year, date.Month));
        _calendar?.UpdateDayButtons();
    }

    private void OnDateSelected(object? sender, CalendarDayButtonEventArgs e)
    {
        SetCurrentValue(SelectedDateProperty, e.Date);
        SetCurrentValue(IsDropdownOpenProperty, false);
    }
    
    private void SyncSelectedDateToText(DateTime? date)
    {
        if (date is null)
        {
            _textBox?.SetValue(TextBox.TextProperty, null);
            _calendar?.ClearSelection();
        }
        else
        {
            _textBox?.SetValue(TextBox.TextProperty, date.Value.ToString(DisplayFormat ?? "yyyy-MM-dd"));
            _calendar?.MarkDates(startDate: date.Value, endDate: date.Value);
        }
    }

    /// <inheritdoc/>
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (!e.Handled && e.Source is Visual source)
        {
            if (_popup?.IsInsidePopup(source) == true)
            {
                e.Handled = true;
            }
            else
            {
                SetCurrentValue(IsDropdownOpenProperty, true);
            }
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape:
                SetCurrentValue(IsDropdownOpenProperty, false);
                e.Handled = true;
                return;
            case Key.Down:
                SetCurrentValue(IsDropdownOpenProperty, true);
                e.Handled = true;
                return;
            case Key.Tab:
                SetCurrentValue(IsDropdownOpenProperty, false);
                return;
            case Key.Enter:
            {
                SetCurrentValue(IsDropdownOpenProperty, false);
                CommitInput();
                e.Handled = true;
                return;
            }
            default:
                base.OnKeyDown(e);
                break;
        }
    }

    public void Clear()
    {
        SetCurrentValue(SelectedDateProperty, null);
    }

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        base.OnLostFocus(e);
        var element = e.NewFocusedElement;
        if (Equals(element, _textBox))
        {
            return;
        }

        if (element is Visual v && _popup?.IsInsidePopup(v) == true)
        {
            return;
        }

        if (Equals(element, _textBox)) return;
        CommitInput();
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    private void CommitInput()
    {
        if (DateTime.TryParseExact(_textBox?.Text, DisplayFormat, CultureInfo.CurrentUICulture, DateTimeStyles.None,
                out var date))
        {
            SetCurrentValue(SelectedDateProperty, date);
            if (_calendar is not null)
            {
                _calendar.ContextDate = _calendar.ContextDate.With(year: date.Year, month: date.Month);
                _calendar.UpdateDayButtons();
            }

            _calendar?.MarkDates(startDate: date, endDate: date);
        }
    }

    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        base.UpdateDataValidation(property, state, error);
        if (property == SelectedDateProperty) DataValidationErrors.SetError(this, error);
    }
}