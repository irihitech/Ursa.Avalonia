using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;

public class ElasticWrapPanel : Panel
{
    protected override Size MeasureOverride(Size availableSize)
    {
        var panelDesiredSize = new Size();

        foreach (var child in Children)
        {
            child.Measure(availableSize);
            panelDesiredSize = child.DesiredSize;
        }

        return panelDesiredSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        foreach (var child in Children)
        {
            const double x = 50;
            const double y = 50;

            child.Arrange(new Rect(new Point(x, y), child.DesiredSize));
        }

        return finalSize;
    }
}