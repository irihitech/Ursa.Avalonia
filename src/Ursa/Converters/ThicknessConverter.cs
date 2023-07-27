using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Ursa.Converters;

[Flags]
public enum ThicknessPosition
{
    Left = 1,
    Top = 2,
    Right = 4,
    Bottom = 8,
    TopLeft = 3,
    TopRight = 6,
    BottomLeft = 9,
    BottomRight = 12,
}

public class ThicknessExcludeConverter: IValueConverter
{
    public static ThicknessExcludeConverter Left { get; } = new( ThicknessPosition.Left );
    public static ThicknessExcludeConverter Top { get; } = new( ThicknessPosition.Top );
    public static ThicknessExcludeConverter Right { get; } = new( ThicknessPosition.Right );
    public static ThicknessExcludeConverter Bottom { get; } = new( ThicknessPosition.Bottom );
    public static ThicknessExcludeConverter TopLeft { get; } = new( ThicknessPosition.TopLeft );
    public static ThicknessExcludeConverter TopRight { get; } = new( ThicknessPosition.TopRight );
    public static ThicknessExcludeConverter BottomLeft { get; } = new( ThicknessPosition.BottomLeft );
    public static ThicknessExcludeConverter BottomRight { get; } = new( ThicknessPosition.BottomRight );

    private readonly ThicknessPosition _position;
    public ThicknessExcludeConverter(ThicknessPosition position)
    {
        _position = position;
    }
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Thickness t)
        {
            double left = _position.HasFlag(ThicknessPosition.Left) ?  0d: t.Left;
            double top = _position.HasFlag(ThicknessPosition.Top) ? 0d : t.Top;
            double right = _position.HasFlag(ThicknessPosition.Right) ? 0d : t.Right;
            double bottom = _position.HasFlag(ThicknessPosition.Bottom) ? 0d : t.Bottom;

            return new Thickness(left, top, right, bottom);
        }
        return AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ThicknessIncludeConverter: IValueConverter
{
    public static ThicknessIncludeConverter Left { get; } = new( ThicknessPosition.Left );
    public static ThicknessIncludeConverter Top { get; } = new( ThicknessPosition.Top );
    public static ThicknessIncludeConverter Right { get; } = new( ThicknessPosition.Right );
    public static ThicknessIncludeConverter Bottom { get; } = new( ThicknessPosition.Bottom );
    public static ThicknessIncludeConverter TopLeft { get; } = new( ThicknessPosition.TopLeft );
    public static ThicknessIncludeConverter TopRight { get; } = new( ThicknessPosition.TopRight );
    public static ThicknessIncludeConverter BottomLeft { get; } = new( ThicknessPosition.BottomLeft );
    public static ThicknessIncludeConverter BottomRight { get; } = new( ThicknessPosition.BottomRight );

    private readonly ThicknessPosition _position;
    public ThicknessIncludeConverter(ThicknessPosition position)
    {
        _position = position;
    }
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Thickness t)
        {
            var left = _position.HasFlag(ThicknessPosition.Left) ? t.Left : 0d;
            var top = _position.HasFlag(ThicknessPosition.Top) ? t.Top : 0d;
            var right = _position.HasFlag(ThicknessPosition.Right) ? t.Right : 0d;
            var bottom = _position.HasFlag(ThicknessPosition.Bottom) ? t.Bottom : 0d;
            return new Thickness(left, top, right, bottom);
        }
        return AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}