using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

/// <summary>
/// Navigation Menu Item
/// </summary>
[PseudoClasses(PC_Highlighted, PC_HorizontalCollapsed, PC_VerticalCollapsed, PC_FirstLevel, PC_Selector)]
public class NavMenuItem: HeaderedItemsControl
{
    public const string PC_Highlighted = ":highlighted";
    public const string PC_FirstLevel = ":first-level";
    public const string PC_HorizontalCollapsed = ":horizontal-collapsed";
    public const string PC_VerticalCollapsed = ":vertical-collapsed";
    public const string PC_Selector = ":selector";
    
    private NavMenu? _rootMenu;
    private Panel? _popupPanel;
    private Popup? _popup;
    private Panel? _overflowPanel;
    
    private static readonly Point s_invalidPoint = new (double.NaN, double.NaN);
    private Point _pointerDownPoint = s_invalidPoint;
    
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

    public static readonly StyledProperty<bool> IsSelectedProperty =
        SelectingItemsControl.IsSelectedProperty.AddOwner<NavMenuItem>();

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }
    
    public static readonly RoutedEvent<RoutedEventArgs> IsSelectedChangedEvent = RoutedEvent.Register<SelectingItemsControl, RoutedEventArgs>("IsSelectedChanged", RoutingStrategies.Bubble);

    private bool _isHighlighted;

    public static readonly DirectProperty<NavMenuItem, bool> IsHighlightedProperty =
        AvaloniaProperty.RegisterDirect<NavMenuItem, bool>(
            nameof(IsHighlighted), o => o.IsHighlighted, (o, v) => o.IsHighlighted = v,
            defaultBindingMode: BindingMode.TwoWay);

    public bool IsHighlighted
    {
        get => _isHighlighted;
        private set => SetAndRaise(IsHighlightedProperty, ref _isHighlighted, value);
    }

    public static readonly StyledProperty<bool> IsHorizontalCollapsedProperty =
        NavMenu.IsHorizontalCollapsedProperty.AddOwner<NavMenuItem>();

    public bool IsHorizontalCollapsed
    {
        get => GetValue(IsHorizontalCollapsedProperty);
        set => SetValue(IsHorizontalCollapsedProperty, value);
    }

    public static readonly StyledProperty<bool> IsVerticalCollapsedProperty = AvaloniaProperty.Register<NavMenuItem, bool>(
        nameof(IsVerticalCollapsed));

    public bool IsVerticalCollapsed
    {
        get => GetValue(IsVerticalCollapsedProperty);
        set => SetValue(IsVerticalCollapsedProperty, value);
    }

    public static readonly StyledProperty<double> SubMenuIndentProperty =
        NavMenu.SubMenuIndentProperty.AddOwner<NavMenuItem>();

    public double SubMenuIndent
    {
        get => GetValue(SubMenuIndentProperty);
        set => SetValue(SubMenuIndentProperty, value);
    }

    internal static readonly DirectProperty<NavMenuItem, int> LevelProperty = AvaloniaProperty.RegisterDirect<NavMenuItem, int>(
        nameof(Level), o => o.Level, (o, v) => o.Level = v);
    private int _level;
    public int Level
    {
        get => _level;
        set => SetAndRaise(LevelProperty, ref _level, value);
    }

    public static readonly StyledProperty<bool> IsSeparatorProperty = AvaloniaProperty.Register<NavMenuItem, bool>(
        nameof(IsSeparator));

    public bool IsSeparator
    {
        get => GetValue(IsSeparatorProperty);
        set => SetValue(IsSeparatorProperty, value);
    }

    static NavMenuItem()
    {
        // SelectableMixin.Attach<NavMenuItem>(IsSelectedProperty);
        PressedMixin.Attach<NavMenuItem>();
        LevelProperty.Changed.AddClassHandler<NavMenuItem, int>((item, args) => item.OnLevelChange(args));
        IsHighlightedProperty.AffectsPseudoClass<NavMenuItem>(PC_Highlighted);
        IsHorizontalCollapsedProperty.AffectsPseudoClass<NavMenuItem>(PC_HorizontalCollapsed);
        IsVerticalCollapsedProperty.AffectsPseudoClass<NavMenuItem>(PC_VerticalCollapsed);
        IsSelectedProperty.AffectsPseudoClass<NavMenuItem>(PseudoClassName.PC_Selected, IsSelectedChangedEvent);
        IsHorizontalCollapsedProperty.Changed.AddClassHandler<NavMenuItem, bool>((item, args) =>
            item.OnIsHorizontalCollapsedChanged(args));
    }

    private void OnIsHorizontalCollapsedChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.NewValue.Value)
        {
            if (this.ItemsPanelRoot is OverflowStackPanel s)
            {
                s.MoveChildrenToOverflowPanel();
            }
        }
        else
        {
            if (this.ItemsPanelRoot is OverflowStackPanel s)
            {
                s.MoveChildrenToMainPanel();
            }
        }
    }

    private void OnLevelChange(AvaloniaPropertyChangedEventArgs<int> args)
    {
        PseudoClasses.Set(PC_FirstLevel, args.NewValue.Value == 1);
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
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        SetCurrentValue(LevelProperty,CalculateDistanceFromLogicalParent<NavMenu>(this));
        _popup = e.NameScope.Find<Popup>("PART_Popup");
        _overflowPanel = e.NameScope.Find<Panel>("PART_OverflowPanel");
        if (_rootMenu is not null)
        {
            this.TryBind(IconProperty, _rootMenu.IconBinding);
            this.TryBind(HeaderProperty, _rootMenu.HeaderBinding);
            this.TryBind(ItemsSourceProperty, _rootMenu.SubMenuBinding);
            this.TryBind(CommandProperty, _rootMenu.CommandBinding);
            this[!IconTemplateProperty] = _rootMenu[!NavMenu.IconTemplateProperty];
            this[!HeaderTemplateProperty] = _rootMenu[!NavMenu.HeaderTemplateProperty];
            this[!SubMenuIndentProperty] = _rootMenu[!NavMenu.SubMenuIndentProperty];
            this[!IsHorizontalCollapsedProperty] = _rootMenu[!NavMenu.IsHorizontalCollapsedProperty];
        }
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var root = this.ItemsPanelRoot;
        if (root is OverflowStackPanel stack)
        {
            stack.OverflowPanel = _overflowPanel;
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        if (IsSeparator)
        {
            e.Handled = true;
            return;
        }
        base.OnPointerPressed(e);
        if (e.Handled) return;

        var p = e.GetCurrentPoint(this);
        if (p.Properties.PointerUpdateKind is not (PointerUpdateKind.LeftButtonPressed
            or PointerUpdateKind.RightButtonPressed)) return;
        if (p.Pointer.Type == PointerType.Mouse)
        {
            if (this.ItemCount == 0)
            {
                SelectItem(this);
                Command?.Execute(CommandParameter);
                e.Handled = true;
            }
            else
            {
                if (!IsHorizontalCollapsed) 
                {
                    SetCurrentValue(IsVerticalCollapsedProperty, !IsVerticalCollapsed);
                    e.Handled = true;
                }
                else
                {
                    if (_popup is null || e.Source is not Visual v || _popup.IsInsidePopup(v)) return;
                    if (_popup.IsOpen)
                    {
                        _popup.Close();
                    }
                    else
                    {
                        _popup.Open();
                    }
                }
            }
        }
        else
        {
            _pointerDownPoint = p.Position;
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (!e.Handled && !double.IsNaN(_pointerDownPoint.X) &&
            e.InitialPressMouseButton is MouseButton.Left or MouseButton.Right)
        {
            var point = e.GetCurrentPoint(this);
            if (!new Rect(Bounds.Size).ContainsExclusive(point.Position) || e.Pointer.Type != PointerType.Touch) return;
            if (this.ItemCount == 0)
            {
                SelectItem(this);
                Command?.Execute(CommandParameter);
                e.Handled = true;
            }
            else
            {
                if (!IsHorizontalCollapsed) 
                {
                    SetCurrentValue(IsVerticalCollapsedProperty, !IsVerticalCollapsed);
                    e.Handled = true;
                }
                else
                {
                    if (_popup is null || e.Source is not Visual v || _popup.IsInsidePopup(v)) return;
                    if (_popup.IsOpen)
                    {
                        _popup.Close();
                    }
                    else
                    {
                        _popup.Open();
                    }
                }
            }
        }
    }

    internal void SelectItem(NavMenuItem item)
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
        if(_popup is not null)
        {
            _popup.Close();
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
    
    internal IEnumerable<NavMenuItem> GetLeafMenus()
    {
        if (this.ItemCount == 0)
        {
            yield return this;
            yield break;
        }
        foreach (var child in LogicalChildren)   
        {
            if (child is NavMenuItem item)
            {
                var items = item.GetLeafMenus();
                foreach (var i in items)
                {
                    yield return i;
                }
            }
        }
    }
}