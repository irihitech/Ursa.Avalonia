using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Metadata;
using Avalonia.VisualTree;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_ItemsPresenter, typeof(ItemsPresenter))]
[PseudoClasses(PC_HorizontalCollapsed)]
public class NavMenu : ItemsControl, ICustomKeyboardNavigation
{
    public const string PART_ItemsPresenter = "PART_ItemsPresenter";

    public const string PC_HorizontalCollapsed = ":horizontal-collapsed";

    public static readonly StyledProperty<object?> SelectedItemProperty = AvaloniaProperty.Register<NavMenu, object?>(
        nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<IBinding?> IconBindingProperty =
        AvaloniaProperty.Register<NavMenu, IBinding?>(
            nameof(IconBinding));

    public static readonly StyledProperty<IBinding?> HeaderBindingProperty =
        AvaloniaProperty.Register<NavMenu, IBinding?>(
            nameof(HeaderBinding));

    public static readonly StyledProperty<IBinding?> SubMenuBindingProperty =
        AvaloniaProperty.Register<NavMenu, IBinding?>(
            nameof(SubMenuBinding));

    public static readonly StyledProperty<IBinding?> CommandBindingProperty =
        AvaloniaProperty.Register<NavMenu, IBinding?>(
            nameof(CommandBinding));

    public static readonly StyledProperty<IDataTemplate?> HeaderTemplateProperty =
        AvaloniaProperty.Register<NavMenu, IDataTemplate?>(
            nameof(HeaderTemplate));

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty =
        AvaloniaProperty.Register<NavMenu, IDataTemplate?>(
            nameof(IconTemplate));

    public static readonly StyledProperty<double> SubMenuIndentProperty = AvaloniaProperty.Register<NavMenu, double>(
        nameof(SubMenuIndent));

    public static readonly StyledProperty<bool> IsHorizontalCollapsedProperty =
        AvaloniaProperty.Register<NavMenu, bool>(
            nameof(IsHorizontalCollapsed));

    public static readonly StyledProperty<object?> HeaderProperty =
        HeaderedContentControl.HeaderProperty.AddOwner<NavMenu>();

    public static readonly StyledProperty<object?> FooterProperty = AvaloniaProperty.Register<NavMenu, object?>(
        nameof(Footer));

    public static readonly StyledProperty<double> ExpandWidthProperty = AvaloniaProperty.Register<NavMenu, double>(
        nameof(ExpandWidth), double.NaN);

    public static readonly StyledProperty<double> CollapseWidthProperty = AvaloniaProperty.Register<NavMenu, double>(
        nameof(CollapseWidth), double.NaN);

    public static readonly AttachedProperty<bool> CanToggleProperty =
        AvaloniaProperty.RegisterAttached<NavMenu, InputElement, bool>("CanToggle");

    public static readonly RoutedEvent<SelectionChangedEventArgs> SelectionChangedEvent =
        RoutedEvent.Register<NavMenu, SelectionChangedEventArgs>(nameof(SelectionChanged), RoutingStrategies.Bubble);

    private ItemsPresenter? _itemsPresenter = null;
    private bool _isSelectionFromUI = false;
    private bool _isNavigatingMenu = false;

    static NavMenu()
    {
        SelectedItemProperty.Changed.AddClassHandler<NavMenu, object?>((o, e) => o.OnSelectedItemChange(e));
        IsHorizontalCollapsedProperty.AffectsPseudoClass<NavMenu>(PC_HorizontalCollapsed);
        CanToggleProperty.Changed.AddClassHandler<InputElement, bool>(OnInputRegisteredAsToggle);
    }

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? IconBinding
    {
        get => GetValue(IconBindingProperty);
        set => SetValue(IconBindingProperty, value);
    }

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? HeaderBinding
    {
        get => GetValue(HeaderBindingProperty);
        set => SetValue(HeaderBindingProperty, value);
    }

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? SubMenuBinding
    {
        get => GetValue(SubMenuBindingProperty);
        set => SetValue(SubMenuBindingProperty, value);
    }

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? CommandBinding
    {
        get => GetValue(CommandBindingProperty);
        set => SetValue(CommandBindingProperty, value);
    }

    /// <summary>
    ///     Header Template is used for MenuItem headers, not menu header.
    /// </summary>
    public IDataTemplate? HeaderTemplate
    {
        get => GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public double SubMenuIndent
    {
        get => GetValue(SubMenuIndentProperty);
        set => SetValue(SubMenuIndentProperty, value);
    }

    public bool IsHorizontalCollapsed
    {
        get => GetValue(IsHorizontalCollapsedProperty);
        set => SetValue(IsHorizontalCollapsedProperty, value);
    }

    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public object? Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    public double ExpandWidth
    {
        get => GetValue(ExpandWidthProperty);
        set => SetValue(ExpandWidthProperty, value);
    }

    public double CollapseWidth
    {
        get => GetValue(CollapseWidthProperty);
        set => SetValue(CollapseWidthProperty, value);
    }

    public static void SetCanToggle(InputElement obj, bool value)
    {
        obj.SetValue(CanToggleProperty, value);
    }

    public static bool GetCanToggle(InputElement obj)
    {
        return obj.GetValue(CanToggleProperty);
    }

    public event EventHandler<SelectionChangedEventArgs>? SelectionChanged
    {
        add => AddHandler(SelectionChangedEvent, value);
        remove => RemoveHandler(SelectionChangedEvent, value);
    }
    
    
    public static readonly RoutedEvent<SelectionChangingEventArgs> SelectionChangingEvent =
        RoutedEvent.Register<NavMenu, SelectionChangingEventArgs>(nameof(SelectionChanging), RoutingStrategies.Bubble);

    public event EventHandler<SelectionChangingEventArgs> SelectionChanging
    {
        add => AddHandler(SelectionChangingEvent, value);
        remove => RemoveHandler(SelectionChangingEvent, value);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<NavMenuItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new NavMenuItem();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _itemsPresenter = e.NameScope.Find<ItemsPresenter>(PART_ItemsPresenter);
        if (_itemsPresenter is not null)
            KeyboardNavigation.SetTabNavigation(_itemsPresenter, KeyboardNavigationMode.Once);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        TryToSelectItem(SelectedItem);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Handled) return;

        var source = GetContainerFromEventSource(e.Source);
        if (GetNextItem(source, e.Key) is { } target)
        {
            if (e.Key is Key.Up or Key.Down && source is { Level: 1 } firstLevelSource)
                firstLevelSource.CloseAllOpenPopups();
            e.Handled = target.Focus(NavigationMethod.Directional);
        }
    }

    /// <summary>
    ///     this implementation only works in the case that only leaf menu item is allowed to select. It will be changed if we
    ///     introduce parent level selection in the future.
    /// </summary>
    /// <param name="args"></param>
    private void OnSelectedItemChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        var a = new SelectionChangedEventArgs(
            SelectionChangedEvent,
            new[] { args.OldValue.Value },
            new[] { args.NewValue.Value });
        if (_isSelectionFromUI)
        {
            RaiseEvent(a);
            return;
        }

        var newValue = args.NewValue.Value;
        if (newValue is null)
        {
            ClearAll();
            RaiseEvent(a);
            return;
        }
        var found = TryToSelectItem(newValue);
        if (!found) ClearAll();
        RaiseEvent(a);
    }

