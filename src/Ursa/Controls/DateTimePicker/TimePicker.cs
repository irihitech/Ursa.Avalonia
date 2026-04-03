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

[TemplatePart(PART_TextBox, typeof(TextBox))]
[TemplatePart(PartNames.PART_Popup, typeof(Popup))]
[TemplatePart(PART_Presenter, typeof(TimePickerPresenter))]
public class TimePicker : TimePickerBase, IClearControl
{
    public const string PART_TextBox = "PART_TextBox";
    public const string PART_Presenter = "PART_Presenter";

    public static readonly StyledProperty<TimeSpan?> SelectedTimeProperty =
        AvaloniaProperty.Register<TimePicker, TimeSpan?>(
            nameof(SelectedTime), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<string?> PlaceholderTextProperty =
        TextBox.PlaceholderTextProperty.AddOwner<TimePicker>();

    public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty =
        TextBox.PlaceholderForegroundProperty.AddOwner<TimePicker>();

    [Obsolete("Use PlaceholderTextProperty instead.")]
    public static readonly StyledProperty<string?> WatermarkProperty = PlaceholderTextProperty;
    
    private Popup? _popup;
    private TimePickerPresenter? _presenter;

    private bool _suppressTextPresenterEvent;
    private TextBox? _textBox;

    static TimePicker()
    {
        SelectedTimeProperty.Changed.AddClassHandler<TimePicker, TimeSpan?>((picker, args) =>
            picker.OnSelectionChanged(args));
        DisplayFormatProperty.Changed.AddClassHandler<TimePicker, string?>((picker, args) =>
            picker.OnDisplayFormatChanged(args));
    }

    public string? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public IBrush? PlaceholderForeground
    {
        get => GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
    }

    [Obsolete("Use PlaceholderText instead.")]
    public string? Watermark
    {
        get => PlaceholderText;
        set => PlaceholderText = value;
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
        
        TimePickerPresenter.SelectedTimeChangedEvent.RemoveHandler(OnPresenterTimeChanged, _presenter);
        GotFocusEvent.RemoveHandler(OnTextBoxGetFocus, _textBox);
        LostFocusEvent.RemoveHandler(OnTextBoxLostFocus, _textBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPressed, _textBox);

        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        _popup = e.NameScope.Find<Popup>(PartNames.PART_Popup);
        _presenter = e.NameScope.Find<TimePickerPresenter>(PART_Presenter);
        
        TimePickerPresenter.SelectedTimeChangedEvent.AddHandler(OnPresenterTimeChanged, _presenter);
        GotFocusEvent.AddHandler(OnTextBoxGetFocus, _textBox);
        LostFocusEvent.AddHandler(OnTextBoxLostFocus, _textBox);
        PointerPressedEvent.AddHandler(OnTextBoxPressed, RoutingStrategies.Tunnel, true, _textBox);
        
        // _presenter?.SyncTime(SelectedTime);
        SyncTimeToText(SelectedTime);
    }

    private void OnTextBoxPressed(object? sender, PointerPressedEventArgs e)
    {
        InitializePopupOpen();
    }

    private void OnTextBoxLostFocus(object? sender, FocusChangedEventArgs e)
    {
        CommitInput();
    }
    
    private void InitializePopupOpen()
    {
        SetCurrentValue(IsDropdownOpenProperty, true);
        _presenter?.SyncTime(SelectedTime);
    }

    private void OnPresenterTimeChanged(object? sender, TimeChangedEventArgs e)
    {
        if (!IsInitialized) return;
        if (_suppressTextPresenterEvent) return;
        SetCurrentValue(SelectedTimeProperty, e.NewTime);
    }

    private void OnTextBoxGetFocus(object? sender, FocusChangedEventArgs e)
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
            CommitInput();
            SetCurrentValue(IsDropdownOpenProperty, false);
            e.Handled = true;
            return;
        }

        base.OnKeyDown(e);
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
    }

    public void Dismiss()
    {
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        base.UpdateDataValidation(property, state, error);
        if (property == SelectedTimeProperty) DataValidationErrors.SetError(this, error);
    }
    
    

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        base.OnLostFocus(e);
        var element = e.NewFocusedElement;
        if (Equals(element, _textBox)) return;
        if (element is Visual v && _popup?.IsInsidePopup(v) == true) return;
        if (Equals(element, _textBox)) return;
        CommitInput();
        SetCurrentValue(IsDropdownOpenProperty, false);
    }
    

    private void CommitInput()
    {
        var format = DisplayFormat ?? DEFAULT_TIME_DISPLAY_FORMAT;
        if (string.IsNullOrEmpty(_textBox?.Text))
        {
            _presenter?.SyncTime(null);
        }
        else if (DateTime.TryParseExact(_textBox?.Text, format, CultureInfo.CurrentUICulture, DateTimeStyles.None,
                out var time))
        {
            SetCurrentValue(SelectedTimeProperty, time.TimeOfDay);
            _presenter?.SyncTime(time.TimeOfDay);
        }
        else
        {
            _presenter?.SyncTime(null);
        }
    }
}