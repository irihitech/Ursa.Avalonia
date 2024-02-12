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

/// <summary>
/// Navigation Menu Item
/// <para>Note:</para>
/// <para>collapsed: Entire menu is collapsed, only first level icon is displayed. Submenus are in popup. </para>
/// <para>closed: When menu is not in collapsed mode, represents whether submenu is hidden. </para>
/// </summary>
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

    private bool _isCollapsed;

    public static readonly DirectProperty<NavMenuItem, bool> IsCollapsedProperty = AvaloniaProperty.RegisterDirect<NavMenuItem, bool>(
        nameof(IsCollapsed), o => o.IsCollapsed, (o, v) => o.IsCollapsed = v);

    public bool IsCollapsed
    {
        get => _isCollapsed;
        set => SetAndRaise(IsCollapsedProperty, ref _isCollapsed, value);
    }

    private bool _isClosed;

    public static readonly DirectProperty<NavMenuItem, bool> IsClosedProperty = AvaloniaProperty.RegisterDirect<NavMenuItem, bool>(
        nameof(IsClosed), o => o.IsClosed, (o, v) => o.IsClosed = v);

    public bool IsClosed
    {
        get => _isClosed;
        set => SetAndRaise(IsClosedProperty, ref _isClosed, value);
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
    
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _rootMenu = GetRootMenu();
        if (_rootMenu is not null)
        {
            if (_rootMenu.IconBinding is not null)
            {
                this[!IconProperty] = _rootMenu.IconBinding;
            }
            if (_rootMenu.HeaderBinding is not null)
            {
                this[!HeaderProperty] = _rootMenu.HeaderBinding;
            }
            if (_rootMenu.SubMenuBinding is not null)
            {
                this[!ItemsSourceProperty] = _rootMenu.SubMenuBinding;
            }
            if (_rootMenu.CommandBinding is not null)
            {
                this[!CommandProperty] = _rootMenu.CommandBinding;
            }
            this[!IconTemplateProperty] = _rootMenu[!NavMenu.IconTemplateProperty];
            this[!HeaderTemplateProperty] = _rootMenu[!NavMenu.HeaderTemplateProperty];
        }
        
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
        if (this.ItemCount == 0)
        {
            SelectItem(this);
        }
        else
        {
            
        }
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
            var items = menuItem.LogicalChildren.OfType<NavMenuItem>();
            foreach (var child in items)
            {
                if (child != this)
                {
                    child.ClearSelection();
                }
            }
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