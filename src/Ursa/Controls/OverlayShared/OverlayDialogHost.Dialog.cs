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
using Ursa.Controls.OverlayShared;
using Ursa.Controls.Shapes;
using Ursa.EventArgs;

namespace Ursa.Controls;

public partial class OverlayDialogHost
{
    private Point _lastPoint;
    
    public Thickness SnapThickness { get; set; } = new Thickness(0);
    
    private static void ResetDialogPosition(DialogControlBase control, Size newSize)
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
    
    protected override void OnPointerMoved(PointerEventArgs e)
    {
        if (e.Source is DialogControlBase item)
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
        if (e.Source is DialogControlBase item)
        {
            _lastPoint = e.GetPosition(item);
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (e.Source is DialogControlBase item)
        {
            AnchorAndUpdatePositionInfo(item);
        }
    }

    internal void AddDialog(DialogControlBase control)
    {
        PureRectangle? mask = null;
        if (control.CanLightDismiss)
        {
            CreateOverlayMask(false, control.CanLightDismiss);
        }
        if (mask is not null)
        {
            Children.Add(mask);
        }
        this.Children.Add(control);
        _layers.Add(new DialogPair(mask, control, false));
        control.Measure(this.Bounds.Size);
        control.Arrange(new Rect(control.DesiredSize));
        SetToPosition(control);
        control.AddHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosing);
        control.AddHandler(DialogControlBase.LayerChangedEvent, OnDialogLayerChanged);
        ResetZIndices();
    }
    
    private async void OnDialogControlClosing(object sender, object? e)
    {
        if (sender is DialogControlBase control)
        {
            var layer = _layers.FirstOrDefault(a => a.Element == control);
            if (layer is null) return;
            _layers.Remove(layer);

            control.RemoveHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosing);
            control.RemoveHandler(DialogControlBase.LayerChangedEvent, OnDialogLayerChanged);
            
            Children.Remove(control);

            if (layer.Mask is not null)
            {
                await _maskDisappearAnimation.RunAsync(layer.Mask);
                Children.Remove(layer.Mask);
                if (layer.Modal)
                {
                    _modalCount--;
                    HasModal = _modalCount > 0;
                }
            }
            
            ResetZIndices();
        }
    }

    /// <summary>
    ///  Add a dialog as a modal dialog to the host
    /// </summary>
    /// <param name="control"></param>
    internal void AddModalDialog(DialogControlBase control)
    {
        var mask = CreateOverlayMask(true, control.CanClickOnMaskToClose);
        _layers.Add(new DialogPair(mask, control));
        control.SetAsModal(true);
        ResetZIndices();
        this.Children.Add(mask);
        this.Children.Add(control);
        control.Measure(this.Bounds.Size);
        control.Arrange(new Rect(control.DesiredSize));
        SetToPosition(control);
        control.AddHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosing);
        control.AddHandler(DialogControlBase.LayerChangedEvent, OnDialogLayerChanged);
        _maskAppearAnimation.RunAsync(mask);
        _modalCount++;
        HasModal = _modalCount > 0;
        control.IsClosed = false;
    }

    // Handle dialog layer change event
    private void OnDialogLayerChanged(object sender, DialogLayerChangeEventArgs e)
    {
        if (sender is not DialogControlBase control)
            return;
        var layer = _layers.FirstOrDefault(a => a.Element == control);
        if (layer is null) return;
        int index = _layers.IndexOf(layer);
        _layers.Remove(layer);
        int newIndex = index;
        switch (e.ChangeType)
        {
            case DialogLayerChangeType.BringForward:
                newIndex = MathUtilities.Clamp(index + 1, 0, _layers.Count);
                break;
            case DialogLayerChangeType.SendBackward:
                newIndex = MathUtilities.Clamp(index - 1, 0, _layers.Count);
                break;
            case DialogLayerChangeType.BringToFront:
                newIndex = _layers.Count;
                break;
            case DialogLayerChangeType.SendToBack:
                newIndex = 0;
                break;
        }

        _layers.Insert(newIndex, layer);
        ResetZIndices();
    }
    
    private void SetToPosition(DialogControlBase? control)
    {
        if (control is null) return;
        double left = GetLeftPosition(control);
        double top = GetTopPosition(control);
        SetLeft(control, left);
        SetTop(control, top);
        AnchorAndUpdatePositionInfo(control);
    }
    
    private void AnchorAndUpdatePositionInfo(DialogControlBase control)
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

    private double GetLeftPosition(DialogControlBase control)
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

    private double GetTopPosition(DialogControlBase control)
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

    
}