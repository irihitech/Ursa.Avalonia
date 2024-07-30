using System.Globalization;
using Avalonia.Data.Converters;

namespace Ursa.Themes.Semi.Converters;

public class ClockHandLengthConverter(double ratio) : IValueConverter
{
    public static ClockHandLengthConverter Hour { get; } = new(1-0.618);
    public static ClockHandLengthConverter Minute { get; } = new(0.618);
    public static ClockHandLengthConverter Second { get; } = new(1);

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double d)
        {
            return d * ratio / 2;
        }
        return 0.0;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}