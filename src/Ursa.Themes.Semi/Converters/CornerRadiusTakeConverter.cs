using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Ursa.Themes.Semi.Converters;

[Obsolete("This converter is deprecated. Use CornerRadiusMixerConverter instead.")]
public class CornerRadiusTakeConverter: IValueConverter
{
    private  readonly Dock _dock;
    internal CornerRadiusTakeConverter(Dock dock)
    {
        _dock = dock;
    }
    public static CornerRadiusTakeConverter Left { get; } = new(Dock.Left);
    public static CornerRadiusTakeConverter Top { get; } = new(Dock.Top);
    public static CornerRadiusTakeConverter Right { get; } = new(Dock.Right);
    public static CornerRadiusTakeConverter Bottom { get; } = new(Dock.Bottom);
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is not CornerRadius c) return AvaloniaProperty.UnsetValue;
        return _dock switch
        {
            Dock.Left => new CornerRadius(c.TopLeft, 0, 0, c.BottomLeft),
            Dock.Top => new CornerRadius(c.TopLeft, c.TopRight, 0, 0),
            Dock.Right => new CornerRadius(0, c.TopRight, c.BottomRight, 0),
            Dock.Bottom => new CornerRadius(0, 0, c.BottomRight, c.BottomLeft),
            _ => AvaloniaProperty.UnsetValue
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}