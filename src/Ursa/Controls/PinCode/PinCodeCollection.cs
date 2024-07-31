using Avalonia.Controls;
using Avalonia.Input;

namespace Ursa.Controls;

public class PinCodeCollection: ItemsControl
{
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<PinCodeItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new PinCodeItem()
        {
            [InputMethod.IsInputMethodEnabledProperty] = false,
        };
    }
}