using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Gma.QrCodeNet.Encoding;

namespace Ursa.Controls;

/// <summary>
/// Avalonia implementation of a Quick Response code (QR Code) with smooth borders and support for gradient brushes
/// For spec, see: https://www.swisseduc.ch/informatik/theoretische_informatik/qr_codes/docs/qr_standard.pdf
/// </summary>
public partial class QRCode : Control
{
    #region Properties

    /// <summary>
    /// Property for the Background brush (i.e. the area that has no data)
    /// </summary>
    public static readonly StyledProperty<IBrush?> BackgroundProperty = Border.BackgroundProperty.AddOwner<QRCode>();

    /// <summary>
    /// Property for the Foreground brush (i.e. the actual data)
    /// </summary>
    public static readonly StyledProperty<IBrush?> ForegroundProperty =
        TextElement.ForegroundProperty.AddOwner<TemplatedControl>();

    /// <summary>
    /// Property indicating how rounded the corners will be
    /// </summary>
    public static readonly StyledProperty<CornerRadius> CornerRadiusProperty =
        Border.CornerRadiusProperty.AddOwner<QRCode>();

    /// <summary>
    /// Property indicating the corner ratio for rounded QR code symbols.
    /// Value ranges from 0.0 (sharp corners) to 1.0 (fully rounded).
    /// Default is 0.5 (corner radius is half the symbol width).
    /// </summary>
    public static readonly StyledProperty<double> SymbolCornerRatioProperty =
        AvaloniaProperty.Register<QRCode, double>(nameof(SymbolCornerRatio), 0.5, coerce: CoerceSymbolCornerRatio);

    private static double CoerceSymbolCornerRatio(AvaloniaObject obj, double value)
    {
        return value switch
        {
            < 0.0 => 0.0,
            > 0.5 => 0.5,
            _ => value
        };
    }

    /// <summary>
    /// Property indicating the Quiet Zone (distance between the edge of the control and where the data actually starts)
    /// 
    /// Note: The Quiet Zone (aka Padding) is defined in the QR Code standard (ISO 18004) as the width of 4 modules on all
    /// sides, but is implemented separately in this control.  Official support may wish to remove this property as adjusting
    /// it will technically make the generated QRCodes "non-standard".  This implementation does not currently concern itself
    /// with this as the code itself it not meant for public consumption.
    /// </summary>
    public static readonly StyledProperty<Thickness> PaddingProperty = Decorator.PaddingProperty.AddOwner<QRCode>();

    /// <summary>
    /// Property indicating whether the Quiet Zone of 4 modules should be added to the QR Code as additional padding.  Default: True
    ///
    /// Note: Disabling the Quiet Zone makes the generated QRCodes "non-standard" according to the ISO 18004 standard.
    /// The padding created by the Quiet Zone depends on the module size and therefore on the amount of data. This can be
    /// disabled and a fixed <see cref="Padding"/> can be set instead to have more control over the layout. 
    /// </summary>
    public static readonly StyledProperty<bool> IsQuietZoneEnabledProperty =
        AvaloniaProperty.Register<QRCode, bool>(nameof(IsQuietZoneEnabled), true);

    /// <summary>
    /// Property indicating the Error Correction Code of the generated data.  Default: Medium
    ///
    /// Note: See <see cref="EccLevel" /> for the specific definitions of each value.
    /// </summary>
    public static readonly StyledProperty<EccLevel> ErrorCorrectionProperty =
        AvaloniaProperty.Register<QRCode, EccLevel>(nameof(ErrorCorrection), EccLevel.Medium);

    /// <summary>
    /// Property for the data represented in the QRCode
    /// </summary>
    public static readonly StyledProperty<string?> DataProperty =
        AvaloniaProperty.Register<QRCode, string?>(nameof(Data));

