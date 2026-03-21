using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

public class TitleBar : TemplatedControl
{
    public static readonly StyledProperty<object?> LeftContentProperty = AvaloniaProperty.Register<TitleBar, object?>(
        nameof(LeftContent));

    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    public static readonly StyledProperty<object?> CenterContentProperty = AvaloniaProperty.Register<TitleBar, object?>(
        nameof(CenterContent));

    public object? CenterContent
    {
        get => GetValue(CenterContentProperty);
        set => SetValue(CenterContentProperty, value);
    }

    public static readonly StyledProperty<object?> RightContentProperty = AvaloniaProperty.Register<TitleBar, object?>(
        nameof(RightContent));

    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }
}