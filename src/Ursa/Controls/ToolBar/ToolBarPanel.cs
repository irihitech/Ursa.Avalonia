using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

public class ToolBarPanel: StackPanel
{
    private ToolBar? _parent;
    private Panel? _overflowPanel;

    internal Panel? OverflowPanel => _overflowPanel ??= _parent?.OverflowPanel;
    internal ToolBar? ParentToolBar => _parent ??= this.TemplatedParent as ToolBar;

    static ToolBarPanel()
    {
        OrientationProperty.OverrideDefaultValue<ToolBarPanel>(Orientation.Horizontal);
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);
        _parent = this.TemplatedParent as ToolBar;
        if (_parent is null) return;
        this[!OrientationProperty] = _parent[!OrientationProperty];
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var size = base.MeasureOverride(availableSize);
        var children = this.Children;
        var children2 = this.OverflowPanel?.Children;
        var all = children.ToList();
        if (children2 != null)
        {
            all.AddRange(children2);
        }
        this.Children.Clear();
        OverflowPanel?.Children.Clear();
        for (int i = 0; i < all.Count - 1; i++)
        {
            this.Children.Add(all[i]);
        }
        if (all.Count > 0)
        {
            OverflowPanel?.Children.Add(all.Last());
        }
        return size;
    }
}