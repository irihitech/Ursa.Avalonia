using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[PseudoClasses(PseudoClassName.PC_Active)]
public class TitleBar: ContentControl
{
    private CaptionButtons? _captionButtons;
    private InputElement? _background;
    private Window? _visualRoot;
    private IDisposable? _activeSubscription;
    
    public static readonly StyledProperty<object?> LeftContentProperty = AvaloniaProperty.Register<TitleBar, object?>(
        nameof(LeftContent));

    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    public static readonly StyledProperty<object?> RightContentProperty = AvaloniaProperty.Register<TitleBar, object?>(
        nameof(RightContent));

    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public static readonly StyledProperty<bool> IsTitleVisibleProperty = AvaloniaProperty.Register<TitleBar, bool>(
        nameof(IsTitleVisible));

    public bool IsTitleVisible
    {
        get => GetValue(IsTitleVisibleProperty);
        set => SetValue(IsTitleVisibleProperty, value);
    }
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        this._captionButtons?.Detach();
        this._captionButtons = e.NameScope.Get<CaptionButtons>("PART_CaptionButtons");
        this._background = e.NameScope.Get<InputElement>("PART_Background");
        DoubleTappedEvent.AddHandler(OnDoubleTapped, _background);
        PointerPressedEvent.AddHandler(OnPointerPressed, _background);
        this._captionButtons?.Attach(_visualRoot);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _visualRoot = this.VisualRoot as Window;
        if (_visualRoot is not null)
        {
            _activeSubscription = _visualRoot.GetObservable(WindowBase.IsActiveProperty).Subscribe(isActive =>
            {
                PseudoClasses.Set(PseudoClassName.PC_Active, isActive);
            });
        }
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if(_visualRoot is not null && _visualRoot.WindowState == WindowState.FullScreen)
        {
            return;
        }
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            if (e.ClickCount < 2) 
            {
                _visualRoot?.BeginMoveDrag(e);
            }
        }
    }

    private void OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (_visualRoot is null) return;
        if (!_visualRoot.CanResize) return;
        if ( _visualRoot.WindowState == WindowState.FullScreen) return;
        _visualRoot.WindowState = _visualRoot.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void UpdateSize(Window window)
    {
        Thickness offScreenMargin = window.OffScreenMargin;
        var left = offScreenMargin.Left;
        offScreenMargin = window.OffScreenMargin;
        double top = offScreenMargin.Top;
        offScreenMargin = window.OffScreenMargin;
        double right = offScreenMargin.Right;
        offScreenMargin = window.OffScreenMargin;
        double bottom = offScreenMargin.Bottom;
        this.Margin = new Thickness(left, top, right, bottom);
        if (window.WindowState != WindowState.FullScreen)
        {
            this.Height = window.WindowDecorationMargin.Top;
            if (this._captionButtons != null)
                this._captionButtons.Height = this.Height;
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _captionButtons?.Detach();
        _activeSubscription?.Dispose();
    }
}