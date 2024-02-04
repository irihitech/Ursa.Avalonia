using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Utilities;
using Ursa.EventArgs;

namespace Ursa.Controls;

public class OverlayDialogHost : Canvas
{
    private readonly List<DialogControl> _dialogs = new();
    private readonly List<Control> _modalDialogs = new(); 
    private readonly List<Border> _masks = new();

    public string? HostId { get; set; }

    private Point _lastPoint;


    public DataTemplates DialogDataTemplates { get; set; } = new DataTemplates();
    public Thickness SnapThickness { get; set; } = new Thickness(0);

    public static readonly StyledProperty<IBrush?> OverlayMaskBrushProperty =
        AvaloniaProperty.Register<OverlayDialogHost, IBrush?>(
            nameof(OverlayMaskBrush));

    public IBrush? OverlayMaskBrush
    {
        get => GetValue(OverlayMaskBrushProperty);
        set => SetValue(OverlayMaskBrushProperty, value);
    }

    private Border CreateOverlayMask(bool canCloseOnClick)
    {
        Border border = new()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Width = this.Bounds.Width,
            Height = this.Bounds.Height,
            [!BackgroundProperty] = this[!OverlayMaskBrushProperty],
            IsVisible = true,
        };
        if (canCloseOnClick)
        {
            border.AddHandler(PointerReleasedEvent, ClickBorderToCloseDialog);
        }
        return border;
    }

    private void ClickBorderToCloseDialog(object sender, PointerReleasedEventArgs e)
    {
        if (sender is Border border)
        {
            int i = _masks.IndexOf(border);
            if (_modalDialogs[i] is DialogControl dialog)
            {
                dialog?.CloseDialog();
                border.RemoveHandler(PointerReleasedEvent, ClickBorderToCloseDialog);
            }
            else if(_modalDialogs[i] is DrawerControlBase drawer)
            {
                drawer.CloseDrawer();
                border.RemoveHandler(PointerReleasedEvent, ClickBorderToCloseDialog);
            }
        }
    }

    protected sealed override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        OverlayDialogManager.RegisterHost(this, HostId);
    }

    protected sealed override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        for (int i = 0; i < _masks.Count; i++)
        {
            _masks[i].Width = this.Bounds.Width;
            _masks[i].Height = this.Bounds.Height;
        }

        var oldSize = e.PreviousSize;
        var newSize = e.NewSize;
        foreach (var dialog in _dialogs)
        {
            ResetDialogPosition(dialog, oldSize, newSize);
        }

        foreach (var modalDialog in _modalDialogs)
        {
            if (modalDialog is DialogControl c)
            {
                ResetDialogPosition(c, oldSize, newSize);
            }
            
        }
    }
    
    private void ResetDialogPosition(DialogControl control, Size oldSize, Size newSize)
    {
        var width = newSize.Width - control.Bounds.Width;
        var height = newSize.Height - control.Bounds.Height;
        var newLeft = width * control.HorizontalOffsetRatio??0;
        var newTop = height * control.VerticalOffsetRatio??0;
        if(control.ActualHorizontalAnchor == HorizontalPosition.Left)
        {
            newLeft = 0;
        }
        if (control.ActualHorizontalAnchor == HorizontalPosition.Right)
        {
            newLeft = newSize.Width - control.Bounds.Width;
        }
        if (control.ActualVerticalAnchor == VerticalPosition.Top)
        {
            newTop = 0;
        }
        if (control.ActualVerticalAnchor == VerticalPosition.Bottom)
        {
            newTop = newSize.Height - control.Bounds.Height;
        }
        SetLeft(control, Math.Max(0.0, newLeft));
        SetTop(control, Math.Max(0.0, newTop));
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        OverlayDialogManager.UnregisterHost(HostId);
        base.OnDetachedFromVisualTree(e);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        if (e.Source is DialogControl item)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                var p = e.GetPosition(this);
                var left = p.X - _lastPoint.X;
                var top = p.Y - _lastPoint.Y;
                left = MathUtilities.Clamp(left, 0, Bounds.Width - item.Bounds.Width);
                top = MathUtilities.Clamp(top, 0, Bounds.Height - item.Bounds.Height);
                SetLeft(item, left);
                SetTop(item, top);
            }
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        if (e.Source is DialogControl item)
        {
            _lastPoint = e.GetPosition(item);
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (e.Source is DialogControl item)
        {
            AnchorDialog(item);
        }
    }

    internal void AddDialog(DialogControl control)
    {
        this.Children.Add(control);
        _dialogs.Add(control);
        control.Measure(this.Bounds.Size);
        control.Arrange(new Rect(control.DesiredSize));
        SetToPosition(control);
        control.AddHandler(DialogControl.ClosedEvent, OnDialogControlClosing);
        control.AddHandler(DialogControl.LayerChangedEvent, OnDialogLayerChanged);
        ResetZIndices();
    }



    private void OnDialogControlClosing(object sender, object? e)
    {
        if (sender is DialogControl control)
        {
            Children.Remove(control);
            control.RemoveHandler(DialogControl.ClosedEvent, OnDialogControlClosing);
            control.RemoveHandler(DialogControl.LayerChangedEvent, OnDialogLayerChanged);
            if (_dialogs.Contains(control))
            {
                _dialogs.Remove(control);
            }
            else if (_modalDialogs.Contains(control))
            {
                _modalDialogs.Remove(control);
                if (_masks.Count > 0)
                {
                    var last = _masks.Last();
                    this.Children.Remove(last);
                    _masks.Remove(last);
                    if (_masks.Count > 0)
                    {
                        _masks.Last().IsVisible = true;
                    }
                }
            }
            ResetZIndices();
        }
    }

    /// <summary>
    ///  Add a dialog as a modal dialog to the host
    /// </summary>
    /// <param name="control"></param>
    internal void AddModalDialog(DialogControl control)
    {
        var mask = CreateOverlayMask(control.CanClickOnMaskToClose);
        _masks.Add(mask);
        _modalDialogs.Add(control);
        control.SetAsModal(true);
        for (int i = 0; i < _masks.Count-1; i++)
        {
            _masks[i].Opacity = 0.5;
        }
        ResetZIndices();
        this.Children.Add(mask);
        this.Children.Add(control);
        control.Measure(this.Bounds.Size);
        control.Arrange(new Rect(control.DesiredSize));
        SetToPosition(control);
        control.AddHandler(DialogControl.ClosedEvent, OnDialogControlClosing);
        control.AddHandler(DialogControl.LayerChangedEvent, OnDialogLayerChanged);
    }
    
    internal async void AddDrawer(DrawerControlBase control)
    {
        var mask = CreateOverlayMask(false);
        mask.Opacity = 0;
        _masks.Add(mask);
        _modalDialogs.Add(control);
        // control.SetAsModal(true);
        for (int i = 0; i < _masks.Count-1; i++)
        {
            _masks[i].Opacity = 0.5;
        }
        ResetZIndices();
        this.Children.Add(mask);
        this.Children.Add(control);
        control.Measure(this.Bounds.Size);
        control.Arrange(new Rect(control.DesiredSize));
        control.Height = this.Bounds.Height;
        control.AddHandler(DrawerControlBase.ClosedEvent, OnDrawerControlClosing);
        // SetLeft(control, this.Bounds.Width - control.Bounds.Width);
        var animation = CreateAnimation(control.Bounds.Width);
        var animation2 = CreateOpacityAnimation();
        await Task.WhenAll(animation.RunAsync(control), animation2.RunAsync(mask));
    }

    private Animation CreateAnimation(double width)
    {
        var animation = new Animation();
        animation.Easing = new CubicEaseOut();
        animation.FillMode = FillMode.Forward;
        var keyFrame1 = new KeyFrame(){ Cue = new Cue(0.0) };
        keyFrame1.Setters.Add(new Setter() { Property = Canvas.LeftProperty, Value = Bounds.Width });
        var keyFrame2 = new KeyFrame() { Cue = new Cue(1.0) };
        keyFrame2.Setters.Add(new Setter() { Property = Canvas.LeftProperty, Value = Bounds.Width - width });
        animation.Children.Add(keyFrame1);
        animation.Children.Add(keyFrame2);
        animation.Duration = TimeSpan.FromSeconds(0.3);
        return animation;
    }
    
    private Animation CreateOpacityAnimation()
    {
        var animation = new Animation();
        animation.FillMode = FillMode.Forward;
        var keyFrame1 = new KeyFrame(){ Cue = new Cue(0.0) };
        keyFrame1.Setters.Add(new Setter(){ Property = OpacityProperty, Value = 0.0});
        var keyFrame2 = new KeyFrame() { Cue = new Cue(1.0) };
        keyFrame2.Setters.Add(new Setter() { Property = OpacityProperty, Value = 1.0 });
        animation.Children.Add(keyFrame1);
        animation.Children.Add(keyFrame2);
        animation.Duration = TimeSpan.FromSeconds(0.3);
        return animation;
    }

    private void OnDrawerControlClosing(object sender, ResultEventArgs e)
    {
        if (sender is DrawerControlBase control)
        {
            Children.Remove(control);
            control.RemoveHandler(DialogControl.ClosedEvent, OnDialogControlClosing);
            control.RemoveHandler(DialogControl.LayerChangedEvent, OnDialogLayerChanged);
            if (_modalDialogs.Contains(control))
            {
                _modalDialogs.Remove(control);
                if (_masks.Count > 0)
                {
                    var last = _masks.Last();
                    this.Children.Remove(last);
                    _masks.Remove(last);
                    if (_masks.Count > 0)
                    {
                        _masks.Last().IsVisible = true;
                    }
                }
            }
            ResetZIndices();
        }
    }

    // Handle dialog layer change event
    private void OnDialogLayerChanged(object sender, DialogLayerChangeEventArgs e)
    {
        if (sender is not DialogControl control)
            return;
        if (!_dialogs.Contains(control))
            return;
        int index = _dialogs.IndexOf(control);
        _dialogs.Remove(control);
        int newIndex = index;
        switch (e.ChangeType)
        {
            case DialogLayerChangeType.BringForward:
                newIndex = MathUtilities.Clamp(index + 1, 0, _dialogs.Count);
                break;
            case DialogLayerChangeType.SendBackward:
                newIndex = MathUtilities.Clamp(index - 1, 0, _dialogs.Count);
                break;
            case DialogLayerChangeType.BringToFront:
                newIndex = _dialogs.Count;
                break;
            case DialogLayerChangeType.SendToBack:
                newIndex = 0;
                break;
        }

        _dialogs.Insert(newIndex, control);
        for (int i = 0; i < _dialogs.Count; i++)
        {
            _dialogs[i].ZIndex = i;
        }

        for (int i = 0; i < _masks.Count * 2; i += 2) 
        {
            _masks[i].ZIndex = _dialogs.Count + i;
            _modalDialogs[i].ZIndex = _dialogs.Count + i + 1;
        }
        
    }

    private void ResetZIndices()
    {
        int index = 0;
        for ( int i = 0; i< _dialogs.Count; i++)
        {
            _dialogs[i].ZIndex = index;
            index++;
        }
        for(int i = 0; i< _masks.Count; i++)
        {
            _masks[i].ZIndex = index;
            index++;
            _modalDialogs[i].ZIndex = index;
            index++;
        }
    }

    private void SetToPosition(DialogControl? control)
    {
        if (control is null) return;
        double left = GetLeftPosition(control);
        double top = GetTopPosition(control);
        SetLeft(control, left);
        SetTop(control, top);
        AnchorDialog(control);
    }
    
    private void AnchorDialog(DialogControl control)
    {
        control.ActualHorizontalAnchor = HorizontalPosition.Center;
        control.ActualVerticalAnchor = VerticalPosition.Center;
        double left = GetLeft(control);
        double top = GetTop(control);
        double right = Bounds.Width - left - control.Bounds.Width;
        double bottom = Bounds.Height - top - control.Bounds.Height;
        if(top < SnapThickness.Top)
        {
            SetTop(control, 0);
            control.ActualVerticalAnchor = VerticalPosition.Top;
            control.VerticalOffsetRatio = 0;
        }
        if(bottom < SnapThickness.Bottom)
        {
            SetTop(control, Bounds.Height - control.Bounds.Height);
            control.ActualVerticalAnchor = VerticalPosition.Bottom;
            control.VerticalOffsetRatio = 1;
        }
        if(left < SnapThickness.Left)
        {
            SetLeft(control, 0);
            control.ActualHorizontalAnchor = HorizontalPosition.Left;
            control.HorizontalOffsetRatio = 0;
        }
        if(right < SnapThickness.Right)
        {
            SetLeft(control, Bounds.Width - control.Bounds.Width);
            control.ActualHorizontalAnchor = HorizontalPosition.Right;
            control.HorizontalOffsetRatio = 1;
        }
        left = GetLeft(control);
        top = GetTop(control);
        right = Bounds.Width - left - control.Bounds.Width;
        bottom = Bounds.Height - top - control.Bounds.Height;

        control.HorizontalOffsetRatio = (left + right) == 0 ? 0 : left / (left + right);
        control.VerticalOffsetRatio = (top + bottom) == 0 ? 0 : top / (top + bottom);
    }

    private double GetLeftPosition(DialogControl control)
    {
        double left = 0;
        double offset = Math.Max(0, control.HorizontalOffset ?? 0);
        left = this.Bounds.Width - control.Bounds.Width;
        if (control.HorizontalAnchor == HorizontalPosition.Center)
        {
            left *= 0.5;
            (double min, double max) = MathUtilities.GetMinMax(0, Bounds.Width * 0.5);
            left = MathUtilities.Clamp(left, min, max);
        }
        else if (control.HorizontalAnchor == HorizontalPosition.Left)
        {
            (double min, double max) = MathUtilities.GetMinMax(0, offset);
            left = MathUtilities.Clamp(left, min, max);
        }
        else if (control.HorizontalAnchor == HorizontalPosition.Right)
        {
            double leftOffset = Bounds.Width - control.Bounds.Width - offset;
            leftOffset = Math.Max(0, leftOffset);
            if(control.HorizontalOffset.HasValue)
            {
                left = MathUtilities.Clamp(left, 0, leftOffset);
            }
        }
        return left;
    }

    private double GetTopPosition(DialogControl control)
    {
        double top = 0;
        double offset = Math.Max(0, control.VerticalOffset ?? 0);
        top = this.Bounds.Height - control.Bounds.Height;
        if (control.VerticalAnchor == VerticalPosition.Center)
        {
            top *= 0.5;
            (double min, double max) = MathUtilities.GetMinMax(0, Bounds.Height * 0.5);
            top = MathUtilities.Clamp(top, min, max);
        }
        else if (control.VerticalAnchor == VerticalPosition.Top)
        {
            top = MathUtilities.Clamp(top, 0, offset);
        }
        else if (control.VerticalAnchor == VerticalPosition.Bottom)
        {
            var topOffset = Math.Max(0, Bounds.Height - control.Bounds.Height - offset);
            top = MathUtilities.Clamp(top, 0, topOffset);
        }
        return top;
    }

    internal IDataTemplate? GetDataTemplate(object? o)
    {
        if (o is null) return null;
        IDataTemplate? result = null;
        var templates = this.DialogDataTemplates;
        result = templates.FirstOrDefault(a => a.Match(o));
        if (result != null) return result;
        var keys = this.Resources.Keys;
        foreach (var key in keys)
        {
            if (Resources.TryGetValue(key, out var value) && value is IDataTemplate t)
            {
                result = t;
                break;
            }
        }
        return result;
    }
}