using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Ursa.Common;

namespace Ursa.Controls;

public class IconDropDownButton : RepeatButton, IIconButton
{
    public static readonly StyledProperty<object?> IconProperty =
        IconButton.IconProperty.AddOwner<IconDropDownButton>();

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty =
        IconButton.IconTemplateProperty.AddOwner<IconDropDownButton>();

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public static readonly StyledProperty<bool> IsLoadingProperty =
        IconButton.IsLoadingProperty.AddOwner<IconDropDownButton>();

    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public static readonly StyledProperty<Position> IconPlacementProperty =
        IconButton.IconPlacementProperty.AddOwner<IconDropDownButton>();

    public Position IconPlacement
    {
        get => GetValue(IconPlacementProperty);
        set => SetValue(IconPlacementProperty, value);
    }

    IPseudoClasses IIconButton.PseudoClasses => PseudoClasses;

    static IconDropDownButton()
    {
        ReversibleStackPanelUtils.EnsureBugFixed();
    }
}