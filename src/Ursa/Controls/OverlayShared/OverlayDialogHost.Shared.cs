using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Media;
using Ursa.Controls.OverlayShared;
using Avalonia.Layout;
using Avalonia.Media.Immutable;
using Avalonia.Styling;
using Ursa.Controls.Shapes;

namespace Ursa.Controls;

public partial class OverlayDialogHost: Canvas
{
    private static readonly Animation _maskAppearAnimation;
    private static readonly Animation _maskDisappearAnimation;

    private readonly List<DialogPair> _layers = new List<DialogPair>();
    
    private class DialogPair
    {
        internal PureRectangle? Mask;
        internal OverlayFeedbackElement Element;

        public DialogPair(PureRectangle? mask, OverlayFeedbackElement element)
        {
            Mask = mask;
            Element = element;
        }
    }
    
    static OverlayDialogHost()
    {
        ClipToBoundsProperty.OverrideDefaultValue<OverlayDialogHost>(true);
        _maskAppearAnimation = CreateOpacityAnimation(true);
        _maskDisappearAnimation = CreateOpacityAnimation(false);
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
            rec[!Shape.FillProperty] = this[!OverlayMaskBrushProperty];
        }
        else if(canCloseOnClick) 
        { 
            rec.SetCurrentValue(Shape.FillProperty, Brushes.Transparent);
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
    
    protected sealed override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        OverlayDialogManager.RegisterHost(this, HostId);
    }
    
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        OverlayDialogManager.UnregisterHost(HostId);
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
}