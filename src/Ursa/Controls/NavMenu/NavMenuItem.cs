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
///     Navigation Menu Item
/// </summary>
[PseudoClasses(
    PC_Highlighted,
    PC_HorizontalCollapsed,
    PC_VerticalCollapsed,
    PC_FirstLevel,
    PC_Selector)]
public class NavMenuItem : HeaderedItemsControl
{
    public const string PC_Highlighted = ":highlighted";
    public const string PC_FirstLevel = ":first-level";
    public const string PC_HorizontalCollapsed = ":horizontal-collapsed";
    public const string PC_VerticalCollapsed = ":vertical-collapsed";
    public const string PC_Selector = ":selector";

    public static readonly StyledProperty<object?> IconProperty = AvaloniaProperty.Register<NavMenuItem, object?>(
        nameof(Icon));

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty =
        AvaloniaProperty.Register<NavMenuItem, IDataTemplate?>(
            nameof(IconTemplate));

    public static readonly StyledProperty<ICommand?> CommandProperty = Button.CommandProperty.AddOwner<NavMenuItem>();

    public static readonly StyledProperty<object?> CommandParameterProperty =
        Button.CommandParameterProperty.AddOwner<NavMenuItem>();

    public static readonly StyledProperty<bool> IsSelectedProperty =
        SelectingItemsControl.IsSelectedProperty.AddOwner<NavMenuItem>();

    public static readonly RoutedEvent<RoutedEventArgs> IsSelectedChangedEvent =
        RoutedEvent.Register<SelectingItemsControl, RoutedEventArgs>("IsSelectedChanged", RoutingStrategies.Bubble);

    public static readonly DirectProperty<NavMenuItem, bool> IsHighlightedProperty =
        AvaloniaProperty.RegisterDirect<NavMenuItem, bool>(
            nameof(IsHighlighted), o => o.IsHighlighted, (o, v) => o.IsHighlighted = v,
            defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> IsHorizontalCollapsedProperty =
        NavMenu.IsHorizontalCollapsedProperty.AddOwner<NavMenuItem>();

    public static readonly StyledProperty<bool> IsVerticalCollapsedProperty =
        AvaloniaProperty.Register<NavMenuItem, bool>(
            nameof(IsVerticalCollapsed));

    public static readonly StyledProperty<double> SubMenuIndentProperty =
        NavMenu.SubMenuIndentProperty.AddOwner<NavMenuItem>();

    internal static readonly DirectProperty<NavMenuItem, int> LevelProperty =
        AvaloniaProperty.RegisterDirect<NavMenuItem, int>(
            nameof(Level), o => o.Level, (o, v) => o.Level = v);

    public static readonly StyledProperty<bool> IsSeparatorProperty = AvaloniaProperty.Register<NavMenuItem, bool>(
        nameof(IsSeparator));

    private bool _isHighlighted;
    private int _level;
    private Panel? _overflowPanel;
    private bool _isPointerDown = false;
    private Popup? _popup;

    static NavMenuItem()
    {
        // SelectableMixin.Attach<NavMenuItem>(IsSelectedProperty);
        PressedMixin.Attach<NavMenuItem>();
        FocusableProperty.OverrideDefaultValue<NavMenuItem>(true);
        LevelProperty.Changed.AddClassHandler<NavMenuItem, int>((item, args) => item.OnLevelChange(args));
        IsHighlightedProperty.AffectsPseudoClass<NavMenuItem>(PC_Highlighted);
        IsHorizontalCollapsedProperty.AffectsPseudoClass<NavMenuItem>(PC_HorizontalCollapsed);
        IsVerticalCollapsedProperty.AffectsPseudoClass<NavMenuItem>(PC_VerticalCollapsed);
        IsSelectedProperty.AffectsPseudoClass<NavMenuItem>(PseudoClassName.PC_Selected, IsSelectedChangedEvent);
        IsHorizontalCollapsedProperty.Changed.AddClassHandler<NavMenuItem, bool>((item, args) =>
            item.OnIsHorizontalCollapsedChanged(args));
    }

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public bool IsHighlighted
    {
        get => _isHighlighted;
        private set => SetAndRaise(IsHighlightedProperty, ref _isHighlighted, value);
    }

    public bool IsHorizontalCollapsed
    {
        get => GetValue(IsHorizontalCollapsedProperty);
        set => SetValue(IsHorizontalCollapsedProperty, value);
    }

    public bool IsVerticalCollapsed
    {
        get => GetValue(IsVerticalCollapsedProperty);
        set => SetValue(IsVerticalCollapsedProperty, value);
    }

    public double SubMenuIndent
    {
        get => GetValue(SubMenuIndentProperty);
        set => SetValue(SubMenuIndentProperty, value);
    }

    public int Level
    {
        get => _level;
        set => SetAndRaise(LevelProperty, ref _level, value);
    }

    public bool IsSeparator
    {
        get => GetValue(IsSeparatorProperty);
        set => SetValue(IsSeparatorProperty, value);
    }

    internal NavMenu? RootMenu { get; private set; }

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
        RootMenu = GetRootMenu();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        SetCurrentValue(LevelProperty, CalculateDistanceFromLogicalParent<NavMenu>(this));
        _popup = e.NameScope.Find<Popup>("PART_Popup");
        _overflowPanel = e.NameScope.Find<Panel>("PART_OverflowPanel");
        if (RootMenu is not null)
        {
            this.TryBind(IconProperty, RootMenu.IconBinding);
            this.TryBind(HeaderProperty, RootMenu.HeaderBinding);
            this.TryBind(ItemsSourceProperty, RootMenu.SubMenuBinding);
            this.TryBind(CommandProperty, RootMenu.CommandBinding);
            this[!IconTemplateProperty] = RootMenu[!NavMenu.IconTemplateProperty];
            this[!HeaderTemplateProperty] = RootMenu[!NavMenu.HeaderTemplateProperty];
            this[!SubMenuIndentProperty] = RootMenu[!NavMenu.SubMenuIndentProperty];
            this[!IsHorizontalCollapsedProperty] = RootMenu[!NavMenu.IsHorizontalCollapsedProperty];
        }
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var root = ItemsPanelRoot;
        if (root is OverflowStackPanel stack) stack.OverflowPanel = _overflowPanel;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Handled) return;

