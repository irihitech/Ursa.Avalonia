using System.Globalization;
using Avalonia;
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

[TemplatePart(PART_StartTextBox, typeof(TextBox))]
[TemplatePart(PART_EndTextBox, typeof(TextBox))]
[TemplatePart(PartNames.PART_Popup, typeof(Popup))]
[TemplatePart(PART_StartPresenter, typeof(TimePickerPresenter))]
[TemplatePart(PART_EndPresenter, typeof(TimePickerPresenter))]
[TemplatePart(PART_Button, typeof(Button))]
[PseudoClasses(PseudoClassName.PC_Empty)]
public class TimeRangePicker : TimePickerBase, IClearControl
{
    public const string PART_StartTextBox = "PART_StartTextBox";
    public const string PART_EndTextBox = "PART_EndTextBox";
    public const string PART_StartPresenter = "PART_StartPresenter";
    public const string PART_EndPresenter = "PART_EndPresenter";
    public const string PART_Button = "PART_Button";


    public static readonly StyledProperty<TimeSpan?> StartTimeProperty =
        AvaloniaProperty.Register<TimeRangePicker, TimeSpan?>(
            nameof(StartTime), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<TimeSpan?> EndTimeProperty =
        AvaloniaProperty.Register<TimeRangePicker, TimeSpan?>(
            nameof(EndTime), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<string?> StartWatermarkProperty =
        AvaloniaProperty.Register<TimeRangePicker, string?>(
            nameof(StartWatermark));

    public static readonly StyledProperty<string?> EndWatermarkProperty =
        AvaloniaProperty.Register<TimeRangePicker, string?>(
            nameof(EndWatermark));

    private Button? _button;
    private TimePickerPresenter? _endPresenter;
    private TextBox? _endTextBox;
    private bool _isFocused;
    private Popup? _popup;
    private TimePickerPresenter? _startPresenter;

    private TextBox? _startTextBox;
    private bool _suppressTextPresenterEvent;

    static TimeRangePicker()
    {
        FocusableProperty.OverrideDefaultValue<TimeRangePicker>(true);
        StartTimeProperty.Changed.AddClassHandler<TimeRangePicker, TimeSpan?>((picker, args) =>
            picker.OnSelectionChanged(args));
        EndTimeProperty.Changed.AddClassHandler<TimeRangePicker, TimeSpan?>((picker, args) =>
            picker.OnSelectionChanged(args, false));
        DisplayFormatProperty.Changed.AddClassHandler<TimeRangePicker, string?>((picker, args) =>
            picker.OnDisplayFormatChanged(args));
    }


    public string? EndWatermark
    {
        get => GetValue(EndWatermarkProperty);
        set => SetValue(EndWatermarkProperty, value);
    }

    public string? StartWatermark
    {
        get => GetValue(StartWatermarkProperty);
        set => SetValue(StartWatermarkProperty, value);
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

    public void Clear()
    {
        Focus(NavigationMethod.Pointer);
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
        _suppressTextPresenterEvent = true;
        var presenter = start ? _startPresenter : _endPresenter;
        presenter?.SyncTime(args.NewValue.Value);
        _suppressTextPresenterEvent = false;
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

        GotFocusEvent.RemoveHandler(OnTextBoxGetFocus, _startTextBox, _endTextBox);
        Button.ClickEvent.RemoveHandler(OnButtonClick, _button);
        TimePickerPresenter.SelectedTimeChangedEvent.RemoveHandler(OnPresenterTimeChanged, _startPresenter,
            _endPresenter);
        TextBox.TextChangedEvent.RemoveHandler(OnTextChanged, _startTextBox, _endTextBox);

        _popup = e.NameScope.Find<Popup>(PartNames.PART_Popup);
        _startTextBox = e.NameScope.Find<TextBox>(PART_StartTextBox);
        _endTextBox = e.NameScope.Find<TextBox>(PART_EndTextBox);
        _startPresenter = e.NameScope.Find<TimePickerPresenter>(PART_StartPresenter);
        _endPresenter = e.NameScope.Find<TimePickerPresenter>(PART_EndPresenter);
        _button = e.NameScope.Find<Button>(PART_Button);

        GotFocusEvent.AddHandler(OnTextBoxGetFocus, _startTextBox, _endTextBox);
        Button.ClickEvent.AddHandler(OnButtonClick, _button);
        TimePickerPresenter.SelectedTimeChangedEvent.AddHandler(OnPresenterTimeChanged, _startPresenter, _endPresenter);
        TextBox.TextChangedEvent.AddHandler(OnTextChanged, _startTextBox, _endTextBox);

        _startPresenter?.SyncTime(StartTime);
        _endPresenter?.SyncTime(EndTime);
        SyncTimeToText(StartTime);
        SyncTimeToText(EndTime, false);
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (Equals(sender, _startTextBox))
            OnTextChangedInternal(_startTextBox, _startPresenter, StartTimeProperty, true);
        else if (Equals(sender, _endTextBox)) OnTextChangedInternal(_endTextBox, _endPresenter, EndTimeProperty, true);
    }

    private void OnTextChangedInternal(TextBox? textBox, TimePickerPresenter? presenter, AvaloniaProperty property,
        bool fromText = false)
    {
        if (textBox?.Text is null || string.IsNullOrEmpty(textBox.Text))
        {
            SetCurrentValue(property, null);
            presenter?.SyncTime(null);
        }
        else if (DisplayFormat is null || DisplayFormat.Length == 0)
        {
            if (DateTime.TryParse(textBox.Text, out var defaultTime))
            {
                SetCurrentValue(property, defaultTime.TimeOfDay);
                presenter?.SyncTime(defaultTime.TimeOfDay);
            }
        }
        else
        {
            if (DateTime.TryParseExact(textBox.Text, DisplayFormat, CultureInfo.CurrentUICulture, DateTimeStyles.None,
                    out var date))
            {
                SetCurrentValue(property, date.TimeOfDay);
                presenter?.SyncTime(date.TimeOfDay);
            }
            else
            {
                if (!fromText)
                {
                    SetCurrentValue(property, null);
                    textBox.SetValue(TextBox.TextProperty, null);
                    presenter?.SyncTime(null);
                }
            }
        }
    }

    private void OnPresenterTimeChanged(object? sender, TimeChangedEventArgs e)
    {
        if (!IsInitialized) return;
        if (_suppressTextPresenterEvent) return;
        SetCurrentValue(Equals(sender, _startPresenter) ? StartTimeProperty : EndTimeProperty, e.NewTime);
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        SetCurrentValue(IsDropdownOpenProperty, !IsDropdownOpen);
    }

    private void OnTextBoxGetFocus(object? sender, GotFocusEventArgs e)
    {
        SetCurrentValue(IsDropdownOpenProperty, true);
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
            CommitInput(true);
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
        Focus();
    }

    public void Dismiss()
    {
        SetCurrentValue(IsDropdownOpenProperty, false);
        Focus();
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
        if (element is Visual v && _popup?.IsInsidePopup(v) == true) return;
        if (Equals(element, _startTextBox) || Equals(element, _endTextBox)) return;
        CommitInput(true);
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

    private void CommitInput(bool clearWhenInvalid)
    {
        if (DateTime.TryParseExact(_startTextBox?.Text, DisplayFormat, CultureInfo.CurrentUICulture,
                DateTimeStyles.None,
                out var start))
        {
            _startPresenter?.SyncTime(start.TimeOfDay);
            SetCurrentValue(StartTimeProperty, start.TimeOfDay);
        }
        else
        {
            if (clearWhenInvalid)
            {
                _startTextBox?.SetValue(TextBox.TextProperty, null);
                _startPresenter?.SyncTime(null);
            }
        }

        if (DateTime.TryParseExact(_endTextBox?.Text, DisplayFormat, CultureInfo.CurrentUICulture, DateTimeStyles.None,
                out var end))
        {
            _endPresenter?.SyncTime(end.TimeOfDay);
            SetCurrentValue(EndTimeProperty, end.TimeOfDay);
        }
        else
        {
            if (clearWhenInvalid)
            {
                _endTextBox?.SetValue(TextBox.TextProperty, null);
                _endPresenter?.SyncTime(null);
            }
        }
    }
}