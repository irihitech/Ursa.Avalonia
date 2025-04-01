using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[PseudoClasses(PseudoClassName.PC_Active)]
[TemplatePart(Name = PART_CaptionButtons, Type = typeof(CaptionButtons))]
public class TitleBar: ContentControl
{
    public const string PART_CaptionButtons = "PART_CaptionButtons";
    
    private CaptionButtons? _captionButtons;
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

    public static readonly AttachedProperty<bool> IsTitleBarHitTestVisibleProperty =
        AvaloniaProperty.RegisterAttached<TitleBar, Window, bool>("IsTitleBarHitTestVisible", defaultValue: true);
    public static void SetIsTitleBarHitTestVisible(Window obj, bool value) => obj.SetValue(IsTitleBarHitTestVisibleProperty, value);
    public static bool GetIsTitleBarHitTestVisible(Window obj) => obj.GetValue(IsTitleBarHitTestVisibleProperty);
    
    public bool IsTitleBarHitTestVisible
    {
        get => GetValue(IsTitleBarHitTestVisibleProperty);
        set => SetValue(IsTitleBarHitTestVisibleProperty, value);
    }
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        this._captionButtons?.Detach();
        this._captionButtons = e.NameScope.Get<CaptionButtons>(PART_CaptionButtons);
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

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _captionButtons?.Detach();
        _activeSubscription?.Dispose();
    }
}