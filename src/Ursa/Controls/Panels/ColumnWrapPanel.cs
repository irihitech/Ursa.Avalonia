using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class ColumnWrapPanel: Panel, INavigableContainer
{
    public static readonly StyledProperty<int> ColumnProperty = AvaloniaProperty.Register<ColumnWrapPanel, int>(
        nameof(Column), int.MaxValue);

    public int Column
    {
        get => GetValue(ColumnProperty);
        set => SetValue(ColumnProperty, value);
    }

    static ColumnWrapPanel()
    {
        AffectsMeasure<ColumnWrapPanel>(ColumnProperty);
        AffectsArrange<ColumnWrapPanel>(ColumnProperty);
    }
    
    protected override Size MeasureOverride(Size availableSize)
    {
        double unit = availableSize.Width / Column;
        double x = 0;
        double y = 0;
        double rowHeight = 0;
        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            child.Measure(availableSize);
            var desiredSize = child.DesiredSize;
            // calculate how many columns the child will take
            int colSpan = (int)Math.Ceiling(desiredSize.Width / unit);
            if (colSpan > Column) colSpan = Column; // limit to max columns
            double childWidth = colSpan * unit;
            if (MathHelpers.GreaterThan(x + childWidth , availableSize.Width)) // wrap to next row
            {
                x = 0;
                y += rowHeight;
                rowHeight = 0;
            }
            x += childWidth;
            rowHeight = Math.Max(rowHeight, desiredSize.Height);
        }
        return new Size(availableSize.Width, y + rowHeight);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        double unit = finalSize.Width / Column;
        double x = 0;
        double y = 0;
        double rowHeight = 0;
        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            var desiredSize = child.DesiredSize;
            var remainingWidth = finalSize.Width - x;
            if (MathHelpers.GreaterThan(desiredSize.Width, remainingWidth))
            {
                x = 0;
                y += rowHeight;
                rowHeight = 0;
            }

            child.Arrange(new Rect(x, y, desiredSize.Width, desiredSize.Height));
            int colSpan = (int)Math.Ceiling(desiredSize.Width / unit);
            if (colSpan > Column) colSpan = Column; // limit to max columns
            x += colSpan * unit;
            rowHeight = Math.Max(rowHeight, desiredSize.Height);
        }
        return new Size(finalSize.Width, y + rowHeight);
    }

    public IInputElement? GetControl(NavigationDirection direction, IInputElement? from, bool wrap)
    {
        return null;
    }
}