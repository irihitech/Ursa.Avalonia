using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Ursa.Controls;

public class GroupBoxBorder : Decorator
{
    // -------------------------------------------------------------------------
    // Styled properties
    // -------------------------------------------------------------------------

    /// <summary>Defines the <see cref="Background"/> property.</summary>
    public static readonly StyledProperty<IBrush?> BackgroundProperty =
        Border.BackgroundProperty.AddOwner<GroupBoxBorder>();

    /// <summary>Defines the <see cref="BorderBrush"/> property.</summary>
    public static readonly StyledProperty<IBrush?> BorderBrushProperty =
        Border.BorderBrushProperty.AddOwner<GroupBoxBorder>();

    /// <summary>Defines the <see cref="BorderThickness"/> property.</summary>
    public static readonly StyledProperty<Thickness> BorderThicknessProperty =
        Border.BorderThicknessProperty.AddOwner<GroupBoxBorder>();

    /// <summary>Defines the <see cref="CornerRadius"/> property.</summary>
    public static readonly StyledProperty<CornerRadius> CornerRadiusProperty =
        Border.CornerRadiusProperty.AddOwner<GroupBoxBorder>();

    /// <summary>Defines the <see cref="Header"/> property.</summary>
    public static readonly StyledProperty<Control?> HeaderProperty =
        AvaloniaProperty.Register<GroupBoxBorder, Control?>(nameof(Header));

    /// <summary>Defines the <see cref="HeaderSpacing"/> property.</summary>
    public static readonly StyledProperty<double> HeaderSpacingProperty =
        AvaloniaProperty.Register<GroupBoxBorder, double>(nameof(HeaderSpacing), 4.0);

    // -------------------------------------------------------------------------
    // Static constructor — wire up invalidation
    // -------------------------------------------------------------------------
    static GroupBoxBorder()
    {
        AffectsRender<GroupBoxBorder>(BackgroundProperty, BorderBrushProperty);
        AffectsMeasure<GroupBoxBorder>(BorderThicknessProperty, PaddingProperty, HeaderSpacingProperty,
            CornerRadiusProperty);

    }

    // -------------------------------------------------------------------------
    // CLR property wrappers
    // -------------------------------------------------------------------------

    /// <summary>Gets or sets the background brush painted inside the border.</summary>
    public IBrush? Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    /// <summary>Gets or sets the brush used to draw the border stroke.</summary>
    public IBrush? BorderBrush
    {
        get => GetValue(BorderBrushProperty);
        set => SetValue(BorderBrushProperty, value);
    }

