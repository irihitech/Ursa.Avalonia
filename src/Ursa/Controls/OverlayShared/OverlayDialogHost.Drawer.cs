using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Styling;
using Ursa.Common;
using Ursa.Controls.OverlayShared;
using Ursa.Controls.Shapes;
using Ursa.EventArgs;

namespace Ursa.Controls;

public partial class OverlayDialogHost
{
    internal async void AddDrawer(DrawerControlBase control)
    {
        PureRectangle? mask = null;
        if (control.ShowMask == false && control.CanLightDismiss)
        {
            mask = CreateOverlayMask(false, true);
        }
        else if (control.ShowMask)
        {
            mask = CreateOverlayMask(control.ShowMask, control.CanClickOnMaskToClose);
        }
        _layers.Add(new DialogPair(mask, control));
        ResetZIndices();
        if(mask is not null)this.Children.Add(mask);
        this.Children.Add(control);
        control.Measure(this.Bounds.Size);
        control.Arrange(new Rect(control.DesiredSize));
        SetDrawerPosition(control);
        control.AddHandler(OverlayFeedbackElement.ClosedEvent, OnDrawerControlClosing);
        var animation = CreateAnimation(control.Bounds.Size, control.Position, true);
        await Task.WhenAll(animation.RunAsync(control), _maskAppearAnimation.RunAsync(mask));
    }

    private void SetDrawerPosition(DrawerControlBase control)
    {
        if(control.Position is Position.Left or Position.Right)
        {
            control.Height = this.Bounds.Height;
        }
        if(control.Position is Position.Top or Position.Bottom)
        {
            control.Width = this.Bounds.Width;
        }
    }

    private Animation CreateAnimation(Size elementBounds, Position position, bool appear = true)
    {
        // left or top.
        double source = 0;
        double target = 0;
        if (position == Position.Left)
        {
            source = appear ? -elementBounds.Width : 0;
            target = appear ? 0 : -elementBounds.Width;
        }

        if (position == Position.Right)
        {
            source = appear ? Bounds.Width : Bounds.Width - elementBounds.Width;
            target = appear ? Bounds.Width - elementBounds.Width : Bounds.Width;
        }
        
        if (position == Position.Top)
        {
            source = appear ? -elementBounds.Height : 0;
            target = appear ? 0 : -elementBounds.Height;
        }
        
        if (position == Position.Bottom)
        {
            source = appear ? Bounds.Height : Bounds.Height - elementBounds.Height;
            target = appear ? Bounds.Height - elementBounds.Height : Bounds.Height;
        }
        
        var targetProperty = position==Position.Left || position==Position.Right ? Canvas.LeftProperty : Canvas.TopProperty;
        var animation = new Animation();
        animation.Easing = new CubicEaseOut();
        animation.FillMode = FillMode.Forward;
        var keyFrame1 = new KeyFrame(){ Cue = new Cue(0.0) };
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
    
    private async void OnDrawerControlClosing(object sender, ResultEventArgs e)
    {
        if (sender is DrawerControlBase control)
        {
            var layer = _layers.FirstOrDefault(a => a.Element == control);
            if(layer is null) return;
            _layers.Remove(layer);
            control.RemoveHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosing);
            control.RemoveHandler(DialogControlBase.LayerChangedEvent, OnDialogLayerChanged); 
            if (layer.Mask is not null)
            {
                layer.Mask.RemoveHandler(PointerPressedEvent, ClickMaskToCloseDialog);
                var disappearAnimation = CreateAnimation(control.Bounds.Size, control.Position, false);
                await Task.WhenAll(disappearAnimation.RunAsync(control), _maskDisappearAnimation.RunAsync(layer.Mask));
                Children.Remove(layer.Mask);
            }
            else
            {
                var disappearAnimation = CreateAnimation(control.Bounds.Size, control.Position, false);
                await disappearAnimation.RunAsync(control);
            }
            Children.Remove(control);
            ResetZIndices();
        }
    }
}