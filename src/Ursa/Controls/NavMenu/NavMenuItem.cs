using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;

namespace Ursa.Controls;

[PseudoClasses(PC_Highlighted)]
public class NavMenuItem: HeaderedSelectingItemsControl
{
    public const string PC_Highlighted = "highlighted";
    
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

    public static readonly StyledProperty<ICommand?> CommandProperty = AvaloniaProperty.Register<NavMenuItem, ICommand?>(
        nameof(Command));

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
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

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is NavMenuItem navMenuItem)
        {
            if (_rootMenu?.HeaderBinding is not null)
            {
                container[!HeaderProperty] = _rootMenu.HeaderBinding;
            }
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _rootMenu = GetRootMenu();
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        _rootMenu?.SelectItem(this);
        e.Handled = true;
    }

    private NavMenu? GetRootMenu()
    {
        var root = this.FindAncestorOfType<NavMenu>() ?? this.FindLogicalAncestorOfType<NavMenu>();
        return root;
    }
}