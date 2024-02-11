using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

public class NavMenu: ItemsControl
{
    public static readonly StyledProperty<object?> SelectedItemProperty = AvaloniaProperty.Register<NavMenu, object?>(
        nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    static NavMenu()
    {
        SelectedItemProperty.Changed.AddClassHandler<NavMenu, object?>((o, e) => o.OnSelectedItemChange(e));
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
    
    internal void SelectItem(NavMenuItem item)
    {
        if (item.IsSelected) return;
        var children = this.LogicalChildren.OfType<NavMenuItem>();
        foreach (var child in children)
        {
            if (child != item)
            {
                child.IsSelected = false;
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