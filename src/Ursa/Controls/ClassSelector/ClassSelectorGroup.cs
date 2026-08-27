using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

public class ClassSelectorGroup : HeaderedItemsControl
{
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<ClassSelectorItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new ClassSelectorItem();
    }
}
