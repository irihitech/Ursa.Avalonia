using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.VisualTree;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Shapes;
using Ursa.Common;
using Ursa.Controls.OverlayShared;
using Ursa.EventArgs;
using Ursa.Helpers;

namespace Ursa.Controls;

public partial class OverlayDialogHost
{
    internal async void AddDrawer(DrawerControlBase control)
    {
        PureRectangle? mask = null;
        if (control.CanLightDismiss)
        {
            mask = CreateOverlayMask(false, true);
        }

        _layers.Add(new DialogPair(mask, control));
        ResetZIndices();
        var safePadding = SafePadding;
        var safeWidth = Bounds.Width - safePadding.Left - safePadding.Right;
        var safeHeight = Bounds.Height - safePadding.Top - safePadding.Bottom;
        control.MaxWidth = Math.Min(control.MaxWidth, safeWidth);
        control.MaxHeight = Math.Min(control.MaxHeight, safeHeight);
        if (mask is not null) this.Children.Add(mask);
        this.Children.Add(control);
        control.Measure(new Size(safeWidth, safeHeight));
        control.Arrange(new Rect(control.DesiredSize));
        SetDrawerPosition(control);
        control.AddHandler(OverlayFeedbackElement.ClosedEvent, OnDrawerControlClosing);
        var animation = CreateAnimation(control.Bounds.Size, control.Position);
        if (IsAnimationDisabled)
        {
            ResetDrawerPosition(control, this.Bounds.Size);
        }
        else
        {
            if (mask is null)
            {
                await animation.RunAsync(control);
            }
            else
            {
                await Task.WhenAll(animation.RunAsync(control), MaskAppearAnimation.RunAsync(mask));
            }
        }
    }

    internal async void AddModalDrawer(DrawerControlBase control)
    {
        PureRectangle mask = CreateOverlayMask(true, control.CanLightDismiss);
        var safePadding = SafePadding;
        var safeWidth = Bounds.Width - safePadding.Left - safePadding.Right;
        var safeHeight = Bounds.Height - safePadding.Top - safePadding.Bottom;
        control.MaxWidth = Math.Min(control.MaxWidth, safeWidth);
        control.MaxHeight = Math.Min(control.MaxHeight, safeHeight);
        _layers.Add(new DialogPair(mask, control));
        this.Children.Add(mask);
        this.Children.Add(control);
        ResetZIndices();
        control.Measure(new Size(safeWidth, safeHeight));
        control.Arrange(new Rect(control.DesiredSize));
        SetDrawerPosition(control);
        _modalCount++;
        IsInModalStatus = _modalCount > 0;
        control.AddHandler(OverlayFeedbackElement.ClosedEvent, OnDrawerControlClosing);
        var animation = CreateAnimation(control.Bounds.Size, control.Position);
        if (IsAnimationDisabled)
        {
            ResetDrawerPosition(control, this.Bounds.Size);
        }
        else
        {
            await Task.WhenAll(animation.RunAsync(control), MaskAppearAnimation.RunAsync(mask));
        }

        var element = control.GetVisualDescendants().OfType<InputElement>()
            .FirstOrDefault(FocusHelper.GetDialogFocusHint);
        if (element is null)
        {
            element = control.GetVisualDescendants().OfType<InputElement>().FirstOrDefault(a => a.Focusable);
        }

        element?.Focus();
    }

    private void SetDrawerPosition(DrawerControlBase control)
    {
        var safePadding = SafePadding;
        if (control.Position is Position.Left or Position.Right)
        {
            control.Height = this.Bounds.Height - safePadding.Top - safePadding.Bottom;
            SetTop(control, safePadding.Top);
        }

        if (control.Position is Position.Top or Position.Bottom)
        {
            control.Width = this.Bounds.Width - safePadding.Left - safePadding.Right;
            SetLeft(control, safePadding.Left);
        }
    }

