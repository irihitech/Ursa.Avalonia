using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
[TemplatePart(PART_RestoreButton, typeof(Button))]
[TemplatePart(PART_MinimizeButton, typeof(Button))]
[TemplatePart(PART_FullScreenButton, typeof(Button))]
[PseudoClasses(":minimized", ":normal", ":maximized", ":fullscreen")]
public class CaptionButtons : Avalonia.Controls.Chrome.CaptionButtons
{
    private const string PART_CloseButton = "PART_CloseButton";
    private const string PART_RestoreButton = "PART_RestoreButton";
    private const string PART_MinimizeButton = "PART_MinimizeButton";
    private const string PART_FullScreenButton = "PART_FullScreenButton";
    private IDisposable? _camMaximizeSubscription;
    private IDisposable? _canMinimizeSubscription;

    private Button? _closeButton;
    private IDisposable? _closeSubscription;
    private Button? _fullScreenButton;
    private IDisposable? _fullScreenSubscription;
    private Button? _minimizeButton;
    private IDisposable? _minimizeSubscription;

    /// <summary>
    ///     切换进入全屏前 窗口的状态
    /// </summary>
    private WindowState? _oldWindowState;

    private Button? _restoreButton;
    private IDisposable? _restoreSubscription;

    private IDisposable? _windowStateSubscription;

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

        if (HostWindow is not null && (!HostWindow.CanResize || !HostWindow.CanMaximize))
            _restoreButton.IsEnabled = false;
        UpdateVisibility();
    }

    protected override void OnToggleFullScreen()
    {
        if (HostWindow != null)
        {
            if (HostWindow.WindowState != WindowState.FullScreen)
                HostWindow.WindowState = WindowState.FullScreen;
            else
                HostWindow.WindowState = _oldWindowState ?? WindowState.Normal;
        }
    }

    public override void Attach(Window? hostWindow)
    {
        if (hostWindow is null) return;
        base.Attach(hostWindow);
        if (HostWindow is not null) HostWindow.PropertyChanged += OnWindowPropertyChanged;
    }

    private void OnWindowPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == Window.WindowStateProperty)
        {
            UpdateVisibility();
            if (e.GetNewValue<WindowState>() == WindowState.FullScreen) _oldWindowState = e.GetOldValue<WindowState>();
        }

        if (e.Property == UrsaWindow.IsFullScreenButtonVisibleProperty
            || e.Property == UrsaWindow.IsMinimizeButtonVisibleProperty
            || e.Property == UrsaWindow.IsRestoreButtonVisibleProperty
            || e.Property == UrsaWindow.IsCloseButtonVisibleProperty
            || e.Property == Window.CanMaximizeProperty 
            || e.Property == Window.CanMinimizeProperty)
        {
            UpdateVisibility();
        }
    }

    private void UpdateVisibility()
    {
        if (HostWindow is UrsaWindow u)
        {
            IsVisibleProperty.SetValue(u.IsCloseButtonVisible, _closeButton);
            IsVisibleProperty.SetValue(u.CanMaximize && u.WindowState != WindowState.FullScreen && u.IsRestoreButtonVisible,
                _restoreButton);
            IsVisibleProperty.SetValue(
                u.CanMinimize && u.WindowState != WindowState.FullScreen && u.IsMinimizeButtonVisible,
                _minimizeButton);
            IsVisibleProperty.SetValue(u.IsFullScreenButtonVisible, _fullScreenButton);
        }
        else if (HostWindow is { } s)
        {
            IsVisibleProperty.SetValue(s.CanMaximize && s.WindowState != WindowState.FullScreen, _restoreButton);
            IsVisibleProperty.SetValue(s.CanMinimize && s.WindowState != WindowState.FullScreen, _minimizeButton);
        }
    }

    public override void Detach()
    {
        if (HostWindow is not null)
        {
             HostWindow.PropertyChanged -= OnWindowPropertyChanged;
        }
        base.Detach();
        
    }
}