    private static void OnInputRegisteredAsToggle(InputElement input, AvaloniaPropertyChangedEventArgs<bool> e)
    {
        if (e.NewValue.Value)
            input.AddHandler(PointerPressedEvent, OnElementToggle);
        else
            input.RemoveHandler(PointerPressedEvent, OnElementToggle);
    }

    private static void OnElementToggle(object? sender, RoutedEventArgs args)
    {
        if (sender is not InputElement input) return;
        var nav = input.FindLogicalAncestorOfType<NavMenu>();
        if (nav is null) return;
        var collapsed = nav.IsHorizontalCollapsed;
        nav.IsHorizontalCollapsed = !collapsed;
    }

    internal void SelectItem(NavMenuItem item, NavMenuItem parent)
    {
        _isSelectionFromUI = true;
        foreach (var child in LogicalChildren)
        {
            if (Equals(child, parent)) continue;
            if (child is NavMenuItem navMenuItem) navMenuItem.ClearSelection();
        }

        if (item.DataContext is not null && item.DataContext != DataContext)
            SelectedItem = item.DataContext;
        else
            SelectedItem = item;
        item.BringIntoView();
        _isSelectionFromUI = false;
    }

    private void ClearAll()
    {
        foreach (var child in LogicalChildren)
            if (child is NavMenuItem item)
                item.ClearSelection();
    }

