using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Ursa.Themes.Semi.Converters;

[Obsolete("This converter is deprecated. Use ThicknessMixerConverter instead.")]
public class ThicknessTakeConverter: IValueConverter
{
    private readonly Dock _dock;
    internal ThicknessTakeConverter(Dock dock)
    {
        _dock = dock;
    }
    public static ThicknessTakeConverter Left { get; } = new(Dock.Left);
    public static ThicknessTakeConverter Top { get; } = new(Dock.Top);
    public static ThicknessTakeConverter Right { get; } = new(Dock.Right);
    public static ThicknessTakeConverter Bottom { get; } = new(Dock.Bottom);
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Thickness t) return AvaloniaProperty.UnsetValue;
        return _dock switch
        {
            Dock.Left => new Thickness(t.Left, 0, 0, 0),
            Dock.Top => new Thickness(0, t.Top, 0, 0),
            Dock.Right => new Thickness(0, 0, t.Right, 0),
            Dock.Bottom => new Thickness(0, 0, 0, t.Bottom),
            _ => AvaloniaProperty.UnsetValue
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}