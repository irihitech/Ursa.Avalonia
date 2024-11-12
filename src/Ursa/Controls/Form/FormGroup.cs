using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

public class FormGroup: HeaderedItemsControl
{
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = null;
        return item is not FormItem;
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        if (item is not Control control) return new FormItem();
        return new FormItem
        {
            Content = control,
            [!FormItem.LabelProperty] = control[!FormItem.LabelProperty],
            [!FormItem.IsRequiredProperty] = control[!FormItem.IsRequiredProperty],
        };
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is FormItem formItem && !formItem.IsSet(ContentControl.ContentTemplateProperty))
        {
            formItem.ContentTemplate = ItemTemplate;
        }
    }
}