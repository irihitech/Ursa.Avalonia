using Avalonia;
using Avalonia.Media;

namespace Ursa.Controls;

/// <summary>
/// Provides static geometry-generation helpers used by <see cref="GroupBoxBorder"/> to build
/// the fill, border-ring, and clip <see cref="StreamGeometry"/> objects.
/// </summary>
internal static class GroupBoxBorderGeometryHelper
{
    /// <summary>
    /// Returns the per-corner outer radii for a border ring whose inner radii are
    /// <paramref name="inner"/> (i.e. the <see cref="GroupBoxBorder.CornerRadius"/> property
    /// value) and whose side thicknesses are given by <paramref name="bt"/>.
    /// Each outer radius equals the inner radius plus the larger of the two adjacent side
    /// thicknesses at that corner.
    /// </summary>
    public static CornerRadius ComputeOuterCornerRadius(CornerRadius inner, Thickness bt) =>
        new CornerRadius(
            topLeft:     inner.TopLeft     + Math.Max(bt.Left,  bt.Top),
            topRight:    inner.TopRight    + Math.Max(bt.Right, bt.Top),
            bottomRight: inner.BottomRight + Math.Max(bt.Right, bt.Bottom),
            bottomLeft:  inner.BottomLeft  + Math.Max(bt.Left,  bt.Bottom));

    /// <summary>
    /// Builds a closed border-ring <see cref="StreamGeometry"/> (filled contour) that
    /// represents the area between <paramref name="outerRect"/> and <paramref name="innerRect"/>.
    /// The outer boundary is traced clockwise (winding +1) and the inner boundary
    /// counter-clockwise (winding −1).  With the default NonZero fill rule the inner
    /// area cancels out, leaving only the ring filled.
    /// </summary>
    public static StreamGeometry BuildClosedBorderRing(
        Rect outerRect, Rect innerRect,
        CornerRadius outerCr, CornerRadius innerCr)
    {
        var geometry = new StreamGeometry();
        using var ctx = geometry.Open();
        AddRoundedRectFigure(ctx, outerRect, outerCr);
        AddRoundedRectFigureCcw(ctx, innerRect, innerCr);

        return geometry;
    }

