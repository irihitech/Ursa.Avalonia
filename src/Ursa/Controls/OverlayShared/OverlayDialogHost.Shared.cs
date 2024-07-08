using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Media;
using Ursa.Controls.OverlayShared;
using Avalonia.Styling;
using Avalonia.VisualTree;
using Irihi.Avalonia.Shared.Helpers;
using Irihi.Avalonia.Shared.Shapes;

namespace Ursa.Controls;

public partial class OverlayDialogHost: Canvas
{
    private static readonly Animation _maskAppearAnimation;
    private static readonly Animation _maskDisappearAnimation;

    private readonly List<DialogPair> _layers = new List<DialogPair>(10);
    
    private class DialogPair
    {
        internal PureRectangle? Mask;
        internal OverlayFeedbackElement Element;
        internal bool Modal;

        public DialogPair(PureRectangle? mask, OverlayFeedbackElement element, bool modal = true)
        {
            Mask = mask;
            Element = element;
            Modal = modal;
        }
    }

    private int _modalCount = 0;

    public static readonly DirectProperty<OverlayDialogHost, bool> HasModalProperty = AvaloniaProperty.RegisterDirect<OverlayDialogHost, bool>(
        nameof(HasModal), o => o.HasModal);
    private bool _hasModal;
    [Obsolete("Use IsInModalStatus")]
    public bool HasModal
    {
        get => _hasModal;
        private set => SetAndRaise(HasModalProperty, ref _hasModal, value);
    }

    public static readonly AttachedProperty<bool> IsModalStatusScopeProperty =
        AvaloniaProperty.RegisterAttached<OverlayDialogHost, Control, bool>("IsModalStatusScope");

    public static void SetIsModalStatusScope(Control obj, bool value) => obj.SetValue(IsModalStatusScopeProperty, value);
    internal static bool GetIsModalStatusScope(Control obj) => obj.GetValue(IsModalStatusScopeProperty);

    public static readonly AttachedProperty<bool> IsInModalStatusProperty =
        AvaloniaProperty.RegisterAttached<OverlayDialogHost, Control, bool>(nameof(IsInModalStatus));

    internal static void SetIsInModalStatus(Control obj, bool value) => obj.SetValue(IsInModalStatusProperty, value);
    public static bool GetIsInModalStatus(Control obj) => obj.GetValue(IsInModalStatusProperty);

    public bool IsInModalStatus
    {
        get => GetValue(IsInModalStatusProperty);
        set => SetValue(IsInModalStatusProperty, value);
    }
    
    public bool IsAnimationDisabled { get; set; }
    
    static OverlayDialogHost()
    {
        ClipToBoundsProperty.OverrideDefaultValue<OverlayDialogHost>(true);
        _maskAppearAnimation = CreateOpacityAnimation(true);
        _maskDisappearAnimation = CreateOpacityAnimation(false);
        // This is only a temporary solution, will be removed in release candidate mode. 
        IsInModalStatusProperty.Changed.AddClassHandler<OverlayDialogHost, bool>((host, args) =>
        {
            host.HasModal = args.NewValue.Value;
        });
    }
    
    private static Animation CreateOpacityAnimation(bool appear)
    {
        var animation = new Animation();
        animation.FillMode = FillMode.Forward;
        var keyFrame1 = new KeyFrame{ Cue = new Cue(0.0) };
        keyFrame1.Setters.Add(new Setter() { Property = OpacityProperty, Value = appear ? 0.0 : 1.0 });
        var keyFrame2 = new KeyFrame{ Cue = new Cue(1.0) };
        keyFrame2.Setters.Add(new Setter() { Property = OpacityProperty, Value = appear ? 1.0 : 0.0 });
        animation.Children.Add(keyFrame1);
        animation.Children.Add(keyFrame2);
        animation.Duration = TimeSpan.FromSeconds(0.2);
        return animation;
    }
    
    public string? HostId { get; set; }
    
    public DataTemplates DialogDataTemplates { get; set; } = new DataTemplates();
    
