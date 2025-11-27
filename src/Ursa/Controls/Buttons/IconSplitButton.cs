using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Ursa.Common;

namespace Ursa.Controls;

public class IconSplitButton : SplitButton
{
    public static readonly StyledProperty<object?> IconProperty =
        IconButton.IconProperty.AddOwner<IconSplitButton>();

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty =
        IconButton.IconTemplateProperty.AddOwner<IconSplitButton>();

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public static readonly StyledProperty<bool> IsLoadingProperty =
        IconButton.IsLoadingProperty.AddOwner<IconSplitButton>();

    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public static readonly StyledProperty<Position> IconPlacementProperty =
        IconButton.IconPlacementProperty.AddOwner<IconSplitButton>();

    public Position IconPlacement
    {
        get => GetValue(IconPlacementProperty);
        set => SetValue(IconPlacementProperty, value);
    }

    static IconSplitButton()
    {
        ReversibleStackPanelUtils.EnsureBugFixed();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        IconButton.UpdatePseudoClasses(this);
    }
}