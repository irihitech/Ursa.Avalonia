using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;

public class AspectRatioLayoutItem : ContentControl
{
    public static readonly StyledProperty<AspectRatioMode> AcceptScaleModeProperty =
        AvaloniaProperty.Register<AspectRatioLayoutItem, AspectRatioMode>(
            nameof(AcceptAspectRatioMode));

    public AspectRatioMode AcceptAspectRatioMode
    {
        get => GetValue(AcceptScaleModeProperty);
        set => SetValue(AcceptScaleModeProperty, value);
    }
}