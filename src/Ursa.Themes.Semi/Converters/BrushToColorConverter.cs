using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Ursa.Themes.Semi.Converters;

public class BrushToColorConverter: IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ISolidColorBrush b)
        {
            return b.Color;
        }
        return Colors.Transparent;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}