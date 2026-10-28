using System.Globalization;
using Avalonia.Media;
using Irihi.Avalonia.Shared.Converters;

namespace Ursa.Themes.Fluent2.Converters;

public class BrushToColorConverter : MarkupValueConverter
{
    public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ISolidColorBrush b)
        {
            return b.Color;
        }

        return Colors.Transparent;
    }
}