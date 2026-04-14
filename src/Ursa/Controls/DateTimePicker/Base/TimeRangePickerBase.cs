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
[TemplatePart(PART_Popup, typeof(Popup))]
[TemplatePart(PART_StartPresenter, typeof(TimePickerPresenter))]
[TemplatePart(PART_EndPresenter, typeof(TimePickerPresenter))]
[PseudoClasses(PseudoClassName.PC_Empty)]
public abstract class TimeRangePickerBase : TemplatedControl, IInnerContentControl, IPopupInnerContent, IClearControl
{
    public const string PART_StartTextBox = "PART_StartTextBox";
    public const string PART_EndTextBox = "PART_EndTextBox";
    public const string PART_Popup = "PART_Popup";
    public const string PART_StartPresenter = "PART_StartPresenter";
    public const string PART_EndPresenter = "PART_EndPresenter";

    protected const string DEFAULT_TIME_DISPLAY_FORMAT = "HH:mm:ss";

    protected TimePickerPresenter? _startPresenter;
    protected TimePickerPresenter? _endPresenter;
    protected TextBox? _startTextBox;
    protected TextBox? _endTextBox;
    protected Popup? _popup;
    private bool _isFocused;
    private readonly RangePickerStatus _status = new();

    public static readonly StyledProperty<string?> DisplayFormatProperty =
        AvaloniaProperty.Register<TimeRangePickerBase, string?>(
            nameof(DisplayFormat), DEFAULT_TIME_DISPLAY_FORMAT);

    public static readonly StyledProperty<string> PanelFormatProperty =
        AvaloniaProperty.Register<TimeRangePickerBase, string>(
            nameof(PanelFormat), "HH mm ss");

    public static readonly StyledProperty<bool> NeedConfirmationProperty =
        AvaloniaProperty.Register<TimeRangePickerBase, bool>(nameof(NeedConfirmation));

    public static readonly StyledProperty<object?> InnerLeftContentProperty =
        AvaloniaProperty.Register<TimeRangePickerBase, object?>(nameof(InnerLeftContent));

    public static readonly StyledProperty<object?> InnerRightContentProperty =
        AvaloniaProperty.Register<TimeRangePickerBase, object?>(nameof(InnerRightContent));

    public static readonly StyledProperty<object?> PopupInnerTopContentProperty =
        AvaloniaProperty.Register<TimeRangePickerBase, object?>(nameof(PopupInnerTopContent));

    public static readonly StyledProperty<object?> PopupInnerBottomContentProperty =
        AvaloniaProperty.Register<TimeRangePickerBase, object?>(nameof(PopupInnerBottomContent));

