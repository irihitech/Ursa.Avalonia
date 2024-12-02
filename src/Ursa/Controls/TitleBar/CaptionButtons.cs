using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof (Button))]
[TemplatePart(PART_RestoreButton, typeof (Button))]
[TemplatePart(PART_MinimizeButton, typeof (Button))]
[TemplatePart(PART_FullScreenButton, typeof (Button))]
[PseudoClasses(":minimized", ":normal", ":maximized", ":fullscreen")]
public class CaptionButtons: Avalonia.Controls.Chrome.CaptionButtons
{
    private const string PART_CloseButton = "PART_CloseButton";
    private const string PART_RestoreButton = "PART_RestoreButton";
    private const string PART_MinimizeButton = "PART_MinimizeButton";
    private const string PART_FullScreenButton = "PART_FullScreenButton";

    private Button? _closeButton;
    private Button? _restoreButton;
    private Button? _minimizeButton;
    private Button? _fullScreenButton;
    
    private IDisposable? _windowStateSubscription;
    private IDisposable? _fullScreenSubscription;
    private IDisposable? _minimizeSubscription;
    private IDisposable? _restoreSubscription;
    private IDisposable? _closeSubscription;

    /// <summary>
    /// 切换进入全屏前 窗口的状态
    /// </summary>
    private WindowState? _oldWindowState;
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _closeButton = e.NameScope.Get<Button>(PART_CloseButton);
        _restoreButton = e.NameScope.Get<Button>(PART_RestoreButton);
        _minimizeButton = e.NameScope.Get<Button>(PART_MinimizeButton);
        _fullScreenButton = e.NameScope.Get<Button>(PART_FullScreenButton);
        Button.ClickEvent.AddHandler((_, _) => OnClose(), _closeButton);
        Button.ClickEvent.AddHandler((_, _) => OnRestore(), _restoreButton);
        Button.ClickEvent.AddHandler((_, _) => OnMinimize(), _minimizeButton);
        Button.ClickEvent.AddHandler((_, _) => OnToggleFullScreen(), _fullScreenButton);

        Window.WindowStateProperty.Changed.AddClassHandler<Window, WindowState>(WindowStateChanged);
        if (this.HostWindow is not null && !HostWindow.CanResize)
        {
            _restoreButton.IsEnabled = false;
        }
        UpdateVisibility();
    }

    private void WindowStateChanged(Window window, AvaloniaPropertyChangedEventArgs<WindowState> e)
    {
        if (window != HostWindow) return;
        if (e.NewValue.Value == WindowState.FullScreen)
        {
            _oldWindowState = e.OldValue.Value;
        }
    }

    protected override void OnToggleFullScreen()
    {
        if (HostWindow != null)
        {
            if (HostWindow.WindowState != WindowState.FullScreen)
            {
                HostWindow.WindowState = WindowState.FullScreen;
            }
            else
            {
                HostWindow.WindowState = _oldWindowState.HasValue ? _oldWindowState.Value : WindowState.Normal;
            }
        }
    }
    public override void Attach(Window? hostWindow)
    {
        if (hostWindow is null) return;
        base.Attach(hostWindow);
        _windowStateSubscription = HostWindow?.GetObservable(Window.WindowStateProperty).Subscribe(_ =>
        {
            UpdateVisibility();
        });
        Action<bool> a = (_) => UpdateVisibility();
        _fullScreenSubscription = HostWindow?.GetObservable(UrsaWindow.IsFullScreenButtonVisibleProperty).Subscribe(a);
        _minimizeSubscription = HostWindow?.GetObservable(UrsaWindow.IsMinimizeButtonVisibleProperty).Subscribe(a);
        _restoreSubscription = HostWindow?.GetObservable(UrsaWindow.IsRestoreButtonVisibleProperty).Subscribe(a);
        _closeSubscription = HostWindow?.GetObservable(UrsaWindow.IsCloseButtonVisibleProperty).Subscribe(a);
    }
    
    private void UpdateVisibility()
    {
        if (HostWindow is not UrsaWindow u)
        {
            return;
        }

        IsVisibleProperty.SetValue(u.IsCloseButtonVisible, _closeButton);
        IsVisibleProperty.SetValue(u.WindowState != WindowState.FullScreen && u.IsRestoreButtonVisible,
            _restoreButton);
        IsVisibleProperty.SetValue(u.WindowState != WindowState.FullScreen && u.IsMinimizeButtonVisible,
            _minimizeButton);
        IsVisibleProperty.SetValue(u.IsFullScreenButtonVisible, _fullScreenButton);
    }

    public override void Detach()
    {
        base.Detach();
        _windowStateSubscription?.Dispose();
        _fullScreenSubscription?.Dispose();
        _minimizeSubscription?.Dispose();
        _restoreSubscription?.Dispose();
        _closeSubscription?.Dispose();
    }
}