using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace Ursa.Controls;

public class TitleBar : TemplatedControl
{
    // Control-level chrome interaction instead of WindowDecorationProperties.ElementRole:
    // on X11 the backend swallows presses on TitleBar-role elements for WM move/resize,
    // so double-click and app-level handling never fire (avaloniaui/avalonia#22239).
    // Upstream fix PR avaloniaui/avalonia#22586 moves this into the backends; once our
    // minimum Avalonia version includes it, we can restore ElementRole and drop this.
    private bool _maximizeTogglePending;

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

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (e.Handled || !e.Properties.IsLeftButtonPressed)
        {
            return;
        }

        if (FindHostWindow() is not { } window ||
            window.WindowState == WindowState.FullScreen ||
            !IsChromeArea(e.Source))
        {
            return;
        }

        if (e.ClickCount == 2)
        {
            // Defer the state change to PointerReleased: switching away from
            // Maximized during the press resizes the window and breaks the
            // ongoing pointer event chain on Windows.
            _maximizeTogglePending = true;
            e.Handled = true;
        }
        else
        {
            window.BeginMoveDrag(e);
            e.Handled = true;
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (_maximizeTogglePending &&
            e.InitialPressMouseButton == MouseButton.Left &&
            FindHostWindow() is { CanResize: true, CanMaximize: true } window &&
            window.WindowState is WindowState.Normal or WindowState.Maximized)
        {
            window.WindowState = window.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }
        _maximizeTogglePending = false;
    }

    protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e)
    {
        base.OnPointerCaptureLost(e);
        _maximizeTogglePending = false;
    }

    /// <summary>
    ///     The title bar only drives the window when it is the window's own chrome.
    ///     A TitleBar hosted inside an OverlayDialogHost belongs to a dialog and
    ///     must not move or maximize the host window.
    /// </summary>
    private UrsaWindow? FindHostWindow()
    {
        if (this.FindAncestorOfType<OverlayDialogHost>() is not null)
        {
            return null;
        }
        return this.FindAncestorOfType<UrsaWindow>();
    }

    /// <summary>
    ///     True when the press landed on the title bar itself (background or
    ///     template panels) rather than on content hosted in one of the three
    ///     content slots, so interactive content keeps receiving clicks.
    /// </summary>
    private bool IsChromeArea(object? source)
    {
        var visual = source as Visual;
        while (visual is not null && !ReferenceEquals(visual, this))
        {
            if (visual is ContentPresenter)
            {
                return false;
            }
            visual = visual.GetVisualParent();
        }
        return visual is not null;
    }
}
