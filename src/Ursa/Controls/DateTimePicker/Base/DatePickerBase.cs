using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

[TemplatePart(PART_Popup, typeof(Popup))]
[TemplatePart(PART_TextBox, typeof(TextBox))]
[TemplatePart(PART_Calendar, typeof(DatePickerCalendarView))]
[PseudoClasses(PseudoClassName.PC_Empty)]
public abstract class DatePickerBase: TemplatedControl, IInnerContentControl, IPopupInnerContent, IClearControl
{
    public const string PART_Popup = "PART_Popup";
    public const string PART_TextBox = "PART_TextBox";
    public const string PART_Calendar = "PART_Calendar";

    protected const string DEFAULT_DATE_DISPLAY_FORMAT = "yyyy-MM-dd";

    public static readonly StyledProperty<string?> DisplayFormatProperty =
        AvaloniaProperty.Register<DatePickerBase, string?>(
            nameof(DisplayFormat), DEFAULT_DATE_DISPLAY_FORMAT);

    public static readonly StyledProperty<AvaloniaList<DateRange>> BlackoutDatesProperty =
        AvaloniaProperty.Register<DatePickerBase, AvaloniaList<DateRange>>(nameof(BlackoutDates));

    public static readonly StyledProperty<IDateSelector?> BlackoutDateRuleProperty =
        AvaloniaProperty.Register<DatePickerBase, IDateSelector?>(nameof(BlackoutDateRule));

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        AvaloniaProperty.Register<DatePickerBase, DayOfWeek>(
            nameof(FirstDayOfWeek), DateTimeHelper.GetCurrentDateTimeFormatInfo().FirstDayOfWeek);

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty =
        AvaloniaProperty.Register<DatePickerBase, bool>(nameof(IsTodayHighlighted), true);

    public static readonly StyledProperty<object?> InnerLeftContentProperty =
        AvaloniaProperty.Register<DatePickerBase, object?>(nameof(InnerLeftContent));

    public static readonly StyledProperty<object?> InnerRightContentProperty =
        AvaloniaProperty.Register<DatePickerBase, object?>(nameof(InnerRightContent));

    public static readonly StyledProperty<object?> PopupInnerTopContentProperty =
        AvaloniaProperty.Register<DatePickerBase, object?>(nameof(PopupInnerTopContent));

    public static readonly StyledProperty<object?> PopupInnerBottomContentProperty =
        AvaloniaProperty.Register<DatePickerBase, object?>(nameof(PopupInnerBottomContent));

    public static readonly StyledProperty<bool> IsDropdownOpenProperty =
        AvaloniaProperty.Register<DatePickerBase, bool>(nameof(IsDropdownOpen), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> IsReadonlyProperty =
        AvaloniaProperty.Register<DatePickerBase, bool>(nameof(IsReadonly));

    public static readonly StyledProperty<bool> NeedConfirmationProperty =
        AvaloniaProperty.Register<DatePickerBase, bool>(nameof(NeedConfirmation));

    public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty =
        TextBox.PlaceholderForegroundProperty.AddOwner<DatePickerBase>();
    
    [SuppressMessage("AvaloniaProperty", "AVP1013",
        Justification = "Obsolete property alias for backward compatibility.")]
    public static readonly StyledProperty<string?> PlaceholderTextProperty =
        TextBox.PlaceholderTextProperty.AddOwner<DatePickerBase>();

    public string? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    [Obsolete("Use PlaceholderTextProperty instead.")]
    public static readonly StyledProperty<string?> WatermarkProperty = PlaceholderTextProperty;

    [Obsolete("Use PlaceholderText instead.")]
    [SuppressMessage("AvaloniaProperty", "AVP1012",
        Justification = "Obsolete property alias for backward compatibility.")]
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
    
    public AvaloniaList<DateRange> BlackoutDates
    {
        get => GetValue(BlackoutDatesProperty);
        set => SetValue(BlackoutDatesProperty, value);
    }

    public IDateSelector? BlackoutDateRule
    {
        get => GetValue(BlackoutDateRuleProperty);
        set => SetValue(BlackoutDateRuleProperty, value);
    }

    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }

    public bool IsReadonly
    {
        get => GetValue(IsReadonlyProperty);
        set => SetValue(IsReadonlyProperty, value);
    }

    public bool NeedConfirmation
    {
        get => GetValue(NeedConfirmationProperty);
        set => SetValue(NeedConfirmationProperty, value);
    }

    public bool IsDropdownOpen
    {
        get => GetValue(IsDropdownOpenProperty);
        set => SetValue(IsDropdownOpenProperty, value);
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

    public string? DisplayFormat
    {
        get => GetValue(DisplayFormatProperty);
        set => SetValue(DisplayFormatProperty, value);
    }

    public abstract void Clear();
    public abstract void Confirm();
    public abstract void Dismiss();
}