    (bool handled, IInputElement? next) ICustomKeyboardNavigation.GetNext(IInputElement element, NavigationDirection direction)
    {
        if (direction is not NavigationDirection.Next and not NavigationDirection.Previous ||
            _isNavigatingMenu ||
            _itemsPresenter is null) return (false, null);

        var visual = element as Visual;

        if (!_itemsPresenter.IsVisualAncestorOf(visual))
        {
            _isNavigatingMenu = true;
            var next = KeyboardNavigationHandler.GetNext(element, direction);
            _isNavigatingMenu = false;

            if (_itemsPresenter.IsVisualAncestorOf(next as Visual))
            {
                var target = IsHorizontalCollapsed
                    ? GetRootMenuItem(GetContainerForItem(SelectedItem))
                    : GetUncollapsedOrTopmostMenuItem(GetContainerForItem(SelectedItem));
                target ??= ContainerFromIndex(this, 0);

                return (target is not null, target);
            }
        }
        else
        {
            _isNavigatingMenu = true;
            var next = KeyboardNavigationHandler.GetNext(_itemsPresenter, direction);
            _isNavigatingMenu = false;

            if (element is NavMenuItem { Level: 1 } firstLevelItem) firstLevelItem.CloseAllOpenPopups();

            return (true, next);
        }

        return (false, null);
    }

    private NavMenuItem? GetNextItem(NavMenuItem? current, Key key, bool skipChildren = false)
    {
        if (current?.Parent is not ItemsControl parent) return null;

        var index = IndexFromContainer(parent, current);

        return key switch
        {
            Key.Up => NavigateUp(),
            Key.Down => NavigateDown(),
            Key.Right when !IsHorizontalCollapsed => NavigateDown(),
            Key.Left or Key.Escape => parent as NavMenuItem,
            Key.Right or Key.Enter => FirstChild(current),
            _ => null
        };

        NavMenuItem? NavigateUp()
        {
            if (FindSibling(parent, index - 1, -1) is { } previous)
                return !IsHorizontalCollapsed && !previous.IsVerticalCollapsed && previous.ItemCount > 0
                    ? LastChild(previous)
                    : previous;

            return IsHorizontalCollapsed ? LastChild(parent) : parent as NavMenuItem;
        }

        NavMenuItem? NavigateDown()
        {
            if (!skipChildren &&
                !IsHorizontalCollapsed &&
                !current.IsVerticalCollapsed &&
                current.ItemCount > 0 &&
                FirstChild(current) is { } firstChild) return firstChild;

            if (FindSibling(parent, index + 1, 1) is { } next) return next;

            if (IsHorizontalCollapsed) return FirstChild(parent);

            return GetNextItem(parent as NavMenuItem, key, true);
        }

        static NavMenuItem? FirstChild(ItemsControl control) => FindSibling(control, 0, 1);
        static NavMenuItem? LastChild(ItemsControl control) => FindSibling(control, control.ItemCount - 1, -1);

        static NavMenuItem? FindSibling(ItemsControl control, int start, int step)
        {
            for (var i = start; i >= 0 && i < control.ItemCount; i += step)
                if (ContainerFromIndex(control, i) is { IsEnabled: true, IsSeparator: false } item) return item;
            return null;
        }
    }

