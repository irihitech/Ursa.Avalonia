using System.Globalization;
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

[TemplatePart( PART_TextBox, typeof(TextBox))]
[TemplatePart( PART_Popup, typeof(Popup))]
[TemplatePart( PART_Presenter, typeof(TimePickerPresenter))]
public class TimePicker : TemplatedControl, IClearControl, IInnerContentControl, IPopupInnerContent
{
    public const string PART_TextBox = "PART_TextBox";
    public const string PART_Popup = "PART_Popup";
    public const string PART_Presenter = "PART_Presenter";
    
    private TextBox? _textBox;
    private Popup? _popup;
    private TimePickerPresenter? _presenter;

    private bool _updateFromPresenter;
    
    public static readonly StyledProperty<string?> DisplayFormatProperty = AvaloniaProperty.Register<TimePicker, string?>(
        nameof(DisplayFormat), "HH:mm:ss");

    public static readonly StyledProperty<string> PanelFormatProperty = AvaloniaProperty.Register<TimePicker, string>(
        nameof(PanelFormat), "HH mm ss");

    public static readonly StyledProperty<TimeSpan?> SelectedTimeProperty =
        AvaloniaProperty.Register<TimePicker, TimeSpan?>(
            nameof(SelectedTime));

    public static readonly StyledProperty<bool> NeedConfirmationProperty = AvaloniaProperty.Register<TimePicker, bool>(
        nameof(NeedConfirmation));

    public static readonly StyledProperty<object?> InnerLeftContentProperty =
        AvaloniaProperty.Register<TimePicker, object?>(
            nameof(InnerLeftContent));

    public static readonly StyledProperty<object?> InnerRightContentProperty =
        AvaloniaProperty.Register<TimePicker, object?>(
            nameof(InnerRightContent));


    public static readonly StyledProperty<object?> PopupInnerTopContentProperty =
        AvaloniaProperty.Register<TimePicker, object?>(
            nameof(PopupInnerTopContent));

    public static readonly StyledProperty<object?> PopupInnerBottomContentProperty =
        AvaloniaProperty.Register<TimePicker, object?>(
            nameof(PopupInnerBottomContent));

    public static readonly StyledProperty<string?> WatermarkProperty = AvaloniaProperty.Register<TimePicker, string?>(
        nameof(Watermark));

    public static readonly StyledProperty<bool> IsDropdownOpenProperty = AvaloniaProperty.Register<TimePicker, bool>(
        nameof(IsDropdownOpen), defaultBindingMode: BindingMode.TwoWay);

    public bool IsDropdownOpen
    {
        get => GetValue(IsDropdownOpenProperty);
        set => SetValue(IsDropdownOpenProperty, value);
    }

    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    private TimeSpan? _selectedTimeHolder;

    static TimePicker()
    {
        PanelFormatProperty.Changed.AddClassHandler<TimePicker, string>((picker, args) =>
            picker.OnPanelFormatChanged(args));
        SelectedTimeProperty.Changed.AddClassHandler<TimePicker, TimeSpan?>((picker, args) =>
            picker.OnSelectionChanged(args));
    }

    public string? DisplayFormat
    {
        get => GetValue(DisplayFormatProperty);
        set => SetValue(DisplayFormatProperty, value);
    }

    public string PanelFormat
    {
        get => GetValue(PanelFormatProperty);
        set => SetValue(PanelFormatProperty, value);
    }

    public TimeSpan? SelectedTime
    {
        get => GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    public bool NeedConfirmation
    {
        get => GetValue(NeedConfirmationProperty);
        set => SetValue(NeedConfirmationProperty, value);
    }

    public void Clear()
    {
        SetCurrentValue(SelectedTimeProperty, null);
    }

    public object? InnerLeftContent
    {
        get => GetValue(InnerLeftContentProperty);
        set => SetValue(InnerLeftContentProperty, value);
    }

    public object? InnerRightContent
    {
        get => GetValue(InnerRightContentProperty);
        set => SetValue(InnerRightContentProperty, value);
    }

    public object? PopupInnerTopContent
    {
        get => GetValue(PopupInnerTopContentProperty);
        set => SetValue(PopupInnerTopContentProperty, value);
    }

    public object? PopupInnerBottomContent
    {
        get => GetValue(PopupInnerBottomContentProperty);
        set => SetValue(PopupInnerBottomContentProperty, value);
    }

    private void OnPanelFormatChanged(AvaloniaPropertyChangedEventArgs<string> args)
    {
        var format = args.NewValue.Value;
        var parts = format.Split(' ', '-', ':');
    }


    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        
        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        _popup = e.NameScope.Find<Popup>(PART_Popup);
        _presenter = e.NameScope.Find<TimePickerPresenter>(PART_Presenter);
        TextBox.GotFocusEvent.AddHandler(OnTextBoxGetFocus, _textBox);
        TextBox.TextChangedEvent.AddDisposableHandler(OnTextChanged, _textBox);
        TextBox.PointerPressedEvent.AddHandler(OnTextBoxPointerPressed, RoutingStrategies.Tunnel, false, _textBox);
    }

    private void OnTextBoxPointerPressed(object sender, PointerPressedEventArgs e)
    {
        SetCurrentValue(IsDropdownOpenProperty, true);
    }

    private void OnTextBoxGetFocus(object sender, GotFocusEventArgs e)
    {
        IsDropdownOpen = true;
    }


    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (DisplayFormat is null || DisplayFormat.Length == 0)
        {
            if (TimeSpan.TryParse(_textBox?.Text, out var defaultTime))
            {
                TimePickerPresenter.TimeProperty.SetValue(defaultTime, _presenter);
            }
        }
        else
        {
            if(DateTime.TryParseExact(_textBox?.Text, DisplayFormat, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var time))
            {
                TimePickerPresenter.TimeProperty.SetValue(time.TimeOfDay, _presenter);
            }
        }
    }

    private void OnSelectionChanged(AvaloniaPropertyChangedEventArgs<TimeSpan?> args)
    {
        if (_textBox is null) return;
        var time = args.NewValue.Value;
        var text = new DateTime(1,1,1, time?.Hours ?? 0, time?.Minutes ?? 0, time?.Seconds ?? 0).ToString(DisplayFormat);
        _textBox.Text = text;
    }

    public void Confirm()
    {
        if (NeedConfirmation)
            // TODO: close popup. 
            SetCurrentValue(SelectedTimeProperty, _selectedTimeHolder);
    }

    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        base.UpdateDataValidation(property, state, error);
        if (property == SelectedTimeProperty) DataValidationErrors.SetError(this, error);
    }
}