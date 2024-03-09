using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Media;

namespace Ursa.Controls;

public class VerificationCodeCollection: ItemsControl
{
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<TextBox>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new TextBox()
        {
            TextAlignment = TextAlignment.Center,
            [InputMethod.IsInputMethodEnabledProperty] = false,
        };
    }
}