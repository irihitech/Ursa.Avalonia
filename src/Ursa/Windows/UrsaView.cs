using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

/// <summary>
///     Represents a custom content control with additional properties for managing a title bar and related content.
///     This control can be used as the top level container for platforms without windowing toplevel support.
/// </summary>
public class UrsaView : ContentControl
{
    /// <summary>
    ///     The name of the dialog host part in the control template.
    /// </summary>
    public const string PART_DialogHost = "PART_DialogHost";

    /// <summary>
    ///     Defines the visibility of the title bar.
    /// </summary>
    public static readonly StyledProperty<bool> IsTitleBarVisibleProperty =
        UrsaWindow.IsTitleBarVisibleProperty.AddOwner<UrsaView>();

    /// <summary>
    ///     Defines the content on the left side of the control.
    /// </summary>
    public static readonly StyledProperty<object?> LeftContentProperty =
        UrsaWindow.LeftContentProperty.AddOwner<UrsaView>();

    /// <summary>
    ///     Defines the content on the right side of the control.
    /// </summary>
    public static readonly StyledProperty<object?> RightContentProperty =
        UrsaWindow.RightContentProperty.AddOwner<UrsaView>();

    /// <summary>
    ///     Defines the content of the title bar.
    /// </summary>
    public static readonly StyledProperty<object?> TitleBarContentProperty =
        UrsaWindow.TitleBarContentProperty.AddOwner<UrsaView>();

    /// <summary>
    ///     Defines the margin of the title bar.
    /// </summary>
    public static readonly StyledProperty<Thickness> TitleBarMarginProperty =
        UrsaWindow.TitleBarMarginProperty.AddOwner<UrsaView>();

    /// <summary>
    ///     Gets or sets a value indicating whether the title bar is visible.
    /// </summary>
    public bool IsTitleBarVisible
    {
        get => GetValue(IsTitleBarVisibleProperty);
        set => SetValue(IsTitleBarVisibleProperty, value);
    }

    /// <summary>
    ///     Gets or sets the content on the left side of the control.
    /// </summary>
    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    /// <summary>
    ///     Gets or sets the content on the right side of the control.
    /// </summary>
    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    /// <summary>
    ///     Gets or sets the content of the title bar.
    /// </summary>
    public object? TitleBarContent
    {
        get => GetValue(TitleBarContentProperty);
        set => SetValue(TitleBarContentProperty, value);
    }

    /// <summary>
    ///     Gets or sets the margin of the title bar.
    /// </summary>
    public Thickness TitleBarMargin
    {
        get => GetValue(TitleBarMarginProperty);
        set => SetValue(TitleBarMarginProperty, value);
    }

    /// <summary>
    ///     Applies the control template and initializes the dialog host if present.
    /// </summary>
    /// <param name="e">The event arguments containing the template information.</param>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var host = e.NameScope.Find<OverlayDialogHost>(PART_DialogHost);
        if (host is not null) LogicalChildren.Add(host);
    }
}