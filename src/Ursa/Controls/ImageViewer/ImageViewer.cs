using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;

namespace Ursa.Controls;

/// <summary>
/// An interactive image viewer built around a templated <see cref="ImageViewerPresenter"/>.
/// Handles drag-to-pan, mouse-wheel zoom and double-click-to-fit; passes the resulting
/// <see cref="Zoom"/>, <see cref="OffsetX"/> and <see cref="OffsetY"/> through to the
/// internal presenter via the default control template.
/// </summary>
[TemplatePart(PART_Image, typeof(ImageViewerPresenter))]
[TemplatePart(PART_Layer, typeof(VisualLayerManager))]
[PseudoClasses(PC_Moving)]
public class ImageViewer: TemplatedControl
{
    
    // ── Styled properties (mirrored on the internal ImageViewerPresenter) ────

    /// <summary>Defines the <see cref="Source"/> property.</summary>
    public static readonly StyledProperty<IImage?> SourceProperty =
        AvaloniaProperty.Register<ImageViewer, IImage?>(nameof(Source));

    /// <summary>Defines the <see cref="Stretch"/> property.</summary>
    public static readonly StyledProperty<Stretch> StretchProperty =
        AvaloniaProperty.Register<ImageViewer, Stretch>(
            nameof(Stretch), Stretch.Uniform);

    /// <summary>Defines the <see cref="StretchDirection"/> property.</summary>
    public static readonly StyledProperty<StretchDirection> StretchDirectionProperty =
        AvaloniaProperty.Register<ImageViewer, StretchDirection>(
            nameof(StretchDirection), StretchDirection.Both);

    /// <summary>Defines the <see cref="BlendMode"/> property.</summary>
    public static readonly StyledProperty<BitmapBlendingMode> BlendModeProperty =
        AvaloniaProperty.Register<ImageViewer, BitmapBlendingMode>(
            nameof(BlendMode));

    /// <summary>Defines the <see cref="Zoom"/> property.</summary>
    public static readonly StyledProperty<double> ZoomProperty =
        AvaloniaProperty.Register<ImageViewer, double>(
            nameof(Zoom), 1.0,
            coerce: CoerceZoom);

    /// <summary>Defines the <see cref="MinZoom"/> property.</summary>
    public static readonly StyledProperty<double> MinZoomProperty =
        AvaloniaProperty.Register<ImageViewer, double>(nameof(MinZoom), 0.1);

    /// <summary>Defines the <see cref="MaxZoom"/> property.</summary>
    public static readonly StyledProperty<double> MaxZoomProperty =
        AvaloniaProperty.Register<ImageViewer, double>(nameof(MaxZoom), 10.0);

    /// <summary>Defines the <see cref="OffsetX"/> property.</summary>
    public static readonly StyledProperty<double> OffsetXProperty =
        AvaloniaProperty.Register<ImageViewer, double>(nameof(OffsetX));

    /// <summary>Defines the <see cref="OffsetY"/> property.</summary>
    public static readonly StyledProperty<double> OffsetYProperty =
        AvaloniaProperty.Register<ImageViewer, double>(nameof(OffsetY));
    
    // ── CLR property wrappers ────────────────────────────────────────────────

    /// <summary>Gets or sets the image to display.</summary>
    [Content]
    public IImage? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    /// <summary>Gets or sets the stretch mode for the initial fit-to-view.</summary>
    public Stretch Stretch
    {
        get => GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    /// <summary>Gets or sets the stretch direction.</summary>
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

    /// <summary>Gets or sets the minimum allowed zoom level (default 0.1).</summary>
    public double MinZoom
    {
        get => GetValue(MinZoomProperty);
        set => SetValue(MinZoomProperty, value);
    }

    /// <summary>Gets or sets the maximum allowed zoom level (default 10.0).</summary>
    public double MaxZoom
    {
        get => GetValue(MaxZoomProperty);
        set => SetValue(MaxZoomProperty, value);
    }

    /// <summary>Gets or sets the horizontal pixel offset (positive = right).</summary>
    public double OffsetX
    {
        get => GetValue(OffsetXProperty);
        set => SetValue(OffsetXProperty, value);
    }

    /// <summary>Gets or sets the vertical pixel offset (positive = down).</summary>
    public double OffsetY
    {
        get => GetValue(OffsetYProperty);
        set => SetValue(OffsetYProperty, value);
    }

    // ── Coerce callback ──────────────────────────────────────────────────────

    private static double CoerceZoom(AvaloniaObject sender, double value)
    {
        if (sender is not ImageViewer viewer) return value;
        return Math.Clamp(value, viewer.MinZoom, viewer.MaxZoom);
    }
    
    public const string PART_Image = "PART_Image";
    public const string PART_Layer = "PART_Layer";
    public const string PC_Moving = ":moving";

    public static readonly StyledProperty<Control?> OverlayerProperty = AvaloniaProperty.Register<ImageViewer, Control?>(
        nameof(Overlayer));

    public Control? Overlayer
    {
        get => GetValue(OverlayerProperty);
        set => SetValue(OverlayerProperty, value);
    }

    [Obsolete($"Replace with {nameof(ZoomProperty)}")]
    public static readonly DirectProperty<ImageViewer, double> ScaleProperty = AvaloniaProperty.RegisterDirect<ImageViewer, double>(
        nameof(Scale), o => o.Scale, (o,v)=> o.Scale = v, unsetValue: 1);

    [Obsolete($"Replace with {nameof(Zoom)}")]
    public double Scale
    {
        get => GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, value);
    }

