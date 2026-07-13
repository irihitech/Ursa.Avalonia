using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;


/// <summary>
/// Defines a layout panel that positions child elements using <see cref="RelativeScalar"/>
/// values for the <c>Left</c>, <c>Top</c>, <c>Right</c>, and <c>Bottom</c> attached
/// properties.  When the panel is resized, children automatically maintain their
/// proportional positioning — <see cref="RelativeUnit.Relative"/> values are recomputed
/// as a fraction of the panel's current size, while <see cref="RelativeUnit.Absolute"/>
/// values remain fixed in device-independent pixels.
/// </summary>
/// <remarks>
/// <para>
/// <b>Positioning rules:</b>
/// </para>
/// <list type="bullet">
///   <item>If only <c>Left</c> and <c>Top</c> are set, the child is positioned from
///         the top-left corner.</item>
///   <item>If <c>Left</c> and <c>Right</c> are both set, the child's width is stretched
///         to fill the space between them.</item>
///   <item>If <c>Top</c> and <c>Bottom</c> are both set, the child's height is stretched
///         to fill the space between them.</item>
///   <item>If all four properties are set, the child fills the defined rectangle.</item>
///   <item>Unset properties default to <c>0</c> (<see cref="RelativeUnit.Absolute"/>).</item>
/// </list>
/// <para>
/// <b>Example XAML usage:</b>
/// </para>
/// <code>
/// &lt;controls:ProportionalCanvas Width="400" Height="300"&gt;
///     &lt;Border controls:ProportionalCanvas.Left="25%"
///            controls:ProportionalCanvas.Top="10%"
///            Width="100" Height="50" Background="Red" /&gt;
///     &lt;Border controls:ProportionalCanvas.Left="25%"
///            controls:ProportionalCanvas.Right="25%"
///            controls:ProportionalCanvas.Top="40%"
///            controls:ProportionalCanvas.Bottom="10%"
///            Background="Blue" /&gt;
/// &lt;/controls:ProportionalCanvas&gt;
/// </code>
/// </remarks>
public class ProportionalCanvas : Panel
{
    #region Sentinel value for "unset"

    /// <summary>
    /// A special <see cref="RelativeScalar"/> used as the default value for all four
    /// attached properties.  Callers can detect "not set" by checking
    /// <c>double.IsNaN(value.Scalar)</c>.
    /// </summary>
    internal static readonly RelativeScalar UnsetValue = new(double.NaN, RelativeUnit.Absolute);

    #endregion

    #region Attached properties

    /// <summary>
    /// Defines the <c>ProportionalCanvas.Left</c> attached property.
    /// </summary>
    public static readonly AttachedProperty<RelativeScalar> LeftProperty =
        AvaloniaProperty.RegisterAttached<ProportionalCanvas, Control, RelativeScalar>(
            "Left", UnsetValue, true);

    /// <summary>
    /// Defines the <c>ProportionalCanvas.Top</c> attached property.
    /// </summary>
    public static readonly AttachedProperty<RelativeScalar> TopProperty =
        AvaloniaProperty.RegisterAttached<ProportionalCanvas, Control, RelativeScalar>(
            "Top", UnsetValue, true);

    /// <summary>
    /// Defines the <c>ProportionalCanvas.Right</c> attached property.
    /// </summary>
    public static readonly AttachedProperty<RelativeScalar> RightProperty =
        AvaloniaProperty.RegisterAttached<ProportionalCanvas, Control, RelativeScalar>(
            "Right", UnsetValue, true);

    /// <summary>
    /// Defines the <c>ProportionalCanvas.Bottom</c> attached property.
    /// </summary>
    public static readonly AttachedProperty<RelativeScalar> BottomProperty =
        AvaloniaProperty.RegisterAttached<ProportionalCanvas, Control, RelativeScalar>(
            "Bottom", UnsetValue, true);

    #endregion

    #region Attached property accessors

    /// <summary>Gets the value of the <c>ProportionalCanvas.Left</c> attached property.</summary>
    public static RelativeScalar GetLeft(Control element) => element.GetValue(LeftProperty);

    /// <summary>Sets the value of the <c>ProportionalCanvas.Left</c> attached property.</summary>
    public static void SetLeft(Control element, RelativeScalar value) => element.SetValue(LeftProperty, value);

    /// <summary>Gets the value of the <c>ProportionalCanvas.Top</c> attached property.</summary>
    public static RelativeScalar GetTop(Control element) => element.GetValue(TopProperty);

    /// <summary>Sets the value of the <c>ProportionalCanvas.Top</c> attached property.</summary>
    public static void SetTop(Control element, RelativeScalar value) => element.SetValue(TopProperty, value);

    /// <summary>Gets the value of the <c>ProportionalCanvas.Right</c> attached property.</summary>
    public static RelativeScalar GetRight(Control element) => element.GetValue(RightProperty);

