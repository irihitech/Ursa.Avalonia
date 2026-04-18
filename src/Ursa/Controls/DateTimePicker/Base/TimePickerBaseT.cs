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

public abstract class TimePickerBase<T> : TimePickerBase where T : struct
{
    public static readonly StyledProperty<T?> SelectedTimeProperty =
        AvaloniaProperty.Register<TimePickerBase<T>, T?>(
            nameof(SelectedTime), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    public T? SelectedTime
    {
        get => GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    protected Popup? _popup;
    protected TimePickerPresenter? _presenter;
    protected TextBox? _textBox;
    protected TimeOnly? _pendingTime;

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

        SyncTimeToText();
        PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedTime is null);
    }

    private void OnTextBoxPressed(object? sender, PointerPressedEventArgs e) => InitializePopupOpen();
    private void OnTextBoxLostFocus(object? sender, FocusChangedEventArgs e) => CommitInput();
    private void OnTextBoxGetFocus(object? sender, FocusChangedEventArgs e) => InitializePopupOpen();

    private void InitializePopupOpen()
    {
        _pendingTime = ToTimeOnly(SelectedTime);
        SetCurrentValue(IsDropdownOpenProperty, true);
        _presenter?.SyncTime(ToTimeOnly(SelectedTime));
    }

    private void OnPresenterTimeChangedCore(object? sender, TimeChangedEventArgs e)
    {
        if (!IsInitialized) return;
        if (NeedConfirmation)
            _pendingTime = e.NewTime;
        else
            SetCurrentValue(SelectedTimeProperty,
                e.NewTime.HasValue ? (T?)FromTimeOnly(e.NewTime.Value) : (T?)null);
    }

    private void SyncTimeToText()
    {
        _textBox?.SetValue(TextBox.TextProperty,
            Format(SelectedTime, DisplayFormat ?? DEFAULT_TIME_DISPLAY_FORMAT));
        _presenter?.SyncTime(ToTimeOnly(SelectedTime));
    }

    private void CommitInput()
    {
        var format = DisplayFormat ?? DEFAULT_TIME_DISPLAY_FORMAT;
        if (string.IsNullOrWhiteSpace(_textBox?.Text))
        {
            SetCurrentValue(SelectedTimeProperty, (T?)null);
            _presenter?.SyncTime(null);
            return;
        }

        var parsed = Parse(_textBox?.Text, format);
        if (parsed.HasValue)
        {
            SetCurrentValue(SelectedTimeProperty, parsed);
        }
        else
        {
            SetCurrentValue(SelectedTimeProperty, (T?)null);
            _presenter?.SyncTime(null);
        }
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

    public override void Confirm()
    {
        if (NeedConfirmation && _pendingTime.HasValue)
            SetCurrentValue(SelectedTimeProperty, (T?)FromTimeOnly(_pendingTime.Value));
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    public override void Dismiss() => SetCurrentValue(IsDropdownOpenProperty, false);

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == SelectedTimeProperty)
        {
            SyncTimeToText();
            PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedTime is null);
        }
        else if (change.Property == DisplayFormatProperty && _textBox is not null)
            SyncTimeToText();
    }

    public override void Clear()
    {
        SetCurrentValue(SelectedTimeProperty, (T?)null);
        _presenter?.SyncTime(null);
    }
}
