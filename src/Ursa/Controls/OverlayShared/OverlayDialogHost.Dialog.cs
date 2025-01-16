using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;
using Irihi.Avalonia.Shared.Shapes;
using Ursa.Controls.OverlayShared;
using Ursa.Helpers;

namespace Ursa.Controls;

public partial class OverlayDialogHost
{
    public Thickness SnapThickness { get; set; } = new(0);

    private static void ResetDialogPosition(DialogControlBase control, Size newSize)
    {
        control.MaxWidth = newSize.Width;
        control.MaxHeight = newSize.Height;
        if (control.IsFullScreen)
        {
            control.Width = newSize.Width;
            control.Height = newSize.Height;
            SetLeft(control, 0);
            SetTop(control, 0);
            return;
        }

        var width = newSize.Width - control.Bounds.Width;
        var height = newSize.Height - control.Bounds.Height;
        var newLeft = width * control.HorizontalOffsetRatio ?? 0;
        var newTop = height * control.VerticalOffsetRatio ?? 0;
        if (control.ActualHorizontalAnchor == HorizontalPosition.Left) newLeft = 0;
        if (control.ActualHorizontalAnchor == HorizontalPosition.Right) newLeft = newSize.Width - control.Bounds.Width;
        if (control.ActualVerticalAnchor == VerticalPosition.Top) newTop = 0;
        if (control.ActualVerticalAnchor == VerticalPosition.Bottom) newTop = newSize.Height - control.Bounds.Height;
        SetLeft(control, Math.Max(0.0, newLeft));
        SetTop(control, Math.Max(0.0, newTop));
    }

    internal void AddDialog(DialogControlBase control)
    {
        PureRectangle? mask = null;
        if (control.CanLightDismiss) mask = CreateOverlayMask(false, control.CanLightDismiss);
        if (mask is not null) Children.Add(mask);
        Children.Add(control);
        _layers.Add(new DialogPair(mask, control, false));
        if (control.IsFullScreen)
        {
            control.Width = Bounds.Width;
            control.Height = Bounds.Height;
        }

        control.MaxWidth = Bounds.Width;
        control.MaxHeight = Bounds.Height;
        control.Measure(Bounds.Size);
        control.Arrange(new Rect(control.DesiredSize));
        SetToPosition(control);
        control.AddHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosing);
        control.AddHandler(DialogControlBase.LayerChangedEvent, OnDialogLayerChanged);
        ResetZIndices();
    }

    private async void OnDialogControlClosing(object? sender, object? e)
    {
        if (sender is not DialogControlBase control) return;
        if (control.IsShowAsync is false && e is RoutedEventArgs args)
            args.Handled = true;
        var layer = _layers.FirstOrDefault(a => a.Element == control);
        if (layer is null) return;
        _layers.Remove(layer);

        control.RemoveHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosing);
        control.RemoveHandler(DialogControlBase.LayerChangedEvent, OnDialogLayerChanged);
        layer.Mask?.RemoveHandler(PointerPressedEvent, DragMaskToMoveWindow);

        Children.Remove(control);

        if (layer.Mask is not null)
        {
            Children.Remove(layer.Mask);
            if (layer.Modal)
            {
                _modalCount--;
                IsInModalStatus = _modalCount > 0;
                if (!IsAnimationDisabled) await MaskDisappearAnimation.RunAsync(layer.Mask);
            }
        }

        ResetZIndices();
    }

    /// <summary>
    ///     Add a dialog as a modal dialog to the host
    /// </summary>
    /// <param name="control"></param>
    internal void AddModalDialog(DialogControlBase control)
    {
        var mask = CreateOverlayMask(true, control.CanLightDismiss);
        _layers.Add(new DialogPair(mask, control));
        control.SetAsModal(true);
        ResetZIndices();
        Children.Add(mask);
        Children.Add(control);
        if (control.IsFullScreen)
        {
            control.Width = Bounds.Width;
            control.Height = Bounds.Height;
        }

        control.MaxWidth = Bounds.Width;
        control.MaxHeight = Bounds.Height;
        control.Measure(Bounds.Size);
        control.Arrange(new Rect(control.DesiredSize));
        SetToPosition(control);
        control.AddHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosing);
        control.AddHandler(DialogControlBase.LayerChangedEvent, OnDialogLayerChanged);
        // Notice: mask animation here is not really awaited, because currently dialogs appears immediately.
        if (!IsAnimationDisabled) MaskAppearAnimation.RunAsync(mask);

        var element = control.GetVisualDescendants().OfType<InputElement>()
            .FirstOrDefault(FocusHelper.GetDialogFocusHint);
        if (element is null)
        {
            element = control.GetVisualDescendants().OfType<InputElement>().FirstOrDefault(a => a.Focusable);
        }

        element?.Focus();
        _modalCount++;
        IsInModalStatus = _modalCount > 0;
        control.IsClosed = false;
        // control.Focus();
    }

    // Handle dialog layer change event
    private void OnDialogLayerChanged(object? sender, DialogLayerChangeEventArgs e)
    {
        if (sender is not DialogControlBase control)
            return;
        var layer = _layers.FirstOrDefault(a => a.Element == control);
        if (layer is null) return;
        var index = _layers.IndexOf(layer);
        _layers.Remove(layer);
        var newIndex = index;
        switch (e.ChangeType)
        {
            case DialogLayerChangeType.BringForward:
                newIndex = MathHelpers.SafeClamp(index + 1, 0, _layers.Count);
                break;
            case DialogLayerChangeType.SendBackward:
                newIndex = MathHelpers.SafeClamp(index - 1, 0, _layers.Count);
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
        var left = GetLeftPosition(control);
        var top = GetTopPosition(control);
        SetLeft(control, left);
        SetTop(control, top);
        control.AnchorAndUpdatePositionInfo();
    }

    private double GetLeftPosition(DialogControlBase control)
    {
        var offset = Math.Max(0, control.HorizontalOffset ?? 0);
        var left = Bounds.Width - control.Bounds.Width;
        if (control.HorizontalAnchor == HorizontalPosition.Center)
        {
            left *= 0.5;
            left = MathHelpers.SafeClamp(left, 0, Bounds.Width * 0.5);
        }
        else if (control.HorizontalAnchor == HorizontalPosition.Left)
        {
            left = MathHelpers.SafeClamp(left, 0, offset);
        }
        else if (control.HorizontalAnchor == HorizontalPosition.Right)
        {
            var leftOffset = Bounds.Width - control.Bounds.Width - offset;
            leftOffset = Math.Max(0, leftOffset);
            if (control.HorizontalOffset.HasValue) left = MathHelpers.SafeClamp(left, 0, leftOffset);
        }

        return left;
    }

    private double GetTopPosition(DialogControlBase control)
    {
        var offset = Math.Max(0, control.VerticalOffset ?? 0);
        var top = Bounds.Height - control.Bounds.Height;
        if (control.VerticalAnchor == VerticalPosition.Center)
        {
            top *= 0.5;
            top = MathHelpers.SafeClamp(top, 0, Bounds.Height * 0.5);
        }
        else if (control.VerticalAnchor == VerticalPosition.Top)
        {
            top = MathHelpers.SafeClamp(top, 0, offset);
        }
        else if (control.VerticalAnchor == VerticalPosition.Bottom)
        {
            var topOffset = Math.Max(0, Bounds.Height - control.Bounds.Height - offset);
            top = MathHelpers.SafeClamp(top, 0, topOffset);
        }

        return top;
    }
}