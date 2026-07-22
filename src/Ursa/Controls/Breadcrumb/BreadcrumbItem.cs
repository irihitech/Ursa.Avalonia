using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Input;

namespace Ursa.Controls;

[PseudoClasses(PC_Last)]
public class BreadcrumbItem : IconButton
{
    public const string PC_Last = ":last";

    public static readonly StyledProperty<object?> SeparatorProperty = AvaloniaProperty.Register<BreadcrumbItem, object?>(nameof(Separator));

    public object? Separator
    {
        get => GetValue(SeparatorProperty);
        set => SetValue(SeparatorProperty, value);
    }

    public static readonly StyledProperty<bool> IsReadOnlyProperty = AvaloniaProperty.Register<BreadcrumbItem, bool>(nameof(IsReadOnly));

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (!IsReadOnly)
        {
            base.OnPointerReleased(e);
        }
    }

    internal void SetPseudoClassLast(bool isLast)
    {
        PseudoClasses.Set(PC_Last, isLast);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var parent = this.Parent as Breadcrumb;
        parent?.InvalidateContainers();
    }
}