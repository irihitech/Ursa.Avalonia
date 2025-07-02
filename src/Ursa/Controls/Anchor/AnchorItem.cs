using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace Ursa.Controls;

public class AnchorItem: ContentControl
{
    public static readonly StyledProperty<Control?> TargetProperty = AvaloniaProperty.Register<AnchorItem, Control?>(
        nameof(Target));

    public Control? Target
    {
        get => GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }
    
    public static readonly StyledProperty<string?> AnchorNameProperty = AvaloniaProperty.Register<AnchorItem, string?>(
        nameof(AnchorName));
    public string? AnchorName
    {
        get => GetValue(AnchorNameProperty);
        set => SetValue(AnchorNameProperty, value);
    }

    private Anchor? _root;

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _root = this.FindAncestorOfType<Anchor>() ??
                throw new InvalidOperationException("AnchorItem must be inside an Anchor control.");
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (_root is null)
            return;
        if (Target is not null)
        {
            _root.ScrollToAnchor(Target);
        }
    }
}