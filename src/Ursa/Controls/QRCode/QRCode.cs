using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Threading;
using Gma.QrCodeNet.Encoding;

namespace Ursa.Controls;

/// <summary>
/// Avalonia implementation of a Quick Response code (QR Code) with smooth borders and support for gradient brushes
/// For spec, see: https://www.swisseduc.ch/informatik/theoretische_informatik/qr_codes/docs/qr_standard.pdf
/// </summary>
public class QRCode : Control
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
    /// Property indicating the Quiet Zone (distance between the edge of the control and where the data actually starts)
    /// 
    /// Note: The Quiet Zone (aka Padding) is defined in the QC Code standard (ISO 18004) as the width of 4 modules on all
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
    /// A cache of currently set bits in the bit matrix.  This is used to potentially speed up processing.
    /// </summary>
    private readonly Hashtable _setBitsTable = new();

    /// <summary>
    /// A cache of the last encoded QRCode.  This is used to reuse the last generated data whenever a style property like Width, Height or Padding was changed.
    /// </summary>
    private Gma.QrCodeNet.Encoding.QrCode? _encodedQrCode;

    // QRCode specs mandate a standard 4-symbol-sized space on each side of the data.  We support custom Padding and will ignore this zone when processing
    private int QuietZoneCount => IsQuietZoneEnabled ? 4 : 0;
    private int QuietMargin => QuietZoneCount * 2;

    /// <summary>
    /// Defines the geometry of the previously displayed QRCode
    /// </summary>
    private (PathGeometry, double)? _oldQrCodeGeometry;

    /// <summary>
    /// Defines the geometry of the currently displayed QRCode
    /// </summary>
    private (PathGeometry, double)? _qrCodeGeometry;

    private Task? _transitionTask;

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
            lock (_setBitsTable)
                _setBitsTable.Clear();

            QrCodeGenerator.ErrorCorrectionLevel = ToQrCoderEccLevel(ErrorCorrection);
            _encodedQrCode = string.IsNullOrEmpty(Data)? null: QrCodeGenerator.Encode(Data);
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
                OnLayoutChanged(_encodedQrCode);
                InvalidateVisual();
                break;
                // This is hard coded for now as I'm sure there is a better and more "Avalonia" way to transition between renders.
                // Eventually, it may be a property of some sort.
                if (_transitionTask == null || _transitionTask.IsCompleted)
                {
                    _transitionTask = Dispatcher.UIThread.Invoke(async () =>
                    {
                        while (_qrCodeGeometry is (_, < 1))
                        {
                            if (_qrCodeGeometry is var (newGeometry, newOpacity))
                                _qrCodeGeometry = (newGeometry, Math.Min(1, newOpacity + 0.1));
                            InvalidateVisual();
                            // await Task.Delay(30);
                        }

                        _oldQrCodeGeometry = null;
                        
                    });
                }

                break;
        }
    }

    /// <summary>
    /// Raised whenever a property of the control changes that impacts the layout of the QRCode geometry
    /// </summary>
    /// <param name="qrCodeData">The QRCode Data with the underlying bit matrix</param>
    private void OnLayoutChanged(Gma.QrCodeNet.Encoding.QrCode? qrCodeData)
    {
        /*
         * The following code turns the QRCode bit matrix into a geometry path.  The path represents the SHAPE of the QRCode and
         * thus is achieved maybe unintuitively by ensuring that the background covers the whole control and then "carving" out
         * the areas where the foreground should appear.  In the case of the markers, pathing over a "carved" out area will
         * re-add the background color and, indeed, create the ring effects in the finished render.
         *
         * This logic is in place to ensure that the the whole QRCode is contained in one "Geometry" object and will thus be
         * rendered with one brush to support a gradient across the whole control if so desired.
         */

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

        // QR Code Shape
        var geometry = new PathGeometry();

        // The entire area is drawn here as the idea is to cover the control with the background brush and "carve" out the data showing the foreground
        geometry.Figures!.Add(new PathFigure
        {
            Segments = new PathSegments
            {
                new LineSegment { Point = bounds.BottomLeft },
                new LineSegment { Point = bounds.BottomRight },
                new LineSegment { Point = bounds.TopRight }
                // No need to have the additional line segment back to 0,0 as PathFigures are closed (IsClosed) by default and this segment will be assumed
            }
        });

        // Adds the three Position Detection Pattern
        AddPositionDetectionPattern(geometry, bounds, symbolSize);

        for (var row = 0; row < matrix.Height; row++)
        {
            ProcessRow(geometry, matrix, row, symbolSize);
        }

        _oldQrCodeGeometry = _qrCodeGeometry;
        _qrCodeGeometry = (geometry, 1); // start at 0% opacity
    }

    /// <summary>
    /// Processes a full row of the the bit matrix and adds geometry as needed
    /// </summary>
    /// <param name="geometry">Geometry of the QR Code</param>
    /// <param name="bitMatrix">The bit matrix being processed</param>
    /// <param name="row">The row to process</param>
    /// <param name="symbolSize">The calculated size of each symbol</param>
    private void ProcessRow(PathGeometry geometry, BitMatrix bitMatrix, int row, Size symbolSize)
    {
        // Loop through each item within the row
        for (var column = 0; column < bitMatrix.Width; column++)
        {
            ProcessSymbol(geometry, bitMatrix, row, column, symbolSize);
        }
    }

    /// <summary>
    /// Processes a symbol and adds geometry as needed
    /// </summary>
    /// <param name="geometry">Geometry of the QR Code</param>
    /// <param name="bitMatrix">The bit matrix being processed</param>
    /// <param name="row">The row to process</param>
    /// <param name="column">The column of the symbol being processed</param>
    /// <param name="symbolSize">The calculated size of each symbol</param>
    private void ProcessSymbol(PathGeometry geometry, BitMatrix bitMatrix, int row, int column, Size symbolSize)
    {
        // The full bounds of the symbol
        var symbolBounds = new Rect(
            (column + QuietZoneCount) * symbolSize.Width + Padding.Left,
            (row + QuietZoneCount) * symbolSize.Height + Padding.Top,
            symbolSize.Width,
            symbolSize.Height
        );

        if (ProcessSymbolIfSet(geometry, bitMatrix, row, column, symbolBounds))
            return;

        ProcessSymbolIfUnset(geometry, bitMatrix, row, column, symbolBounds);
    }

    /// <summary>
    /// Processes a symbol if set and adds the required geometry.
    /// </summary>
    /// <param name="geometry">Geometry containing the QRCode Geometry</param>
    /// <param name="bitMatrix">BitMatrix containing the data</param>
    /// <param name="row">The row of the symbol being processed</param>
    /// <param name="column">The column of the symbol being processed</param>
    /// <param name="symbolBounds">The bounds of the symbol being processed</param>
    /// <returns>True if the symbol was processed, otherwise false</returns>
    private bool ProcessSymbolIfSet(PathGeometry geometry, BitMatrix bitMatrix, int row, int column, Rect symbolBounds)
    {
        // If not filled, no action required
        if (!IsValid(bitMatrix, column, row))
            return false;

        var boundsRadius = symbolBounds.Size / 2;
        var cornerFlags = GetSetSymbolCornerFlags(bitMatrix, row, column);
        var figure = new PathFigure
            { StartPoint = new Point(symbolBounds.Left, symbolBounds.Top + boundsRadius.Height) };

        // Top Left
        if ((cornerFlags & CornerFlags.TopLeft) != 0)
        {
            figure.Segments!.Add(new LineSegment { Point = symbolBounds.TopLeft });
            figure.Segments!.Add(new LineSegment
                { Point = new Point(symbolBounds.Left + boundsRadius.Width, symbolBounds.Top) });
        }
        else
        {
            figure.Segments!.Add(new ArcSegment
            {
                SweepDirection = SweepDirection.Clockwise,
                Point = new Point(symbolBounds.Left + boundsRadius.Width, symbolBounds.Top),
                Size = boundsRadius
            });
        }

        // Top Right
        if ((cornerFlags & CornerFlags.TopRight) != 0)
        {
            figure.Segments!.Add(new LineSegment { Point = symbolBounds.TopRight });
            figure.Segments!.Add(new LineSegment
                { Point = new Point(symbolBounds.Right, symbolBounds.Top + boundsRadius.Height) });
        }
        else
        {
            figure.Segments!.Add(new ArcSegment
            {
                SweepDirection = SweepDirection.Clockwise,
                Point = new Point(symbolBounds.Right, symbolBounds.Top + boundsRadius.Height),
                Size = boundsRadius
            });
        }

        // Bottom Right
        if ((cornerFlags & CornerFlags.BottomRight) != 0)
        {
            figure.Segments!.Add(new LineSegment { Point = symbolBounds.BottomRight });
            figure.Segments!.Add(new LineSegment
                { Point = new Point(symbolBounds.Right - boundsRadius.Width, symbolBounds.Bottom) });
        }
        else
        {
            figure.Segments!.Add(new ArcSegment
            {
                SweepDirection = SweepDirection.Clockwise,
                Point = new Point(symbolBounds.Right - boundsRadius.Width, symbolBounds.Bottom),
                Size = boundsRadius
            });
        }

        // Bottom Left
        if ((cornerFlags & CornerFlags.BottomLeft) != 0)
        {
            figure.Segments!.Add(new LineSegment { Point = symbolBounds.BottomLeft });
            figure.Segments!.Add(new LineSegment { Point = figure.StartPoint });
        }
        else
        {
            figure.Segments!.Add(new ArcSegment
            {
                SweepDirection = SweepDirection.Clockwise,
                Point = figure.StartPoint,
                Size = boundsRadius
            });
        }

        geometry.Figures?.Add(figure);
        return true;
    }

    /// <summary>
    /// Gets the corner flags indicating how a set symbol is to be processed
    /// </summary>
    /// <param name="bitMatrix">BitMatrix containing the data</param>
    /// <param name="row">The row of the symbol being processed</param>
    /// <param name="column">The column of the symbol being processed</param>
    /// <returns>The corner flags for a set symbol</returns>
    private CornerFlags GetSetSymbolCornerFlags(BitMatrix bitMatrix, int row, int column)
    {
        var flags = CornerFlags.None;

        if (!IsValid(bitMatrix, column, row))
            return flags;

        if (IsValid(bitMatrix, column, row - 1) || IsValid(bitMatrix, column - 1, row))
            flags |= CornerFlags.TopLeft;
        if (IsValid(bitMatrix, column, row - 1) || IsValid(bitMatrix, column + 1, row))
            flags |= CornerFlags.TopRight;
        if (IsValid(bitMatrix, column, row + 1) || IsValid(bitMatrix, column + 1, row))
            flags |= CornerFlags.BottomRight;
        if (IsValid(bitMatrix, column, row + 1) || IsValid(bitMatrix, column - 1, row))
            flags |= CornerFlags.BottomLeft;

        return flags;
    }

    /// <summary>
    /// Processes a symbol if unset and adds the required geometry.
    /// </summary>
    /// <param name="geometry">Geometry containing the QRCode Geometry</param>
    /// <param name="bitMatrix">BitMatrix containing the data</param>
    /// <param name="row">The row of the symbol being processed</param>
    /// <param name="column">The column of the symbol being processed</param>
    /// <param name="symbolBounds">The bounds of the symbol being processed</param>
    private void ProcessSymbolIfUnset(PathGeometry geometry, BitMatrix bitMatrix, int row, int column,
        Rect symbolBounds)
    {
        // If filled, no action required
        if (IsValid(bitMatrix, column, row))
            return;

        var cornerFlags = GetUnsetSymbolCornerFlags(bitMatrix, row, column);

        // If there are no nearby bits set, there's no need to smooth corners
        if (cornerFlags == CornerFlags.None)
            return;

        var boundsRadius = symbolBounds.Size / 2;

        // Top Left
        if ((cornerFlags & CornerFlags.TopLeft) != 0)
        {
            var start = new Point(symbolBounds.Left, symbolBounds.Top + boundsRadius.Height);

            geometry.Figures!.Add(new PathFigure
            {
                StartPoint = start,
                Segments = new PathSegments
                {
                    new LineSegment { Point = symbolBounds.TopLeft },
                    new LineSegment { Point = new Point(symbolBounds.Left + boundsRadius.Width, symbolBounds.Top) },
                    new ArcSegment
                    {
                        SweepDirection = SweepDirection.CounterClockwise,
                        Point = start,
                        Size = boundsRadius
                    }
                }
            });
        }

        // Top Right
        if ((cornerFlags & CornerFlags.TopRight) != 0)
        {
            var start = new Point(symbolBounds.Right - boundsRadius.Width, symbolBounds.Top);

            geometry.Figures!.Add(new PathFigure
            {
                StartPoint = start,
                Segments = new PathSegments
                {
                    new LineSegment { Point = symbolBounds.TopRight },
                    new LineSegment { Point = new Point(symbolBounds.Right, symbolBounds.Top + boundsRadius.Height) },
                    new ArcSegment
                    {
                        SweepDirection = SweepDirection.CounterClockwise,
                        Point = start,
                        Size = boundsRadius
                    }
                }
            });
        }

        // Bottom Right
        if ((cornerFlags & CornerFlags.BottomRight) != 0)
        {
            var start = new Point(symbolBounds.Right, symbolBounds.Bottom - boundsRadius.Height);

            geometry.Figures!.Add(new PathFigure
            {
                StartPoint = start,
                Segments = new PathSegments
                {
                    new LineSegment { Point = symbolBounds.BottomRight },
                    new LineSegment { Point = new Point(symbolBounds.Right - boundsRadius.Width, symbolBounds.Bottom) },
                    new ArcSegment
                    {
                        SweepDirection = SweepDirection.CounterClockwise,
                        Point = start,
                        Size = boundsRadius
                    }
                }
            });
        }

        // Bottom Left
        if ((cornerFlags & CornerFlags.BottomLeft) != 0)
        {
            var start = new Point(symbolBounds.Left + boundsRadius.Width, symbolBounds.Bottom);

            geometry.Figures!.Add(new PathFigure
            {
                StartPoint = start,
                Segments = new PathSegments
                {
                    new LineSegment { Point = symbolBounds.BottomLeft },
                    new LineSegment { Point = new Point(symbolBounds.Left, symbolBounds.Bottom - boundsRadius.Height) },
                    new ArcSegment
                    {
                        SweepDirection = SweepDirection.CounterClockwise,
                        Point = start,
                        Size = boundsRadius
                    }
                }
            });
        }
    }

    /// <summary>
    /// Gets the corner flags indicating how an unset symbol is to be processed
    /// </summary>
    /// <param name="bitMatrix">BitMatrix containing the data</param>
    /// <param name="row">The row of the symbol being processed</param>
    /// <param name="column">The column of the symbol being processed</param>
    /// <returns>The corner flags for an unset symbol</returns>
    private CornerFlags GetUnsetSymbolCornerFlags(BitMatrix bitMatrix, int row, int column)
    {
        var flags = CornerFlags.None;

        if (IsValid(bitMatrix, column, row))
            return flags;

        if (IsValid(bitMatrix, column, row - 1) && IsValid(bitMatrix, column - 1, row - 1) &&
            IsValid(bitMatrix, column - 1, row))
            flags |= CornerFlags.TopLeft;
        if (IsValid(bitMatrix, column, row - 1) && IsValid(bitMatrix, column + 1, row - 1) &&
            IsValid(bitMatrix, column + 1, row))
            flags |= CornerFlags.TopRight;
        if (IsValid(bitMatrix, column, row + 1) && IsValid(bitMatrix, column + 1, row + 1) &&
            IsValid(bitMatrix, column + 1, row))
            flags |= CornerFlags.BottomRight;
        if (IsValid(bitMatrix, column, row + 1) && IsValid(bitMatrix, column - 1, row + 1) &&
            IsValid(bitMatrix, column - 1, row))
            flags |= CornerFlags.BottomLeft;

        return flags;
    }

    /// <summary>
    /// Returns whether or not the specified symbol should be considered "set"
    /// </summary>
    /// <param name="bitMatrix">BitMatrix containing the data</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool IsValid(BitMatrix bitMatrix, int x, int y)
    {
        // Validate bounds of the bit matrix
        if (x < 0 || y < 0 || x >= bitMatrix.Width || y >= bitMatrix.Height)
            return false;

        var key = (x, y).GetHashCode();

        lock (_setBitsTable)
        {
            if (_setBitsTable.ContainsKey(key))
                return (bool)_setBitsTable[key]!;

            // Top Left Marker
            if (x < 8 && y < 8)
                return (bool)(_setBitsTable[key] = false);
            // Top Right Marker
            if (x > bitMatrix.Width - 9 && y < 8)
                return (bool)(_setBitsTable[key] = false);
            // Bottom Left Marker
            if (x < 8 && y > bitMatrix.Height - 9)
                return (bool)(_setBitsTable[key] = false);

            /*
             * ToDo: You can add additional logic here to exclude an additional portion of data.
             *  This is not supported in the example as careful consideration must be made to ensure
             *  that the QRCode is still readable based on the ECC Level selected.  Additionally,
             *  you may want to accept a path to render a logo in the center to make it fit with the
             *  current design.
             */

            return (bool)(_setBitsTable[key] = bitMatrix[y, x]);
        }
    }

    /// <summary>
    /// Adds the Position Detection Patterns (the three markers
    /// </summary>
    /// <param name="geometry">Geometry containing the QRCode Geometry</param>
    /// <param name="bounds">Bounds of the control itself</param>
    /// <param name="symbolSize">The size of each symbol</param>
    private void AddPositionDetectionPattern(PathGeometry geometry, Rect bounds, Size symbolSize)
    {
        // Pre-calculations to reduce the amount of repeat math
        var dataBounds = bounds
                         .Deflate(Padding)
                         .Deflate(new Thickness(symbolSize.Width * QuietZoneCount, symbolSize.Height * QuietZoneCount));
        var markerSize = symbolSize * 7;
        var markerRadiusSize = markerSize / 2;
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
            var markerPosition = new Point(
                i == 1 ? dataBounds.Right - markerSize.Width : dataBounds.Left,
                i == 2 ? dataBounds.Bottom - markerRadiusSize.Height : dataBounds.Top + markerRadiusSize.Height
            );

            // Starting position of the circles.  These are adjusted each loop to make them smaller and smaller
            var startPoint = markerPosition;
            var endPoint = startPoint.WithX(startPoint.X + markerSize.Width);
            var arcSize = markerRadiusSize;

            // Three "rings" per marker
            for (var x = 0; x < 3; x++)
            {
                geometry.Figures!.Add(new PathFigure
                {
                    StartPoint = startPoint,
                    Segments = new PathSegments
                    {
                        new ArcSegment { Size = arcSize, Point = endPoint },
                        new ArcSegment { Size = arcSize, Point = startPoint }
                    }
                });

                // Adjusts the "rings" to make them progressively smaller with each loop
                startPoint = startPoint.WithX(startPoint.X + symbolSize.Width);
                endPoint = endPoint.WithX(endPoint.X - symbolSize.Width);
                arcSize -= twiceSymbolSize;
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
        context.PushClip(new RoundedRect(bounds, CornerRadius.TopLeft, CornerRadius.TopRight, CornerRadius.BottomRight,
            CornerRadius.BottomLeft));

        if (_oldQrCodeGeometry is var (oldGeometry, _))
        {
            // The foreground will show through as the qr code will be "cut out" of the background
            context.DrawRectangle(Foreground, null, bounds);
            // Render background over the foreground as the geometry has "cut outs" that allow the foreground to show through
            context.DrawGeometry(Background, null, oldGeometry);
        }

        if (_qrCodeGeometry is var (newGeometry, newOpacity))
        {
            using var _ = context.PushOpacity(newOpacity);

            // The foreground will show through as the qr code will be "cut out" of the background
            context.DrawRectangle(Foreground, null, bounds);
            // Render background over the foreground as the geometry has "cut outs" that allow the foreground to show through
            context.DrawGeometry(Background, null, newGeometry);
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

/// <summary>
/// Indicates the level of error correction available in case of data loss or corruption.  The higher the correction level, the more data will be included in the QRCode
/// </summary>
public enum EccLevel
{
    /// <summary>
    /// The lowest level of error correction where up to ~7% of data can be be recovered if lost and uses the least amount of symbols to represent the data
    /// </summary>
    Lowest,

    /// <summary>
    /// The standard level of error correction where up to ~15% of data can be be recovered if lost and represents a good compromise between a small size and reliability
    /// </summary>
    Medium,

    /// <summary>
    /// A high readability level of error correction where up to ~25% of data can be be recovered if lost but requires a larger footprint to represent the data
    /// </summary>
    Quality,

    /// <summary>
    /// The maximum level of error correction where up to ~30% of data can be be recovered if lost and represents the maximum achievable reliability
    /// </summary>
    Highest,
}
