using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace Ursa.Controls.Shapes;

/// <summary>
/// A rectangle, with no corner radius.
/// </summary>
public class PureRectangle: Shape
{
    static PureRectangle()
    {
        FocusableProperty.OverrideDefaultValue<PureRectangle>(false);
        AffectsGeometry<PureRectangle>(BoundsProperty);
    }
    protected override Geometry? CreateDefiningGeometry()
    {
        StreamGeometry geometry = new StreamGeometry();
        Rect rect = new Rect(this.Bounds.Size).Deflate(this.StrokeThickness / 2.0);
        using StreamGeometryContext context = geometry.Open();
        context.BeginFigure(new Point(rect.Left, rect.Top), true);
        context.LineTo(new Point(rect.Right, rect.Top));
        context.LineTo(new Point(rect.Right, rect.Bottom));
        context.LineTo(new Point(rect.Left, rect.Bottom));
        context.LineTo(new Point(rect.Left, rect.Top));
        context.EndFigure(true);
        return geometry;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        return new Size(this.StrokeThickness, this.StrokeThickness);
    }
}