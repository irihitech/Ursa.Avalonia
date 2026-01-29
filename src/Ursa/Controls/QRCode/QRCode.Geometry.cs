using Avalonia;
using Avalonia.Media;
using Gma.QrCodeNet.Encoding;

namespace Ursa.Controls;

public partial class QRCode
{
        /// <summary>
    /// Processes a symbol if set and adds the required geometry.
    /// </summary>
    /// <param name="geometry">Geometry containing the QRCode Geometry</param>
    /// <param name="bitMatrix">BitMatrix containing the data</param>
    /// <param name="row">The row of the symbol being processed</param>
    /// <param name="column">The column of the symbol being processed</param>
    /// <param name="symbolBounds">The bounds of the symbol being processed</param>
    /// <param name="cornerRatio"></param>
    /// <returns>True if the symbol was processed, otherwise false</returns>
    private static void ProcessSymbolIfSet(
        PathGeometry geometry, 
        BitMatrix bitMatrix, 
        int row, 
        int column, 
        Rect symbolBounds, 
        double cornerRatio)
    {
        if (cornerRatio == 0)
        {
            var simpleFigure = new PathFigure() { StartPoint =  symbolBounds.TopLeft, };
            simpleFigure.Segments!.Add( new LineSegment { Point = symbolBounds.TopRight });
            simpleFigure.Segments .Add( new LineSegment { Point = symbolBounds.BottomRight });
            simpleFigure.Segments .Add( new LineSegment { Point = symbolBounds.BottomLeft });
            geometry.Figures?.Add(simpleFigure);
            return;
        }
        var cornerRadius = symbolBounds.Size * cornerRatio;
        var cornerFlags = GetSetSymbolCornerFlags(bitMatrix, row, column);
        var figure = new PathFigure
            { StartPoint = new Point(symbolBounds.Left, symbolBounds.Top + cornerRadius.Height) };

        // Top Left
        if ((cornerFlags & CornerFlags.TopLeft) != 0)
        {
            figure.Segments!.Add(new LineSegment { Point = symbolBounds.TopLeft });
            figure.Segments!.Add(new LineSegment
                { Point = new Point(symbolBounds.Right - cornerRadius.Width, symbolBounds.Top) });
        }
        else
        {
            figure.Segments!.Add(new ArcSegment
            {
                SweepDirection = SweepDirection.Clockwise,
                Point = new Point(symbolBounds.Left + cornerRadius.Width, symbolBounds.Top),
                Size = cornerRadius
            });
            figure.Segments.Add(new LineSegment()
            {
                Point =  new Point(symbolBounds.Right - cornerRadius.Width, symbolBounds.Top),
            });
        }

        // Top Right
        if ((cornerFlags & CornerFlags.TopRight) != 0)
        {
            figure.Segments!.Add(new LineSegment { Point = symbolBounds.TopRight });
            figure.Segments.Add(new LineSegment()
            {
                Point = new Point(symbolBounds.Right, symbolBounds.Bottom - cornerRadius.Height),
            });
        }
        else
        {
            figure.Segments!.Add(new ArcSegment
            {
                SweepDirection = SweepDirection.Clockwise,
                Point = new Point(symbolBounds.Right, symbolBounds.Top + cornerRadius.Height),
                Size = cornerRadius
            });
            figure.Segments.Add(new LineSegment()
            {
                Point = new Point(symbolBounds.Right, symbolBounds.Bottom - cornerRadius.Height),
            });
        }

        // Bottom Right
        if ((cornerFlags & CornerFlags.BottomRight) != 0)
        {
            figure.Segments!.Add(new LineSegment { Point = symbolBounds.BottomRight });
            figure.Segments!.Add(new LineSegment
                { Point = new Point(symbolBounds.Left + cornerRadius.Width, symbolBounds.Bottom) });
        }
        else
        {
            figure.Segments!.Add(new ArcSegment
            {
                SweepDirection = SweepDirection.Clockwise,
                Point = new Point(symbolBounds.Right - cornerRadius.Width, symbolBounds.Bottom),
                Size = cornerRadius
            });
            figure.Segments!.Add(new LineSegment
                { Point = new Point(symbolBounds.Left + cornerRadius.Width, symbolBounds.Bottom) });
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
                Point = new Point(symbolBounds.Left, symbolBounds.Bottom - cornerRadius.Height),
                Size = cornerRadius
            });
            figure.Segments!.Add(new LineSegment { Point = figure.StartPoint });
        }

        geometry.Figures?.Add(figure);
    }

    /// <summary>
    /// Gets the corner flags indicating how a set symbol is to be processed
    /// </summary>
    /// <param name="bitMatrix">BitMatrix containing the data</param>
    /// <param name="row">The row of the symbol being processed</param>
    /// <param name="column">The column of the symbol being processed</param>
    /// <returns>The corner flags for a set symbol</returns>
    private static CornerFlags GetSetSymbolCornerFlags(BitMatrix bitMatrix, int row, int column)
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
    /// <param name="cornerRatio"></param>
    private static void ProcessSymbolIfUnset(PathGeometry geometry, BitMatrix bitMatrix, int row, int column,
        Rect symbolBounds, double cornerRatio)
    {
        // If filled, no action required
        if (IsValid(bitMatrix, column, row))
            return;
        if (cornerRatio == 0) return;

        var cornerFlags = GetUnsetSymbolCornerFlags(bitMatrix, row, column);

        // If there are no nearby bits set, there's no need to smooth corners
        if (cornerFlags == CornerFlags.None)
            return;

        var cornerRadius = symbolBounds.Size * cornerRatio;

        // Top Left
        if ((cornerFlags & CornerFlags.TopLeft) != 0)
        {
            var start = new Point(symbolBounds.Left, symbolBounds.Top + cornerRadius.Height);

            geometry.Figures!.Add(new PathFigure
            {
                StartPoint = start,
                Segments =
                [
                    new LineSegment { Point = symbolBounds.TopLeft },
                    new LineSegment { Point = new Point(symbolBounds.Left + cornerRadius.Width, symbolBounds.Top) },
                    new ArcSegment
                    {
                        SweepDirection = SweepDirection.CounterClockwise,
                        Point = start,
                        Size = cornerRadius
                    }
                ]
            });
        }

        // Top Right
        if ((cornerFlags & CornerFlags.TopRight) != 0)
        {
            var start = new Point(symbolBounds.Right - cornerRadius.Width, symbolBounds.Top);

            geometry.Figures!.Add(new PathFigure
            {
                StartPoint = start,
                Segments =
                [
                    new LineSegment { Point = symbolBounds.TopRight },
                    new LineSegment { Point = new Point(symbolBounds.Right, symbolBounds.Top + cornerRadius.Height) },
                    new ArcSegment
                    {
                        SweepDirection = SweepDirection.CounterClockwise,
                        Point = start,
                        Size = cornerRadius
                    }
                ]
            });
        }

        // Bottom Right
        if ((cornerFlags & CornerFlags.BottomRight) != 0)
        {
            var start = new Point(symbolBounds.Right, symbolBounds.Bottom - cornerRadius.Height);

            geometry.Figures!.Add(new PathFigure
            {
                StartPoint = start,
                Segments =
                [
                    new LineSegment { Point = symbolBounds.BottomRight },
                    new LineSegment { Point = new Point(symbolBounds.Right - cornerRadius.Width, symbolBounds.Bottom) },
                    new ArcSegment
                    {
                        SweepDirection = SweepDirection.CounterClockwise,
                        Point = start,
                        Size = cornerRadius
                    }
                ]
            });
        }

        // Bottom Left
        if ((cornerFlags & CornerFlags.BottomLeft) != 0)
        {
            var start = new Point(symbolBounds.Left + cornerRadius.Width, symbolBounds.Bottom);

            geometry.Figures!.Add(new PathFigure
            {
                StartPoint = start,
                Segments =
                [
                    new LineSegment { Point = symbolBounds.BottomLeft },
                    new LineSegment { Point = new Point(symbolBounds.Left, symbolBounds.Bottom - cornerRadius.Height) },
                    new ArcSegment
                    {
                        SweepDirection = SweepDirection.CounterClockwise,
                        Point = start,
                        Size = cornerRadius
                    }
                ]
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
    private static CornerFlags GetUnsetSymbolCornerFlags(BitMatrix bitMatrix, int row, int column)
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
    private static bool IsValid(BitMatrix bitMatrix, int x, int y)
    {
        // Validate bounds of the bit matrix
        if (x < 0 || y < 0 || x >= bitMatrix.Width || y >= bitMatrix.Height)
            return false;
        if (x < 8 && y < 8) return false;
        if (x > bitMatrix.Width - 9 && y < 8) return false;
        if (x < 8 && y > bitMatrix.Height - 9) return false;
        return bitMatrix[y, x];
    }
}