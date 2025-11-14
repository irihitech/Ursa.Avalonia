using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Irihi.Avalonia.Shared.Converters;

namespace Ursa.Themes.Semi.Converters;

public class BrushToConicGradientConverter: MarkupValueConverter
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ISolidColorBrush brush)
        {
            var color = brush.Color;
            return new ConicGradientBrush
            {
                Angle = 50,
                GradientStops =
                [
                    new GradientStop(new Color(0, color.R, color.G, color.B), 0.4),
                    new GradientStop(color, 0.8),
                    new GradientStop(color, 1.0)
                ]
            };
        }
        return Colors.Transparent;
    }
}