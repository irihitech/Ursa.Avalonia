using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Utilities;
using Avalonia.VisualTree;

namespace Ursa.Controls; 

public class OverlayDialogHost: Canvas
{
    private readonly List<DialogControl> _dialogs = new();
    private readonly List<DialogControl> _modalDialogs = new();

    private Rectangle _overlayMask = new()
    {
        Fill = new SolidColorBrush(new Color(1, 0, 0, 0)),
        [Rectangle.ZIndexProperty] = 0,
    };
    
    public static readonly StyledProperty<string> HostIdProperty = AvaloniaProperty.Register<OverlayDialogHost, string>(
        nameof(HostId));

    public string HostId
    {
        get => GetValue(HostIdProperty);
        set => SetValue(HostIdProperty, value);
    }
    
    private Point _lastPoint;
    
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        OverlayDialogManager.RegisterOverlayDialogHost(this, HostId);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        OverlayDialogManager.UnregisterOverlayDialogHost(HostId);
        base.OnDetachedFromVisualTree(e);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (e.Source is Control item)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                var parent  = item.FindAncestorOfType<DialogControl>();
                if (parent is null)
                {
                    return;
                }
                var p = e.GetPosition(this);
                var left=  p.X - _lastPoint.X;
                var top = p.Y - _lastPoint.Y;
                left = MathUtilities.Clamp(left, 0, Bounds.Width - parent.Bounds.Width);
                top = MathUtilities.Clamp(top, 0, Bounds.Height - parent.Bounds.Height);
                Canvas.SetLeft(parent, left);
                Canvas.SetTop(parent, top);
            }
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (e.Source is Control item)
        {
            var parent  = item.FindAncestorOfType<DialogControl>();
            if (parent is null)
            {
                return;
            }
            _lastPoint = e.GetPosition(parent);
        }
    }

    public void AddDialog(DialogControl control)
    {
        this.Children.Add(control);
    }

    public void AddModalDialog(DialogControl control)
    {
        this.Children.Add(control);
    }
}