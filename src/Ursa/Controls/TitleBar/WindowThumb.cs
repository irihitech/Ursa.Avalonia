using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Ursa.Controls;

public class WindowThumb: Control
{
    public static readonly StyledProperty<IBrush?> BackgroundProperty = AvaloniaProperty.Register<WindowThumb, IBrush?>(
        nameof(Background), Brushes.Transparent);

    public IBrush? Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }
    
    static WindowThumb()
    {
        IsHitTestVisibleProperty.OverrideDefaultValue<WindowThumb>(true);
        HorizontalAlignmentProperty.OverrideDefaultValue<WindowThumb>(Avalonia.Layout.HorizontalAlignment.Stretch);
        VerticalAlignmentProperty.OverrideDefaultValue<WindowThumb>(Avalonia.Layout.VerticalAlignment.Stretch);
    }

    public WindowThumb()
    {
        this.AddHandler(PointerPressedEvent, OnPressed, RoutingStrategies.Direct);
        this.AddHandler(DoubleTappedEvent, OnTapped, RoutingStrategies.Direct);
    }

    private void OnTapped(object sender, TappedEventArgs e)
    {
        if (this.VisualRoot is not Window visualRoot) return;
        if (!visualRoot.CanResize) return;
        if ( visualRoot.WindowState == WindowState.FullScreen) return;
        visualRoot.WindowState = visualRoot.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void OnPressed(object sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            if (VisualRoot is Window window)
            {
                window.BeginMoveDrag(e);
            }
        }
    }
}