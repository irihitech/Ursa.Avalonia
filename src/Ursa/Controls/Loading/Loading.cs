using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;

public class Loading: ContentControl
{
    public static readonly StyledProperty<object?> IndicatorProperty = AvaloniaProperty.Register<Loading, object?>(
        nameof(Indicator));

    public object? Indicator
    {
        get => GetValue(IndicatorProperty);
        set => SetValue(IndicatorProperty, value);
    }

    public static readonly StyledProperty<bool> IsLoadingProperty = AvaloniaProperty.Register<Loading, bool>(
        nameof(IsLoading));

    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }
    
}