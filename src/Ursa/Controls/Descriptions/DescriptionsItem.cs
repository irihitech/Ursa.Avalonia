using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Irihi.Avalonia.Shared.Common;
using Ursa.Common;

namespace Ursa.Controls;

[PseudoClasses(PseudoClassName.PC_Horizontal, PseudoClassName.PC_Vertical)]
public class DescriptionsItem: LabeledContentControl
{
    
    public static readonly StyledProperty<Position> LabelPositionProperty = AvaloniaProperty.Register<DescriptionsItem, Position>(
        nameof(LabelPosition));

    public Position LabelPosition
    {
        get => GetValue(LabelPositionProperty);
        set => SetValue(LabelPositionProperty, value);
    }

    public static readonly StyledProperty<ItemAlignment> ItemAlignmentProperty = AvaloniaProperty.Register<DescriptionsItem, ItemAlignment>(
        nameof(ItemAlignment));

    public ItemAlignment ItemAlignment
    {
        get => GetValue(ItemAlignmentProperty);
        set => SetValue(ItemAlignmentProperty, value);
    }

    public static readonly StyledProperty<double> LabelWidthProperty = AvaloniaProperty.Register<DescriptionsItem, double>(
        nameof(LabelWidth));

    public double LabelWidth
    {
        get => GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }

    static DescriptionsItem()
    {
        LabelPositionProperty.Changed.AddClassHandler<DescriptionsItem, Position>((item, args)=> item.OnLabelPositionChanged(args));
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        PseudoClasses.Set(PseudoClassName.PC_Horizontal, this.LabelPosition is Position.Left or Position.Right);
        PseudoClasses.Set(PseudoClassName.PC_Vertical, this.LabelPosition is Position.Top or Position.Bottom);
    }

    private void OnLabelPositionChanged(AvaloniaPropertyChangedEventArgs<Position> args)
    {
        PseudoClasses.Set(PseudoClassName.PC_Horizontal, args.GetNewValue<Position>() is Position.Left or Position.Right);
        PseudoClasses.Set(PseudoClassName.PC_Vertical, args.GetNewValue<Position>() is Position.Top or Position.Bottom);
    }
    
    
}