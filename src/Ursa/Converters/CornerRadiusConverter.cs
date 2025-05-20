using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Ursa.Converters;

[Obsolete("This converter is deprecated. Use CornerRadiusMixerConverter instead.")]
[Flags]
public enum CornerRadiusPosition
{
    TopLeft = 1,
    TopRight = 2,
    BottomLeft = 4,
    BottomRight = 8,
    Top = 3,
    Left = 5,
    Right = 10,
    Bottom = 12,
}

[Obsolete("This converter is deprecated. Use CornerRadiusMixerConverter instead.")]
public class CornerRadiusIncludeConverter(CornerRadiusPosition position) : IValueConverter
{
    public static CornerRadiusIncludeConverter TopLeft { get; } = new( CornerRadiusPosition.TopLeft );
    public static CornerRadiusIncludeConverter TopRight { get; } = new( CornerRadiusPosition.TopRight );
    public static CornerRadiusIncludeConverter BottomLeft { get; } = new( CornerRadiusPosition.BottomLeft );
    public static CornerRadiusIncludeConverter BottomRight { get; } = new( CornerRadiusPosition.BottomRight );
    public static CornerRadiusIncludeConverter Top { get; } = new( CornerRadiusPosition.Top );
    public static CornerRadiusIncludeConverter Left { get; } = new( CornerRadiusPosition.Left );
    public static CornerRadiusIncludeConverter Right { get; } = new( CornerRadiusPosition.Right );
    public static CornerRadiusIncludeConverter Bottom { get; } = new( CornerRadiusPosition.Bottom );

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is CornerRadius r)
        {
            double topLeft = position.HasFlag(CornerRadiusPosition.TopLeft) ? r.TopLeft : 0;
            double topRight = position.HasFlag(CornerRadiusPosition.TopRight) ? r.TopRight : 0;
            double bottomLeft = position.HasFlag(CornerRadiusPosition.BottomLeft) ? r.BottomLeft : 0;
            double bottomRight = position.HasFlag(CornerRadiusPosition.BottomRight) ? r.BottomRight : 0;
            return new CornerRadius(topLeft, topRight, bottomRight, bottomLeft);
        }
        return AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[Obsolete("This converter is deprecated. Use CornerRadiusMixerConverter instead.")]
public class CornerRadiusExcludeConverter : IValueConverter
{
    public static CornerRadiusExcludeConverter TopLeft { get; } = new( CornerRadiusPosition.TopLeft );
    public static CornerRadiusExcludeConverter TopRight { get; } = new( CornerRadiusPosition.TopRight );
    public static CornerRadiusExcludeConverter BottomLeft { get; } = new( CornerRadiusPosition.BottomLeft );
    public static CornerRadiusExcludeConverter BottomRight { get; } = new( CornerRadiusPosition.BottomRight );
    public static CornerRadiusExcludeConverter Top { get; } = new( CornerRadiusPosition.Top );
    public static CornerRadiusExcludeConverter Left { get; } = new( CornerRadiusPosition.Left );
    public static CornerRadiusExcludeConverter Right { get; } = new( CornerRadiusPosition.Right );
    public static CornerRadiusExcludeConverter Bottom { get; } = new( CornerRadiusPosition.Bottom );
    
    private readonly CornerRadiusPosition _position;
    
    public CornerRadiusExcludeConverter(CornerRadiusPosition position)
    {
        _position = position;
    }
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is CornerRadius r)
        {
            double topLeft = _position.HasFlag(CornerRadiusPosition.TopLeft) ? 0 : r.TopLeft;
            double topRight = _position.HasFlag(CornerRadiusPosition.TopRight) ? 0 : r.TopRight;
            double bottomLeft = _position.HasFlag(CornerRadiusPosition.BottomLeft) ? 0 : r.BottomLeft;
            double bottomRight = _position.HasFlag(CornerRadiusPosition.BottomRight) ? 0 : r.BottomRight;
            return new CornerRadius(topLeft, topRight, bottomRight, bottomLeft);
        }
        return AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}