using System.Globalization;
using Avalonia.Data.Converters;

namespace Ursa.Themes.Semi.Converters;

public class BooleansToOpacityConverter: IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
        {
            return b? 1.0: 0.0;
        }
        return 1;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}