    public static readonly StyledProperty<IBrush?> OverlayMaskBrushProperty =
        AvaloniaProperty.Register<OverlayDialogHost, IBrush?>(
            nameof(OverlayMaskBrush));

    public IBrush? OverlayMaskBrush
    {
        get => GetValue(OverlayMaskBrushProperty);
        set => SetValue(OverlayMaskBrushProperty, value);
    }
    
    private PureRectangle CreateOverlayMask(bool modal, bool canCloseOnClick)
    {
        PureRectangle rec = new()
        {
            Width = this.Bounds.Width,
            Height = this.Bounds.Height,
            IsVisible = true,
        };
        if (modal)
        {
            rec[!PureRectangle.BackgroundProperty] = this[!OverlayMaskBrushProperty];
        }
        else if(canCloseOnClick) 
        { 
            rec.SetCurrentValue(PureRectangle.BackgroundProperty, Brushes.Transparent);
        }
        if (canCloseOnClick)
        {
            rec.AddHandler(PointerReleasedEvent, ClickMaskToCloseDialog);
        }
        return rec;
    }
    
    private void ClickMaskToCloseDialog(object sender, PointerReleasedEventArgs e)
    {
        if (sender is PureRectangle border)
        {
            var layer = _layers.FirstOrDefault(a => a.Mask == border);
            if (layer is not null)
            {
                layer.Element.Close();
                border.RemoveHandler(PointerReleasedEvent, ClickMaskToCloseDialog);
            }
        }
    }
    private IDisposable? _modalStatusSubscription;
    protected sealed override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var hash = TopLevel.GetTopLevel(this)?.GetHashCode();
        var modalHost = this.GetVisualAncestors().OfType<Control>().FirstOrDefault(GetIsModalStatusScope);
        if (modalHost is not null)
        {
            _modalStatusSubscription = this.GetObservable(IsInModalStatusProperty)
                .Subscribe(a => OverlayDialogHost.SetIsInModalStatus(modalHost, a));
        }
        OverlayDialogManager.RegisterHost(this, HostId, hash);
    }
    
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        while (_layers.Count>0)
        {
            _layers[0].Element.Close();
        }
        _modalStatusSubscription?.Dispose();
        var hash = TopLevel.GetTopLevel(this)?.GetHashCode();
        OverlayDialogManager.UnregisterHost(HostId, hash);
        base.OnDetachedFromVisualTree(e);
    }
    
    
    protected sealed override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        for (int i = 0; i < _layers.Count; i++)
        {
            if (_layers[i].Mask is { } rect)
            {
                rect.Width = this.Bounds.Width;
                rect.Height = this.Bounds.Height;
            }
            if (_layers[i].Element is DialogControlBase d)
            {
                ResetDialogPosition(d, e.NewSize);
            }
            else if (_layers[i].Element is DrawerControlBase drawer)
            {
                ResetDrawerPosition(drawer, e.NewSize);
            }
        }
    }
    
    private void ResetZIndices()
    {
        int index = 0;
        for (int i = 0; i < _layers.Count; i++)
        {
            if(_layers[i].Mask is { } mask)
            {
                mask.ZIndex = index;
                index++;
            }
            if(_layers[i].Element is { } dialog)
            {
                dialog.ZIndex = index;
                index++;
            }
        }
    }
    
    internal IDataTemplate? GetDataTemplate(object? o)
    {
        if (o is null) return null;
        IDataTemplate? result = null;
        var templates = this.DialogDataTemplates;
        result = templates.FirstOrDefault(a => a.Match(o));
        if (result != null) return result;
        var keys = this.Resources.Keys;
        foreach (var key in keys)
        {
            if (Resources.TryGetValue(key, out var value) && value is IDataTemplate t)
            {
                result = t;
                break;
            }
        }
        return result;
    }

    internal T? Recall<T>()
    {
        var element = _layers.LastOrDefault(a => a.Element.Content?.GetType() == typeof(T));
        return element?.Element.Content is T t ? t : default;
    }
}