using Avalonia.Controls;
using Avalonia.Controls.Primitives;

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
}