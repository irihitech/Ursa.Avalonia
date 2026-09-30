using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Ursa.Controls;

public class OverflowStackPanel : StackPanel
{
    public Panel? OverflowPanel { get; set; }

    public void MoveChildrenToOverflowPanel()
    {
        var children = Children.ToList();
        foreach (var child in children)
        {
            Children.Remove(child);
            OverflowPanel?.Children.Add(child);
        }
    }

    public void MoveChildrenToMainPanel()
    {
        var children = OverflowPanel?.Children.ToList();
        if (children is not null && children.Count > 0)
            foreach (var child in children)
            {
                OverflowPanel?.Children.Remove(child);
                Children.Add(child);

                // Reparenting from a Popup can leave descendant visuals with stale
                // render data even after the item is attached to the menu again.
                child.InvalidateVisual();
                foreach (var visual in child.GetVisualDescendants())
                    visual.InvalidateVisual();
            }
    }
}
