using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Ursa.Controls.Shimmer;

/// <summary>
///     A <see cref="TextBlock" /> whose text sweeps with a shimmering gradient —
///     a common loading effect in AI chat applications.
/// </summary>
/// <remarks>
///     A typed wrapper over the <see cref="Shimmer" /> attached properties (shared via
///     <c>AddOwner</c>).  Use <see cref="ShimmerSelectableText" /> for a selectable
///     variant.
/// </remarks>
public class ShimmerText : TextBlock
{
    /// <summary>Defines the <see cref="BaseColor" /> property (shared with <see cref="Shimmer" />).</summary>
    public static readonly AttachedProperty<Color?> BaseColorProperty =
        Shimmer.BaseColorProperty.AddOwner<ShimmerText>();

    /// <summary>Defines the <see cref="HighlightColor" /> property (shared with <see cref="Shimmer" />).</summary>
    public static readonly AttachedProperty<Color?> HighlightColorProperty =
        Shimmer.HighlightColorProperty.AddOwner<ShimmerText>();

    /// <summary>Defines the <see cref="Duration" /> property (shared with <see cref="Shimmer" />).</summary>
    public static readonly AttachedProperty<TimeSpan> DurationProperty =
        Shimmer.DurationProperty.AddOwner<ShimmerText>();

    /// <summary>Defines the <see cref="IsActive" /> property (shared with <see cref="Shimmer" />).</summary>
    public static readonly AttachedProperty<bool> IsActiveProperty =
        Shimmer.IsActiveProperty.AddOwner<ShimmerText>();

    /// <summary>
    ///     Defines the <see cref="ShimmerOffset" /> property.  This is the internal
    ///     animation driver: the gradient's relative start point (and end point,
    ///     offset by one) follow this value.
    /// </summary>
    public static readonly AttachedProperty<double> ShimmerOffsetProperty =
        Shimmer.OffsetProperty.AddOwner<ShimmerText>();

    /// <summary>
    ///     Initializes a new instance of the <see cref="ShimmerText" /> class.
    /// </summary>
    public ShimmerText()
    {
        // The animator lives in Shimmer's shared registry; this only ensures it
        // exists (CreateAnimator infers TextBlock.ForegroundProperty for us).
        Shimmer.EnsureAnimator(this);
    }

    /// <inheritdoc />
    protected override Type StyleKeyOverride { get; } = typeof(ShimmerText);

    /// <summary>Gets or sets the resting text color (defaults to the current foreground).</summary>
    public Color? BaseColor
    {
        get => GetValue(BaseColorProperty);
        set => SetValue(BaseColorProperty, value);
    }

    /// <summary>Gets or sets the sweeping highlight color (defaults to the theme accent).</summary>
    public Color? HighlightColor
    {
        get => GetValue(HighlightColorProperty);
        set => SetValue(HighlightColorProperty, value);
    }

    /// <summary>Gets or sets the duration of one sweep (default 1.5 s).</summary>
    public TimeSpan Duration
    {
        get => GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    /// <summary>Gets or sets whether the shimmer animation runs (default <see langword="true" />).</summary>
    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    /// <summary>Gets or sets the internal gradient offset (-1 … 2), normally driven by the animation.</summary>
    public double ShimmerOffset
    {
        get => GetValue(ShimmerOffsetProperty);
        set => SetValue(ShimmerOffsetProperty, value);
    }
}
