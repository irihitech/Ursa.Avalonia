using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_StartTextBox, typeof(TextBox))]
[TemplatePart(PART_EndTextBox, typeof(TextBox))]
[TemplatePart(PartNames.PART_Popup, typeof(Popup))]
[TemplatePart(PART_StartPresenter, typeof(TimePickerPresenter))]
[TemplatePart(PART_EndPresenter, typeof(TimePickerPresenter))]
[PseudoClasses(PseudoClassName.PC_Empty)]
public class TimeRangePicker : TimePickerBase, IClearControl
{
    public const string PART_StartTextBox = "PART_StartTextBox";
    public const string PART_EndTextBox = "PART_EndTextBox";
    public const string PART_StartPresenter = "PART_StartPresenter";
    public const string PART_EndPresenter = "PART_EndPresenter";


    public static readonly StyledProperty<TimeSpan?> StartTimeProperty =
        AvaloniaProperty.Register<TimeRangePicker, TimeSpan?>(
            nameof(StartTime), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<TimeSpan?> EndTimeProperty =
        AvaloniaProperty.Register<TimeRangePicker, TimeSpan?>(
            nameof(EndTime), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<string?> StartPlaceholderTextProperty =
        TextBox.PlaceholderTextProperty.AddOwner<TimeRangePicker>();

    public static readonly StyledProperty<string?> EndPlaceholderTextProperty =
        TextBox.PlaceholderTextProperty.AddOwner<TimeRangePicker>();

    public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty =
        TextBox.PlaceholderForegroundProperty.AddOwner<TimeRangePicker>();

    [Obsolete("Use StartPlaceholderTextProperty instead.")]
    public static readonly StyledProperty<string?> StartWatermarkProperty = StartPlaceholderTextProperty;

    [Obsolete("Use EndPlaceholderTextProperty instead.")]
    public static readonly StyledProperty<string?> EndWatermarkProperty = EndPlaceholderTextProperty;

    private TimePickerPresenter? _endPresenter;
    private TextBox? _endTextBox;
    private bool _isFocused;
    private Popup? _popup;
    private TimePickerPresenter? _startPresenter;

    private TextBox? _startTextBox;
    private RangePickerStatus _status = new();

    static TimeRangePicker()
    {
        StartTimeProperty.Changed.AddClassHandler<TimeRangePicker, TimeSpan?>((picker, args) =>
            picker.OnSelectionChanged(args));
        EndTimeProperty.Changed.AddClassHandler<TimeRangePicker, TimeSpan?>((picker, args) =>
            picker.OnSelectionChanged(args, false));
        DisplayFormatProperty.Changed.AddClassHandler<TimeRangePicker, string?>((picker, args) =>
            picker.OnDisplayFormatChanged(args));
    }


    public string? StartPlaceholderText
    {
        get => GetValue(StartPlaceholderTextProperty);
        set => SetValue(StartPlaceholderTextProperty, value);
    }

    public string? EndPlaceholderText
    {
        get => GetValue(EndPlaceholderTextProperty);
        set => SetValue(EndPlaceholderTextProperty, value);
    }

    public TimeSpan? StartTime
    {
        get => GetValue(StartTimeProperty);
        set => SetValue(StartTimeProperty, value);
    }

    public TimeSpan? EndTime
    {
        get => GetValue(EndTimeProperty);
        set => SetValue(EndTimeProperty, value);
    }

    public IBrush? PlaceholderForeground
    {
        get => GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
    }

    [Obsolete("Use StartPlaceholderText instead.")]
    public string? StartWatermark
    {
        get => StartPlaceholderText;
        set => StartPlaceholderText = value;
    }

    [Obsolete("Use EndPlaceholderText instead.")]
    public string? EndWatermark
    {
        get => EndPlaceholderText;
        set => EndPlaceholderText = value;
    }

    public void Clear()
    {
        SetCurrentValue(StartTimeProperty, null);
        SetCurrentValue(EndTimeProperty, null);
        _startPresenter?.SyncTime(null);
        _endPresenter?.SyncTime(null);
    }

    private void OnDisplayFormatChanged(AvaloniaPropertyChangedEventArgs<string?> args)
    {
        if (_startTextBox is not null) SyncTimeToText(StartTime);
        if (_endTextBox is not null) SyncTimeToText(EndTime, false);
    }

    private void OnSelectionChanged(AvaloniaPropertyChangedEventArgs<TimeSpan?> args, bool start = true)
    {
        SyncTimeToText(args.NewValue.Value, start);
        //SyncTimeToText(args.NewValue.Value);
        PseudoClasses.Set(PseudoClassName.PC_Empty, StartTime is null && EndTime is null);
    }

    private void SyncTimeToText(TimeSpan? time, bool start = true)
    {
        var textBox = start ? _startTextBox : _endTextBox;
        if (textBox is null) return;
        if (time is null)
        {
            textBox.Text = null;
            return;
        }

        var date = new DateTime(1, 1, 1, time.Value.Hours, time.Value.Minutes, time.Value.Seconds);
        var text = date.ToString(DisplayFormat);
        textBox.Text = text;
        PseudoClasses.Set(PseudoClassName.PC_Empty, StartTime is null && EndTime is null);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        
        TimePickerPresenter.SelectedTimeChangedEvent.RemoveHandler(OnTimeSelected, _startPresenter,
            _endPresenter);
        GotFocusEvent.RemoveHandler(OnTextBoxGetFocus, _startTextBox, _endTextBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPressed, _startTextBox, _endTextBox);
        LostFocusEvent.RemoveHandler(OnTextBoxLostFocus, _startTextBox, _endTextBox);

        _popup = e.NameScope.Find<Popup>(PartNames.PART_Popup);
        _startTextBox = e.NameScope.Find<TextBox>(PART_StartTextBox);
        _endTextBox = e.NameScope.Find<TextBox>(PART_EndTextBox);
        _startPresenter = e.NameScope.Find<TimePickerPresenter>(PART_StartPresenter);
        _endPresenter = e.NameScope.Find<TimePickerPresenter>(PART_EndPresenter);
        
        TimePickerPresenter.SelectedTimeChangedEvent.AddHandler(OnTimeSelected, _startPresenter, _endPresenter);
        GotFocusEvent.AddHandler(OnTextBoxGetFocus, _startTextBox, _endTextBox);
        PointerPressedEvent.AddHandler(OnTextBoxPressed, RoutingStrategies.Tunnel, true, _startTextBox, _endTextBox);
        LostFocusEvent.AddHandler(OnTextBoxLostFocus, _startTextBox, _endTextBox); 

        _startPresenter?.SyncTime(StartTime);
        _endPresenter?.SyncTime(EndTime);
        SyncTimeToText(StartTime);
        SyncTimeToText(EndTime, false);
    }

    private void OnTextBoxLostFocus(object? sender, FocusChangedEventArgs e)
    {
        CommitInput();
        if (_status.Current == Status.End && _status.Previous == Status.Start && sender == _endTextBox && _endTextBox?.IsFocused == true)
        {
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
    }

    private void OnTextBoxPressed(object? sender, PointerPressedEventArgs e)
    {
        InitializePopupOpen(sender as TextBox);
    }

    private void InitializePopupOpen(TextBox? sender)
    {
        if (sender is null) return;
        SetCurrentValue(IsDropdownOpenProperty, true);
        if (Equals(sender, _startTextBox))
        {
            _status.Push(Status.Start);
        }
        else if (Equals(sender, _endTextBox))
        {
            _status.Push(Status.End);
        }
        _startPresenter?.SyncTime(StartTime);
        _endPresenter?.SyncTime(EndTime);
    }

    private void OnTimeSelected(object? sender, TimeChangedEventArgs e)
    {
        if (Equals(sender, _startPresenter))
        {
            SetCurrentValue(StartTimeProperty, e.NewTime);
        }
        else if (Equals(sender, _endPresenter))
        {
            SetCurrentValue(EndTimeProperty, e.NewTime);
        }
    }

    private void OnTextBoxGetFocus(object? sender, FocusChangedEventArgs e)
    {
        InitializePopupOpen(sender as TextBox);
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

    public void Confirm()
    {
        _startPresenter?.Confirm();
        _endPresenter?.Confirm();
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    public void Dismiss()
    {
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        base.OnLostFocus(e);
        FocusChanged(IsKeyboardFocusWithin);
        var element = e.NewFocusedElement;
        if (Equals(element, _endTextBox) || Equals(element, _startTextBox))
        {
            return;
        }
        if (element is Visual v && _popup?.IsInsidePopup(v) == true) return;
        CommitInput();
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    private void FocusChanged(bool hasFocus)
    {
        var wasFocused = _isFocused;
        _isFocused = hasFocus;
        if (hasFocus)
            if (!wasFocused && _startTextBox != null)
                _startTextBox.Focus();
    }

    private void CommitInput()
    {
        TimeSpan? startDate = null;
        var format = this.DisplayFormat ?? DEFAULT_TIME_DISPLAY_FORMAT;
        if (string.IsNullOrWhiteSpace(_startTextBox?.Text))
        {
            startDate = null;
            SetCurrentValue(StartTimeProperty, startDate);
        }
        else if (TimeSpan.TryParseExact(_startTextBox?.Text, format, CultureInfo.CurrentUICulture, out var start))
        {
            startDate = start;
            SetCurrentValue(StartTimeProperty, startDate);
        }
        TimeSpan? endDate = null;
        if (string.IsNullOrWhiteSpace(_endTextBox?.Text))
        {
            endDate = null;
            SetCurrentValue(EndTimeProperty, endDate);
        }
        else if (TimeSpan.TryParseExact(_endTextBox?.Text, format, CultureInfo.CurrentUICulture, out var end))
        {
            endDate = end;
            SetCurrentValue(EndTimeProperty, endDate);
        }
        _startPresenter?.SyncTime(StartTime);
        _endPresenter?.SyncTime(EndTime);
    }
    
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (e.Source is Visual source)
        {
            var inPopup = _popup?.IsInsidePopup(source);
            if (inPopup is true) return;
        }

        if (_startTextBox?.IsFocused == false)
        {
            _startTextBox?.Focus();
        }
        else
        {
            SetCurrentValue(IsDropdownOpenProperty, true);
        }
    }
}