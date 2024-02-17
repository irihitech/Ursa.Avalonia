using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

public class FormGroup: HeaderedItemsControl
{
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = null;
        return item is not FormItem or FormGroup;
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        if (item is not Control control) return new FormItem();
        var label = Form.GetLabel(control);
        var isRequired = Form.GetIsRequired(control);
        return new FormItem() { Label = label, IsRequired = isRequired, Content = control };
    }
}