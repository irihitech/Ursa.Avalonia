using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;

/// <summary>
/// Ursa Window is an advanced Window control that provides a lot of features and customization options.
/// </summary>
public class UrsaWindow: Window
{
    protected override Type StyleKeyOverride => typeof(UrsaWindow);

    public static readonly StyledProperty<bool> IsFullScreenButtonVisibleProperty = AvaloniaProperty.Register<UrsaWindow, bool>(
        nameof(IsFullScreenButtonVisible));

    public bool IsFullScreenButtonVisible
    {
        get => GetValue(IsFullScreenButtonVisibleProperty);
        set => SetValue(IsFullScreenButtonVisibleProperty, value);
    }
    
    public static readonly StyledProperty<bool> IsMinimizeButtonVisibleProperty = AvaloniaProperty.Register<UrsaWindow, bool>(
        nameof(IsMinimizeButtonVisible), true);
    
    public bool IsMinimizeButtonVisible
    {
        get => GetValue(IsMinimizeButtonVisibleProperty);
        set => SetValue(IsMinimizeButtonVisibleProperty, value);
    }

    public static readonly StyledProperty<bool> IsRestoreButtonVisibleProperty = AvaloniaProperty.Register<UrsaWindow, bool>(
        nameof(IsRestoreButtonVisible), true);

    public bool IsRestoreButtonVisible
    {
        get => GetValue(IsRestoreButtonVisibleProperty);
        set => SetValue(IsRestoreButtonVisibleProperty, value);
    }
    
    public static readonly StyledProperty<bool> IsCloseButtonVisibleProperty = AvaloniaProperty.Register<UrsaWindow, bool>(
        nameof(IsCloseButtonVisible), true);
    
    public bool IsCloseButtonVisible
    {
        get => GetValue(IsCloseButtonVisibleProperty);
        set => SetValue(IsCloseButtonVisibleProperty, value);
    }
    
    public static readonly StyledProperty<bool> IsTitleBarVisibleProperty = AvaloniaProperty.Register<UrsaWindow, bool>(
        nameof(IsTitleBarVisible), true);
    
    public bool IsTitleBarVisible
    {
        get => GetValue(IsTitleBarVisibleProperty);
        set => SetValue(IsTitleBarVisibleProperty, value);
    }
    
    public static readonly StyledProperty<object?> TitleBarContentProperty = AvaloniaProperty.Register<UrsaWindow, object?>(
        nameof(TitleBarContent));
    
    public object? TitleBarContent
    {
        get => GetValue(TitleBarContentProperty);
        set => SetValue(TitleBarContentProperty, value);
    }
    
    public static readonly StyledProperty<object?> LeftContentProperty = AvaloniaProperty.Register<UrsaWindow, object?>(
        nameof(LeftContent));
    
    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }
    
    public static readonly StyledProperty<object?> RightContentProperty = AvaloniaProperty.Register<UrsaWindow, object?>(
        nameof(RightContent));
    
    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public static readonly StyledProperty<Thickness> TitleBarMarginProperty = AvaloniaProperty.Register<UrsaWindow, Thickness>(
        nameof(TitleBarMargin));

    public Thickness TitleBarMargin
    {
        get => GetValue(TitleBarMarginProperty);
        set => SetValue(TitleBarMarginProperty, value);
    }
    
    
}