using System.Globalization;
using Avalonia;
using Avalonia.Controls.Templates;
using Avalonia.Data.Converters;

namespace Ursa.Themes.Semi.Converters;

public class SelectedItemTemplateConverter: IMultiValueConverter
{
    public static SelectedItemTemplateConverter Instance { get; } = new SelectedItemTemplateConverter();
    
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if(values.Count>0 && values[0] is IDataTemplate template1)
        {
            return template1;
        }
        if(values.Count>1 && values[1] is IDataTemplate template2)
        {
            return template2;
        }
        return AvaloniaProperty.UnsetValue;
    }
}