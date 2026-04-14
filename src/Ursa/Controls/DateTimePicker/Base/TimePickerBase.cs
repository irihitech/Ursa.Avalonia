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
[TemplatePart(PART_Popup, typeof(Popup))]
[TemplatePart(PART_Presenter, typeof(TimePickerPresenter))]
public abstract class TimePickerBase : TemplatedControl, IInnerContentControl, IPopupInnerContent, IClearControl
{
    public const string PART_TextBox = "PART_TextBox";
    public const string PART_Popup = "PART_Popup";
    public const string PART_Presenter = "PART_Presenter";

    protected const string DEFAULT_TIME_DISPLAY_FORMAT = "HH:mm:ss";

    protected Popup? _popup;
    protected TimePickerPresenter? _presenter;
    protected TextBox? _textBox;

    public static readonly StyledProperty<string?> DisplayFormatProperty =
        AvaloniaProperty.Register<TimePickerBase, string?>(
            nameof(DisplayFormat), DEFAULT_TIME_DISPLAY_FORMAT);

    public static readonly StyledProperty<string> PanelFormatProperty =
        AvaloniaProperty.Register<TimePickerBase, string>(
            nameof(PanelFormat), "HH mm ss");

    public static readonly StyledProperty<bool> NeedConfirmationProperty =
        AvaloniaProperty.Register<TimePickerBase, bool>(nameof(NeedConfirmation));

    public static readonly StyledProperty<object?> InnerLeftContentProperty =
        AvaloniaProperty.Register<TimePickerBase, object?>(nameof(InnerLeftContent));

    public static readonly StyledProperty<object?> InnerRightContentProperty =
        AvaloniaProperty.Register<TimePickerBase, object?>(nameof(InnerRightContent));

    public static readonly StyledProperty<object?> PopupInnerTopContentProperty =
        AvaloniaProperty.Register<TimePickerBase, object?>(nameof(PopupInnerTopContent));

    public static readonly StyledProperty<object?> PopupInnerBottomContentProperty =
        AvaloniaProperty.Register<TimePickerBase, object?>(nameof(PopupInnerBottomContent));

    public static readonly StyledProperty<bool> IsDropdownOpenProperty =
        AvaloniaProperty.Register<TimePickerBase, bool>(
            nameof(IsDropdownOpen), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> IsReadonlyProperty =
        AvaloniaProperty.Register<TimePickerBase, bool>(nameof(IsReadonly));

    public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty =
        TextBox.PlaceholderForegroundProperty.AddOwner<TimePickerBase>();

    public static readonly StyledProperty<string?> PlaceholderTextProperty =
        TextBox.PlaceholderTextProperty.AddOwner<TimePickerBase>();

    [Obsolete("Use PlaceholderTextProperty instead.")]
    public static readonly StyledProperty<string?> WatermarkProperty = PlaceholderTextProperty;

    public string? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    [Obsolete("Use PlaceholderText instead.")]
    public string? Watermark
    {
        get => PlaceholderText;
        set => PlaceholderText = value;
    }

    public IBrush? PlaceholderForeground
    {
        get => GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
    }

    public bool IsReadonly
    {
        get => GetValue(IsReadonlyProperty);
        set => SetValue(IsReadonlyProperty, value);
    }

    public bool IsDropdownOpen
    {
        get => GetValue(IsDropdownOpenProperty);
        set => SetValue(IsDropdownOpenProperty, value);
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

    public bool NeedConfirmation
    {
        get => GetValue(NeedConfirmationProperty);
        set => SetValue(NeedConfirmationProperty, value);
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

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        TimePickerPresenter.SelectedTimeChangedEvent.RemoveHandler(OnPresenterTimeChangedCore, _presenter);
        GotFocusEvent.RemoveHandler(OnTextBoxGetFocus, _textBox);
        LostFocusEvent.RemoveHandler(OnTextBoxLostFocus, _textBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPressed, _textBox);

        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        _popup = e.NameScope.Find<Popup>(PART_Popup);
        _presenter = e.NameScope.Find<TimePickerPresenter>(PART_Presenter);

        TimePickerPresenter.SelectedTimeChangedEvent.AddHandler(OnPresenterTimeChangedCore, _presenter);
        GotFocusEvent.AddHandler(OnTextBoxGetFocus, _textBox);
        LostFocusEvent.AddHandler(OnTextBoxLostFocus, _textBox);
        PointerPressedEvent.AddHandler(OnTextBoxPressed, RoutingStrategies.Tunnel, true, _textBox);

        SyncToUI();
    }

    private void OnTextBoxPressed(object? sender, PointerPressedEventArgs e) => InitializePopupOpen();
    private void OnTextBoxLostFocus(object? sender, FocusChangedEventArgs e) => CommitInput();
    private void OnTextBoxGetFocus(object? sender, FocusChangedEventArgs e) => InitializePopupOpen();

    private void InitializePopupOpen()
    {
        SetCurrentValue(IsDropdownOpenProperty, true);
        _presenter?.SyncTime(GetSelectedTimeOnly());
    }

    private void OnPresenterTimeChangedCore(object? sender, TimeChangedEventArgs e)
    {
        if (!IsInitialized) return;
        OnPresenterTimeSelected(e.NewTime);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (!e.Handled && e.Source is Visual source)
        {
            if (_popup?.IsInsidePopup(source) == true)
                e.Handled = true;
            else
                _textBox?.Focus();
        }
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

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        base.OnLostFocus(e);
        var element = e.NewFocusedElement;
        if (Equals(element, _textBox)) return;
        if (element is Visual v && _popup?.IsInsidePopup(v) == true) return;
        CommitInput();
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    public void Confirm()
    {
        _presenter?.Confirm();
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    public void Dismiss() => SetCurrentValue(IsDropdownOpenProperty, false);

    protected abstract void SyncToUI();
    protected abstract void CommitInput();
    protected abstract TimeOnly? GetSelectedTimeOnly();
    protected abstract void OnPresenterTimeSelected(TimeOnly? time);
    public abstract void Clear();
}