    /// <summary>Gets or sets the thickness of the border stroke.</summary>
    public Thickness BorderThickness
    {
        get => GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    /// <summary>Gets or sets the corner radius of the border.</summary>
    public CornerRadius CornerRadius
    {
        get => GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the control displayed in the cut-out gap at the top of the border.
    /// When <see langword="null"/> a closed rounded-rectangle border is drawn.
    /// </summary>
    public Control? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the extra space (in device-independent pixels) added on each side of
    /// the <see cref="Header"/> control within the border gap.  Defaults to 4.
    /// </summary>
    public double HeaderSpacing
    {
        get => GetValue(HeaderSpacingProperty);
        set => SetValue(HeaderSpacingProperty, value);
    }

    // -------------------------------------------------------------------------
    // Cached geometries (built once in ArrangeOverride, reused in Render)
    // -------------------------------------------------------------------------

    /// <summary>Closed rounded-rect fill shape. Used for <see cref="Background"/> fill.</summary>
    private StreamGeometry? _fillGeometry;

    /// <summary>
    /// Border stroke path — open-gap path when a header is present, or the same object as
    /// <see cref="_fillGeometry"/> when there is no header (closed path doubles as stroke).
    /// Built once in <see cref="ArrangeOverride"/>; reused in every <see cref="Render"/> call
    /// so that brush-only invalidations do not regenerate geometry.
    /// </summary>
    private StreamGeometry? _borderGeometry;

    /// <summary>
    /// Closed rounded-rect in the <em>child's</em> local coordinate space.
    /// Applied to <see cref="Decorator.Child"/><c>.Clip</c> after each arrange pass so child
    /// content is clipped to the rounded-corner boundary.
    /// </summary>
    private StreamGeometry? _clipGeometry;

    // -------------------------------------------------------------------------
    // Visual-child management for the Header
    // -------------------------------------------------------------------------

    /// <inheritdoc/>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == HeaderProperty)
        {
            var oldHeader = change.GetOldValue<Control?>();
            var newHeader = change.GetNewValue<Control?>();

            if (oldHeader is not null)
            {
                VisualChildren.Remove(oldHeader);
                LogicalChildren.Remove(oldHeader);
            }

            if (newHeader is not null)
            {
                VisualChildren.Add(newHeader);
                LogicalChildren.Add(newHeader);
            }

            InvalidateMeasure();
            InvalidateVisual();
        }

        // When the child is replaced, release the clip we attached so the departing
        // control does not carry a stale GroupBoxBorder clip into its next parent.
        if (change.Property == ChildProperty)
        {
            var oldChild = change.GetOldValue<Control?>();
            oldChild?.Clip = null;
        }
    }

    // -------------------------------------------------------------------------
    // Layout overrides
    // -------------------------------------------------------------------------

    /// <inheritdoc/>
    protected override Size MeasureOverride(Size availableSize)
    {
        var bt = BorderThickness;
        var padding = Padding;
        var spacing = HeaderSpacing;
        var header = Header;
        var child = Child;

        // Measure the header so we know how tall the "notch" row is.
        double headerH = 0;
        double headerW = 0;
        if (header is not null)
        {
            header.Measure(availableSize);
            headerH = header.DesiredSize.Height;
            headerW = header.DesiredSize.Width;
        }

        // When there is no header the top inset must cover the border stroke so
        // that the child starts inside the border, just like the left/right/bottom sides.
        var insetLeft = bt.Left + padding.Left;
        var insetRight = bt.Right + padding.Right;
        var insetTop = (header is not null ? headerH : bt.Top) + padding.Top;
        var insetBottom = bt.Bottom + padding.Bottom;

        double childW = 0;
        double childH = 0;
        if (child is not null)
        {
            child.Measure(new Size(
                Math.Max(0, availableSize.Width - insetLeft - insetRight),
                Math.Max(0, availableSize.Height - insetTop - insetBottom)));
            childW = child.DesiredSize.Width;
            childH = child.DesiredSize.Height;
        }

        var width = childW + insetLeft + insetRight;

        // Ensure the control is wide enough to show the header (with gap spacing).
        // The header is placed after the top-left corner arc, so the minimum width
        // must include both corner radii in addition to the border, spacing, and header.
        if (header is not null)
        {
            var cr = CornerRadius;
            var minHeaderTotalW = bt.Left + cr.TopLeft + spacing + headerW + spacing + cr.TopRight + bt.Right;
            width = Math.Max(minHeaderTotalW, width);
        }

        return new Size(width, childH + insetTop + insetBottom);
    }

    /// <inheritdoc/>
    protected override Size ArrangeOverride(Size finalSize)
    {
        var bt = BorderThickness;
        var padding = Padding;
        var spacing = HeaderSpacing;
        var header = Header;
        var child = Child;

        // Place the header so its vertical center falls on the top borderline.
        // The header starts after the top-left corner arc to avoid overlapping it.
        var cr = CornerRadius;
        double headerH = 0;
        if (header is not null)
        {
            var headerX = bt.Left + cr.TopLeft + spacing;
            headerH = header.DesiredSize.Height;
            header.Arrange(new Rect(headerX, 0, header.DesiredSize.Width, headerH));
        }

        // childTopY mirrors the insetTop logic in MeasureOverride.
        var childTopY = (header is not null ? headerH : bt.Top) + padding.Top;

        var childLeft = bt.Left + padding.Left;

        // Place the child below the header row and inside the border + padding.
        child?.Arrange(new Rect(
            childLeft,
            childTopY,
            Math.Max(0, finalSize.Width - childLeft - bt.Right - padding.Right),
            Math.Max(0, finalSize.Height - childTopY - bt.Bottom - padding.Bottom)));

        // ── Build and cache geometries ────────────────────────────────────────
        // header.Bounds is populated after Arrange above, so we can use it here
        // with the exact same logic as Render() to guarantee the geometry matches
        // the visual layout.
        double borderTopY;
        double gapStart = 0, gapEnd = 0;
        var hasGap = false;

        if (header?.Bounds is { Width: > 0, Height: > 0 })
        {
            var hb = header.Bounds;
            borderTopY = hb.Y + hb.Height / 2.0;
            gapStart = hb.Left - spacing;
            gapEnd = hb.Right + spacing;
            hasGap = true;
        }
        else
        {
            borderTopY = bt.Top / 2.0;
        }

        // The outer/inner boundaries of the border are offset from the border
        // center line by half the top thickness in each direction, so that the
        // full border ring spans from outerTopY (outer face) to innerTopY (inner face).
        var outerTopY = borderTopY - bt.Top / 2.0;
        var innerTopY = borderTopY + bt.Top / 2.0;

        var outerRect = new Rect(
            x: 0,
            y: outerTopY,
            width: finalSize.Width,
            height: Math.Max(0, finalSize.Height - outerTopY));

        var innerRect = new Rect(
            x: bt.Left,
            y: innerTopY,
            width: Math.Max(0, finalSize.Width - bt.Left - bt.Right),
            height: Math.Max(0, finalSize.Height - innerTopY - bt.Bottom));

        var outerCr = GroupBoxBorderGeometryHelper.ComputeOuterCornerRadius(cr, bt);

        _fillGeometry = GroupBoxBorderGeometryHelper.BuildRoundedRectGeometry(innerRect, cr, closeAtStart: true);
        _borderGeometry = hasGap ?
            GroupBoxBorderGeometryHelper.BuildGapBorderRing(outerRect, innerRect, outerCr, cr, gapStart, gapEnd, bt.Top) :
            GroupBoxBorderGeometryHelper.BuildClosedBorderRing(outerRect, innerRect, outerCr, cr);

        if (child is not null)
        {
            // Express the inner boundary in child-local coordinates for the clip.
            _clipGeometry = GroupBoxBorderGeometryHelper.BuildRoundedRectGeometry(
                new Rect(
                    innerRect.X - childLeft,
                    innerRect.Y - childTopY,
                    innerRect.Width,
                    innerRect.Height),
                cr, closeAtStart: true);
            child.Clip = _clipGeometry;
        }
        else
        {
            _clipGeometry = null;
        }

        return finalSize;
    }

    // -------------------------------------------------------------------------
    // Rendering
    // -------------------------------------------------------------------------

    /// <inheritdoc/>
    public override void Render(DrawingContext context)
    {
        // Geometries are null until ArrangeOverride runs, or when the control has no size.
        if (_fillGeometry is null || _borderGeometry is null)
            return;

        // Background fill.
        var background = Background;
        if (background is not null)
            context.DrawGeometry(background, null, _fillGeometry);

        // Border stroke.
        var brush = BorderBrush;
        if (brush is null)
            return;

        // Draw the border as a filled contour so each side's actual thickness is
        // honored exactly — no single stroke thickness can represent a non-uniform
        // Thickness(left, top, right, bottom).
        context.DrawGeometry(brush, null, _borderGeometry);
    }

}
