using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Ursa.Common;

namespace Ursa.Controls;

public class IconToggleButton : ToggleButton, IIconButton
{
    public static readonly StyledProperty<object?> IconProperty =
        IconButton.IconProperty.AddOwner<IconRepeatButton>();

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty =
        IconButton.IconTemplateProperty.AddOwner<IconRepeatButton>();

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public static readonly StyledProperty<bool> IsLoadingProperty =
        IconButton.IsLoadingProperty.AddOwner<IconRepeatButton>();

    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public static readonly StyledProperty<Position> IconPlacementProperty =
        IconButton.IconPlacementProperty.AddOwner<IconToggleButton>();

    public Position IconPlacement
    {
        get => GetValue(IconPlacementProperty);
        set => SetValue(IconPlacementProperty, value);
    }

    IPseudoClasses IIconButton.PseudoClasses => PseudoClasses;

    static IconToggleButton()
    {
        ReversibleStackPanelUtils.EnsureBugFixed();
    }
}