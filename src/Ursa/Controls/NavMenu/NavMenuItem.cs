using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;

namespace Ursa.Controls;

public class NavMenuItem: HeaderedSelectingItemsControl
{
    private NavMenu? _rootMenu;
    
    public static readonly StyledProperty<object?> IconProperty = AvaloniaProperty.Register<NavMenuItem, object?>(
        nameof(Icon));

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty = AvaloniaProperty.Register<NavMenuItem, IDataTemplate?>(
        nameof(IconTemplate));

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public new static readonly StyledProperty<bool> IsSelectedProperty =
        SelectingItemsControl.IsSelectedProperty.AddOwner<NavMenuItem>();

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    static NavMenuItem()
    {
        SelectableMixin.Attach<NavMenuItem>(IsSelectedProperty);
        PressedMixin.Attach<NavMenuItem>();
    }
    
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<NavMenuItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new NavMenuItem();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _rootMenu = GetRootMenu();
        UpdateSelection(1);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        _rootMenu?.SelectItem(this);
    }

    private NavMenu? GetRootMenu()
    {
        var root = this.FindAncestorOfType<NavMenu>();
        if (root is null)
        {
            root = this.FindLogicalAncestorOfType<NavMenu>();
        }
        return root;
    }
}