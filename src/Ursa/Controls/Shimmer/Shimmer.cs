using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace Ursa.Controls;


/// <summary>
/// Attached properties that add a shimmer sweep effect to any control — e.g.
/// skeleton screens on <see cref="Rectangle"/> / <see cref="Border"/>.
/// </summary>
/// <remarks>
/// The target's brush property is replaced by an animated gradient.  When
/// <see cref="BrushProperty"/> is unset it is inferred: <c>TextBlock.Foreground</c>,
/// <c>Shape.Fill</c>, else <c>Border/Panel.Background</c>.
/// </remarks>
public static class Shimmer
{
    /// <summary>Defines the <see cref="GetIsActive"/> / <see cref="SetIsActive"/> attached property.</summary>
    public static readonly AttachedProperty<bool> IsActiveProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>("IsActive", typeof(Shimmer), true);

    /// <summary>Defines the <see cref="GetBaseColor"/> / <see cref="SetBaseColor"/> attached property.</summary>
    public static readonly AttachedProperty<Color?> BaseColorProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, Color?>("BaseColor", typeof(Shimmer));

    /// <summary>Defines the <see cref="GetHighlightColor"/> / <see cref="SetHighlightColor"/> attached property.</summary>
    public static readonly AttachedProperty<Color?> HighlightColorProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, Color?>("HighlightColor", typeof(Shimmer));

    /// <summary>Defines the <see cref="GetDuration"/> / <see cref="SetDuration"/> attached property.</summary>
    public static readonly AttachedProperty<TimeSpan> DurationProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, TimeSpan>("Duration", typeof(Shimmer), TimeSpan.FromSeconds(1.5));

    /// <summary>
    /// Defines the <see cref="GetBrushProperty"/> / <see cref="SetBrushProperty"/> attached property:
    /// the target <see cref="IBrush"/> property to animate.  When unset it is inferred from the
    /// control type.
    /// </summary>
    public static readonly AttachedProperty<AvaloniaProperty?> BrushProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, AvaloniaProperty?>("BrushProperty", typeof(Shimmer));

    /// <summary>Internal animation driver: the gradient offset property.</summary>
    internal static readonly AttachedProperty<double> OffsetProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, double>("Offset", typeof(Shimmer), 0d);

    /// <summary>Maps each target visual to its animator (weak, does not prevent GC).</summary>
    private static readonly ConditionalWeakTable<AvaloniaObject, ShimmerAnimator> Animators = new();

    static Shimmer()
    {
        IsActiveProperty.Changed.AddClassHandler<AvaloniaObject>((o, e) => EnsureAnimator(o));
        BaseColorProperty.Changed.AddClassHandler<AvaloniaObject>((o, e) => EnsureAnimator(o));
        HighlightColorProperty.Changed.AddClassHandler<AvaloniaObject>((o, e) => EnsureAnimator(o));
        DurationProperty.Changed.AddClassHandler<AvaloniaObject>((o, e) => EnsureAnimator(o));
        BrushProperty.Changed.AddClassHandler<AvaloniaObject>((o, e) => OnBrushPropertyChanged(o));
    }

    /// <summary>Gets the value of <see cref="IsActiveProperty"/>.</summary>
    public static bool GetIsActive(AvaloniaObject element) => element.GetValue(IsActiveProperty);

    /// <summary>Sets the value of <see cref="IsActiveProperty"/>.</summary>
    public static void SetIsActive(AvaloniaObject element, bool value)
    {
        element.SetValue(IsActiveProperty, value);
        // Setting the default value does not raise a change event, so create the
        // animator eagerly here (the property observable covers binding scenarios).
        EnsureAnimator(element);
    }

    /// <summary>Gets the value of <see cref="BaseColorProperty"/>.</summary>
    public static Color? GetBaseColor(AvaloniaObject element) => element.GetValue(BaseColorProperty);

    /// <summary>Sets the value of <see cref="BaseColorProperty"/>.</summary>
    public static void SetBaseColor(AvaloniaObject element, Color? value) =>
        element.SetValue(BaseColorProperty, value);

    /// <summary>Gets the value of <see cref="HighlightColorProperty"/>.</summary>
    public static Color? GetHighlightColor(AvaloniaObject element) => element.GetValue(HighlightColorProperty);

    /// <summary>Sets the value of <see cref="HighlightColorProperty"/>.</summary>
    public static void SetHighlightColor(AvaloniaObject element, Color? value) =>
        element.SetValue(HighlightColorProperty, value);

    /// <summary>Gets the value of <see cref="DurationProperty"/>.</summary>
    public static TimeSpan GetDuration(AvaloniaObject element) => element.GetValue(DurationProperty);

    /// <summary>Sets the value of <see cref="DurationProperty"/>.</summary>
    public static void SetDuration(AvaloniaObject element, TimeSpan value) =>
        element.SetValue(DurationProperty, value);

    /// <summary>Gets the value of <see cref="BrushProperty"/>.</summary>
    public static AvaloniaProperty? GetBrushProperty(AvaloniaObject element) => element.GetValue(BrushProperty);

    /// <summary>Sets the value of <see cref="BrushProperty"/>.</summary>
    public static void SetBrushProperty(AvaloniaObject element, AvaloniaProperty? value)
    {
        element.SetValue(BrushProperty, value);
        OnBrushPropertyChanged(element);
    }

    /// <summary>
    /// Ensures an animator exists for <paramref name="target"/>, creating it on
    /// first use.  <see cref="ShimmerText"/> / <see cref="ShimmerSelectableText"/>
    /// call this from their constructors so they share the same animator registry
    /// (and the same property instances) as the attached-property path.
    /// </summary>
    internal static void EnsureAnimator(AvaloniaObject target)
    {
        if (target is not Visual visual)
            return;

        Animators.GetValue(visual, CreateAnimator);
    }

    private static void OnBrushPropertyChanged(AvaloniaObject target)
    {
        if (target is not Visual visual)
            return;

        Animators.Remove(visual);
        EnsureAnimator(visual);
    }

    private static ShimmerAnimator CreateAnimator(AvaloniaObject target)
    {
        var visual = target as Visual
            ?? throw new ArgumentException("Shimmer can only be applied to visuals.", nameof(target));

        var brushProperty = target.GetValue(BrushProperty) as AvaloniaProperty<IBrush?>
            ?? InferBrushProperty(visual)
            ?? throw new InvalidOperationException(
                $"Shimmer: cannot infer a brush property for {target.GetType().Name}; " +
                "set Shimmer.BrushProperty explicitly.");
        
        return new ShimmerAnimator(
            visual,
            brushProperty,
            BaseColorProperty,
            HighlightColorProperty,
            DurationProperty,
            IsActiveProperty,
            OffsetProperty);
    }

    /// <summary>
    /// Infers the brush property to animate from the control type:
    /// <c>TextBlock.Foreground</c>, <c>Shape.Fill</c>, else <c>Border/Panel.Background</c>.
    /// </summary>
    private static AvaloniaProperty<IBrush?>? InferBrushProperty(Visual target) => target switch
    {
        TextBlock => TextBlock.ForegroundProperty,
        Shape => Shape.FillProperty,
        Border => Border.BackgroundProperty,
        Panel => Panel.BackgroundProperty,
        _ => null,
    };
}
