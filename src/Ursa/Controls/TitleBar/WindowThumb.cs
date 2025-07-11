using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;

namespace Ursa.Controls;

public class WindowThumb: Control
{
    public static readonly StyledProperty<IBrush?> BackgroundProperty =
        TemplatedControl.BackgroundProperty.AddOwner<WindowThumb>();

    public IBrush? Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }
    static WindowThumb()
    {
        IsHitTestVisibleProperty.OverrideDefaultValue<WindowThumb>(true);
    }

    public WindowThumb()
    {
        this.AddHandler(PointerPressedEvent, OnPressed);
        this.AddHandler(DoubleTappedEvent, OnTapped);
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        if (this.VisualRoot is not Window window) return;
        if (!window.CanResize) return;
        if ( window.WindowState == WindowState.FullScreen) return;
        window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void OnPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
        if (e.ClickCount > 1) return;
        if (VisualRoot is Window window)
        {
            window.BeginMoveDrag(e);
        }
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        if(Background is not null)
        {
            context.FillRectangle(Background, new Rect(Bounds.Size));
        }
    }
}