using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Ursa.Controls;

namespace Ursa.Themes.Semi.Converters;

public class TimelineItemTypeToIconForegroundConverter: AvaloniaObject, IMultiValueConverter
{
    public static readonly StyledProperty<IBrush> DefaultBrushProperty = AvaloniaProperty.Register<TimelineItemTypeToIconForegroundConverter, IBrush>(
        nameof(DefaultBrush));

    public IBrush DefaultBrush
    {
        get => GetValue(DefaultBrushProperty);
        set => SetValue(DefaultBrushProperty, value);
    }
    
    public static readonly StyledProperty<IBrush> OngoingBrushProperty = AvaloniaProperty.Register<TimelineItemTypeToIconForegroundConverter, IBrush>(
        nameof(OngoingBrush));
    
    public IBrush OngoingBrush
    {
        get => GetValue(OngoingBrushProperty);
        set => SetValue(OngoingBrushProperty, value);
    }
    
    public static readonly StyledProperty<IBrush> SuccessBrushProperty = AvaloniaProperty.Register<TimelineItemTypeToIconForegroundConverter, IBrush>(
        nameof(SuccessBrush));

    public IBrush SuccessBrush
    {
        get => GetValue(SuccessBrushProperty);
        set => SetValue(SuccessBrushProperty, value);
    }
    
    public static readonly StyledProperty<IBrush> WarningBrushProperty = AvaloniaProperty.Register<TimelineItemTypeToIconForegroundConverter, IBrush>(
        nameof(WarningBrush));
    
    public IBrush WarningBrush
    {
        get => GetValue(WarningBrushProperty);
        set => SetValue(WarningBrushProperty, value);
    }
    
    public static readonly StyledProperty<IBrush> ErrorBrushProperty = AvaloniaProperty.Register<TimelineItemTypeToIconForegroundConverter, IBrush>(
        nameof(ErrorBrush));
    
    public IBrush ErrorBrush
    {
        get => GetValue(ErrorBrushProperty);
        set => SetValue(ErrorBrushProperty, value);
    }


    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is TimelineItemType type)
        {
            switch (type)
            {
                case TimelineItemType.Error:
                    return ErrorBrush;
                case TimelineItemType.Warning:
                    return WarningBrush;
                case TimelineItemType.Success:
                    return SuccessBrush;
                case TimelineItemType.Ongoing:
                    return OngoingBrush;
                case TimelineItemType.Default:
                    if (values[1] is IBrush brush)
                    {
                        return brush;
                    }
                    return DefaultBrush;
            }
        }
        return AvaloniaProperty.UnsetValue;
    }
}