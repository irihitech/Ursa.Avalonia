using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

public class AnchorItem : HeaderedItemsControl, ISelectable
{
    public static readonly StyledProperty<string?> AnchorIdProperty = AvaloniaProperty.Register<AnchorItem, string?>(
        nameof(AnchorId));

    public static readonly StyledProperty<bool> IsSelectedProperty =
        SelectingItemsControl.IsSelectedProperty.AddOwner<AnchorItem>();

    private static readonly FuncTemplate<Panel?> DefaultPanel =
        new(() => new StackPanel());

    private Anchor? _root;

    static AnchorItem()
    {
        SelectableMixin.Attach<AnchorItem>(IsSelectedProperty);
        PressedMixin.Attach<AnchorItem>();
        ItemsPanelProperty.OverrideDefaultValue<TreeViewItem>(DefaultPanel);
    }

    public string? AnchorId
    {
        get => GetValue(AnchorIdProperty);
        set => SetValue(AnchorIdProperty, value);
    }

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _root = this.GetLogicalAncestors().OfType<Anchor>().FirstOrDefault();
        if (ItemTemplate is null && _root?.ItemTemplate is not null)
        {
            SetCurrentValue(ItemTemplateProperty, _root.ItemTemplate);
        }

        if (ItemContainerTheme is null && _root?.ItemContainerTheme is not null)
        {
            SetCurrentValue(ItemContainerThemeProperty, _root.ItemContainerTheme);
        }
    }
    
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return EnsureRoot().CreateContainerForItemOverride_INTERNAL(item, index, recycleKey);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return EnsureRoot().NeedsContainerOverride_INTERNAL(item, index, out recycleKey);
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        EnsureRoot().PrepareContainerForItemOverride_INTERNAL(container, item, index);
    }

    protected override void ContainerForItemPreparedOverride(Control container, object? item, int index)
    {
        EnsureRoot().ContainerForItemPreparedOverride_INTERNAL(container, item, index);
    }

    private Anchor EnsureRoot()
    {
        return _root ?? throw new InvalidOperationException("AnchorItem must be inside an Anchor control.");
    }
}