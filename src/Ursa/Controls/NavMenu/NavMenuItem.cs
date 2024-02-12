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

[PseudoClasses(PC_Highlighted, PC_Collapsed, PC_Closed, PC_FirstLevel, PC_Selector)]
public class NavMenuItem: HeaderedSelectingItemsControl
{
    public const string PC_Highlighted = ":highlighted";
    public const string PC_FirstLevel = ":first-level";
    public const string PC_Collapsed = ":collapsed";
    public const string PC_Closed = ":closed";
    public const string PC_Selector = ":selector";
    
    private NavMenu? _rootMenu;
    private Panel? _popupPanel;
    
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

    public static readonly StyledProperty<ICommand?> CommandProperty = Button.CommandProperty.AddOwner<NavMenuItem>();

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<object?> CommandParameterProperty =
        Button.CommandParameterProperty.AddOwner<NavMenuItem>();

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public new static readonly StyledProperty<bool> IsSelectedProperty =
        SelectingItemsControl.IsSelectedProperty.AddOwner<NavMenuItem>();

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    private bool _isHighlighted;

    public static readonly DirectProperty<NavMenuItem, bool> IsHighlightedProperty = AvaloniaProperty.RegisterDirect<NavMenuItem, bool>(
        nameof(IsHighlighted), o => o.IsHighlighted, (o, v) => o.IsHighlighted = v);

    public bool IsHighlighted
    {
        get => _isHighlighted;
        private set => SetAndRaise(IsHighlightedProperty, ref _isHighlighted, value);
    }
    
    
    internal int Level { get; set; }
    

    static NavMenuItem()
    {
        SelectableMixin.Attach<NavMenuItem>(IsSelectedProperty);
        PressedMixin.Attach<NavMenuItem>();
        IsHighlightedProperty.Changed.AddClassHandler<NavMenuItem, bool>((o, e) => o.OnIsHighlightedChange(e));
    }

    private void OnIsHighlightedChange(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        PseudoClasses.Set(PC_Highlighted, args.NewValue.Value);
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
            if (_rootMenu?.IconBinding is not null)
            {
                container[!IconProperty] = _rootMenu.IconBinding;
            }
            if (_rootMenu?.SubMenuBinding is not null)
            {
                container[!ItemsSourceProperty] = _rootMenu.SubMenuBinding;
            }
            if (_rootMenu?.CommandBinding is not null)
            {
                container[!CommandProperty] = _rootMenu.CommandBinding;
            }
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _rootMenu = GetRootMenu();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        var children = this.ItemsPanelRoot?.Children.ToList();
        base.OnApplyTemplate(e);
        Level = CalculateDistanceFromLogicalParent<NavMenuItem>(this);
        PseudoClasses.Set(PC_FirstLevel, Level == 0);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        SelectItem(this);
        Command?.Execute(CommandParameter);
        e.Handled = true;
    }
    
    protected void SelectItem(NavMenuItem item)
    {
        if (item == this)
        {
            SetCurrentValue(IsSelectedProperty, true);
            SetCurrentValue(IsHighlightedProperty, true);
        }
        else
        {
            SetCurrentValue(IsSelectedProperty, false);
            SetCurrentValue(IsHighlightedProperty, true);
        }
        if (this.Parent is NavMenuItem menuItem)
        {
            menuItem.SelectItem(item);
        }
        else if (this.Parent is NavMenu menu)
        {
            menu.SelectItem(item, this);
        }
    }

    internal void ClearSelection()
    {
        SetCurrentValue(IsHighlightedProperty, false);
        SetCurrentValue(IsSelectedProperty, false);
        foreach (var child in LogicalChildren)
        {
            if (child is NavMenuItem item)
            {
                item.ClearSelection();
            }
        }
    }

    private NavMenu? GetRootMenu()
    {
        var root = this.FindAncestorOfType<NavMenu>() ?? this.FindLogicalAncestorOfType<NavMenu>();
        return root;
    }
    
    private static int CalculateDistanceFromLogicalParent<T>(ILogical? logical, int @default = -1) where T : class
    {
        var result = 0;

        while (logical != null && !(logical is T))
        {
            if (logical is NavMenuItem)
            {
                result++;
            }
            logical = logical.LogicalParent;
        }

        return logical != null ? result : @default;
    }
}