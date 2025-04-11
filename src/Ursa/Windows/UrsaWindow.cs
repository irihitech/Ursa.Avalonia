using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

/// <summary>
///     Ursa Window is an advanced Window control that provides a lot of features and customization options.
/// </summary>
public class UrsaWindow : Window
{
    /// <summary>
    /// The name of the dialog host part in the control template.
    /// </summary>
    public const string PART_DialogHost = "PART_DialogHost";

    /// <summary>
    /// Defines the visibility of the full-screen button.
    /// </summary>
    public static readonly StyledProperty<bool> IsFullScreenButtonVisibleProperty =
        AvaloniaProperty.Register<UrsaWindow, bool>(
            nameof(IsFullScreenButtonVisible));

    /// <summary>
    /// Defines the visibility of the minimize button.
    /// </summary>
    public static readonly StyledProperty<bool> IsMinimizeButtonVisibleProperty =
        AvaloniaProperty.Register<UrsaWindow, bool>(
            nameof(IsMinimizeButtonVisible), true);

    /// <summary>
    /// Defines the visibility of the restore button.
    /// </summary>
    public static readonly StyledProperty<bool> IsRestoreButtonVisibleProperty =
        AvaloniaProperty.Register<UrsaWindow, bool>(
            nameof(IsRestoreButtonVisible), true);

    /// <summary>
    /// Defines the visibility of the close button.
    /// </summary>
    public static readonly StyledProperty<bool> IsCloseButtonVisibleProperty =
        AvaloniaProperty.Register<UrsaWindow, bool>(
            nameof(IsCloseButtonVisible), true);

    /// <summary>
    /// Defines the visibility of the title bar.
    /// </summary>
    public static readonly StyledProperty<bool> IsTitleBarVisibleProperty = AvaloniaProperty.Register<UrsaWindow, bool>(
        nameof(IsTitleBarVisible), true);

    /// <summary>
    /// Defines the visibility of the managed resizer.
    /// </summary>
    public static readonly StyledProperty<bool> IsManagedResizerVisibleProperty =
        AvaloniaProperty.Register<UrsaWindow, bool>(
            nameof(IsManagedResizerVisible));

    /// <summary>
    /// Defines the content of the title bar.
    /// </summary>
    public static readonly StyledProperty<object?> TitleBarContentProperty =
        AvaloniaProperty.Register<UrsaWindow, object?>(
            nameof(TitleBarContent));

    /// <summary>
    /// Defines the content on the left side of the window.
    /// </summary>
    public static readonly StyledProperty<object?> LeftContentProperty = AvaloniaProperty.Register<UrsaWindow, object?>(
        nameof(LeftContent));

    /// <summary>
    /// Defines the content on the right side of the window.
    /// </summary>
    public static readonly StyledProperty<object?> RightContentProperty =
        AvaloniaProperty.Register<UrsaWindow, object?>(
            nameof(RightContent));

    /// <summary>
    /// Defines the margin of the title bar.
    /// </summary>
    public static readonly StyledProperty<Thickness> TitleBarMarginProperty =
        AvaloniaProperty.Register<UrsaWindow, Thickness>(
            nameof(TitleBarMargin));

    private bool _canClose;
    
    /// <summary>
    /// Gets the style key override for the control.
    /// </summary>
    protected override Type StyleKeyOverride => typeof(UrsaWindow);

    /// <summary>
    /// Gets or sets a value indicating whether the full-screen button is visible.
    /// </summary>
    public bool IsFullScreenButtonVisible
    {
        get => GetValue(IsFullScreenButtonVisibleProperty);
        set => SetValue(IsFullScreenButtonVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the minimize button is visible.
    /// </summary>
    public bool IsMinimizeButtonVisible
    {
        get => GetValue(IsMinimizeButtonVisibleProperty);
        set => SetValue(IsMinimizeButtonVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the restore button is visible.
    /// </summary>
    public bool IsRestoreButtonVisible
    {
        get => GetValue(IsRestoreButtonVisibleProperty);
        set => SetValue(IsRestoreButtonVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the close button is visible.
    /// </summary>
    public bool IsCloseButtonVisible
    {
        get => GetValue(IsCloseButtonVisibleProperty);
        set => SetValue(IsCloseButtonVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the title bar is visible.
    /// </summary>
    public bool IsTitleBarVisible
    {
        get => GetValue(IsTitleBarVisibleProperty);
        set => SetValue(IsTitleBarVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the managed resizer is visible.
    /// </summary>
    public bool IsManagedResizerVisible
    {
        get => GetValue(IsManagedResizerVisibleProperty);
        set => SetValue(IsManagedResizerVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets the content of the title bar.
    /// </summary>
    public object? TitleBarContent
    {
        get => GetValue(TitleBarContentProperty);
        set => SetValue(TitleBarContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the content on the left side of the window.
    /// </summary>
    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the content on the right side of the window.
    /// </summary>
    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin of the title bar.
    /// </summary>
    public Thickness TitleBarMargin
    {
        get => GetValue(TitleBarMarginProperty);
        set => SetValue(TitleBarMarginProperty, value);
    }
    
    /// <inheritdoc/>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var host = e.NameScope.Find<OverlayDialogHost>(PART_DialogHost);
        if (host is not null) LogicalChildren.Add(host);
    }

    /// <summary>
    /// Determines whether the window can close.
    /// </summary>
    /// <returns>A task that resolves to true if the window can close; otherwise, false.</returns>
    protected virtual async Task<bool> CanClose()
    {
        return await Task.FromResult(true);
    }

    /// <summary>
    /// Handles the window closing event and determines whether the window should close.
    /// </summary>
    /// <param name="e">The event arguments for the closing event.</param>
    protected override async void OnClosing(WindowClosingEventArgs e)
    {
        VerifyAccess();
        if (!_canClose)
        {
            e.Cancel = true;
            _canClose = await CanClose();
            if (_canClose)
            {
                Close();
                return;
            }
        }
        base.OnClosing(e);
    }
}