using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public abstract class TimeRangePickerBase<T> : TimeRangePickerBase where T : struct
{
    public static readonly StyledProperty<T?> SelectedStartTimeProperty =
        AvaloniaProperty.Register<TimeRangePickerBase<T>, T?>(
            nameof(SelectedStartTime), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    public static readonly StyledProperty<T?> SelectedEndTimeProperty =
        AvaloniaProperty.Register<TimeRangePickerBase<T>, T?>(
            nameof(SelectedEndTime), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    public T? SelectedStartTime
    {
        get => GetValue(SelectedStartTimeProperty);
        set => SetValue(SelectedStartTimeProperty, value);
    }

    public T? SelectedEndTime
    {
        get => GetValue(SelectedEndTimeProperty);
        set => SetValue(SelectedEndTimeProperty, value);
    }

    private TimePickerPresenter? _startPresenter;
    private TimePickerPresenter? _endPresenter;
    private TextBox? _startTextBox;
    private TextBox? _endTextBox;
    private Popup? _popup;
    private bool _isFocused;
    private readonly RangePickerStatus _status = new();
    private TimeOnly? _pendingStartTime;
    private TimeOnly? _pendingEndTime;

    /// <summary>Converts a <typeparamref name="T"/> value to <see cref="TimeOnly"/> for presenter sync.</summary>
    protected abstract TimeOnly? ToTimeOnly(T? value);

    /// <summary>Creates a <typeparamref name="T"/> value from a presenter-selected <see cref="TimeOnly"/>.</summary>
    protected abstract T FromTimeOnly(TimeOnly time);

    /// <summary>Parses a text string with the given format into a <typeparamref name="T"/> value, or <see langword="null"/> on failure.</summary>
    protected abstract T? Parse(string? text, string? format);

    /// <summary>Formats a <typeparamref name="T"/> value to a display string using the given format.</summary>
    protected abstract string? Format(T? value, string? format);

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
        PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedStartTime is null && SelectedEndTime is null);
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
        _pendingStartTime = ToTimeOnly(SelectedStartTime);
        _pendingEndTime = ToTimeOnly(SelectedEndTime);
        SetCurrentValue(IsDropdownOpenProperty, true);
        if (Equals(sender, _startTextBox))
            _status.Push(Status.Start);
        else if (Equals(sender, _endTextBox))
            _status.Push(Status.End);
        _startPresenter?.SyncTime(ToTimeOnly(SelectedStartTime));
        _endPresenter?.SyncTime(ToTimeOnly(SelectedEndTime));
    }

    private void OnTimeSelectedCore(object? sender, TimeChangedEventArgs e)
    {
        if (NeedConfirmation)
        {
            if (Equals(sender, _startPresenter))
                _pendingStartTime = e.NewTime;
            else if (Equals(sender, _endPresenter))
                _pendingEndTime = e.NewTime;
        }
        else
        {
            if (Equals(sender, _startPresenter))
                SetCurrentValue(SelectedStartTimeProperty,
                    e.NewTime.HasValue ? (T?)FromTimeOnly(e.NewTime.Value) : (T?)null);
            else if (Equals(sender, _endPresenter))
                SetCurrentValue(SelectedEndTimeProperty,
                    e.NewTime.HasValue ? (T?)FromTimeOnly(e.NewTime.Value) : (T?)null);
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

    public override void Confirm()
    {
        if (NeedConfirmation)
        {
            if (_pendingStartTime.HasValue)
                SetCurrentValue(SelectedStartTimeProperty, (T?)FromTimeOnly(_pendingStartTime.Value));
            if (_pendingEndTime.HasValue)
                SetCurrentValue(SelectedEndTimeProperty, (T?)FromTimeOnly(_pendingEndTime.Value));
        }
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    public override void Dismiss() => SetCurrentValue(IsDropdownOpenProperty, false);

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

    private void SyncToUI()
    {
        var format = DisplayFormat ?? DEFAULT_TIME_DISPLAY_FORMAT;
        _startTextBox?.SetCurrentValue(TextBox.TextProperty, Format(SelectedStartTime, format));
        _endTextBox?.SetCurrentValue(TextBox.TextProperty, Format(SelectedEndTime, format));
        _startPresenter?.SyncTime(ToTimeOnly(SelectedStartTime));
        _endPresenter?.SyncTime(ToTimeOnly(SelectedEndTime));
    }

    private void CommitInput()
    {
        var format = DisplayFormat ?? DEFAULT_TIME_DISPLAY_FORMAT;

        if (string.IsNullOrWhiteSpace(_startTextBox?.Text))
            SetCurrentValue(SelectedStartTimeProperty, (T?)null);
        else
            SetCurrentValue(SelectedStartTimeProperty, Parse(_startTextBox?.Text, format));

        if (string.IsNullOrWhiteSpace(_endTextBox?.Text))
            SetCurrentValue(SelectedEndTimeProperty, (T?)null);
        else
            SetCurrentValue(SelectedEndTimeProperty, Parse(_endTextBox?.Text, format));
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == SelectedStartTimeProperty || change.Property == SelectedEndTimeProperty)
        {
            SyncToUI();
            PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedStartTime is null && SelectedEndTime is null);
        }
        else if (change.Property == DisplayFormatProperty && _startTextBox is not null)
        {
            SyncToUI();
        }
    }

    public override void Clear()
    {
        SetCurrentValue(SelectedStartTimeProperty, (T?)null);
        SetCurrentValue(SelectedEndTimeProperty, (T?)null);
        _startPresenter?.SyncTime(null);
        _endPresenter?.SyncTime(null);
        _status.Reset();
    }
}
