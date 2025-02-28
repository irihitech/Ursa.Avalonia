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

[TemplatePart(PART_TextBox, typeof(TextBox))]
[TemplatePart(PartNames.PART_Popup, typeof(Popup))]
[TemplatePart(PART_Presenter, typeof(TimePickerPresenter))]
[TemplatePart(PART_Button, typeof(Button))]
public class TimePicker : TimePickerBase, IClearControl
{
    public const string PART_TextBox = "PART_TextBox";
    public const string PART_Presenter = "PART_Presenter";
    public const string PART_Button = "PART_Button";

    public static readonly StyledProperty<TimeSpan?> SelectedTimeProperty =
        AvaloniaProperty.Register<TimePicker, TimeSpan?>(
            nameof(SelectedTime), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<string?> WatermarkProperty = AvaloniaProperty.Register<TimePicker, string?>(
        nameof(Watermark));

    private Button? _button;

    private bool _isFocused;
    private Popup? _popup;
    private TimePickerPresenter? _presenter;

    private bool _suppressTextPresenterEvent;
    private TextBox? _textBox;

    static TimePicker()
    {
        FocusableProperty.OverrideDefaultValue<TimePicker>(true);
        SelectedTimeProperty.Changed.AddClassHandler<TimePicker, TimeSpan?>((picker, args) =>
            picker.OnSelectionChanged(args));
        DisplayFormatProperty.Changed.AddClassHandler<TimePicker, string?>((picker, args) =>
            picker.OnDisplayFormatChanged(args));
    }

    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public TimeSpan? SelectedTime
    {
        get => GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    public void Clear()
    {
        SetCurrentValue(SelectedTimeProperty, null);
        Focus(NavigationMethod.Pointer);
    }

    private void OnDisplayFormatChanged(AvaloniaPropertyChangedEventArgs<string?> _)
    {
        if (_textBox is null) return;
        SyncTimeToText(SelectedTime);
    }

    /// <inheritdoc />
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (!e.Handled && e.Source is Visual source)
            if (_popup?.IsInsidePopup(source) == true)
                e.Handled = true;
    }


    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        GotFocusEvent.RemoveHandler(OnTextBoxGetFocus, _textBox);
        TextBox.TextChangedEvent.RemoveHandler(OnTextChanged, _textBox);
        Button.ClickEvent.RemoveHandler(OnButtonClick, _button);
        TimePickerPresenter.SelectedTimeChangedEvent.RemoveHandler(OnPresenterTimeChanged, _presenter);

        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        _popup = e.NameScope.Find<Popup>(PartNames.PART_Popup);
        _presenter = e.NameScope.Find<TimePickerPresenter>(PART_Presenter);
        _button = e.NameScope.Find<Button>(PART_Button);

        GotFocusEvent.AddHandler(OnTextBoxGetFocus, _textBox);
        TextBox.TextChangedEvent.AddHandler(OnTextChanged, _textBox);
        Button.ClickEvent.AddHandler(OnButtonClick, _button);
        TimePickerPresenter.SelectedTimeChangedEvent.AddHandler(OnPresenterTimeChanged, _presenter);
        
        _presenter?.SyncTime(SelectedTime);
        SyncTimeToText(SelectedTime);
    }

    private void OnPresenterTimeChanged(object? sender, TimeChangedEventArgs e)
    {
        if (!IsInitialized) return;
        if (_suppressTextPresenterEvent) return;
        SetCurrentValue(SelectedTimeProperty, e.NewTime);
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (IsFocused) SetCurrentValue(IsDropdownOpenProperty, !IsDropdownOpen);
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
            SetCurrentValue(IsDropdownOpenProperty, false);
            return;
        }

        if (e.Key == Key.Enter)
        {
            CommitInput(true);
            SetCurrentValue(IsDropdownOpenProperty, false);
            e.Handled = true;
            return;
        }
        base.OnKeyDown(e);
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(_textBox?.Text))
        {
            _presenter?.SyncTime(null);
        }
        else if (DisplayFormat is null || DisplayFormat.Length == 0)
        {
            if (TimeSpan.TryParse(_textBox?.Text, out var defaultTime)) _presenter?.SyncTime(defaultTime);
        }
        else
        {
            CommitInput(false);
        }
    }

    private void OnSelectionChanged(AvaloniaPropertyChangedEventArgs<TimeSpan?> args)
    {
        if (_textBox is null) return;
        _suppressTextPresenterEvent = true;
        _presenter?.SyncTime(args.NewValue.Value);
        SyncTimeToText(args.NewValue.Value);
        _suppressTextPresenterEvent = false;
    }

    private void SyncTimeToText(TimeSpan? time)
    {
        if (_textBox is null) return;
        if (time is null)
        {
            _textBox.Text = null;
            return;
        }

        var date = new DateTime(1, 1, 1, time.Value.Hours, time.Value.Minutes, time.Value.Seconds);
        var text = date.ToString(DisplayFormat);
        _textBox.Text = text;
    }

    public void Confirm()
    {
        _presenter?.Confirm();
        SetCurrentValue(IsDropdownOpenProperty, false);
        Focus();
    }

    public void Dismiss()
    {
        SetCurrentValue(IsDropdownOpenProperty, false);
        Focus();
    }

    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        base.UpdateDataValidation(property, state, error);
        if (property == SelectedTimeProperty) DataValidationErrors.SetError(this, error);
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
        if (Equals(element, _textBox)) return;
        CommitInput(true);
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    private void FocusChanged(bool hasFocus)
    {
        var wasFocused = _isFocused;
        _isFocused = hasFocus;
        if (hasFocus)
            if (!wasFocused && _textBox != null)
                _textBox.Focus();
    }
    
    private void CommitInput(bool clearWhenInvalid)
    {
        if (DateTime.TryParseExact(_textBox?.Text, DisplayFormat, CultureInfo.CurrentUICulture, DateTimeStyles.None,
                out var time))
        {
            SetCurrentValue(SelectedTimeProperty, time.TimeOfDay);
            _presenter?.SyncTime(time.TimeOfDay);
        }
        else
        {
            if (clearWhenInvalid)
            {
                SetCurrentValue(SelectedTimeProperty, null);
            }
            _presenter?.SyncTime(null);
        }
    }
}