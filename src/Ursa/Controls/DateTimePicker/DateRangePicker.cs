using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_Button, typeof(Button))]
[TemplatePart(PART_Popup, typeof(Popup))]
[TemplatePart(PART_StartCalendar, typeof(CalendarView))]
[TemplatePart(PART_EndCalendar, typeof(CalendarView))]
[TemplatePart(PART_StartTextBox, typeof(TextBox))]
[TemplatePart(PART_EndTextBox, typeof(TextBox))]
public class DateRangePicker : DatePickerBase
{
    public const string PART_Button = "PART_Button";
    public const string PART_Popup = "PART_Popup";
    public const string PART_StartCalendar = "PART_StartCalendar";
    public const string PART_EndCalendar = "PART_EndCalendar";
    public const string PART_StartTextBox = "PART_StartTextBox";
    public const string PART_EndTextBox = "PART_EndTextBox";

    public static readonly StyledProperty<DateTime?> SelectedStartDateProperty =
        AvaloniaProperty.Register<DateRangePicker, DateTime?>(
            nameof(SelectedStartDate));

    public static readonly StyledProperty<DateTime?> SelectedEndDateProperty =
        AvaloniaProperty.Register<DateRangePicker, DateTime?>(
            nameof(SelectedEndDate));

    private Button? _button;
    private CalendarView? _endCalendar;
    private TextBox? _endTextBox;
    private Popup? _popup;
    private CalendarView? _startCalendar;
    private TextBox? _startTextBox;

    static DateRangePicker()
    {
        SelectedStartDateProperty.Changed.AddClassHandler<DateRangePicker, DateTime?>((picker, args) =>
            picker.OnSelectionChanged(args));
        SelectedEndDateProperty.Changed.AddClassHandler<DateRangePicker, DateTime?>((picker, args) =>
            picker.OnSelectionChanged(args));
    }

    public DateTime? SelectedStartDate
    {
        get => GetValue(SelectedStartDateProperty);
        set => SetValue(SelectedStartDateProperty, value);
    }

    public DateTime? SelectedEndDate
    {
        get => GetValue(SelectedEndDateProperty);
        set => SetValue(SelectedEndDateProperty, value);
    }


