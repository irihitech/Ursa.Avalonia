using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;

/// <summary>
/// Ursa window is designed to 
/// </summary>
public class UrsaView: ContentControl
{
    public static readonly StyledProperty<bool> IsTitleBarVisibleProperty =
        UrsaWindow.IsTitleBarVisibleProperty.AddOwner<UrsaView>();

    public bool IsTitleBarVisible
    {
        get => GetValue(IsTitleBarVisibleProperty);
        set => SetValue(IsTitleBarVisibleProperty, value);
    }

    public static readonly StyledProperty<object?> LeftContentProperty =
        UrsaWindow.LeftContentProperty.AddOwner<UrsaView>();

    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    public static readonly StyledProperty<object?> RightContentProperty =
        UrsaWindow.RightContentProperty.AddOwner<UrsaView>();

    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public static readonly StyledProperty<object?> TitleBarContentProperty =
        UrsaWindow.TitleBarContentProperty.AddOwner<UrsaView>();

    public object? TitleBarContent
    {
        get => GetValue(TitleBarContentProperty);
        set => SetValue(TitleBarContentProperty, value);
    }

    public static readonly StyledProperty<Thickness> TitleBarMarginProperty =
        UrsaWindow.TitleBarMarginProperty.AddOwner<UrsaView>();
    
    public Thickness TitleBarMargin
    {
        get => GetValue(TitleBarMarginProperty);
        set => SetValue(TitleBarMarginProperty, value);
    }
}