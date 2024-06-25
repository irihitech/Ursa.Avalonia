using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;

namespace Ursa.Controls;

/// <summary>
/// Ursa Window is an advanced Window control that provides a lot of features and customization options.
/// </summary>
public class UrsaWindow: Window
{
    public static readonly StyledProperty<bool> ShowFullScreenButtonProperty = AvaloniaProperty.Register<UrsaWindow, bool>(
        nameof(ShowFullScreenButton), false);

    public bool ShowFullScreenButton
    {
        get => GetValue(ShowFullScreenButtonProperty);
        set => SetValue(ShowFullScreenButtonProperty, value);
    }

    public static readonly StyledProperty<bool> ShowMaximumButtonProperty = AvaloniaProperty.Register<UrsaWindow, bool>(
        nameof(ShowMaximumButton));

    public bool ShowMaximumButton
    {
        get => GetValue(ShowMaximumButtonProperty);
        set => SetValue(ShowMaximumButtonProperty, value);
    }
    
    public static readonly StyledProperty<bool> ShowMinimumButtonProperty = AvaloniaProperty.Register<UrsaWindow, bool>(
        nameof(ShowMinimumButton));
    
    public bool ShowMinimumButton
    {
        get => GetValue(ShowMinimumButtonProperty);
        set => SetValue(ShowMinimumButtonProperty, value);
    }
    
    public static readonly StyledProperty<bool> ShowCloseButtonProperty = AvaloniaProperty.Register<UrsaWindow, bool>(
        nameof(ShowCloseButton));
    
    public bool ShowCloseButton
    {
        get => GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }
    
    
    
}