    private void OnSelectionChanged(AvaloniaPropertyChangedEventArgs<DateTime?> args)
    {
        if (args.Property == SelectedStartDateProperty)
        {
            if (args.NewValue.Value is null)
            {
                _startCalendar?.ClearSelection();
                _startTextBox?.Clear();
            }
            else
            {
                _startCalendar?.MarkDates(args.NewValue.Value, args.NewValue.Value);
                _startTextBox?.SetValue(TextBox.TextProperty,
                    args.NewValue.Value.Value.ToString(DisplayFormat ?? "yyyy-MM-dd"));
            }
        }
        else if (args.Property == SelectedEndDateProperty)
        {
            if (args.NewValue.Value is null)
            {
                _endCalendar?.ClearSelection();
                _endTextBox?.Clear();
            }
            else
            {
                _endCalendar?.MarkDates(args.NewValue.Value, args.NewValue.Value);
                _endTextBox?.SetValue(TextBox.TextProperty,
                    args.NewValue.Value.Value.ToString(DisplayFormat ?? "yyyy-MM-dd"));
            }
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        GotFocusEvent.RemoveHandler(OnTextBoxGetFocus, _startTextBox);
        TextBox.TextChangedEvent.RemoveHandler(OnTextChanged, _startTextBox, _endTextBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPointerPressed, _startTextBox, _endTextBox);
        Button.ClickEvent.RemoveHandler(OnButtonClick, _button);
        if (_startCalendar != null)
        {
            _startCalendar.OnDateSelected -= OnDateSelected;
            _startCalendar.OnDatePreviewed -= OnDatePreviewed;
        }
        if (_endCalendar != null)
        {
            _endCalendar.OnDateSelected -= OnDateSelected;
            _endCalendar.OnDatePreviewed -= OnDatePreviewed;
        }
        _button = e.NameScope.Find<Button>(PART_Button);
        _popup = e.NameScope.Find<Popup>(PART_Popup);
        _startCalendar = e.NameScope.Find<CalendarView>(PART_StartCalendar);
        _endCalendar = e.NameScope.Find<CalendarView>(PART_EndCalendar);
        _startTextBox = e.NameScope.Find<TextBox>(PART_StartTextBox);
        _endTextBox = e.NameScope.Find<TextBox>(PART_EndTextBox);
        
        Button.ClickEvent.AddHandler(OnButtonClick, RoutingStrategies.Tunnel, true, _button);
        GotFocusEvent.AddHandler(OnTextBoxGetFocus, _startTextBox, _endTextBox);
        TextBox.TextChangedEvent.AddHandler(OnTextChanged, _startTextBox, _endTextBox);
        PointerPressedEvent.AddHandler(OnTextBoxPointerPressed, RoutingStrategies.Tunnel, false, _startTextBox, _endTextBox);
        
        if (_startCalendar != null)
        {
            _startCalendar.OnDateSelected += OnDateSelected;
            _startCalendar.OnDatePreviewed += OnDatePreviewed;
        }
        if (_endCalendar != null)
        {
            _endCalendar.OnDateSelected += OnDateSelected;
            _endCalendar.OnDatePreviewed += OnDatePreviewed;
        }
    }

    private DateTime? _previewStart;
    private DateTime? _previewEnd;
    private bool? _start;
    
    private void OnDatePreviewed(object sender, CalendarDayButtonEventArgs e)
    {
        if (_start == true)
        {
            _previewStart = e.Date;
            _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
            _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
        }
        else if(_start == false)
        {
            _previewEnd = e.Date;
            _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
            _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
        }
    }

    private void OnDateSelected(object sender, CalendarDayButtonEventArgs e)
    {
        if (_start == true)
        {
            if (SelectedEndDate < e.Date)
            {
                // Select a start date that is out of current range, so clear selection.
                // _startCalendar?.ClearSelection();
                // _endCalendar?.ClearSelection();
                SelectedStartDate = null;
                SelectedEndDate = null;
            }
            SetCurrentValue(SelectedStartDateProperty, e.Date);
            _startTextBox?.SetValue(TextBox.TextProperty, e.Date.ToString(DisplayFormat ?? "yyyy-MM-dd"));
            _start = false;
            _previewStart = null;
            _previewEnd = null;
            _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
            _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
            _endTextBox?.Focus();
        }
        else if(_start == false)
        {
            if (SelectedStartDate > e.Date)
            {
                // Select a start date that is out of current range, so clear selection.
                // _startCalendar?.ClearSelection();
                // _endCalendar?.ClearSelection();
                SelectedStartDate = null;
                SelectedEndDate = null;
            }
            SetCurrentValue(SelectedEndDateProperty, e.Date);
            _endTextBox?.SetValue(TextBox.TextProperty, e.Date.ToString(DisplayFormat ?? "yyyy-MM-dd"));
            _start = null;
            _previewStart = null;
            _previewEnd = null;
            _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
            _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
        
    }

    private void OnButtonClick(object sender, RoutedEventArgs e)
    {
        Focus(NavigationMethod.Pointer);
        SetCurrentValue(IsDropdownOpenProperty, !IsDropdownOpen);
        _start = true;
    }

    private void OnTextBoxPointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (sender == _startTextBox)
        {
            _start = true;
            if (_startCalendar is not null)
            {
                var date = SelectedStartDate ?? DateTime.Today;
                _startCalendar.ContextCalendar = new CalendarContext(date.Year, date.Month, 1);
                _startCalendar.UpdateDayButtons();
            }

            if (_endCalendar is not null)
            {
                var date2 = SelectedStartDate ?? SelectedEndDate ?? DateTime.Today;
                date2 = SelectedStartDate is null ? date2 : date2.AddMonths(1);
                _endCalendar.ContextCalendar = new CalendarContext(date2.Year, date2.Month, 1);
                _endCalendar.UpdateDayButtons();
            }
        }
        else if (sender == _endTextBox)
        {
            _start = false;
            if (_startCalendar is not null)
            {
                var date2 = SelectedEndDate ?? DateTime.Today;
                date2 = date2.AddMonths(-1);
                _startCalendar.ContextCalendar = new CalendarContext(date2.Year, date2.Month, 1);
                _startCalendar.UpdateDayButtons();
            }
            if (_endCalendar is not null)
            {
                var date = SelectedEndDate ?? DateTime.Today;
                _endCalendar.ContextCalendar = new CalendarContext(date.Year, date.Month, 1);
                _endCalendar.UpdateDayButtons();
            }
        }
        SetCurrentValue(IsDropdownOpenProperty, true);
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        //throw new NotImplementedException();
    }

    private void OnTextBoxGetFocus(object sender, GotFocusEventArgs e)
    {
        if (_startCalendar is not null)
        {
            var date = SelectedStartDate ?? DateTime.Today;
            _startCalendar.ContextCalendar = new CalendarContext(date.Year, date.Month, 1);
            _startCalendar.UpdateDayButtons();
        }

        if (_endCalendar is not null)
        {
            var date2 = SelectedStartDate ?? DateTime.Today;
            date2 = date2.AddMonths(1);
            _endCalendar.ContextCalendar = new CalendarContext(date2.Year, date2.Month, 1);
            _endCalendar.UpdateDayButtons();
        }
        SetCurrentValue(IsDropdownOpenProperty, true);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        //base.OnLostFocus(e);
        //SetCurrentValue(IsDropdownOpenProperty, false);
        //_start = null;
    }
}