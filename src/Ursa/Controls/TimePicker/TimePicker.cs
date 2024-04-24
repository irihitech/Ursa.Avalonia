using Avalonia;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls.TimePicker;

public class TimePicker: TemplatedControl
{
    public static readonly StyledProperty<string> DisplayFormatProperty = AvaloniaProperty.Register<TimePicker, string>(
        nameof(DisplayFormat), defaultValue:"HH:mm:ss");

    public string DisplayFormat
    {
        get => GetValue(DisplayFormatProperty);
        set => SetValue(DisplayFormatProperty, value);
    }

    public static readonly StyledProperty<string> PanelPlacementProperty =
        AvaloniaProperty.Register<TimePicker, string>(
            nameof(PanelPlacement), defaultValue: "HH mm ss");

    public string PanelPlacement
    {
        get => GetValue(PanelPlacementProperty);
        set => SetValue(PanelPlacementProperty, value);
    }

    public static readonly StyledProperty<TimeSpan?> SelectedTimeProperty = AvaloniaProperty.Register<TimePicker, TimeSpan?>(
        nameof(SelectedTime));

    public TimeSpan? SelectedTime
    {
        get => GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }
}