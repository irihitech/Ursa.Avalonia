using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Styling;
using Ursa.Controls.OverlayShared;
using Ursa.EventArgs;

namespace Ursa.Controls;

public partial class OverlayDialogHost
{
    internal async void AddDrawer(DrawerControlBase control)
    {
        var mask = CreateOverlayMask(true, false);
        mask.Opacity = 0;
        _layers.Add(new DialogPair(mask, control));
        ResetZIndices();
        this.Children.Add(mask);
        this.Children.Add(control);
        control.Measure(this.Bounds.Size);
        control.Arrange(new Rect(control.DesiredSize).WithHeight(this.Bounds.Height));
        // control.Height = this.Bounds.Height;
        control.AddHandler(OverlayFeedbackElement.ClosedEvent, OnDrawerControlClosing);
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
            var layer = _layers.FirstOrDefault(a => a.Element == control);
            if(layer is null) return;
            _layers.Remove(layer);
            if (layer.Mask is not null)
            {
                layer.Mask.RemoveHandler(PointerPressedEvent, ClickMaskToCloseDialog);
            }
            Children.Remove(control);
            control.RemoveHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosing);
            control.RemoveHandler(DialogControlBase.LayerChangedEvent, OnDialogLayerChanged);
            ResetZIndices();
        }
    }
}