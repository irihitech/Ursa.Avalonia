using System.Globalization;
using Avalonia.Controls.Templates;
using Avalonia.Data.Converters;

namespace Ursa.Converters;

public class SelectionBoxTemplateConverter: IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        var selectedItemTemplate = values.Count > 0 ? values[0] as IDataTemplate : null;
        if (selectedItemTemplate is not null) return selectedItemTemplate;
        var itemTemplate = values.Count > 1 ? values[1] as IDataTemplate : null;
        return itemTemplate;
    }
}