    /// <inheritdoc cref="BackgroundProperty" />
    public IBrush? Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    /// <inheritdoc cref="ForegroundProperty" />
    public IBrush? Foreground
    {
        get => GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    /// <inheritdoc cref="CornerRadiusProperty" />
    public CornerRadius CornerRadius
    {
        get => GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <inheritdoc cref="SymbolCornerRatioProperty" />
    public double SymbolCornerRatio
    {
        get => GetValue(SymbolCornerRatioProperty);
        set => SetValue(SymbolCornerRatioProperty, value);
    }

    /// <inheritdoc cref="PaddingProperty" />
    public Thickness Padding
    {
        get => GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    /// <inheritdoc cref="IsQuietZoneEnabledProperty" />
    public bool IsQuietZoneEnabled
    {
        get => GetValue(IsQuietZoneEnabledProperty);
        set => SetValue(IsQuietZoneEnabledProperty, value);
    }

    /// <inheritdoc cref="ErrorCorrectionProperty" />
    public EccLevel ErrorCorrection
    {
        get => GetValue(ErrorCorrectionProperty);
        set => SetValue(ErrorCorrectionProperty, value);
    }

    /// <inheritdoc cref="DataProperty" />
    public string? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    #endregion

    /// <summary>
    /// Engine to actually calculate the bit matrix of the QRCode.  Currently a Nuget package, but official support may wish to implement and remove such dependency 
    /// </summary>
    private static readonly QrEncoder QrCodeGenerator = new();

    /// <summary>
    /// A cache of the last encoded QRCode.  This is used to reuse the last generated data whenever a style property like Width, Height or Padding was changed.
    /// </summary>
    private QrCode? _encodedQrCode;

    // QRCode specs mandate a standard 4-symbol-sized space on each side of the data.  We support custom Padding and will ignore this zone when processing
    private int QuietZoneCount => IsQuietZoneEnabled ? 4 : 0;
    private int QuietMargin => QuietZoneCount * 2;

    /// <summary>
    /// Defines the geometry of the currently displayed QRCode
    /// </summary>
    private PathGeometry? _qrCodeGeometry;

    public QRCode()
    {
        // These properties change how the control is rendered, but not the data that's being displayed
        // See "OnPropertyChanged" for the properties that require the data to be updated.
        AffectsRender<QRCode>(BackgroundProperty, ForegroundProperty, CornerRadiusProperty, WidthProperty,
            HeightProperty);
    }

    /// <summary>
    /// Raised whenever a property on this control is changed.
    /// </summary>
    /// <param name="change">Event Args for the changed property including old and new values</param>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        // When any property is changed, we will recalculate the bit matrix and rerender the control
        // For properties that do not require the data to be reprocessed, see the constructor.

        // We can only reprocess the data when data is available to reprocess...
        if (Data == null)
            return;

        // Invalidates the cached QRCode if needed.  We do not need recreate the bit matrix for layout changes.
        switch (change.Property.Name)
        {
            // Error Correction change requires the data to be reprocessed to recalculate the new bit matrix.  This is unavoidable.
            case nameof(ErrorCorrection):
            // A change in data obviously indicates the need to update the bit matrix    
            case nameof(Data):
                _encodedQrCode = null;
                break;
        }

        // Generating the QRCode bit matrix if needed.
        if (_encodedQrCode is null)
        {
            QrCodeGenerator.ErrorCorrectionLevel = ToQrCoderEccLevel(ErrorCorrection);
            _encodedQrCode = string.IsNullOrEmpty(Data) ? null : QrCodeGenerator.Encode(Data);
        }

        switch (change.Property.Name)
        {
            // Padding and size requires the geometry paths to be adjusted to match the new locations. ToDo: Can this be simulated with a scale to enhance performance?
            case nameof(Padding):
            case nameof(Width):
            case nameof(Height):
            case nameof(IsQuietZoneEnabled):
            case nameof(ErrorCorrection):
            case nameof(Data):
            case nameof(SymbolCornerRatio):
                OnLayoutChanged(_encodedQrCode);
                InvalidateVisual();
                break;
        }
    }

    /// <summary>
    /// Raised whenever a property of the control changes that impacts the layout of the QRCode geometry
    /// </summary>
    /// <param name="qrCodeData">The QRCode Data with the underlying bit matrix</param>
    private void OnLayoutChanged(QrCode? qrCodeData)
    {
        // Bounds of the entire control
        if (qrCodeData is null)
        {
            _qrCodeGeometry = null;
            return;
        }

        var bounds = new Rect(0, 0, Width, Height);
        var matrix = qrCodeData.Matrix;
        var columnCount = matrix.Width + QuietMargin;
        var rowCount = matrix.Height + QuietMargin;

        // The size of each symbol taking into account the size of the QRCode and our custom quiet zone aka padding
        var symbolSize = new Size(
            (Width - Padding.Left - Padding.Right) / columnCount,
            (Height - Padding.Top - Padding.Bottom) / rowCount
        );
        var cornerRatio = SymbolCornerRatio;

        // QR Code Shape
        var geometry = new PathGeometry();

        // Adds the three Position Detection Pattern
        AddPositionDetectionPattern(geometry, bounds, symbolSize, cornerRatio);

        for (var row = 0; row < matrix.Height; row++)
        {
            for (int column = 0; column < matrix.Width; column++)
            {
                ProcessSymbol(geometry, matrix, row, column, symbolSize, cornerRatio);
            }
        }

        _qrCodeGeometry = geometry;
    }

    /// <summary>
    /// Processes a symbol and adds geometry as needed
    /// </summary>
    /// <param name="geometry">Geometry of the QR Code</param>
    /// <param name="bitMatrix">The bit matrix being processed</param>
    /// <param name="row">The row to process</param>
    /// <param name="column">The column of the symbol being processed</param>
    /// <param name="symbolSize">The calculated size of each symbol</param>
    /// <param name="cornerRatio"></param>
    private void ProcessSymbol(
        PathGeometry geometry,
        BitMatrix bitMatrix,
        int row,
        int column,
        Size symbolSize,
        double cornerRatio)
    {
        // The full bounds of the symbol
        var symbolBounds = new Rect(
            (column + QuietZoneCount) * symbolSize.Width + Padding.Left,
            (row + QuietZoneCount) * symbolSize.Height + Padding.Top,
            symbolSize.Width,
            symbolSize.Height
        );
        if (IsValid(bitMatrix, column, row))
        {
            ProcessSymbolIfSet(geometry, bitMatrix, row, column, symbolBounds, cornerRatio);
        }
        else
        {
            ProcessSymbolIfUnset(geometry, bitMatrix, row, column, symbolBounds, cornerRatio);
        }
    }

    /// <summary>
    /// Adds the Position Detection Patterns (the three markers
    /// </summary>
    /// <param name="geometry">Geometry containing the QRCode Geometry</param>
    /// <param name="bounds">Bounds of the control itself</param>
    /// <param name="symbolSize">The size of each symbol</param>
    /// <param name="cornerRatio"></param>
    private void AddPositionDetectionPattern(PathGeometry geometry, Rect bounds, Size symbolSize, double cornerRatio)
    {
        // Pre-calculations to reduce the amount of repeat math
        var dataBounds = bounds
                         .Deflate(Padding)
                         .Deflate(new Thickness(symbolSize.Width * QuietZoneCount, symbolSize.Height * QuietZoneCount));
        var twiceSymbolSize = symbolSize * 2;
        // Three Position Patters
        for (var i = 0; i < 3; i++)
        {
            /*
             * Determines the X/Y location of this marker:
             * 0: Top-Left
             * 1: Top-Right
             * 2: Bottom-Left
             */
            var markerSize = symbolSize * 7;
            var markerLeftTopPosition = new Point(
                i == 1 ? dataBounds.Right - markerSize.Width : dataBounds.Left,
                i == 2 ? dataBounds.Bottom - markerSize.Height : dataBounds.Top
            );
            var arcSize = markerSize * SymbolCornerRatio;
            
            // Three "rings" per marker
            for (var x = 0; x < 3; x++)
            {
                var markerBounds = new Rect(markerLeftTopPosition, markerSize);

                // Starting position of the circles.  These are adjusted each loop to make them smaller and smaller
                var startPoint = new Point(
                    markerLeftTopPosition.X,
                    markerLeftTopPosition.Y + arcSize.Height
                );
                geometry.Figures!.Add(new PathFigure
                {
                    StartPoint = startPoint,
                    Segments =
                    [
                        new ArcSegment()
                            { SweepDirection = SweepDirection.Clockwise, Size = arcSize, Point = new Point(markerBounds.Left + arcSize.Width, markerBounds.Top) },
                        new LineSegment() { Point = new Point(markerBounds.Right - arcSize.Width, markerBounds.Top) },
                        new ArcSegment()
                        {
                            SweepDirection = SweepDirection.Clockwise, Size = arcSize, Point = new Point(markerBounds.Right, markerBounds.Top + arcSize.Height)
                        },
                        new LineSegment()
                            { Point = new Point(markerBounds.Right, markerBounds.Bottom - arcSize.Height) },
                        new ArcSegment()
                        {
                            SweepDirection = SweepDirection.Clockwise, Size = arcSize, Point = new Point(markerBounds.Right - arcSize.Width, markerBounds.Bottom)
                        },
                        new LineSegment() { Point = new Point(markerBounds.Left + arcSize.Width, markerBounds.Bottom) },
                        new ArcSegment()
                        {
                            SweepDirection = SweepDirection.Clockwise, Size = arcSize, Point = new Point(markerBounds.Left, markerBounds.Bottom - arcSize.Height)
                        },
                        new LineSegment() { Point = new Point(markerBounds.Left, markerBounds.Top + arcSize.Height) },
                    ]
                });

                // Adjusts the "rings" to make them progressively smaller with each loop
                markerLeftTopPosition = new Point(markerLeftTopPosition.X + symbolSize.Width,
                    markerLeftTopPosition.Y + symbolSize.Height);
                markerSize -= twiceSymbolSize;
                arcSize = markerSize * SymbolCornerRatio;
            }
        }
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        // Render nothing when there's no data.
        // Note, when using in a scenario when you may not have data right away, you can render something over the QRCode like a spinner, etc
        if (_qrCodeGeometry == null)
            return;

        var bounds = new Rect(0, 0, Width, Height);

        // Rounded corners
        using (context.PushClip(new RoundedRect(bounds, 
                   CornerRadius.TopLeft, 
                   CornerRadius.TopRight,
                   CornerRadius.BottomRight,
                   CornerRadius.BottomLeft)))
        {
            if (_qrCodeGeometry is var newGeometry)
            {
                context.DrawRectangle(Background, null, bounds);
                context.DrawGeometry(Foreground, null, newGeometry);
            }
        }
    }

    /// <summary>
    /// Converts from our EccLevel to the one used by whichever algorithm being used.
    /// This exists as an abstraction layer for if/when the package or namespace of the actual QR Generator changes so that breaking changes are not introduced  
    /// </summary>
    /// <param name="eccLevel">The selected ECC Level to convert</param>
    /// <returns>The appropriate ECC Level type used by the generator</returns>
    /// <exception cref="ArgumentOutOfRangeException">When an unsupported ECC Level is provided</exception>
    protected static ErrorCorrectionLevel ToQrCoderEccLevel(EccLevel eccLevel)
    {
        return eccLevel switch
        {
            EccLevel.Lowest => ErrorCorrectionLevel.L,
            EccLevel.Medium => ErrorCorrectionLevel.M,
            EccLevel.Quality => ErrorCorrectionLevel.Q,
            EccLevel.Highest => ErrorCorrectionLevel.H,
            _ => throw new ArgumentOutOfRangeException(nameof(eccLevel), eccLevel, null)
        };
    }

    [Flags]
    private enum CornerFlags
    {
        None = 0,
        TopLeft = 1 << 0,
        TopRight = 1 << 1,
        BottomRight = 1 << 2,
        BottomLeft = 1 << 3
    }
}