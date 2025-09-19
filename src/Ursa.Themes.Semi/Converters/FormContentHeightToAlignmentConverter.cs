using System.Globalization;
using Avalonia.Layout;
using Irihi.Avalonia.Shared.Converters;

namespace Ursa.Themes.Semi.Converters;

public class FormContentHeightToAlignmentConverter : MarkupValueConverter
{
    public static FormContentHeightToAlignmentConverter Instance = new(32);
    public double Threshold { get; set; }

    public FormContentHeightToAlignmentConverter()
    {
        Threshold = 32;
    }

    // ReSharper disable once ConvertToPrimaryConstructor
    // Justification: need to keep the default constructor for XAML
    public FormContentHeightToAlignmentConverter(double threshold)
    {
        Threshold = threshold;
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double d) return VerticalAlignment.Center;
        return d > Threshold ? VerticalAlignment.Top : VerticalAlignment.Center;
    }
}