using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;

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

    public abstract void Confirm();
    public abstract void Dismiss();
    public abstract void Clear();
}
