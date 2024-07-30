using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

[PseudoClasses(PC_Vertical)]
public class ToolBarSeparator: TemplatedControl
{
    public const string PC_Vertical = ":vertical";
    
    public static readonly StyledProperty<Orientation> OrientationProperty =
        ToolBar.OrientationProperty.AddOwner<ToolBarSeparator>();

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    static ToolBarSeparator()
    {
        OrientationProperty.OverrideDefaultValue<ToolBarSeparator>(Orientation.Horizontal);
        OrientationProperty.Changed.AddClassHandler<ToolBarSeparator, Orientation>((separator, args) =>
        {
            separator.PseudoClasses.Set(PC_Vertical, args.NewValue.Value == Orientation.Vertical);
        });
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);
        var ancestor = this.GetLogicalAncestors().OfType<ToolBar>().FirstOrDefault();
        if (ancestor is null) return;
        this[!OrientationProperty] = ancestor[!ToolBar.OrientationProperty];
    }
}