    private bool TryToSelectItem(object? item)
    {
        if (item is null) return false;
        var leaves = GetLeafMenus();
        var found = false;
        foreach (var leaf in leaves)
            if (leaf == item || leaf.DataContext == item)
            {
                leaf.SelectItem(leaf);
                found = true;
            }

        return found;
    }

    private IEnumerable<NavMenuItem> GetLeafMenus()
    {
        foreach (var child in LogicalChildren)
            if (child is NavMenuItem item)
            {
                var leafs = item.GetLeafMenus();
                foreach (var leaf in leafs) yield return leaf;
            }
    }

    public NavMenuItem? GetContainerForItem(object? item)
    {
        if (item == null) return null;
        return GetContainerForItem(this, item);

        static NavMenuItem? GetContainerForItem(ItemsControl control, object item)
        {
            if (ContainerFromItem(control, item) is NavMenuItem container) return container;

            var children = GetRealizedContainers(control);
            foreach (var child in children)
                if (GetContainerForItem(child, item) is { } childContainer) return childContainer;

            return null;
        }
    }

    private NavMenuItem? GetContainerFromEventSource(object? eventSource)
    {
        if (eventSource is not Visual visual) return null;

        return visual.GetSelfAndVisualAncestors()
            .OfType<NavMenuItem>()
            .FirstOrDefault(i => i.RootMenu == this);
    }

    private static NavMenuItem? ContainerFromIndex(ItemsControl itemsControl, int index) =>
        itemsControl is NavMenuItem navMenuItem
            ? navMenuItem.CollapsedAwareContainerFromIndex(index)
            : (NavMenuItem?)itemsControl.ContainerFromIndex(index);

    private static NavMenuItem? ContainerFromItem(ItemsControl itemsControl, object item) =>
        itemsControl is NavMenuItem navMenuItem
            ? navMenuItem.CollapsedAwareContainerFromItem(item)
            : (NavMenuItem?)itemsControl.ContainerFromItem(item);

    private static int IndexFromContainer(ItemsControl itemsControl, NavMenuItem container) =>
        itemsControl is NavMenuItem navMenuItem
            ? navMenuItem.CollapsedAwareIndexFromContainer(container)
            : itemsControl.IndexFromContainer(container);

    private static IEnumerable<NavMenuItem> GetRealizedContainers(ItemsControl itemsControl)
    {
        var realizedContainers = itemsControl is NavMenuItem navMenuItem
            ? navMenuItem.CollapsedAwareRealizedContainers()
            : itemsControl.GetRealizedContainers();

        return realizedContainers.OfType<NavMenuItem>();
    }

    private static NavMenuItem? GetRootMenuItem(NavMenuItem? item)
    {
        if (item is null) return null;

        return item.GetSelfAndLogicalAncestors()
            .OfType<NavMenuItem>()
            .FirstOrDefault(i => i.Level == 1);
    }

    private static NavMenuItem? GetUncollapsedOrTopmostMenuItem(NavMenuItem? item)
    {
        NavMenuItem? result = null;

        while (item is not null)
        {
            var currentParent = item.Parent as NavMenuItem;
            result ??= currentParent?.IsVerticalCollapsed is false ? item : null;
            if (item.Level == 1) return result ?? item;
            item = currentParent;
        }

        return null;
    }

    internal bool CanChangeSelection(NavMenuItem item)
    {
        object? newSelection = null;
        if (item.DataContext is not null && item.DataContext != DataContext)
            newSelection = item.DataContext;
        else
            newSelection = item;
        var args = new SelectionChangingEventArgs(
            SelectionChangingEvent, 
            new[] { SelectedItem },
            new[] { newSelection })
        {
            Source = this,
        };
        RaiseEvent(args);
        var result =  args.CanSelect;
        if (result == false)
        {
            var container = GetContainerForItem(SelectedItem);
            container?.Focus(NavigationMethod.Directional);
        }
        return result;
    }
}