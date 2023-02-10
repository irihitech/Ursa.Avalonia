using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Metadata;

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

    public bool CanClose
    {
        get => GetValue(CanCloseProperty);
        set => SetValue(CanCloseProperty, value);
    }

    public static readonly StyledProperty<bool> ShowIconProperty = AvaloniaProperty.Register<Banner, bool>(
        nameof(ShowIcon), true);

    public bool ShowIcon
    {
        get => GetValue(ShowIconProperty);
        set => SetValue(ShowIconProperty, value);
    }

    public static readonly StyledProperty<object?> IconProperty = AvaloniaProperty.Register<Banner, object?>(
        nameof(Icon));

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<NotificationType> TypeProperty = AvaloniaProperty.Register<Banner, NotificationType>(
        nameof(Type));

    public NotificationType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (_closeButton != null)
        {
            _closeButton.Click -= OnCloseClick;
        }
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        if (_closeButton != null)
        {
            _closeButton.Click += OnCloseClick;
        }
        
    }

    private void OnCloseClick(object sender, RoutedEventArgs args)
    {
        IsVisible = false;
    }
}