using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;

namespace Ursa.Controls;

[PseudoClasses(PC_Overflow)]
[TemplatePart(PART_OverflowPanel, typeof(Panel))]
public class ToolBar: HeaderedItemsControl
{
    public const string PART_OverflowPanel = "PART_OverflowPanel";
    public const string PC_Overflow = ":overflow";
    
    internal Panel? OverflowPanel { get; private set; }
    
    private static readonly ITemplate<Panel?> DefaultTemplate =
        new FuncTemplate<Panel?>(() => new ToolBarPanel() { Orientation = Orientation.Horizontal });
    
    public static readonly StyledProperty<Orientation> OrientationProperty =
        StackPanel.OrientationProperty.AddOwner<ToolBar>();

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly StyledProperty<PlacementMode> PopupPlacementProperty =
        Popup.PlacementProperty.AddOwner<ToolBar>();

    public PlacementMode PopupPlacement
    {
        get => GetValue(PopupPlacementProperty);
        set => SetValue(PopupPlacementProperty, value);
    }

    public static readonly AttachedProperty<OverflowMode> OverflowModeProperty =
        AvaloniaProperty.RegisterAttached<ToolBar, Control, OverflowMode>("OverflowMode");

    public static void SetOverflowMode(Control obj, OverflowMode value) => obj.SetValue(OverflowModeProperty, value);
    public static OverflowMode GetOverflowMode(Control obj) => obj.GetValue(OverflowModeProperty);

    internal static readonly AttachedProperty<bool> IsOverflowItemProperty =
        AvaloniaProperty.RegisterAttached<ToolBar, Control, bool>("IsOverflowItem");

    internal static void SetIsOverflowItem(Control obj, bool value) => obj.SetValue(IsOverflowItemProperty, value);
    internal static bool GetIsOverflowItem(Control obj) => obj.GetValue(IsOverflowItemProperty);

    private bool _hasOverflowItems;
    internal bool HasOverflowItems
    {
        get => _hasOverflowItems;
        set
        {
            _hasOverflowItems = value;
            PseudoClasses.Set(PC_Overflow, value);
        }
    }

    static ToolBar()
    {
        IsTabStopProperty.OverrideDefaultValue<ToolBar>(false);
        ItemsPanelProperty.OverrideDefaultValue<ToolBar>(DefaultTemplate);
        OrientationProperty.OverrideDefaultValue<ToolBar>(Orientation.Horizontal);
        // TODO: use helper method after merged and upgrade helper dependency. 
        IsOverflowItemProperty.Changed.AddClassHandler<Control, bool>((o, e) =>
        {
            PseudolassesExtensions.Set(o.Classes, PC_Overflow, e.NewValue.Value);
        });
    }
    
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<Control>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        if(item is Control c)
        {
            return c;
        }
        if(ItemTemplate is not null && ItemTemplate.Match(item))
        {
            return ItemTemplate.Build(item)?? new ContentPresenter();
        }
        return new ContentPresenter();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        OverflowPanel = e.NameScope.Find<Panel>(PART_OverflowPanel);
    }
}