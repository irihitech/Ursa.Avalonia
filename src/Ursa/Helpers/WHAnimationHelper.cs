using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Styling;

namespace Ursa.Helpers;

public delegate Animation WHAnimationHelperCreateAnimationDelegate(Control animationTargetControl, Size oldDesiredSize,
    Size newDesiredSize);

public class WHAnimationHelper : AvaloniaObject
{
    public static readonly AttachedProperty<WHAnimationHelperCreateAnimationDelegate> CreateAnimationProperty =
        AvaloniaProperty
            .RegisterAttached<WHAnimationHelper, Control, WHAnimationHelperCreateAnimationDelegate>(
                "CreateAnimation", CreateAnimationPropertyDefaultValue);

    internal static readonly AttachedProperty<CancellationTokenSource?> AnimationCancellationTokenSourceProperty =
        AvaloniaProperty.RegisterAttached<WHAnimationHelper, Control, CancellationTokenSource?>(
            "AnimationCancellationTokenSource");

    public static readonly AttachedProperty<AvaloniaProperty?> TriggerAvaloniaPropertyProperty =
        AvaloniaProperty.RegisterAttached<WHAnimationHelper, Control, AvaloniaProperty?>("TriggerAvaloniaProperty");

    public static readonly AttachedProperty<bool> EnableWHAnimationProperty =
        AvaloniaProperty.RegisterAttached<WHAnimationHelper, Control, bool>("EnableWHAnimation");

    private static Animation CreateAnimationPropertyDefaultValue(Control animationTargetControl, Size oldDesiredSize,
        Size newDesiredSize)
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
                        new Setter(Layoutable.WidthProperty, oldDesiredSize.Width),
                        new Setter(Layoutable.HeightProperty, oldDesiredSize.Height)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1.0),
                    Setters =
                    {
                        new Setter(Layoutable.WidthProperty, newDesiredSize.Width),
                        new Setter(Layoutable.HeightProperty, newDesiredSize.Height)
                    }
                }
            }
        };
    }


    public static void SetCreateAnimation(Control obj, WHAnimationHelperCreateAnimationDelegate value)
    {
        obj.SetValue(CreateAnimationProperty, value);
    }

    public static WHAnimationHelperCreateAnimationDelegate GetCreateAnimation(Control obj)
    {
        return obj.GetValue(CreateAnimationProperty);
    }

    internal static void SetAnimationCancellationTokenSource(Control obj, CancellationTokenSource? value)
    {
        obj.SetValue(AnimationCancellationTokenSourceProperty, value);
    }

    internal static CancellationTokenSource? GetAnimationCancellationTokenSource(Control obj)
    {
        return obj.GetValue(AnimationCancellationTokenSourceProperty);
    }

    public static void SetTriggerAvaloniaProperty(Control obj, AvaloniaProperty? value)
    {
        obj.SetValue(TriggerAvaloniaPropertyProperty, value);
    }

    public static AvaloniaProperty? GetTriggerAvaloniaProperty(Control obj)
    {
        return obj.GetValue(TriggerAvaloniaPropertyProperty);
    }

    public static void SetEnableWHAnimation(Control obj, bool value)
    {
        if (value == obj.GetValue(EnableWHAnimationProperty)) return;
        obj.SetValue(EnableWHAnimationProperty, value);
        if (value)
        {
            var triggerProperty = GetTriggerAvaloniaProperty(obj);
            if (triggerProperty == null)
            {
                throw new InvalidOperationException(
                    "WHAnimationHelper requires TriggerAvaloniaProperty to be set when EnableWHAnimation is true.");
            }

            if (triggerProperty == Visual.BoundsProperty ||
                triggerProperty == Layoutable.DesiredSizeProperty)
            {
                throw new InvalidOperationException(
                    "WHAnimationHelper does not support Visual.BoundsProperty or Layoutable.DesiredSizeProperty as trigger property.");
            }

            obj.Loaded += ObjOnLoaded;
        }
        else
        {
            obj.Loaded -= ObjOnLoaded;
            obj.PropertyChanged -= AnimationTargetOnPropertyChanged;
        }

        void ObjOnLoaded(object? sender, RoutedEventArgs e)
        {
            obj.PropertyChanged += AnimationTargetOnPropertyChanged;
            obj.Loaded -= ObjOnLoaded;
        }
    }

    public static bool GetEnableWHAnimation(Control obj)
    {
        return obj.GetValue(EnableWHAnimationProperty);
    }

    private static void AnimationTargetOnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (sender is not Control control ||
            GetEnableWHAnimation(control) is false ||
            e.Property != GetTriggerAvaloniaProperty(control) ||
            e.Property == Visual.BoundsProperty ||
            control.IsLoaded is false ||
            control.IsVisible is false) return;
        var cancellationTokenSource = GetAnimationCancellationTokenSource(control);
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
        cancellationTokenSource = new CancellationTokenSource();
        SetAnimationCancellationTokenSource(control, cancellationTokenSource);

        var oldValue = control.DesiredSize;
        control.UpdateLayout();
        var newValue = control.DesiredSize;
        control.InvalidateArrange();
        var animation = GetCreateAnimation(control)(control, oldValue, newValue);
        animation.RunAsync(control, cancellationTokenSource.Token);
    }
}