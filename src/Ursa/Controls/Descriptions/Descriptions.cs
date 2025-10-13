using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.LogicalTree;
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

    public static readonly StyledProperty<IBinding?> LabelMemberBindingProperty = AvaloniaProperty.Register<Descriptions, IBinding?>(
        nameof(LabelMemberBinding));

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? LabelMemberBinding
    {
        get => GetValue(LabelMemberBindingProperty);
        set => SetValue(LabelMemberBindingProperty, value);
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

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == LabelMemberBindingProperty)
        {
            if (change.NewValue != null && LabelTemplate != null)
                throw new InvalidOperationException("Cannot set both LabelMemberBinding and LabelTemplate.");
            _labelDisplayMemberItemTemplate = null;
            RefreshContainers();
        }
        if (change.Property == LabelTemplateProperty)
        {
            if (change.NewValue != null && LabelMemberBinding != null)
            {
                throw new InvalidOperationException("Cannot set both LabelMemberBinding and LabelTemplate.");
            }
            RefreshContainers();
        }
    }

    private void OnLabelWidthChanged(AvaloniaPropertyChangedEventArgs e)
    {
        foreach (var item in this.GetLogicalDescendants().OfType<DescriptionsItem>())
        {
            item.LabelWidth = LabelWidth.IsAbsolute ? LabelWidth.Value : double.NaN;
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
        SetupBindings(descriptionItem, item);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = null;
        if (item is not DescriptionsItem descriptionItem) return true;
        SetupBindings(descriptionItem, null);
        return false;
    }

    private void SetupBindings(DescriptionsItem container, object? item)
    {
        var effectiveLabelTemplate = GetLabelTemplate();
        if (effectiveLabelTemplate is not null && !container.IsSet(LabeledContentControl.LabelTemplateProperty))
        {
            container.LabelTemplate = effectiveLabelTemplate;
        }
        var effectiveContentTemplate = GetContentTemplate();
        if (effectiveContentTemplate is not null && !container.IsSet(ContentControl.ContentTemplateProperty))
        {
            container.ContentTemplate = effectiveContentTemplate;
        }
        if (!container.IsSet(LabeledContentControl.LabelProperty))
        {
            container.Label = item;
        }
        if (!container.IsSet(LabelPositionProperty))
        {
            container[!LabelPositionProperty] = this[!LabelPositionProperty];
        }
        if (!container.IsSet(DescriptionsItem.ItemAlignmentProperty))
        {
            container[!DescriptionsItem.ItemAlignmentProperty] = this[!ItemAlignmentProperty];
        }
        if (!container.IsSet(DescriptionsItem.LabelWidthProperty))
        {
            container.LabelWidth = LabelWidth.IsAbsolute ? LabelWidth.Value : double.NaN;
        }
    }
    
    private IDataTemplate? _valueDisplayMemberItemTemplate;
    private IDataTemplate? _labelDisplayMemberItemTemplate;
    
    private IDataTemplate? GetContentTemplate()
    {
        IDataTemplate? itemTemplate = this.ItemTemplate;
        if (itemTemplate != null)
            return itemTemplate;
        if (this._valueDisplayMemberItemTemplate == null)
        {
            IBinding? binding = this.DisplayMemberBinding;
            if (binding != null)
                _valueDisplayMemberItemTemplate =
                    new FuncDataTemplate<object>((o, s) => new TextBlock { [!TextBlock.TextProperty] = binding });
        }
        return _valueDisplayMemberItemTemplate;
    }
    
    private IDataTemplate? GetLabelTemplate()
    {
        IDataTemplate? itemTemplate = this.LabelTemplate;
        if (itemTemplate != null)
            return itemTemplate;
        if (this._labelDisplayMemberItemTemplate == null)
        {
            IBinding? binding = this.LabelMemberBinding;
            if (binding != null)
                _labelDisplayMemberItemTemplate =
                    new FuncDataTemplate<object>((o, s) => new TextBlock { [!TextBlock.TextProperty] = binding });
        }
        return _labelDisplayMemberItemTemplate;
    }
}