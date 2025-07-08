using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Ursa.Themes.Semi.Converters;

public class TreeLevelToMarginConverter: IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is int i && values[1] is double indent)
        {
            return new Thickness(Math.Max(i-1, 0) * indent, 0, 0, 0);
        }
        return new Thickness();
    }
}