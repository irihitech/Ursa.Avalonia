using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;

public enum BannerType
{
    None,
    Info,
    Warning,
    Danger,
    Success,
}
public class Banner: ContentControl
{
    public static readonly StyledProperty<bool> CanCloseProperty = AvaloniaProperty.Register<Banner, bool>(
        nameof(CanClose));

    public bool CanClose
    {
        get => GetValue(CanCloseProperty);
        set => SetValue(CanCloseProperty, value);
    }

    public static readonly StyledProperty<bool> ShowIconProperty = AvaloniaProperty.Register<Banner, bool>(
        nameof(ShowIcon));

    public bool ShowIcon
    {
        get => GetValue(ShowIconProperty);
        set => SetValue(ShowIconProperty, value);
    }

    public static readonly StyledProperty<object?> HeaderProperty = AvaloniaProperty.Register<Banner, object?>(
        nameof(Header));

    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }
    
    
}