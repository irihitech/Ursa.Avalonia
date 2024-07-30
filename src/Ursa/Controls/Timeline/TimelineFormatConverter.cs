using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Ursa.Controls;

public class TimelineFormatConverter: IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count> 1 && values[0] is DateTime date && values[1] is string s)
        {
            return date.ToString(s, culture);
        }
        return AvaloniaProperty.UnsetValue;
    }
}