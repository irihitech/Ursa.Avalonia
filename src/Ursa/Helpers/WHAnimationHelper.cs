using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace Ursa.Helpers;

public class WHAnimationHelper(Control control, AvaloniaProperty property)
{
    private CancellationTokenSource? _cancellationTokenSource;

    ~WHAnimationHelper()
    {
        _cancellationTokenSource?.Dispose();
    }

    public void Stop()
    {
        control.PropertyChanged -= AnimationTargetOnPropertyChanged;
    }

    public void Start()
    {
        control.PropertyChanged += AnimationTargetOnPropertyChanged;
    }

    private void AnimationTargetOnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (sender as Control != control ||
            e.Property != property ||
            control.IsLoaded is false ||
            control.IsVisible is false) return;
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();

        var oldValue = control.DesiredSize;
        control.UpdateLayout();
        var newValue = control.DesiredSize;
        control.InvalidateArrange();
        var animation = CreateAnimation(oldValue, newValue);
        animation.RunAsync(control, _cancellationTokenSource.Token);
    }

    protected virtual Animation CreateAnimation(Size oldValue, Size newValue)
    {
        return new Animation
        {
            Duration = TimeSpan.FromMilliseconds(300),
            Easing = new CubicEaseInOut(),
            FillMode = FillMode.None,
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0.0),
                    Setters =
                    {
                        new Setter(Layoutable.WidthProperty, oldValue.Width)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1.0),
                    Setters =
                    {
                        new Setter(Layoutable.WidthProperty, newValue.Width)
                    }
                }
            }
        };
    }
}