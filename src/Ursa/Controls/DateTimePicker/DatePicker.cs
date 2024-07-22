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
    private Popup? _popup;
    private TextBox? _textBox;
    private CalendarView? _calendar;

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
        SelectedDateProperty.Changed.AddClassHandler<DatePicker, DateTime?>((picker, args) =>
            picker.OnSelectionChanged(args));
    }

    private void OnSelectionChanged(AvaloniaPropertyChangedEventArgs<DateTime?> args)
    {
        if (args.NewValue.Value is null)
        {
            _calendar?.ClearSelection();
            _textBox?.Clear();
        }
        else
        {
            _calendar?.MarkDates(startDate: args.NewValue.Value, endDate: args.NewValue.Value);
            _textBox?.SetValue(TextBox.TextProperty, args.NewValue.Value.Value.ToString(DisplayFormat ?? "yyyy-MM-dd"));
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        GotFocusEvent.RemoveHandler(OnTextBoxGetFocus, _textBox);
        TextBox.TextChangedEvent.RemoveHandler(OnTextChanged, _textBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPointerPressed, _textBox);
        Button.ClickEvent.RemoveHandler(OnButtonClick, _button);
        if (_calendar != null)
        {
            _calendar.DateSelected -= OnDateSelected;
        }
        
        _button = e.NameScope.Find<Button>(PART_Button);
        _popup = e.NameScope.Find<Popup>(PART_Popup);
        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        _calendar = e.NameScope.Find<CalendarView>(PART_Calendar);
        
        Button.ClickEvent.AddHandler(OnButtonClick, RoutingStrategies.Bubble, true, _button);
        GotFocusEvent.AddHandler(OnTextBoxGetFocus, _textBox);
        TextBox.TextChangedEvent.AddHandler(OnTextChanged, _textBox);
        PointerPressedEvent.AddHandler(OnTextBoxPointerPressed, RoutingStrategies.Tunnel, false, _textBox);
        
        if (_calendar != null)
        {
            _calendar.DateSelected += OnDateSelected;
        }
    }

    private void OnDateSelected(object sender, CalendarDayButtonEventArgs e)
    {
        SetCurrentValue(SelectedDateProperty, e.Date);
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    private void OnButtonClick(object sender, RoutedEventArgs e)
    {
        Focus(NavigationMethod.Pointer);
        SetCurrentValue(IsDropdownOpenProperty, !IsDropdownOpen);
    }

    private void OnTextBoxPointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (_calendar is not null)
        {
            var date = SelectedDate ?? DateTime.Today;
            _calendar.ContextDate = new CalendarContext(date.Year, date.Month);
            _calendar.UpdateDayButtons();
        }
        SetCurrentValue(IsDropdownOpenProperty, true);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        SetSelectedDate(true);
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
            if (DateTime.TryParseExact(_textBox?.Text, DisplayFormat, CultureInfo.CurrentUICulture, DateTimeStyles.None,
                    out var date))
            {
                SetCurrentValue(SelectedDateProperty, date);
                if (_calendar is not null)
                {
                    var d = SelectedDate ?? DateTime.Today;
                    _calendar.ContextDate = _calendar.ContextDate.With(year: date.Year, month: date.Month);
                    _calendar.UpdateDayButtons();
                }
                _calendar?.MarkDates(startDate: date, endDate: date);
            }
            else
            {
                SetCurrentValue(SelectedDateProperty, null);
                if (!fromText)
                {
                    _textBox?.SetValue(TextBox.TextProperty, null);
                }
                _calendar?.ClearSelection();
            }
        }
    }

    private void OnTextBoxGetFocus(object sender, GotFocusEventArgs e)
    {
        if (_calendar is not null)
        {
            var date = SelectedDate ?? DateTime.Today;
            _calendar.ContextDate = _calendar.ContextDate.With(year: date.Year, month: date.Month);
            _calendar.UpdateDayButtons();
        }
        SetCurrentValue(IsDropdownOpenProperty, true);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        SetCurrentValue(IsDropdownOpenProperty, false);
        SetSelectedDate();
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
            SetCurrentValue(IsDropdownOpenProperty, false);
            return;
        }

        base.OnKeyDown(e);
    }

    public void Clear()
    {
        SetCurrentValue(SelectedDateProperty, null);
    }
}