    /// <summary>Sets the value of the <c>ProportionalCanvas.Right</c> attached property.</summary>
    public static void SetRight(Control element, RelativeScalar value) => element.SetValue(RightProperty, value);

    /// <summary>Gets the value of the <c>ProportionalCanvas.Bottom</c> attached property.</summary>
    public static RelativeScalar GetBottom(Control element) => element.GetValue(BottomProperty);

    /// <summary>Sets the value of the <c>ProportionalCanvas.Bottom</c> attached property.</summary>
    public static void SetBottom(Control element, RelativeScalar value) => element.SetValue(BottomProperty, value);

    #endregion

    #region Helpers

    /// <summary>Returns <see langword="true"/> when the <paramref name="value"/>
    /// is a real, user-set value (not the <see cref="UnsetValue"/> sentinel).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsSet(RelativeScalar value) => !double.IsNaN(value.Scalar);

    /// <summary>
    /// Resolves a <see cref="RelativeScalar"/> to a final pixel offset.
    /// For <see cref="RelativeUnit.Relative"/>, returns <c>Scalar × fullSize</c>;
    /// for <see cref="RelativeUnit.Absolute"/>, returns <c>Scalar</c> as-is.
    /// </summary>
    private static double Resolve(RelativeScalar scalar, double fullSize)
    {
        if (!IsSet(scalar))
            return 0;
        return scalar.Unit == RelativeUnit.Relative
            ? scalar.Scalar * fullSize
            : scalar.Scalar;
    }

    #endregion

    #region Layout

    /// <inheritdoc/>
    protected override Size MeasureOverride(Size availableSize)
    {
        // Measure each child with the available space.
        foreach (var child in Children)
        {
            child.Measure(availableSize);
        }

        // ProportionalCanvas itself sizes to its available space (or children if not constrained).
        if (double.IsInfinity(availableSize.Width) || double.IsInfinity(availableSize.Height))
        {
            double maxW = 0, maxH = 0;
            foreach (var child in Children)
            {
                var left = GetLeft(child);
                var top = GetTop(child);
                var offsetX = IsSet(left) ? Resolve(left, 0) : 0;
                var offsetY = IsSet(top) ? Resolve(top, 0) : 0;
                maxW = Math.Max(maxW, offsetX + child.DesiredSize.Width);
                maxH = Math.Max(maxH, offsetY + child.DesiredSize.Height);
            }
            return new Size(
                double.IsInfinity(availableSize.Width) ? maxW : availableSize.Width,
                double.IsInfinity(availableSize.Height) ? maxH : availableSize.Height);
        }

        return availableSize;
    }

    /// <inheritdoc/>
    protected override Size ArrangeOverride(Size finalSize)
    {
        double panelW = finalSize.Width;
        double panelH = finalSize.Height;

        foreach (var child in Children)
        {
            var left   = GetLeft(child);
            var top    = GetTop(child);
            var right  = GetRight(child);
            var bottom = GetBottom(child);

            bool hasLeft   = IsSet(left);
            bool hasTop    = IsSet(top);
            bool hasRight  = IsSet(right);
            bool hasBottom = IsSet(bottom);

            double x, y, w, h;

            // ── Horizontal ──────────────────────────────────────────────
            if (hasLeft && hasRight)
            {
                // Stretch: child fills the space between left and right edges.
                double offsetL = Resolve(left, panelW);
                double offsetR = Resolve(right, panelW);
                x = offsetL;
                w = Math.Max(0, panelW - offsetL - offsetR);
            }
            else if (hasLeft)
            {
                x = Resolve(left, panelW);
                w = child.DesiredSize.Width;
            }
            else if (hasRight)
            {
                double offsetR = Resolve(right, panelW);
                w = child.DesiredSize.Width;
                x = panelW - offsetR - w;
            }
            else
            {
                // Neither set — default to left-aligned at 0.
                x = 0;
                w = child.DesiredSize.Width;
            }

            // ── Vertical ────────────────────────────────────────────────
            if (hasTop && hasBottom)
            {
                double offsetT = Resolve(top, panelH);
                double offsetB = Resolve(bottom, panelH);
                y = offsetT;
                h = Math.Max(0, panelH - offsetT - offsetB);
            }
            else if (hasTop)
            {
                y = Resolve(top, panelH);
                h = child.DesiredSize.Height;
            }
            else if (hasBottom)
            {
                double offsetB = Resolve(bottom, panelH);
                h = child.DesiredSize.Height;
                y = panelH - offsetB - h;
            }
            else
            {
                y = 0;
                h = child.DesiredSize.Height;
            }

            child.Arrange(new Rect(x, y, w, h));
        }

        return finalSize;
    }

    #endregion
}
