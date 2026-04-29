using System;
using System.Globalization;
using Avalonia;
using Irihi.Avalonia.Shared.Converters;

namespace Ursa.Demo.DataTemplates;

public class StringToUriConverter : MarkupValueConverter
{
    public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string s && !string.IsNullOrEmpty(s) && Uri.TryCreate(s, UriKind.Absolute, out var uri))
            return uri;
        return AvaloniaProperty.UnsetValue;
    }

    public override object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Uri uri)
            return uri.ToString();
        return AvaloniaProperty.UnsetValue;
    }
}