    [Obsolete($"Replace with {nameof(MinZoomProperty)}")]
    public static readonly DirectProperty<ImageViewer, double> MinScaleProperty = AvaloniaProperty.RegisterDirect<ImageViewer, double>(
        nameof(MinScale), o => o.MinScale, (o, v) => o.MinScale = v, unsetValue: 0.1);

    [Obsolete ($"Replace with {nameof(MinZoom)}")]
    public double MinScale
    {
        get => GetValue(MinZoomProperty);
        set => SetValue(MinZoomProperty, value);
    }

    [Obsolete($"Replace with {nameof(OffsetXProperty)}")]
    public static readonly DirectProperty<ImageViewer, double> TranslateXProperty = AvaloniaProperty.RegisterDirect<ImageViewer, double>(
        nameof(TranslateX), o => o.TranslateX, (o,v)=>o.TranslateX = v, unsetValue: 0);

    [Obsolete($"Replace with {nameof(OffsetX)}")]
    public double TranslateX
    {
        get => GetValue(OffsetXProperty);
        set => SetValue(OffsetXProperty, value);
    }

    [Obsolete( $"Replace with {nameof(OffsetYProperty)}")]
    public static readonly DirectProperty<ImageViewer, double> TranslateYProperty =
        AvaloniaProperty.RegisterDirect<ImageViewer, double>(
            nameof(TranslateY), o => o.TranslateY, (o, v) => o.TranslateY = v, unsetValue: 0);

