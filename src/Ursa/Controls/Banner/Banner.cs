using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[PseudoClasses(PC_Icon)]
[TemplatePart(PART_CloseButton, typeof(Button))]
public class Banner: HeaderedContentControl
{
    public const string PC_Icon = ":icon";
    public const string PART_CloseButton = "PART_CloseButton";

    private Button? _closeButton;
    
    public static readonly StyledProperty<bool> CanCloseProperty = AvaloniaProperty.Register<Banner, bool>(
        nameof(CanClose));

    [ExcludeFromCodeCoverage]
    public bool CanClose
    {
        get => GetValue(CanCloseProperty);
        set => SetValue(CanCloseProperty, value);
    }
    
    public static readonly StyledProperty<bool> ShowIconProperty = AvaloniaProperty.Register<Banner, bool>(
        nameof(ShowIcon), true);

    [ExcludeFromCodeCoverage]
    public bool ShowIcon
    {
        get => GetValue(ShowIconProperty);
        set => SetValue(ShowIconProperty, value);
    }

    public static readonly StyledProperty<object?> IconProperty = AvaloniaProperty.Register<Banner, object?>(
        nameof(Icon));

    [ExcludeFromCodeCoverage]
    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<NotificationType> TypeProperty = AvaloniaProperty.Register<Banner, NotificationType>(
        nameof(Type));

    [ExcludeFromCodeCoverage]
    public NotificationType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(OnCloseClick, _closeButton);
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        Button.ClickEvent.AddHandler(OnCloseClick, _closeButton);
    }

    private void OnCloseClick(object? sender, RoutedEventArgs args)
    {
        SetCurrentValue(IsVisibleProperty, false);
    }
}