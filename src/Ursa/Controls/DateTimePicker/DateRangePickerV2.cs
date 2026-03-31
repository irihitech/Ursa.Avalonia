using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

[TemplatePart(PART_Popup, typeof(Popup))]
[TemplatePart(PART_StartCalendar, typeof(CalendarView))]
[TemplatePart(PART_EndCalendar, typeof(CalendarView))]
[TemplatePart(PART_StartTextBox, typeof(TextBox))]
[TemplatePart(PART_EndTextBox, typeof(TextBox))]
[PseudoClasses(PseudoClassName.PC_Empty)]
public class DateRangePickerV2: DatePickerBase, IClearControl
{
    public const string PART_Popup = "PART_Popup";
    public const string PART_StartCalendar = "PART_StartCalendar";
    public const string PART_EndCalendar = "PART_EndCalendar";
    public const string PART_StartTextBox = "PART_StartTextBox";
    public const string PART_EndTextBox = "PART_EndTextBox";

    public static readonly StyledProperty<DateTime?> SelectedStartDateProperty =
        AvaloniaProperty.Register<DateRangePickerV2, DateTime?>(
            nameof(SelectedStartDate), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<DateTime?> SelectedEndDateProperty =
        AvaloniaProperty.Register<DateRangePickerV2, DateTime?>(
            nameof(SelectedEndDate), defaultBindingMode: BindingMode.TwoWay);
    
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
    
    private CalendarView? _startCalendar;
    private CalendarView? _endCalendar;
    private TextBox? _startTextBox;
    private TextBox? _endTextBox;
    private Popup? _popup;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _startCalendar?.RemoveHandler(CalendarView.DateSelectedEvent, OnStartDateSelected);
        _endCalendar?.RemoveHandler(CalendarView.DateSelectedEvent, OnEndDateSelected);
        
        _startCalendar = e.NameScope.Find<CalendarView>(PART_StartCalendar);
        _endCalendar = e.NameScope.Find<CalendarView>(PART_EndCalendar);
        _startTextBox = e.NameScope.Find<TextBox>(PART_StartTextBox);
        _endTextBox = e.NameScope.Find<TextBox>(PART_EndTextBox);
        _popup = e.NameScope.Find<Popup>(PART_Popup);
        
        _startCalendar?.AddHandler(CalendarView.DateSelectedEvent, OnStartDateSelected);
        _endCalendar?.AddHandler(CalendarView.DateSelectedEvent, OnEndDateSelected);
        _startTextBox?.AddHandler(GotFocusEvent, OnTextBoxGotFocus);
        


    }

    private void OnTextBoxGotFocus(object? sender, FocusChangedEventArgs e)
    {
        SetCurrentValue(IsDropdownOpenProperty, true);
    }


    private void OnStartDateSelected(object? sender, CalendarDayButtonEventArgs e)
    {
        SetValue(SelectedStartDateProperty, e.Date);
        //_popup?.Close();
    }
    
    
    private void OnEndDateSelected(object? sender, CalendarDayButtonEventArgs e)
    {
        SetValue(SelectedEndDateProperty, e.Date);
        _endTextBox?.Focus();
    }

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        base.OnLostFocus(e);
        var newItem = e.NewFocusedElement;
        if (newItem == _endCalendar)
        {
            return;
        }
        else
        {
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        //SetCurrentValue(IsDropdownOpenProperty, true);
        _startTextBox?.Focus();
        //SetCurrentValue(IsDropdownOpenProperty, true);
    }

    public void Clear()
    {
        SetCurrentValue(SelectedStartDateProperty, null);
        SetCurrentValue(SelectedEndDateProperty, null);
    }
}