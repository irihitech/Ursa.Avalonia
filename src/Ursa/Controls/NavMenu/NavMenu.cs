using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

public class NavMenu: SelectingItemsControl
{
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
        item.IsSelected = true;
    }
}