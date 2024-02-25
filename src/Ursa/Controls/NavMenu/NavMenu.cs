using System.Diagnostics;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Metadata;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[PseudoClasses(PC_HorizontalCollapsed)]
public class NavMenu: ItemsControl
{
    public const string PC_HorizontalCollapsed = ":horizontal-collapsed";
    
    public static readonly StyledProperty<object?> SelectedItemProperty = AvaloniaProperty.Register<NavMenu, object?>(
        nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly StyledProperty<IBinding?> IconBindingProperty = AvaloniaProperty.Register<NavMenu, IBinding?>(
        nameof(IconBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? IconBinding
    {
        get => GetValue(IconBindingProperty);
        set => SetValue(IconBindingProperty, value);
    }

    public static readonly StyledProperty<IBinding?> HeaderBindingProperty = AvaloniaProperty.Register<NavMenu, IBinding?>(
        nameof(HeaderBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? HeaderBinding
    {
        get => GetValue(HeaderBindingProperty);
        set => SetValue(HeaderBindingProperty, value);
    }

    public static readonly StyledProperty<IBinding?> SubMenuBindingProperty = AvaloniaProperty.Register<NavMenu, IBinding?>(
        nameof(SubMenuBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? SubMenuBinding
    {
        get => GetValue(SubMenuBindingProperty);
        set => SetValue(SubMenuBindingProperty, value);
    }

    public static readonly StyledProperty<IBinding?> CommandBindingProperty = AvaloniaProperty.Register<NavMenu, IBinding?>(
        nameof(CommandBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? CommandBinding
    {
        get => GetValue(CommandBindingProperty);
        set => SetValue(CommandBindingProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> HeaderTemplateProperty = AvaloniaProperty.Register<NavMenu, IDataTemplate?>(
        nameof(HeaderTemplate));

    /// <summary>
    /// Header Template is used for MenuItem headers, not menu header. 
    /// </summary>
    public IDataTemplate? HeaderTemplate
    {
        get => GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty = AvaloniaProperty.Register<NavMenu, IDataTemplate?>(
        nameof(IconTemplate));

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public static readonly StyledProperty<double> SubMenuIndentProperty = AvaloniaProperty.Register<NavMenu, double>(
        nameof(SubMenuIndent));

    public double SubMenuIndent
    {
        get => GetValue(SubMenuIndentProperty);
        set => SetValue(SubMenuIndentProperty, value);
    }

    public static readonly StyledProperty<bool> IsHorizontalCollapsedProperty = AvaloniaProperty.Register<NavMenu, bool>(
        nameof(IsHorizontalCollapsed));

    public bool IsHorizontalCollapsed
    {
        get => GetValue(IsHorizontalCollapsedProperty);
        set => SetValue(IsHorizontalCollapsedProperty, value);
    }

    public static readonly StyledProperty<object?> HeaderProperty =
        HeaderedContentControl.HeaderProperty.AddOwner<NavMenu>();

    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public static readonly StyledProperty<object?> FooterProperty = AvaloniaProperty.Register<NavMenu, object?>(
        nameof(Footer));

    public object? Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    public static readonly AttachedProperty<bool> CanToggleProperty =
        AvaloniaProperty.RegisterAttached<NavMenu, InputElement, bool>("CanToggle");

    public static void SetCanToggle(InputElement obj, bool value) => obj.SetValue(CanToggleProperty, value);
    public static bool GetCanToggle(InputElement obj) => obj.GetValue(CanToggleProperty);
    
    public static readonly RoutedEvent<SelectionChangedEventArgs> SelectionChangedEvent = RoutedEvent.Register<NavMenu, SelectionChangedEventArgs>(nameof(SelectionChanged), RoutingStrategies.Bubble);
    
    public event EventHandler<SelectionChangedEventArgs>? SelectionChanged
    {
        add => AddHandler(SelectionChangedEvent, value);
        remove => RemoveHandler(SelectionChangedEvent, value);
    }
    
    static NavMenu()
    {
        SelectedItemProperty.Changed.AddClassHandler<NavMenu, object?>((o, e) => o.OnSelectedItemChange(e));
        IsHorizontalCollapsedProperty.AffectsPseudoClass<NavMenu>(PC_HorizontalCollapsed);
        CanToggleProperty.Changed.AddClassHandler<InputElement, bool>(OnInputRegisteredAsToggle);
    }

    private static void OnInputRegisteredAsToggle(InputElement input, AvaloniaPropertyChangedEventArgs<bool> e)
    {
        if (e.NewValue.Value)
        {
            input.AddHandler(PointerPressedEvent, OnElementToggle);
        }
        else
        {
            input.RemoveHandler(PointerPressedEvent, OnElementToggle);
        }
    }

    private static void OnElementToggle(object? sender, RoutedEventArgs args)
    {
        if (sender is not InputElement input) return;
        var nav = input.FindLogicalAncestorOfType<NavMenu>();
        if(nav is null) return;
        bool collapsed = nav.IsHorizontalCollapsed;
        nav.IsHorizontalCollapsed = !collapsed;
    }

    /// <summary>
    /// this implementation only works in the case that only leaf menu item is allowed to select. It will be changed if we introduce parent level selection in the future. 
    /// </summary>
    /// <param name="args"></param>
    private void OnSelectedItemChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        SelectionChangedEventArgs a = new SelectionChangedEventArgs(
            SelectionChangedEvent,
            new [] { args.OldValue.Value },
            new [] { args.NewValue.Value });
        if (_updateFromUI)
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
        var leaves = GetLeafMenus();
        bool found = false;
        foreach (var leaf in leaves)
        {
            if (leaf == newValue || leaf.DataContext == newValue)
            {
                leaf.SelectItem(leaf);
                found = true;
            }
        }
        if (!found)
        {
            ClearAll();
        }
        RaiseEvent(a);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<NavMenuItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new NavMenuItem();
    }

    private bool _updateFromUI;

    internal void SelectItem(NavMenuItem item, NavMenuItem parent)
    {
        _updateFromUI = true;
        foreach (var child in LogicalChildren)
        {
            if (child == parent)
            {
                continue;
            }
            if (child is NavMenuItem navMenuItem)
            {
                navMenuItem.ClearSelection();
            }
        }
        if (item.DataContext is not null && item.DataContext != this.DataContext)
        {
            SelectedItem = item.DataContext;
        }
        else
        {
            SelectedItem = item;
        }
        item.BringIntoView();
        _updateFromUI = false;
    }

    private IEnumerable<NavMenuItem> GetLeafMenus()
    {
        foreach (var child in LogicalChildren)   
        {
            if (child is NavMenuItem item)
            {
                var leafs = item.GetLeafMenus();
                foreach (var leaf in leafs)
                {
                    yield return leaf;
                }
            }
        }
    }

    private void ClearAll()
    {
        foreach (var child in LogicalChildren)   
        {
            if (child is NavMenuItem item)
            {
                item.ClearSelection();
            }
        }
    }
}