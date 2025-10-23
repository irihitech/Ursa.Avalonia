using System.Globalization;
using Avalonia;
using Irihi.Avalonia.Shared.Converters;

namespace Ursa.Themes.Semi.Converters;

public class TreeLevelToPaddingConverter : MarkupMultiValueConverter
{
    public override object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is int i && values[1] is Thickness indent)
        {
            return new Thickness(Math.Max(i, 0) * indent.Left, indent.Top, indent.Right, indent.Bottom);
        }

        return new Thickness();
    }
}