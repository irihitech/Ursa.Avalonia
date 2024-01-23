using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Utilities;
using Avalonia.VisualTree;

namespace Ursa.Controls; 

public class OverlayDialogHost: Canvas
{
    private readonly List<DialogControl> _dialogs = new();
    private readonly List<DialogControl> _modalDialogs = new();
    
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
        this.Children.Add(new Border()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Background = Brushes.Black,
            Opacity = 0.3,
            IsVisible = false,
        });
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        if (this.Children.Count > 0)
        {
            this.Children[0].Width = this.Bounds.Width;
            this.Children[0].Height = this.Bounds.Height;
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        OverlayDialogManager.UnregisterOverlayDialogHost(HostId);
        base.OnDetachedFromVisualTree(e);
    }

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
        // base.OnPointerPressed(e);
        if (e.Source is DialogControl item)
        {
            _lastPoint = e.GetPosition(item);
        }
    }

    public void AddDialog(DialogControl control)
    {
        this.Children.Add(control);
        control.OnClose += OnDialogClose;
        if (this.Children.Count > 1)
        {
            this.Children[0].IsVisible = true;
        }
    }

    private void OnDialogClose(object sender, object? e)
    {
        if (sender is DialogControl control)
        {
            this.Children.Remove(control);
            control.OnClose -= OnDialogClose;
            if (this.Children.Count == 1)
            {
                this.Children[0].IsVisible = false;
            }
        }
        
    }

    public void AddModalDialog(DialogControl control)
    {
        this.Children.Add(control);
    }
}