using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Ursa.Common;

namespace Ursa.Controls;

[PseudoClasses(PC_Right)]
public class IconButton: Button
{
    public const string PC_Right = ":right";
    
    public static readonly StyledProperty<object?> IconProperty = AvaloniaProperty.Register<IconButton, object?>(
        nameof(Icon));

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty = AvaloniaProperty.Register<IconButton, IDataTemplate?>(
        nameof(IconTemplate));

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public static readonly StyledProperty<bool> IsLoadingProperty = AvaloniaProperty.Register<IconButton, bool>(
        nameof(IsLoading));

    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public static readonly StyledProperty<IconPlacement> IconPlacementProperty = AvaloniaProperty.Register<IconButton, IconPlacement>(
        nameof(IconPlacement), defaultValue: IconPlacement.Left);

    public IconPlacement IconPlacement
    {
        get => GetValue(IconPlacementProperty);
        set => SetValue(IconPlacementProperty, value);
    }

    static IconButton()
    {
        IconPlacementProperty.Changed.AddClassHandler<IconButton, IconPlacement>((o, e) =>
        {
            o.SetPlacement(e.NewValue.Value);
        });
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        SetPlacement(IconPlacement);
    }

    private void SetPlacement(IconPlacement placement)
    {
        PseudoClasses.Set(PC_Right, placement == IconPlacement.Right);
    }
}