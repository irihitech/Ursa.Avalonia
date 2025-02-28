using System.Globalization;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_Button, typeof(Button))]
[TemplatePart(PART_Popup, typeof(Popup))]
[TemplatePart(PART_TextBox, typeof(TextBox))]
[TemplatePart(PART_Calendar, typeof(CalendarView))]
public class DatePicker: DatePickerBase, IClearControl
{
    public const string PART_Button = "PART_Button";
    public const string PART_Popup = "PART_Popup";
    public const string PART_TextBox = "PART_TextBox";
    public const string PART_Calendar = "PART_Calendar";
    private Button? _button;
    private TextBox? _textBox;
    private CalendarView? _calendar;
    private Popup? _popup;

    public static readonly StyledProperty<DateTime?> SelectedDateProperty = AvaloniaProperty.Register<DatePicker, DateTime?>(
        nameof(SelectedDate), defaultBindingMode: BindingMode.TwoWay);

    public DateTime? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public static readonly StyledProperty<string?> WatermarkProperty = AvaloniaProperty.Register<DatePicker, string?>(
        nameof(Watermark));

    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    static DatePicker()
    {
        FocusableProperty.OverrideDefaultValue<DatePicker>(true);
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

        GotFocusEvent.RemoveHandler(OnTextBoxGetFocus, _textBox);
        TextBox.TextChangedEvent.RemoveHandler(OnTextChanged, _textBox);
        Button.ClickEvent.RemoveHandler(OnButtonClick, _button);
        CalendarView.DateSelectedEvent.RemoveHandler(OnDateSelected, _calendar);
        
        _button = e.NameScope.Find<Button>(PART_Button);
        _popup = e.NameScope.Find<Popup>(PART_Popup);
        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        _calendar = e.NameScope.Find<CalendarView>(PART_Calendar);
        
        Button.ClickEvent.AddHandler(OnButtonClick, RoutingStrategies.Bubble, false, _button);
        GotFocusEvent.AddHandler(OnTextBoxGetFocus, _textBox);
        TextBox.TextChangedEvent.AddHandler(OnTextChanged, _textBox);
        CalendarView.DateSelectedEvent.AddHandler(OnDateSelected, RoutingStrategies.Bubble, true, _calendar);
        SyncSelectedDateToText(SelectedDate);
    }

    private void OnDateSelected(object? sender, CalendarDayButtonEventArgs e)
    {
        SetCurrentValue(SelectedDateProperty, e.Date);
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if(IsFocused)
        {
            SetCurrentValue(IsDropdownOpenProperty, !IsDropdownOpen);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        SetSelectedDate(true);
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

    private void SetSelectedDate(bool fromText = false)
    {
        if (string.IsNullOrEmpty(_textBox?.Text))
        {
            SetCurrentValue(SelectedDateProperty, null);
            _calendar?.ClearSelection();
        }
        else if (DisplayFormat is null || DisplayFormat.Length == 0)
        {
            if (DateTime.TryParse(_textBox?.Text, out var defaultTime))
            {
                SetCurrentValue(SelectedDateProperty, defaultTime);
                _calendar?.MarkDates(startDate: defaultTime, endDate: defaultTime);
            }
        }
        else
        {
            CommitInput(!fromText);
        }
    }

    private void OnTextBoxGetFocus(object? sender, GotFocusEventArgs e)
    {
        if (_calendar is not null)
        {
            var date = SelectedDate ?? DateTime.Today;
            _calendar.ContextDate = _calendar.ContextDate.With(year: date.Year, month: date.Month);
            _calendar.UpdateDayButtons();
        }
        SetCurrentValue(IsDropdownOpenProperty, true);
    }
    
    /// <inheritdoc/>
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if(!e.Handled && e.Source is Visual source)
        {
            if (_popup?.IsInsidePopup(source) == true)
            {
                e.Handled = true;
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
                CommitInput(true);
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
    
    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);
        FocusChanged(IsKeyboardFocusWithin);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        FocusChanged(IsKeyboardFocusWithin);
        var top = TopLevel.GetTopLevel(this);
        var element = top?.FocusManager?.GetFocusedElement();
        if (element is Visual v && _popup?.IsInsidePopup(v) == true)return;
        if (Equals(element, _textBox))return;
        CommitInput(true);
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    private bool _isFocused;
    private void FocusChanged(bool hasFocus)
    {
        bool wasFocused = _isFocused;
        _isFocused = hasFocus;

        if (hasFocus)
        {
            if (!wasFocused && _textBox != null)
            {
                _textBox.Focus();
            }
        }
    }

    private void CommitInput(bool clearWhenInvalid)
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
        else
        {
            if (clearWhenInvalid)
            {
                SetCurrentValue(SelectedDateProperty, null);
            }
            _calendar?.ClearSelection();
        }
    }
}