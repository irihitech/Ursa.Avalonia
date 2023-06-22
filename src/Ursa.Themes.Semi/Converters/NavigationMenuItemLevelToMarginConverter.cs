using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Ursa.Themes.Semi.Converters;

public class NavigationMenuItemLevelToMarginConverter: IValueConverter
{
    public int Indent { get; set; }
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int i)
        {
            return new Thickness((i-1) * Indent, 0, 0, 0);
        }
        return new Thickness();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}