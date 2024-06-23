using System.Globalization;
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

    public static readonly StyledProperty<bool> EnableMonthSyncProperty = AvaloniaProperty.Register<DateRangePicker, bool>(
        nameof(EnableMonthSync));

    public bool EnableMonthSync
    {
        get => GetValue(EnableMonthSyncProperty);
        set => SetValue(EnableMonthSyncProperty, value);
    }

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
            _startCalendar.DateSelected -= OnDateSelected;
            _startCalendar.DatePreviewed -= OnDatePreviewed;
            _startCalendar.ContextDateChanged -= OnContextDateChanged;
        }
        if (_endCalendar != null)
        {
            _endCalendar.DateSelected -= OnDateSelected;
            _endCalendar.DatePreviewed -= OnDatePreviewed;
            _endCalendar.ContextDateChanged -= OnContextDateChanged;
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
            _startCalendar.DateSelected += OnDateSelected;
            _startCalendar.DatePreviewed += OnDatePreviewed;
            _startCalendar.ContextDateChanged += OnContextDateChanged;
        }
        if (_endCalendar != null)
        {
            _endCalendar.DateSelected += OnDateSelected;
            _endCalendar.DatePreviewed += OnDatePreviewed;
            _endCalendar.ContextDateChanged += OnContextDateChanged;
        }
    }

    private void OnContextDateChanged(object sender, CalendarContext e)
    {
        if(sender == _startCalendar)
        {
            bool needsUpdate = EnableMonthSync || _startCalendar?.ContextDate.CompareTo(_endCalendar?.ContextDate) >= 0;
            if (needsUpdate)
            {
                _endCalendar?.SyncContextDate(_startCalendar?.ContextDate.NextMonth());
            }
        }
        else if(sender == _endCalendar)
        {
            bool needsUpdate = EnableMonthSync || _endCalendar?.ContextDate.CompareTo(_startCalendar?.ContextDate) <= 0;
            if (needsUpdate)
            {
                _startCalendar?.SyncContextDate(_endCalendar?.ContextDate.PreviousMonth());
            }
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
                SelectedEndDate = null;
            }
            SetCurrentValue(SelectedStartDateProperty, e.Date);
            _startTextBox?.SetValue(TextBox.TextProperty, e.Date?.ToString(DisplayFormat ?? "yyyy-MM-dd"));
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
                SelectedStartDate = null;
            }
            SetCurrentValue(SelectedEndDateProperty, e.Date);
            _endTextBox?.SetValue(TextBox.TextProperty, e.Date?.ToString(DisplayFormat ?? "yyyy-MM-dd"));
            _start = null;
            _previewStart = null;
            _previewEnd = null;
            _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
            _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
            if (SelectedStartDate is null)
            {
                _start = true;
                _startTextBox?.Focus();
            }
            else
            {
                SetCurrentValue(IsDropdownOpenProperty, false);
            }
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
                _startCalendar.ContextDate = new CalendarContext(date.Year, date.Month);
                _startCalendar.UpdateDayButtons();
            }

            if (_endCalendar is not null)
            {
                var date2 = SelectedEndDate;
                if (date2 is null || (date2.Value.Year==SelectedStartDate?.Year && date2.Value.Month == SelectedStartDate?.Month))
                {
                    date2 = SelectedStartDate ?? DateTime.Today;
                    date2 = date2.Value.AddMonths(1);
                }
                _endCalendar.ContextDate = new CalendarContext(date2?.Year, date2?.Month);
                _endCalendar.UpdateDayButtons();
            }
        }
        else if (sender == _endTextBox)
        {
            _start = false;
            if (_endCalendar is not null)
            {
                var date = SelectedEndDate ?? DateTime.Today;
                _endCalendar.ContextDate = new CalendarContext(date.Year, date.Month);
                _endCalendar.UpdateDayButtons();
            }
            if (_startCalendar is not null)
            {
                var date2 = SelectedStartDate;
                if (date2 is null || (date2.Value.Year==SelectedEndDate?.Year && date2.Value.Month == SelectedEndDate?.Month))
                {
                    date2 = SelectedStartDate ?? DateTime.Today;
                    date2 = date2.Value.AddMonths(-1);
                }
                _startCalendar.ContextDate = new CalendarContext(date2?.Year, date2?.Month);
                _startCalendar.UpdateDayButtons();
            }
            
        }
        SetCurrentValue(IsDropdownOpenProperty, true);
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender == _startTextBox)
        {
            OnTextChangedInternal(_startTextBox, SelectedStartDateProperty);
        }
        else if (sender == _endTextBox)
        {
            OnTextChangedInternal(_endTextBox, SelectedEndDateProperty);
        }
    }

    private void OnTextChangedInternal(TextBox? textBox, AvaloniaProperty property)
    {
        if (string.IsNullOrEmpty(textBox?.Text))
        {
            SetCurrentValue(property, null);
            _startCalendar?.ClearSelection(start: true);
            _endCalendar?.ClearSelection(end: true);
        }
        else if (DisplayFormat is null || DisplayFormat.Length == 0)
        {
            if (DateTime.TryParse(textBox?.Text, out var defaultTime))
            {
                SetCurrentValue(property, defaultTime);
                _startCalendar?.MarkDates(startDate: defaultTime, endDate: defaultTime);
                _endCalendar?.MarkDates(startDate: defaultTime, endDate: defaultTime);
            }
        }
        else
        {
            if (DateTime.TryParseExact(textBox?.Text, DisplayFormat, CultureInfo.CurrentUICulture, DateTimeStyles.None,
                    out var date))
            {
                SetCurrentValue(property, date);
                if (_startCalendar is not null)
                {
                    var date1 = SelectedStartDate ?? DateTime.Today;
                    _startCalendar.ContextDate = new CalendarContext(date1.Year, date.Month);
                    _startCalendar.UpdateDayButtons();
                    _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
                }
                if (_endCalendar is not null)
                {
                    var date2 = SelectedEndDate ?? SelectedStartDate ?? DateTime.Today;
                    if (SelectedEndDate is null) date2 = date2.AddMonths(1);
                    _endCalendar.ContextDate = new CalendarContext(date2.Year, date2.Month);
                    _endCalendar.UpdateDayButtons();
                    _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
                }
            }
        }
    }

    private void OnTextBoxGetFocus(object sender, GotFocusEventArgs e)
    {
        if (_startCalendar is not null)
        {
            var date = SelectedStartDate ?? DateTime.Today;
            _startCalendar.ContextDate = new CalendarContext(date.Year, date.Month);
            _startCalendar.UpdateDayButtons();
        }

        if (_endCalendar is not null)
        {
            var date2 = SelectedStartDate ?? DateTime.Today;
            date2 = date2.AddMonths(1);
            _endCalendar.ContextDate = new CalendarContext(date2.Year, date2.Month);
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