    [Obsolete($"Replace with {nameof(OffsetY)}")]
    public double TranslateY
    {
        get => GetValue(OffsetYProperty);
        set => SetValue(OffsetYProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="SmallChange"/> property.
    /// </summary>
    public static readonly StyledProperty<double> SmallChangeProperty = AvaloniaProperty.Register<ImageViewer, double>(
        nameof(SmallChange), defaultValue: 1);
    
    /// <summary>
    /// Gets or sets the amount to pan when the arrow keys are pressed without Ctrl.
    /// </summary>
    public double SmallChange
    {
        get => GetValue(SmallChangeProperty);
        set => SetValue(SmallChangeProperty, value);
    }
    
    /// <summary>
    /// Defines the <see cref="LargeChange"/> property.
    /// </summary>
    public static readonly StyledProperty<double> LargeChangeProperty = AvaloniaProperty.Register<ImageViewer, double>(
        nameof(LargeChange), defaultValue: 10);
    
    /// <summary>
    /// Gets or sets the amount to pan when the arrow keys are pressed with Ctrl.
    /// </summary>
    public double LargeChange
    {
        get => GetValue(LargeChangeProperty);
        set => SetValue(LargeChangeProperty, value);
    }

    // ── Gesture handler ──────────────────────────────────────────────────────

    private readonly PanZoomGestureHandler _gesture = new();

    // ── Pinch state snapshot (captured when pinch starts) ────────────────────

    private double _pinchStartZoom;
    private double _pinchStartOffsetX;
    private double _pinchStartOffsetY;

    // ── Misc ─────────────────────────────────────────────────────────────────

    private bool _hasInitialFit;

    static ImageViewer()
    {
        OverlayerProperty.Changed.AddClassHandler<ImageViewer, Control?>((o, e) => o.OnOverlayerChanged(e));
    }
    
    private void OnOverlayerChanged(AvaloniaPropertyChangedEventArgs args)
    {
        var control = args.GetNewValue<Control?>();
        if (control is { } c)
        {
            AdornerLayer.SetAdorner(this, c);
        }
    }
    // ── Property changes ─────────────────────────────────────────────────────

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SourceProperty ||
            change.Property == StretchProperty ||
            change.Property == StretchDirectionProperty)
        {
            _hasInitialFit = false;
            SetCurrentValue(OffsetXProperty, 0);
            SetCurrentValue(OffsetYProperty, 0);

            if (Source is not null && Bounds is { Width: > 0, Height: > 0 })
            {
                FitToView();
                _hasInitialFit = true;
            }
        }

        if (change.Property == MinZoomProperty || change.Property == MaxZoomProperty)
        {
            CoerceValue(ZoomProperty);
        }
    }
    
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        _gesture.Pan -= OnGesturePan;
        _gesture.PinchStarted -= OnGesturePinchStarted;
        _gesture.PinchUpdated -= OnGesturePinchUpdated;
        _gesture.Complete();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        _gesture.Pan += OnGesturePan;
        _gesture.PinchStarted += OnGesturePinchStarted;
        _gesture.PinchUpdated += OnGesturePinchUpdated;
    }

    // ── Size changes ─────────────────────────────────────────────────────────

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);

        if (Source is not null && !_hasInitialFit && Bounds is { Width: > 0, Height: > 0 })
        {
            FitToView();
            _hasInitialFit = true;
        }
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

    // ── Pointer / input handling ─────────────────────────────────────────────

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        _gesture.PointerPressed(e, this);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        _gesture.PointerMoved(e, this);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        _gesture.PointerReleased(e, this);
    }

    protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e)
    {
        base.OnPointerCaptureLost(e);
        _gesture.PointerCaptureLost();
    }

    // ── Gesture event handlers ───────────────────────────────────────────────

    private void OnGesturePan(double deltaX, double deltaY)
    {
        OffsetX += deltaX;
        OffsetY += deltaY;
    }

    private void OnGesturePinchStarted()
    {
        _pinchStartZoom = Zoom;
        _pinchStartOffsetX = OffsetX;
        _pinchStartOffsetY = OffsetY;
    }

    private void OnGesturePinchUpdated(PinchUpdateEventArgs e)
    {
        // --- Pan: accumulate translation since pinch start ---
        var tempOffsetX = _pinchStartOffsetX + e.CumulativeTranslationX;
        var tempOffsetY = _pinchStartOffsetY + e.CumulativeTranslationY;

        // --- Zoom: cumulative scale since pinch start, anchored at centre ---
        var newZoom = Math.Clamp(_pinchStartZoom * e.CumulativeScale, MinZoom, MaxZoom);

        var ccx = Bounds.Width / 2;
        var ccy = Bounds.Height / 2;
        var dx = e.CenterX - ccx - tempOffsetX;
        var dy = e.CenterY - ccy - tempOffsetY;

        SetCurrentValue(ZoomProperty, newZoom);
        SetCurrentValue(OffsetXProperty, e.CenterX - ccx - dx * (newZoom / _pinchStartZoom));
        SetCurrentValue(OffsetYProperty, e.CenterY - ccy - dy * (newZoom / _pinchStartZoom));
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);

        if (Source is null) return;

        var oldZoom = Zoom;
        var zoomDelta = e.Delta.Y > 0 ? 1.1 : 1.0 / 1.1;
        var newZoom = Math.Clamp(oldZoom * zoomDelta, MinZoom, MaxZoom);

        // Keep the point under the cursor fixed on screen.
        var point = e.GetCurrentPoint(this);
        var mouseX = point.Position.X;
        var mouseY = point.Position.Y;
        var centerX = Bounds.Width / 2;
        var centerY = Bounds.Height / 2;

        var dx = mouseX - centerX - OffsetX;
        var dy = mouseY - centerY - OffsetY;

        SetCurrentValue(ZoomProperty, newZoom);
        SetCurrentValue(OffsetXProperty, mouseX - centerX - dx * (newZoom / oldZoom));
        SetCurrentValue(OffsetYProperty, mouseY - centerY - dy * (newZoom / oldZoom));

        e.Handled = true;
    }

    protected override void OnDoubleTapped(TappedEventArgs e)
    {
        base.OnDoubleTapped(e);
        FitToView();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        double step = e.KeyModifiers is KeyModifiers.Control or KeyModifiers.Meta ? LargeChange : SmallChange;
        switch (e.Key)
        {
            case Key.Left:
                OffsetX -= step;
                break;
            case Key.Right:
                OffsetX += step;
                break;
            case Key.Up:
                OffsetY -= step;
                break;
            case Key.Down:
                OffsetY += step;
                break;
        }
        base.OnKeyDown(e);
    }
}
