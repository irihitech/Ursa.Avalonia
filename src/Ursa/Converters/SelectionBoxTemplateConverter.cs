using System.Globalization;
using Avalonia.Controls.Templates;
using Avalonia.Data.Converters;

namespace Ursa.Converters;

public class SelectionBoxTemplateConverter: IMultiValueConverter
{
    public static SelectionBoxTemplateConverter Instance { get; } = new();
    
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] is IDataTemplate template) return template;
        }
        return null;
    }
}