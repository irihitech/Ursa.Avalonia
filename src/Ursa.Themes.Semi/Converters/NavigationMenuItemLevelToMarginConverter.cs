using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Ursa.Themes.Semi.Converters;

public class NavigationMenuItemLevelToMarginConverter: IMultiValueConverter
{
    public int Indent { get; set; }

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is int i && values[1] is bool b)
        {
            if (b)
            {
                return new Thickness();
            }
            return new Thickness((i-1) * Indent, 0, 0, 0);
        }
        return new Thickness();
    }
}