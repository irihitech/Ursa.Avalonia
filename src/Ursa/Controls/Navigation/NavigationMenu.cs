using System.Collections;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;

namespace Ursa.Controls;

[PseudoClasses(PC_Closed)]
[TemplatePart(Name = PART_CloseButton, Type = typeof(ToggleButton))]

public class NavigationMenu: HeaderedItemsControl
{
    public const string PC_Closed = ":closed";
    public const string PART_CloseButton = "PART_CloseButton";
    
    public static readonly StyledProperty<object?> FooterProperty = AvaloniaProperty.Register<NavigationMenu, object?>(
        nameof(Footer));

    public object? Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate> FooterTemplateProperty = AvaloniaProperty.Register<NavigationMenu, IDataTemplate>(
        nameof(FooterTemplate));

    public IDataTemplate FooterTemplate
    {
        get => GetValue(FooterTemplateProperty);
        set => SetValue(FooterTemplateProperty, value);
    }

    public static readonly StyledProperty<object?> IconProperty = AvaloniaProperty.Register<NavigationMenu, object?>(
        nameof(Icon));

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
    

    public static readonly StyledProperty<object?> SelectedItemProperty = AvaloniaProperty.Register<NavigationMenu, object?>(
        nameof(SelectedItem));

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly StyledProperty<bool> ShowCollapseButtonProperty = AvaloniaProperty.Register<NavigationMenu, bool>(
        nameof(ShowCollapseButton));

    public bool ShowCollapseButton
    {
        get => GetValue(ShowCollapseButtonProperty);
        set => SetValue(ShowCollapseButtonProperty, value);
    }

    public static readonly StyledProperty<bool> IsClosedProperty = AvaloniaProperty.Register<NavigationMenu, bool>(
        nameof(IsClosed));

    public bool IsClosed
    {
        get => GetValue(IsClosedProperty);
        set => SetValue(IsClosedProperty, value);
    }

    public static readonly StyledProperty<double> OpenedWidthProperty = AvaloniaProperty.Register<NavigationMenu, double>(
        nameof(OpenedWidth));

    public double OpenedWidth
    {
        get => GetValue(OpenedWidthProperty);
        set => SetValue(OpenedWidthProperty, value);
    }

    public static readonly StyledProperty<double> ClosedWidthProperty = AvaloniaProperty.Register<NavigationMenu, double>(
        nameof(ClosedWidth));

    public double ClosedWidth
    {
        get => GetValue(ClosedWidthProperty);
        set => SetValue(ClosedWidthProperty, value);
    }
    
    

    static NavigationMenu()
    {
        SelectedItemProperty.Changed.AddClassHandler<NavigationMenu>((o, e) => o.OnSelectionItemChanged(e));
        IsClosedProperty.Changed.AddClassHandler<NavigationMenu>((o,e)=>o.OnIsClosedChanged(e));
    }

    private void OnSelectionItemChanged(AvaloniaPropertyChangedEventArgs args)
    {
        var newItem = args.GetNewValue<object?>();
        if (newItem is not null)
        {
            UpdateSelectionFromSelectedItem(newItem);
        }
    }

    private void OnIsClosedChanged(AvaloniaPropertyChangedEventArgs args)
    {
        bool newValue = args.GetNewValue<bool>();
        PseudoClasses.Set(PC_Closed, newValue);
    }
    
    internal void UpdateSelection(NavigationMenuItem source)
    {
        var children = this.ItemsPanelRoot?.Children;
        if (children is not null)
        {
            foreach (var child in children)
            {
                NavigationMenuItem? item =  NavigationMenuItem.GetMenuItemFromControl(child);
                if (item != null)
                {
                    if(Equals(item, source)) continue;
                    item.SetSelection(null, false, false);
                }
            }
        }
    }

    internal void UpdateSelectionFromSelectedItem(object? o)
    {
        var children = this.ItemsPanelRoot?.Children;
        if (children is not null)
        {
            foreach (var child in children)
            {
                NavigationMenuItem? item = NavigationMenuItem.GetMenuItemFromControl(child);
                if(item is null) continue;
                item.UpdateSelectionFromSelectedItem(o);
            }
        }
    }
}
