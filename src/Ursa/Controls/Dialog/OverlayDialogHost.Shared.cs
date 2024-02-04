using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Media;
using Ursa.Controls.OverlayShared;
using Avalonia.Layout;
using Avalonia.Styling;

namespace Ursa.Controls;

public partial class OverlayDialogHost: Canvas
{
    private readonly List<OverlayFeedbackElement> _dialogs = new();
    private readonly List<OverlayFeedbackElement> _modalDialogs = new(); 
    private readonly List<Border> _masks = new();
    private static readonly Animation _maskAppearAnimation;
    private static readonly Animation _maskDisappearAnimation;
    
    static OverlayDialogHost()
    {
        _maskAppearAnimation = CreateOpacityAnimation(true);
        _maskDisappearAnimation = CreateOpacityAnimation(false);
    }
    
    private static Animation CreateOpacityAnimation(bool appear)
    {
        var animation = new Animation();
        animation.FillMode = FillMode.Forward;
        var keyFrame1 = new KeyFrame(){ Cue = new Cue(0.0) };
        keyFrame1.Setters.Add(new Setter() { Property = OpacityProperty, Value = appear ? 0.0 : 1.0 });
        var keyFrame2 = new KeyFrame() { Cue = new Cue(1.0) };
        keyFrame2.Setters.Add(new Setter() { Property = OpacityProperty, Value = appear ? 1.0 : 0.0 });
        animation.Children.Add(keyFrame1);
        animation.Children.Add(keyFrame2);
        animation.Duration = TimeSpan.FromSeconds(0.3);
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
    
    private Border CreateOverlayMask(bool canCloseOnClick)
    {
        Border border = new()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Width = this.Bounds.Width,
            Height = this.Bounds.Height,
            [!BackgroundProperty] = this[!OverlayMaskBrushProperty],
            IsVisible = true,
        };
        if (canCloseOnClick)
        {
            border.AddHandler(PointerReleasedEvent, ClickBorderToCloseDialog);
        }
        return border;
    }
    
    private void ClickBorderToCloseDialog(object sender, PointerReleasedEventArgs e)
    {
        if (sender is Border border)
        {
            int i = _masks.IndexOf(border);
            if (_modalDialogs[i] is { } dialog)
            {
                dialog.Close();
                border.RemoveHandler(PointerReleasedEvent, ClickBorderToCloseDialog);
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
        for (int i = 0; i < _masks.Count; i++)
        {
            _masks[i].Width = this.Bounds.Width;
            _masks[i].Height = this.Bounds.Height;
        }

        var oldSize = e.PreviousSize;
        var newSize = e.NewSize;
        foreach (var dialog in _dialogs)
        {
            if (dialog is DialogControlBase c)
            {
                ResetDialogPosition(c, oldSize, newSize);
            }
        }
        foreach (var modalDialog in _modalDialogs)
        {
            if (modalDialog is DialogControlBase c)
            {
                ResetDialogPosition(c, oldSize, newSize);
            }
        }
    }
    
    private void ResetZIndices()
    {
        int index = 0;
        for ( int i = 0; i< _dialogs.Count; i++)
        {
            _dialogs[i].ZIndex = index;
            index++;
        }
        for(int i = 0; i< _masks.Count; i++)
        {
            _masks[i].ZIndex = index;
            index++;
            (_modalDialogs[i] as Control)!.ZIndex = index;
            index++;
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