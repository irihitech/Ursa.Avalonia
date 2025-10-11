using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Ursa.Common;

namespace Ursa.Controls;

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
}