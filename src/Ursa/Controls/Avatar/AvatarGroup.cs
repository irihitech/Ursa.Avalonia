using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Templates;

namespace Ursa.Controls;

[TemplatePart(PART_OverflowButton, typeof(Button))]
public class AvatarGroup : ItemsControl
{
    public const string PART_OverflowButton = "PART_OverflowButton";

    public static readonly StyledProperty<int> MaxCountProperty = AvaloniaProperty.Register<AvatarGroup, int>(
        nameof(MaxCount), int.MaxValue);

    public static readonly StyledProperty<OverlapFromType> OverlapFromProperty =
        AvaloniaProperty.Register<AvatarGroup, OverlapFromType>(nameof(OverlapFrom));

    public static readonly StyledProperty<int> OverflowedItemCountProperty =
        AvatarGroupPanel.OverflowedItemCountProperty.AddOwner<AvatarGroup>();

    public int MaxCount
    {
        get => GetValue(MaxCountProperty);
        set => SetValue(MaxCountProperty, value);
    }

    public OverlapFromType OverlapFrom
    {
        get => GetValue(OverlapFromProperty);
        set => SetValue(OverlapFromProperty, value);
    }

    public int OverflowedItemCount
    {
        get => GetValue(OverflowedItemCountProperty);
        set => SetValue(OverflowedItemCountProperty, value);
    }

    private void InvalidateItemsPanel() => ItemsPanelRoot?.InvalidateMeasure();

    static AvatarGroup()
    {
        ItemsPanelProperty.OverrideDefaultValue<AvatarGroup>(new FuncTemplate<Panel?>(() => new AvatarGroupPanel()));
        MaxCountProperty.Changed.AddClassHandler<AvatarGroup>((group, _) => group.InvalidateItemsPanel());
    }
}

public enum OverlapFromType
{
    Start,
    End
}