using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;

namespace Ursa.Controls;

/// <summary>
/// A lightweight control that renders an <see cref="IImage"/> at a given
/// <see cref="Zoom"/> level and pixel <see cref="OffsetX"/>/<see cref="OffsetY"/>.
/// It does not handle any input events — use <see cref="ImageViewer"/> for
/// a full interactive viewer with pan and zoom.
/// </summary>
public class ImageViewerPresenter : Control
{
    // ── Styled properties ────────────────────────────────────────────────────

    /// <summary>Defines the <see cref="Source"/> property.</summary>
    public static readonly StyledProperty<IImage?> SourceProperty =
        AvaloniaProperty.Register<ImageViewerPresenter, IImage?>(nameof(Source));

    /// <summary>Defines the <see cref="Stretch"/> property.</summary>
    public static readonly StyledProperty<Stretch> StretchProperty =
        AvaloniaProperty.Register<ImageViewerPresenter, Stretch>(
            nameof(Stretch), Stretch.Uniform);

    /// <summary>Defines the <see cref="StretchDirection"/> property.</summary>
    public static readonly StyledProperty<StretchDirection> StretchDirectionProperty =
        AvaloniaProperty.Register<ImageViewerPresenter, StretchDirection>(
            nameof(StretchDirection), StretchDirection.Both);

    /// <summary>Defines the <see cref="BlendMode"/> property.</summary>
    public static readonly StyledProperty<BitmapBlendingMode> BlendModeProperty =
        AvaloniaProperty.Register<ImageViewerPresenter, BitmapBlendingMode>(
            nameof(BlendMode));

    /// <summary>Defines the <see cref="Zoom"/> property.</summary>
    public static readonly StyledProperty<double> ZoomProperty =
        AvaloniaProperty.Register<ImageViewerPresenter, double>(
            nameof(Zoom), 1.0,
            coerce: CoerceZoom);

    /// <summary>Defines the <see cref="MinZoom"/> property.</summary>
    public static readonly StyledProperty<double> MinZoomProperty =
        AvaloniaProperty.Register<ImageViewerPresenter, double>(nameof(MinZoom), 0.1);

    /// <summary>Defines the <see cref="MaxZoom"/> property.</summary>
    public static readonly StyledProperty<double> MaxZoomProperty =
        AvaloniaProperty.Register<ImageViewerPresenter, double>(nameof(MaxZoom), 10.0);

    /// <summary>Defines the <see cref="OffsetX"/> property.</summary>
    public static readonly StyledProperty<double> OffsetXProperty =
        AvaloniaProperty.Register<ImageViewerPresenter, double>(nameof(OffsetX));

    /// <summary>Defines the <see cref="OffsetY"/> property.</summary>
    public static readonly StyledProperty<double> OffsetYProperty =
        AvaloniaProperty.Register<ImageViewerPresenter, double>(nameof(OffsetY));

    // ── Static constructor ───────────────────────────────────────────────────

    static ImageViewerPresenter()
    {
        AffectsRender<ImageViewerPresenter>(
            SourceProperty,
            StretchProperty,
            StretchDirectionProperty,
            BlendModeProperty,
            ZoomProperty,
            OffsetXProperty,
            OffsetYProperty);

        AffectsMeasure<ImageViewerPresenter>(
            SourceProperty,
            StretchProperty,
            StretchDirectionProperty);

        ClipToBoundsProperty.OverrideDefaultValue<ImageViewerPresenter>(true);
    }

    // ── Bypass flow direction ────────────────────────────────────────────────

    /// <inheritdoc />
    protected override bool BypassFlowDirectionPolicies => true;

    // ── CLR property wrappers ────────────────────────────────────────────────

