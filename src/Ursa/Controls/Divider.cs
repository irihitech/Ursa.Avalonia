using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace Ursa.Controls;

public class Divider : ContentControl
{
    static Divider()
    {
        HorizontalContentAlignmentProperty.OverrideDefaultValue<Divider>(HorizontalAlignment.Center);
    }

    public Divider()
    {
        HorizontalContentAlignment = HorizontalAlignment.Center;
    }
    
    public static readonly StyledProperty<Orientation> OrientationProperty = AvaloniaProperty.Register<Divider, Orientation>(
            nameof(Orientation));
    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }
}