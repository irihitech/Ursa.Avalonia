using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

/// <summary>
///     A zero-size probe inside a decorations theme's caption button panel that
///     reports the panel's width to the host window's
///     <see cref="UrsaWindow.CaptionButtonsWidth"/>. Reports 0 while the panel is
///     hidden (fullscreen), not overlapping the client area, or once detached.
/// </summary>
public class CaptionInsetReporter : Control
{
    private UrsaWindow? _hostWindow;
    private Control? _panel;
    private IDisposable? _panelBoundsSubscription;
    private IDisposable? _extensionSubscription;

    public CaptionInsetReporter()
    {
        Width = 0;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        // The decorations render in a separate visual root: the host is only
        // reachable via the logical tree, and the panel relayouts independently
        // of the window, so watch the panel's bounds and the host's layout passes.
        _hostWindow = this.GetLogicalAncestors().OfType<UrsaWindow>().FirstOrDefault();
        _panel = Parent as Control;
        if (_hostWindow is null || _panel is null)
        {
            return;
        }

        _panelBoundsSubscription = _panel.GetObservable(BoundsProperty)
            .Subscribe(_ => Report());
        _extensionSubscription = _hostWindow.GetObservable(Window.IsExtendedIntoWindowDecorationsProperty)
            .Subscribe(_ => Report());
        _hostWindow.LayoutUpdated += OnHostLayoutUpdated;
        Report();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _panelBoundsSubscription?.Dispose();
        _panelBoundsSubscription = null;
        _extensionSubscription?.Dispose();
        _extensionSubscription = null;
        _panel = null;
        if (_hostWindow is not null)
        {
            _hostWindow.LayoutUpdated -= OnHostLayoutUpdated;
            // No probe means no caption button area (decorations disabled or rebuilt).
            _hostWindow.CaptionButtonsWidth = 0;
            _hostWindow = null;
        }
    }

    private void OnHostLayoutUpdated(object? sender, System.EventArgs e)
    {
        Report();
    }

    private void Report()
    {
        if (_hostWindow is null || _panel is not { } panel)
        {
            return;
        }

        // Zero while fullscreen hides the panel or in forced decoration mode,
        // where the buttons never overlap the client area.
        _hostWindow.CaptionButtonsWidth =
            IsEffectivelyVisible && _hostWindow.IsExtendedIntoWindowDecorations ? panel.Bounds.Width : 0;
    }
}
