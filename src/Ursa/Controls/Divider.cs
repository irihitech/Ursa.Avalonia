using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;

namespace Ursa.Controls;

public class Divider : ContentControl
{
    public static readonly StyledProperty<AvaloniaList<double>?> DashArrayProperty = Shape.StrokeDashArrayProperty.AddOwner<Divider>();

    public AvaloniaList<double>? DashArray
    {
        get => GetValue(DashArrayProperty);
        set => SetValue(DashArrayProperty, value);
    }
}