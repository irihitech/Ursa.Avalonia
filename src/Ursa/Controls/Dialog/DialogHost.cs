using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Utilities;

namespace Ursa.Controls;

public class DialogHost: Canvas
{
    private Point _lastPoint;
    
    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (e.Source is DialogControl item)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                var p = e.GetPosition(this);
                var left=  p.X - _lastPoint.X;
                var top = p.Y - _lastPoint.Y;
                left = MathUtilities.Clamp(left, 0, Bounds.Width - item.Bounds.Width);
                top = MathUtilities.Clamp(top, 0, Bounds.Height - item.Bounds.Height);
                Canvas.SetLeft(item, left);
                Canvas.SetTop(item, top);
            }
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (e.Source is DialogControl item)
        {
            _lastPoint = e.GetPosition(item);
        }
    }
}