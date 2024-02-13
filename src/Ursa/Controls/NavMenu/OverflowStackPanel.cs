using Avalonia.Controls;

namespace Ursa.Controls;

public class OverflowStackPanel: StackPanel
{
    public Panel? OverflowPanel { get; set; }
    public void MoveChildrenToOverflowPanel()
    {
        var children = this.Children.ToList();
        foreach (var child in children)
        {
            this.Children.Remove(child);
            this.OverflowPanel?.Children.Add(child);
        }
    }
    
    public void MoveChildrenToMainPanel()
    {
        var children = this.OverflowPanel?.Children.ToList();
        if (children != null && children.Count > 0)
        {
            foreach (var child in children)
            {
                this.OverflowPanel?.Children.Remove(child);
                this.Children.Add(child);
            }
        }
    }
}