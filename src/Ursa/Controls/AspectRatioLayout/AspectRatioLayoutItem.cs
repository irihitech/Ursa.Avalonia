using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;

public class AspectRatioLayoutItem : ContentControl
{
    public static readonly StyledProperty<AspectRatioMode> AcceptScaleModeProperty =
        AvaloniaProperty.Register<AspectRatioLayoutItem, AspectRatioMode>(
            nameof(AcceptAspectRatioMode));

    public static readonly StyledProperty<double> StartAspectRatioValueProperty =
        AvaloniaProperty.Register<AspectRatioLayoutItem, double>(
            nameof(StartAspectRatioValue), defaultValue: double.NaN);

    public double StartAspectRatioValue
    {
        get => GetValue(StartAspectRatioValueProperty);
        set => SetValue(StartAspectRatioValueProperty, value);
    }

    public static readonly StyledProperty<double> EndAspectRatioValueProperty =
        AvaloniaProperty.Register<AspectRatioLayoutItem, double>(
            nameof(EndAspectRatioValue), defaultValue: double.NaN);

    public double EndAspectRatioValue
    {
        get => GetValue(EndAspectRatioValueProperty);
        set => SetValue(EndAspectRatioValueProperty, value);
    }

    public bool IsUseAspectRatioRange =>
        !double.IsNaN(StartAspectRatioValue)
        && !double.IsNaN(EndAspectRatioValue)
        && !(StartAspectRatioValue > EndAspectRatioValue);

    public AspectRatioMode AcceptAspectRatioMode
    {
        get => GetValue(AcceptScaleModeProperty);
        set => SetValue(AcceptScaleModeProperty, value);
    }
}