    /// <summary>Gets or sets the image to display.</summary>
    [Content]
    public IImage? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    /// <summary>
    /// Gets or sets a value controlling how the image will be stretched
    /// for the initial fit-to-view and automatic resizing.
    /// </summary>
    public Stretch Stretch
    {
        get => GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    /// <summary>
    /// Gets or sets a value controlling in what direction the image will be
    /// stretched when <see cref="Stretch"/> is applied.
    /// </summary>
    public StretchDirection StretchDirection
    {
        get => GetValue(StretchDirectionProperty);
        set => SetValue(StretchDirectionProperty, value);
    }

    /// <summary>Gets or sets the blend mode used when drawing the image.</summary>
    public BitmapBlendingMode BlendMode
    {
        get => GetValue(BlendModeProperty);
        set => SetValue(BlendModeProperty, value);
    }

    /// <summary>
    /// Gets or sets the current zoom level.  1.0 = native pixel size.
    /// Clamped between <see cref="MinZoom"/> and <see cref="MaxZoom"/>.
    /// </summary>
    public double Zoom
    {
        get => GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum allowed zoom level (default 0.1).
    /// </summary>
    public double MinZoom
    {
        get => GetValue(MinZoomProperty);
        set => SetValue(MinZoomProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum allowed zoom level (default 10.0).
    /// </summary>
    public double MaxZoom
    {
        get => GetValue(MaxZoomProperty);
        set => SetValue(MaxZoomProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal pixel offset applied after centering.
    /// Positive values shift the image to the right.
    /// </summary>
    public double OffsetX
    {
        get => GetValue(OffsetXProperty);
        set => SetValue(OffsetXProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical pixel offset applied after centering.
    /// Positive values shift the image downward.
    /// </summary>
    public double OffsetY
    {
        get => GetValue(OffsetYProperty);
        set => SetValue(OffsetYProperty, value);
    }

    // ── Coerce callback ──────────────────────────────────────────────────────

    private static double CoerceZoom(AvaloniaObject sender, double value)
    {
        if (sender is not ImageViewerPresenter viewer) return value;
        return Math.Clamp(value, viewer.MinZoom, viewer.MaxZoom);
    }

    // ── Property changes ─────────────────────────────────────────────────────

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == MinZoomProperty || change.Property == MaxZoomProperty)
        {
            CoerceValue(ZoomProperty);
        }
    }

    // ── Layout overrides ─────────────────────────────────────────────────────

    /// <inheritdoc />
    protected override Size MeasureOverride(Size availableSize)
    {
        var source = Source;
        if (source is null)
            return new Size();

        return Stretch.CalculateSize(availableSize, source.Size, StretchDirection);
    }

    /// <inheritdoc />
    protected override Size ArrangeOverride(Size finalSize)
    {
        var source = Source;
        if (source is null)
            return new Size();

        return Stretch.CalculateSize(finalSize, source.Size);
    }

    // ── Fit-to-view ──────────────────────────────────────────────────────────

    /// <summary>
    /// Resets the zoom and offsets so the image fits the control bounds
    /// according to <see cref="Stretch"/> and <see cref="StretchDirection"/>.
    /// </summary>
    public void FitToView()
    {
        if (Source is null) return;

        var imageSize = Source.Size;
        if (imageSize.Width <= 0 || imageSize.Height <= 0) return;
        if (Bounds.Width <= 0 || Bounds.Height <= 0) return;

        var scaling = Stretch.CalculateScaling(Bounds.Size, imageSize, StretchDirection);

        var fitZoom = Stretch == Stretch.Fill
            ? Math.Max(scaling.X, scaling.Y)
            : scaling.X;

        SetCurrentValue(ZoomProperty, fitZoom);
        SetCurrentValue(OffsetXProperty, 0.0);
        SetCurrentValue(OffsetYProperty, 0.0);
    }

    // ── Rendering ────────────────────────────────────────────────────────────

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        context.FillRectangle(
            new SolidColorBrush(Color.Parse("#FF2D2D2D")),
            new Rect(Bounds.Size));

        var source = Source;
        if (source is null) return;

        var sourceSize = source.Size;
        if (sourceSize.Width <= 0 || sourceSize.Height <= 0) return;
        if (Bounds.Width <= 0 || Bounds.Height <= 0) return;

        var zoom = Zoom;
        var destWidth = sourceSize.Width * zoom;
        var destHeight = sourceSize.Height * zoom;

        var centerX = Bounds.Width / 2;
        var centerY = Bounds.Height / 2;
        var destX = centerX - destWidth / 2 + OffsetX;
        var destY = centerY - destHeight / 2 + OffsetY;

        var destRect = new Rect(destX, destY, destWidth, destHeight);
        if (!destRect.Intersects(new Rect(Bounds.Size))) return;

        var sourceRect = new Rect(sourceSize);

        using (context.PushRenderOptions(new RenderOptions
               {
                   BitmapBlendingMode = BlendMode
               }))
        {
            context.DrawImage(source, sourceRect, destRect);
        }
    }
}
