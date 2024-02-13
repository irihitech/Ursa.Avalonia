using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
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

    static NavMenu()
    {
        SelectedItemProperty.Changed.AddClassHandler<NavMenu, object?>((o, e) => o.OnSelectedItemChange(e));
        PropertyToPseudoClassMixin.Attach<NavMenu>(IsHorizontalCollapsedProperty, PC_HorizontalCollapsed);
    }

    private void OnSelectedItemChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        Debug.WriteLine(args.NewValue.Value);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<NavMenuItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new NavMenuItem();
    }

    internal void SelectItem(NavMenuItem item, NavMenuItem parent)
    {
        // if (item.IsSelected) return;
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
        item.IsSelected = true;
    }
}