using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

public abstract class TimePickerBase : TemplatedControl, IInnerContentControl, IPopupInnerContent
{
    public static readonly StyledProperty<string?> DisplayFormatProperty =
        AvaloniaProperty.Register<TimePicker, string?>(
            nameof(DisplayFormat), "HH:mm:ss");

    public static readonly StyledProperty<string> PanelFormatProperty = AvaloniaProperty.Register<TimePicker, string>(
        nameof(PanelFormat), "HH mm ss");

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

    public static readonly StyledProperty<bool> IsDropdownOpenProperty = AvaloniaProperty.Register<TimePicker, bool>(
        nameof(IsDropdownOpen), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> IsReadonlyProperty = AvaloniaProperty.Register<TimePicker, bool>(
        nameof(IsReadonly));

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
}