        if (e.Key is Key.Enter)
        {
            if (IsSeparator)
            {
                e.Handled = true;
                return;
            }
            else if (ItemCount == 0)
            {
                SelectAndExecute();
                e.Handled = true;
                return;
            }
        }

        if (!IsHorizontalCollapsed)
        {
            var handler = e.Key switch
            {
                Key.Left => ApplyToItemOrRecursivelyIfCtrl(FocusAwareCollapseItem, e.KeyModifiers),
                Key.Right => ApplyToItemOrRecursivelyIfCtrl(ExpandItem, e.KeyModifiers),
                Key.Enter => ApplyToItemOrRecursivelyIfCtrl(IsVerticalCollapsed ? ExpandItem : CollapseItem, e.KeyModifiers),
                Key.Subtract => FocusAwareCollapseItem,
                Key.Add => ExpandItem,
                Key.Divide => ApplyToSubtree(CollapseItem),
                Key.Multiply => ApplyToSubtree(ExpandItem),
                _ => null
            };

            if (handler is not null)
                e.Handled = handler(this);

            static bool ToggleCollapse(NavMenuItem item, bool collapse)
            {
                if (item.ItemCount > 0 && item.IsVerticalCollapsed != collapse)
                {
                    item.SetCurrentValue(IsVerticalCollapsedProperty, collapse);
                    return true;
                }

                return false;
            }

            static bool ExpandItem(NavMenuItem item) => ToggleCollapse(item, false);
            static bool CollapseItem(NavMenuItem item) => ToggleCollapse(item, true);

            static bool FocusAwareCollapseItem(NavMenuItem item)
            {
                if (item.ItemCount > 0 && !item.IsVerticalCollapsed)
                {
                    if (item.IsFocused)
                        item.SetCurrentValue(IsVerticalCollapsedProperty, true);
                    else
                        item.Focus(NavigationMethod.Directional);

                    return true;
                }

                return false;
            }

            static Func<NavMenuItem, bool> ApplyToItemOrRecursivelyIfCtrl(Func<NavMenuItem, bool> f, KeyModifiers keyModifiers) => keyModifiers.HasFlag(KeyModifiers.Control)
                ? ApplyToSubtree(f)
                : f;

            static Func<NavMenuItem, bool> ApplyToSubtree(Func<NavMenuItem, bool> f) => i => Subtree(i)
                .ToList()
                .Select(i => f(i))
                .Aggregate(false, (p, c) => p || c);

            static IEnumerable<NavMenuItem> Subtree(NavMenuItem item)
            {
                yield return item;

                var children = item.LogicalChildren
                    .OfType<NavMenuItem>()
                    .SelectMany(child => Subtree(child));
                foreach (var child in children) yield return child;
            }
        }
        else if (e.Key is Key.Right or Key.Enter)
        {
            TogglePopup(e.Source, true, true);
        }
        else if (e.Key is Key.Left or Key.Escape)
        {
            TogglePopup(e.Source, false, false);
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
            ActivateMenuItem(e);
        else
            _isPointerDown = true;
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (e.Handled || !_isPointerDown) return;

        _isPointerDown = false;

        if (e.InitialPressMouseButton is MouseButton.Left or MouseButton.Right)
        {
            var point = e.GetCurrentPoint(this);
            if (new Rect(Bounds.Size).ContainsExclusive(point.Position) && e.Pointer.Type == PointerType.Touch)
                ActivateMenuItem(e);
        }
    }

    private void OnIsHorizontalCollapsedChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.NewValue.Value)
        {
            if (ItemsPanelRoot is OverflowStackPanel s) s.MoveChildrenToOverflowPanel();
        }
        else
        {
            if (ItemsPanelRoot is OverflowStackPanel s) s.MoveChildrenToMainPanel();
        }
    }

    private void OnLevelChange(AvaloniaPropertyChangedEventArgs<int> args)
    {
        PseudoClasses.Set(PC_FirstLevel, args.NewValue.Value == 1);
    }

    public NavMenuItem? CollapsedAwareContainerFromIndex(int index) =>
    IsHorizontalCollapsed && _overflowPanel is not null
        ? index >= 0 && index < _overflowPanel.Children.Count
            ? (NavMenuItem?)_overflowPanel.Children[index]
            : null
        : (NavMenuItem?)ContainerFromIndex(index);

    public NavMenuItem? CollapsedAwareContainerFromItem(object item)
    {
        var index = Items.IndexOf(item);
        return index >= 0 ? CollapsedAwareContainerFromIndex(index) : null;
    }

    public int CollapsedAwareIndexFromContainer(NavMenuItem container) =>
        IsHorizontalCollapsed && _overflowPanel is not null
            ? _overflowPanel.Children.IndexOf(container)
            : IndexFromContainer((Control)container);

    public object? CollapsedAwareItemFromContainer(NavMenuItem container)
    {
        var index = CollapsedAwareIndexFromContainer(container);
        return index >= 0 && index < Items.Count ? Items[index] : null;
    }

    public IEnumerable<Control> CollapsedAwareRealizedContainers() =>
        IsHorizontalCollapsed && _overflowPanel is not null
            ? _overflowPanel.Children
            : GetRealizedContainers();

    internal void SelectItem(NavMenuItem item)
    {
        if (item == this && RootMenu?.CanChangeSelection(item) != true)
        {
            return;
        }
        SetCurrentValue(IsSelectedProperty, item == this);
        SetCurrentValue(IsHighlightedProperty, true);

        if (Parent is NavMenuItem menuItem)
        {
            menuItem.SelectItem(item);
            var items = menuItem.LogicalChildren.OfType<NavMenuItem>();
            foreach (var child in items)
                if (child != this)
                    child.ClearSelection();
        }
        else if (Parent is NavMenu menu)
        {
            menu.SelectItem(item, this);
        }

        _popup?.Close();
    }

    internal void ClearSelection()
    {
        SetCurrentValue(IsHighlightedProperty, false);
        SetCurrentValue(IsSelectedProperty, false);
        foreach (var child in LogicalChildren)
            if (child is NavMenuItem item)
                item.ClearSelection();
    }

    private void SelectAndExecute()
    {
        SelectItem(this);
        Command?.Execute(CommandParameter);
    }

    private void ActivateMenuItem(RoutedEventArgs e)
    {
        if (ItemCount == 0)
        {
            SelectAndExecute();
            e.Handled = true;
            return;
        }

        if (!IsHorizontalCollapsed)
        {
            SetCurrentValue(IsVerticalCollapsedProperty, !IsVerticalCollapsed);
            e.Handled = true;
        }
        else if (_popup is not null)
        {
            TogglePopup(e.Source, true, !_popup.IsOpen);
        }
    }

    private void TogglePopup(object? source, bool outsidePopup, bool open)
    {
        if (ItemCount > 0 &&
            _popup is not null &&
            source is Visual visual &&
            _popup.IsInsidePopup(visual) != outsidePopup &&
            _popup.IsOpen != open)
            _popup.IsOpen = open;
    }

    internal IEnumerable<NavMenuItem> GetLeafMenus()
    {
        if (ItemCount == 0)
        {
            yield return this;
            yield break;
        }

        foreach (var child in LogicalChildren)
            if (child is NavMenuItem item)
            {
                var items = item.GetLeafMenus();
                foreach (var i in items) yield return i;
            }
    }

    internal void CloseAllOpenPopups()
    {
        var items = GetItemsWithChildren(this).Reverse();
        foreach (var item in items)
            if (item._popup is { IsOpen: true } popup) popup.Close();

        static IEnumerable<NavMenuItem> GetItemsWithChildren(NavMenuItem item)
        {
            if (item.ItemCount > 0)
            {
                yield return item;

                var children = item.LogicalChildren
                    .OfType<NavMenuItem>()
                    .SelectMany(child => GetItemsWithChildren(child));
                foreach (var child in children) yield return child;
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

        while (logical is not null and not T)
        {
            if (logical is NavMenuItem) result++;
            logical = logical.LogicalParent;
        }

        return logical is not null ? result : @default;
    }
}