    /// <summary>
    /// Builds a filled border-ring <see cref="StreamGeometry"/> with a gap in the top edge
    /// for the header control.  The ring is a single closed figure: the outer boundary is
    /// traced clockwise, the inner boundary counter-clockwise, and the two are joined at the
    /// gap ends by semicircular end-caps that curve into the gap.
    /// <para>
    /// When the gap collapses after clamping the ring falls back to
    /// <see cref="BuildClosedBorderRing"/>.
    /// </para>
    /// </summary>
    public static StreamGeometry BuildGapBorderRing(
        Rect outerRect, Rect innerRect,
        CornerRadius outerCr, CornerRadius innerCr,
        double gapStart, double gapEnd,
        double topThickness)
    {
        double xo  = outerRect.X;
        double yo  = outerRect.Y;
        double rxo = outerRect.Right;
        double bo  = outerRect.Bottom;
        double tlo = Math.Max(0, outerCr.TopLeft);
        double tro = Math.Max(0, outerCr.TopRight);
        double bro = Math.Max(0, outerCr.BottomRight);
        double blo = Math.Max(0, outerCr.BottomLeft);

        double xi  = innerRect.X;
        double yi  = innerRect.Y;
        double rxi = innerRect.Right;
        double bi  = innerRect.Bottom;
        double tli = Math.Max(0, innerCr.TopLeft);
        double tri = Math.Max(0, innerCr.TopRight);
        double bri = Math.Max(0, innerCr.BottomRight);
        double bli = Math.Max(0, innerCr.BottomLeft);

        // Clamp the gap to the straight portions of both the outer and inner top edges.
        double cgStart = Math.Max(Math.Max(xo + tlo, xi + tli), gapStart);
        double cgEnd   = Math.Min(Math.Min(rxo - tro, rxi - tri), gapEnd);

        // If the gap collapses after clamping, fall back to a plain closed ring.
        if (cgEnd <= cgStart)
            return BuildClosedBorderRing(outerRect, innerRect, outerCr, innerCr);

        // Radius of the semicircular end-cap that joins the outer and inner top edges.
        double capR = topThickness / 2.0;

        var geometry = new StreamGeometry();
        using var ctx = geometry.Open();
        // Start at the outer top edge at the right end of the gap.
        ctx.BeginFigure(new Point(cgEnd, yo), isFilled: true);

        // ── Outer boundary (clockwise) ────────────────────────────────────
        ctx.LineTo(new Point(rxo - tro, yo));
        if (tro > 0)
            ctx.ArcTo(new Point(rxo, yo + tro), new Size(tro, tro), 0, false, SweepDirection.Clockwise);

        ctx.LineTo(new Point(rxo, bo - bro));
        if (bro > 0)
            ctx.ArcTo(new Point(rxo - bro, bo), new Size(bro, bro), 0, false, SweepDirection.Clockwise);

        ctx.LineTo(new Point(xo + blo, bo));
        if (blo > 0)
            ctx.ArcTo(new Point(xo, bo - blo), new Size(blo, blo), 0, false, SweepDirection.Clockwise);

        ctx.LineTo(new Point(xo, yo + tlo));
        if (tlo > 0)
            ctx.ArcTo(new Point(xo + tlo, yo), new Size(tlo, tlo), 0, false, SweepDirection.Clockwise);

        ctx.LineTo(new Point(cgStart, yo));

        // ── Left end-cap: semicircle curving into the gap (clockwise = rightward) ──
        if (capR > 0)
            ctx.ArcTo(new Point(cgStart, yi), new Size(capR, capR), 0, false, SweepDirection.Clockwise);
        else
            ctx.LineTo(new Point(cgStart, yi));

        // ── Inner boundary (counter-clockwise) ───────────────────────────
        // Direction: left along inner top → down left edge → right bottom → up right edge → left to gap end.
        ctx.LineTo(new Point(xi + tli, yi));
        if (tli > 0)
            ctx.ArcTo(new Point(xi, yi + tli), new Size(tli, tli), 0, false, SweepDirection.CounterClockwise);

        ctx.LineTo(new Point(xi, bi - bli));
        if (bli > 0)
            ctx.ArcTo(new Point(xi + bli, bi), new Size(bli, bli), 0, false, SweepDirection.CounterClockwise);

        ctx.LineTo(new Point(rxi - bri, bi));
        if (bri > 0)
            ctx.ArcTo(new Point(rxi, bi - bri), new Size(bri, bri), 0, false, SweepDirection.CounterClockwise);

        ctx.LineTo(new Point(rxi, yi + tri));
        if (tri > 0)
            ctx.ArcTo(new Point(rxi - tri, yi), new Size(tri, tri), 0, false, SweepDirection.CounterClockwise);

        ctx.LineTo(new Point(cgEnd, yi));

        // ── Right end-cap: semicircle curving into the gap (CCW = leftward when going up) ──
        if (capR > 0)
            ctx.ArcTo(new Point(cgEnd, yo), new Size(capR, capR), 0, false, SweepDirection.Clockwise);
        else
            ctx.LineTo(new Point(cgEnd, yo));

        ctx.EndFigure(isClosed: true);

        return geometry;
    }

    /// <summary>
    /// Appends a closed clockwise rounded-rectangle figure to an existing
    /// <see cref="StreamGeometryContext"/>.  Used by <see cref="BuildClosedBorderRing"/>.
    /// </summary>
    public static void AddRoundedRectFigure(StreamGeometryContext ctx, Rect rect, CornerRadius cr)
    {
        double x  = rect.X;
        double y  = rect.Y;
        double rx = rect.Right;
        double b  = rect.Bottom;
        double tl = Math.Max(0, cr.TopLeft);
        double tr = Math.Max(0, cr.TopRight);
        double br = Math.Max(0, cr.BottomRight);
        double bl = Math.Max(0, cr.BottomLeft);

        ctx.BeginFigure(new Point(x + tl, y), isFilled: true);

        ctx.LineTo(new Point(rx - tr, y));
        if (tr > 0)
            ctx.ArcTo(new Point(rx, y + tr), new Size(tr, tr), 0, false, SweepDirection.Clockwise);

        ctx.LineTo(new Point(rx, b - br));
        if (br > 0)
            ctx.ArcTo(new Point(rx - br, b), new Size(br, br), 0, false, SweepDirection.Clockwise);

        ctx.LineTo(new Point(x + bl, b));
        if (bl > 0)
            ctx.ArcTo(new Point(x, b - bl), new Size(bl, bl), 0, false, SweepDirection.Clockwise);

        ctx.LineTo(new Point(x, y + tl));
        if (tl > 0)
            ctx.ArcTo(new Point(x + tl, y), new Size(tl, tl), 0, false, SweepDirection.Clockwise);

        ctx.EndFigure(isClosed: true);
    }

