using System.Globalization;
using Avalonia.Controls.Templates;
using Irihi.Avalonia.Shared.Converters;

namespace Ursa.Converters;

public class SelectionBoxTemplateConverter : MarkupMultiValueConverter
{
    public static SelectionBoxTemplateConverter Instance { get; } = new();

    public override object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] is IDataTemplate template) return template;
        }

        return null;
    }
}