    private void ResetDrawerPosition(DrawerControlBase control, Size newSize)
    {
        var safePadding = SafePadding;
        var safeWidth = newSize.Width - safePadding.Left - safePadding.Right;
        var safeHeight = newSize.Height - safePadding.Top - safePadding.Bottom;
        control.MaxWidth = safeWidth;
        control.MaxHeight = safeHeight;
        if (control.Position == Position.Right)
        {
            control.Height = safeHeight;
            SetLeft(control, newSize.Width - safePadding.Right - control.Bounds.Width);
            SetTop(control, safePadding.Top);
        }
        else if (control.Position == Position.Left)
        {
            control.Height = safeHeight;
            SetLeft(control, safePadding.Left);
            SetTop(control, safePadding.Top);
        }
        else if (control.Position == Position.Top)
        {
            control.Width = safeWidth;
            SetLeft(control, safePadding.Left);
            SetTop(control, safePadding.Top);
        }
        else
        {
            control.Width = safeWidth;
            SetLeft(control, safePadding.Left);
            SetTop(control, newSize.Height - safePadding.Bottom - control.Bounds.Height);
        }
    }

    private Animation CreateAnimation(Size elementBounds, Position position, bool appear = true)
    {
        var safePadding = SafePadding;
        // left or top.
        double source = 0;
        double target = 0;
        if (position == Position.Left)
        {
            source = appear ? -elementBounds.Width : safePadding.Left;
            target = appear ? safePadding.Left : -elementBounds.Width;
        }

        if (position == Position.Right)
        {
            source = appear ? Bounds.Width : Bounds.Width - safePadding.Right - elementBounds.Width;
            target = appear ? Bounds.Width - safePadding.Right - elementBounds.Width : Bounds.Width;
        }

        if (position == Position.Top)
        {
            source = appear ? -elementBounds.Height : safePadding.Top;
            target = appear ? safePadding.Top : -elementBounds.Height;
        }

        if (position == Position.Bottom)
        {
            source = appear ? Bounds.Height : Bounds.Height - safePadding.Bottom - elementBounds.Height;
            target = appear ? Bounds.Height - safePadding.Bottom - elementBounds.Height : Bounds.Height;
        }

        var targetProperty = position == Position.Left || position == Position.Right
            ? Canvas.LeftProperty
            : Canvas.TopProperty;
        var animation = new Animation
        {
            Easing = new CubicEaseOut(),
            FillMode = FillMode.Forward
        };
        var keyFrame1 = new KeyFrame() { Cue = new Cue(0.0) };
        keyFrame1.Setters.Add(new Setter()
            { Property = targetProperty, Value = source });
        var keyFrame2 = new KeyFrame() { Cue = new Cue(1.0) };
        keyFrame2.Setters.Add(new Setter()
            { Property = targetProperty, Value = target });
        animation.Children.Add(keyFrame1);
        animation.Children.Add(keyFrame2);
        animation.Duration = TimeSpan.FromSeconds(0.3);
        return animation;
    }

    private async void OnDrawerControlClosing(object? sender, ResultEventArgs e)
    {
        if (sender is DrawerControlBase control)
        {
            if (control.IsShowAsync is false)
                e.Handled = true;

            var layer = _layers.FirstOrDefault(a => a.Element == control);
            if (layer is null) return;
            _layers.Remove(layer);
            control.RemoveHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosing);
            control.RemoveHandler(DialogControlBase.LayerChangedEvent, OnDialogLayerChanged);
            if (layer.Mask is not null)
            {
                _modalCount--;
                IsInModalStatus = _modalCount > 0;
                layer.Mask.RemoveHandler(PointerPressedEvent, ClickMaskToCloseDialog);
                layer.Mask.RemoveHandler(PointerReleasedEvent, DragMaskToMoveWindow);
                if (!IsAnimationDisabled)
                {
                    var disappearAnimation = CreateAnimation(control.Bounds.Size, control.Position, false);
                    await Task.WhenAll(disappearAnimation.RunAsync(control),
                        MaskDisappearAnimation.RunAsync(layer.Mask));
                }

                Children.Remove(layer.Mask);
            }
            else
            {
                if (!IsAnimationDisabled)
                {
                    var disappearAnimation = CreateAnimation(control.Bounds.Size, control.Position, false);
                    await disappearAnimation.RunAsync(control);
                }
            }

            Children.Remove(control);
            ResetZIndices();
        }
    }
}