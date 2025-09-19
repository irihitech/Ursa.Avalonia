using System.Globalization;
using Irihi.Avalonia.Shared.Converters;

namespace Ursa.Converters;

public class BadgeContentOverflowConverter : MarkupMultiValueConverter
{
    public override object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        var overflowMark = parameter as string ?? "+";
        if (double.TryParse(values[0]?.ToString(), out var b) && values[1] is int count and > 0)
        {
            if (b > count)
            {
                return $"{count}{overflowMark}";
            }
        }

        return values[0];
    }
}