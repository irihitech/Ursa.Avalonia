using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Metadata;
using Ursa.Common;

namespace Ursa.Controls;

[PseudoClasses(PC_FixedWidth)]
public class Descriptions: ItemsControl
{
    public const string PC_FixedWidth = ":fixed-width";
    
    public static readonly StyledProperty<IDataTemplate?> LabelTemplateProperty =
        LabeledContentControl.LabelTemplateProperty.AddOwner<Descriptions>();

    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IDataTemplate? LabelTemplate
    {
        get => GetValue(LabelTemplateProperty);
        set => SetValue(LabelTemplateProperty, value);
    }

    public static readonly StyledProperty<IBinding?> LabelBindingProperty = AvaloniaProperty.Register<Descriptions, IBinding?>(
        nameof(LabelBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? LabelBinding
    {
        get => GetValue(LabelBindingProperty);
        set => SetValue(LabelBindingProperty, value);
    }

    public static readonly StyledProperty<Position> LabelPositionProperty =
        DescriptionsItem.LabelPositionProperty.AddOwner<Descriptions>();

    /// <summary>
    /// The position of the header relative to the content. Only Top and Left are supported.
    /// </summary>
    public Position LabelPosition
    {
        get => GetValue(LabelPositionProperty);
        set => SetValue(LabelPositionProperty, value);
    }

    public static readonly StyledProperty<GridLength> LabelWidthProperty =
        AvaloniaProperty.Register<Descriptions, GridLength>(nameof(LabelWidth));

    public GridLength LabelWidth
    {
        get => GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }

    public static readonly StyledProperty<ItemAlignment> ItemAlignmentProperty =
        DescriptionsItem.ItemAlignmentProperty.AddOwner<Descriptions>();

    public ItemAlignment ItemAlignment
    {
        get => GetValue(ItemAlignmentProperty);
        set => SetValue(ItemAlignmentProperty, value);
    }

    static Descriptions()
    {
        LabelWidthProperty.Changed.AddClassHandler<Descriptions>((x, args) => x.OnLabelWidthChanged(args));
        ItemAlignmentProperty.Changed.AddClassHandler<Descriptions, ItemAlignment>((x, args) =>
            x.OnItemAlignmentChanged(args));
    }

    private void OnItemAlignmentChanged(AvaloniaPropertyChangedEventArgs<ItemAlignment> args)
    {
        PseudoClasses.Set(PC_FixedWidth, args.GetNewValue<ItemAlignment>() != ItemAlignment.Plain);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        PseudoClasses.Set(PC_FixedWidth, this.ItemAlignment != ItemAlignment.Plain);
    }

    private void OnLabelWidthChanged(AvaloniaPropertyChangedEventArgs e)
    {
        foreach (var item in this.VisualChildren)
        {
            if (item is DescriptionsItem descriptionItem)
            {
                descriptionItem.LabelWidth = LabelWidth.IsAbsolute ? LabelWidth.Value : double.NaN;
            }
        }
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new DescriptionsItem();
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is not DescriptionsItem descriptionItem) return;
        if (container == item) return;
        SetupBindings(descriptionItem);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = null;
        if (item is not DescriptionsItem descriptionItem) return true;
        SetupBindings(descriptionItem);
        return false;
    }

    private void SetupBindings(DescriptionsItem item)
    {
        if (LabelBinding is not null)
        {
            if (!item.IsSet(LabeledContentControl.LabelProperty))
            {
                item[!LabeledContentControl.LabelProperty] = LabelBinding;
            }
        }
        if (!item.IsSet(LabelTemplateProperty))
        {
            item[!LabelTemplateProperty] = this[!LabelTemplateProperty];
        }
        if (!item.IsSet(LabelPositionProperty))
        {
            item[!LabelPositionProperty] = this[!LabelPositionProperty];
        }
        if (!item.IsSet(DescriptionsItem.ItemAlignmentProperty))
        {
            item[!DescriptionsItem.ItemAlignmentProperty] = this[!ItemAlignmentProperty];
        }
        if (!item.IsSet(DescriptionsItem.LabelWidthProperty))
        {
            item.LabelWidth = LabelWidth.IsAbsolute ? LabelWidth.Value : double.NaN;
        }
        item[!HeaderedContentControl.HeaderTemplateProperty] = this[!LabelTemplateProperty];
    }
}