    /// <summary>
    /// Appends a closed <em>counter-clockwise</em> rounded-rectangle figure to an existing
    /// <see cref="StreamGeometryContext"/>.  Used by <see cref="BuildClosedBorderRing"/> as
    /// the inner (punch-out) figure.  With the NonZero fill rule the inner region gets
    /// net winding 0 and is therefore not filled.
    /// </summary>
    public static void AddRoundedRectFigureCcw(StreamGeometryContext ctx, Rect rect, CornerRadius cr)
    {
        double x  = rect.X;
        double y  = rect.Y;
        double rx = rect.Right;
        double b  = rect.Bottom;
        double tl = Math.Max(0, cr.TopLeft);
        double tr = Math.Max(0, cr.TopRight);
        double br = Math.Max(0, cr.BottomRight);
        double bl = Math.Max(0, cr.BottomLeft);

        // CCW traversal: start on the left edge just below the top-left corner,
        // then go DOWN → BL → RIGHT → BR → UP → TR → LEFT → TL → close.
        ctx.BeginFigure(new Point(x, y + tl), isFilled: true);

        ctx.LineTo(new Point(x, b - bl));
        if (bl > 0)
            ctx.ArcTo(new Point(x + bl, b), new Size(bl, bl), 0, false, SweepDirection.CounterClockwise);

        ctx.LineTo(new Point(rx - br, b));
        if (br > 0)
            ctx.ArcTo(new Point(rx, b - br), new Size(br, br), 0, false, SweepDirection.CounterClockwise);

        ctx.LineTo(new Point(rx, y + tr));
        if (tr > 0)
            ctx.ArcTo(new Point(rx - tr, y), new Size(tr, tr), 0, false, SweepDirection.CounterClockwise);

        ctx.LineTo(new Point(x + tl, y));
        if (tl > 0)
            ctx.ArcTo(new Point(x, y + tl), new Size(tl, tl), 0, false, SweepDirection.CounterClockwise);

        ctx.EndFigure(isClosed: true);
    }

    /// <summary>
    /// Builds a closed (or open) <see cref="StreamGeometry"/> for a rounded rectangle.
    /// When <paramref name="closeAtStart"/> is <see langword="true"/> the figure is closed,
    /// producing a fillable shape.
    /// </summary>
    public static StreamGeometry BuildRoundedRectGeometry(
        Rect rect, CornerRadius cr, bool closeAtStart)
    {
        double x  = rect.X;
        double y  = rect.Y;
        double rx = rect.Right;
        double b  = rect.Bottom;
        double tl = Math.Max(0, cr.TopLeft);
        double tr = Math.Max(0, cr.TopRight);
        double br = Math.Max(0, cr.BottomRight);
        double bl = Math.Max(0, cr.BottomLeft);

        var geometry = new StreamGeometry();
        using var ctx = geometry.Open();
        ctx.BeginFigure(new Point(x + tl, y), isFilled: closeAtStart);

        // Top edge
        ctx.LineTo(new Point(rx - tr, y));
        if (tr > 0)
            ctx.ArcTo(new Point(rx, y + tr), new Size(tr, tr), 0, false, SweepDirection.Clockwise);

        // Right edge
        ctx.LineTo(new Point(rx, b - br));
        if (br > 0)
            ctx.ArcTo(new Point(rx - br, b), new Size(br, br), 0, false, SweepDirection.Clockwise);

        // Bottom edge
        ctx.LineTo(new Point(x + bl, b));
        if (bl > 0)
            ctx.ArcTo(new Point(x, b - bl), new Size(bl, bl), 0, false, SweepDirection.Clockwise);

        // Left edge
        ctx.LineTo(new Point(x, y + tl));
        if (tl > 0)
            ctx.ArcTo(new Point(x + tl, y), new Size(tl, tl), 0, false, SweepDirection.Clockwise);

        ctx.EndFigure(isClosed: closeAtStart);

        return geometry;
    }
}