    public static readonly StyledProperty<bool> IsDropdownOpenProperty =
        AvaloniaProperty.Register<TimeRangePickerBase, bool>(
            nameof(IsDropdownOpen), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> IsReadonlyProperty =
        AvaloniaProperty.Register<TimeRangePickerBase, bool>(nameof(IsReadonly));

    public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty =
        TextBox.PlaceholderForegroundProperty.AddOwner<TimeRangePickerBase>();

    public static readonly StyledProperty<string?> StartPlaceholderTextProperty =
        TextBox.PlaceholderTextProperty.AddOwner<TimeRangePickerBase>();

    public static readonly StyledProperty<string?> EndPlaceholderTextProperty =
        TextBox.PlaceholderTextProperty.AddOwner<TimeRangePickerBase>();

    [Obsolete("Use StartPlaceholderTextProperty instead.")]
    public static readonly StyledProperty<string?> StartWatermarkProperty = StartPlaceholderTextProperty;

    [Obsolete("Use EndPlaceholderTextProperty instead.")]
    public static readonly StyledProperty<string?> EndWatermarkProperty = EndPlaceholderTextProperty;

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

    public bool IsDropdownOpen
    {
        get => GetValue(IsDropdownOpenProperty);
        set => SetValue(IsDropdownOpenProperty, value);
    }

    public bool IsReadonly
    {
        get => GetValue(IsReadonlyProperty);
        set => SetValue(IsReadonlyProperty, value);
    }

    public IBrush? PlaceholderForeground
    {
        get => GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
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

    /// <summary>Exposes <see cref="RangePickerStatus.Reset"/> so the generic base can call it from <c>Clear()</c>.</summary>
    protected void ResetStatus() => _status.Reset();

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        TimePickerPresenter.SelectedTimeChangedEvent.RemoveHandler(OnTimeSelectedCore, _startPresenter, _endPresenter);
        GotFocusEvent.RemoveHandler(OnTextBoxGetFocus, _startTextBox, _endTextBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPressed, _startTextBox, _endTextBox);
        LostFocusEvent.RemoveHandler(OnTextBoxLostFocus, _startTextBox, _endTextBox);

        _popup = e.NameScope.Find<Popup>(PART_Popup);
        _startTextBox = e.NameScope.Find<TextBox>(PART_StartTextBox);
        _endTextBox = e.NameScope.Find<TextBox>(PART_EndTextBox);
        _startPresenter = e.NameScope.Find<TimePickerPresenter>(PART_StartPresenter);
        _endPresenter = e.NameScope.Find<TimePickerPresenter>(PART_EndPresenter);

        TimePickerPresenter.SelectedTimeChangedEvent.AddHandler(OnTimeSelectedCore, _startPresenter, _endPresenter);
        GotFocusEvent.AddHandler(OnTextBoxGetFocus, _startTextBox, _endTextBox);
        PointerPressedEvent.AddHandler(OnTextBoxPressed, RoutingStrategies.Tunnel, true, _startTextBox, _endTextBox);
        LostFocusEvent.AddHandler(OnTextBoxLostFocus, _startTextBox, _endTextBox);

        SyncToUI();
    }

    private void OnTextBoxLostFocus(object? sender, FocusChangedEventArgs e)
    {
        CommitInput();
        if (_status is { Current: Status.End, Previous: Status.Start }
            && Equals(sender, _endTextBox) && _endTextBox?.IsFocused == true)
        {
            SetCurrentValue(IsDropdownOpenProperty, false);
        }
    }

    private void OnTextBoxPressed(object? sender, PointerPressedEventArgs e) =>
        InitializePopupOpen(sender as TextBox);

    private void OnTextBoxGetFocus(object? sender, FocusChangedEventArgs e) =>
        InitializePopupOpen(sender as TextBox);

    private void InitializePopupOpen(TextBox? sender)
    {
        if (sender is null) return;
        SetCurrentValue(IsDropdownOpenProperty, true);
        if (Equals(sender, _startTextBox))
            _status.Push(Status.Start);
        else if (Equals(sender, _endTextBox))
            _status.Push(Status.End);
        _startPresenter?.SyncTime(GetSelectedStartTimeOnly());
        _endPresenter?.SyncTime(GetSelectedEndTimeOnly());
    }

    private void OnTimeSelectedCore(object? sender, TimeChangedEventArgs e)
    {
        if (Equals(sender, _startPresenter))
            OnStartPresenterTimeSelected(e.NewTime);
        else if (Equals(sender, _endPresenter))
            OnEndPresenterTimeSelected(e.NewTime);
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

    public void Dismiss() => SetCurrentValue(IsDropdownOpenProperty, false);

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        base.OnLostFocus(e);
        FocusChanged(IsKeyboardFocusWithin);
        var element = e.NewFocusedElement;
        if (Equals(element, _endTextBox) || Equals(element, _startTextBox)) return;
        if (element is Visual v && _popup?.IsInsidePopup(v) == true) return;
        CommitInput();
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    private void FocusChanged(bool hasFocus)
    {
        var wasFocused = _isFocused;
        _isFocused = hasFocus;
        if (hasFocus && !wasFocused && _startTextBox != null)
            _startTextBox.Focus();
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (e.Source is Visual source && _popup?.IsInsidePopup(source) == true) return;
        if (_startTextBox?.IsFocused == false)
            _startTextBox?.Focus();
        else
            SetCurrentValue(IsDropdownOpenProperty, true);
    }

    protected abstract void SyncToUI();
    protected abstract void CommitInput();
    protected abstract TimeOnly? GetSelectedStartTimeOnly();
    protected abstract TimeOnly? GetSelectedEndTimeOnly();
    protected abstract void OnStartPresenterTimeSelected(TimeOnly? time);
    protected abstract void OnEndPresenterTimeSelected(TimeOnly? time);
    public abstract void Clear();
}
