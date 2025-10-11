using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Metadata;

namespace Ursa.Controls;

public class Descriptions: ItemsControl
{
    public static readonly StyledProperty<IDataTemplate?> HeaderTemplateProperty =
        HeaderedContentControl.HeaderTemplateProperty.AddOwner<DescriptionsItem>();

    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IDataTemplate? HeaderTemplate
    {
        get => GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    public static readonly StyledProperty<IBinding?> HeaderValueBindingProperty = AvaloniaProperty.Register<Descriptions, IBinding?>(
        nameof(HeaderValueBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? HeaderValueBinding
    {
        get => GetValue(HeaderValueBindingProperty);
        set => SetValue(HeaderValueBindingProperty, value);
    }
    
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new DescriptionsItem();
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is not DescriptionsItem descriptionItem) return;
        if (HeaderValueBinding is not null)
        {
            descriptionItem[!HeaderedContentControl.HeaderProperty] = HeaderValueBinding;
        }
        descriptionItem[!HeaderedContentControl.HeaderTemplateProperty] = this[!HeaderTemplateProperty];
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = null;
        return item is not DescriptionsItem;
    }
}