using Avalonia;
using Avalonia.Controls;
using Ursa.Common;

namespace Ursa.Controls;

public class DescriptionsItem: LabeledContentControl
{
    public const string PC_Horizontal = ":horizontal";
    
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

    private void OnLabelPositionChanged(AvaloniaPropertyChangedEventArgs<Position> args)
    {
        PseudoClasses.Set(PC_Horizontal, args.NewValue.Value == Position.Left || args.NewValue.Value == Position.Right);
    }
    
    
}