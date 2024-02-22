using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Templates;

namespace Ursa.Controls;

[TemplatePart(PART_OverflowPanel, typeof(Panel))]
public class ToolBar: HeaderedItemsControl
{
    public const string PART_OverflowPanel = "PART_OverflowPanel";
    
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
    
    public static readonly StyledProperty<int> BandProperty = AvaloniaProperty.Register<ToolBar, int>(
        nameof(Band));
    
    public int Band
    {
        get => GetValue(BandProperty);
        set => SetValue(BandProperty, value);
    }

    public static readonly AttachedProperty<OverflowMode> OverflowModeProperty =
        AvaloniaProperty.RegisterAttached<ToolBar, Control, OverflowMode>("OverflowMode");

    public static void SetOverflowMode(Control obj, OverflowMode value) => obj.SetValue(OverflowModeProperty, value);
    public static OverflowMode GetOverflowMode(Control obj) => obj.GetValue(OverflowModeProperty);

    internal static readonly AttachedProperty<bool> IsOverflowItemProperty =
        AvaloniaProperty.RegisterAttached<ToolBar, Control, bool>("IsOverflowItem");

    internal static void SetIsOverflowItem(Control obj, bool value) => obj.SetValue(IsOverflowItemProperty, value);
    internal static bool GetIsOverflowItem(Control obj) => obj.GetValue(IsOverflowItemProperty);

    static ToolBar()
    {
        IsTabStopProperty.OverrideDefaultValue<ToolBar>(false);
        ItemsPanelProperty.OverrideDefaultValue<ToolBar>(DefaultTemplate);
        OrientationProperty.OverrideDefaultValue<ToolBar>(Orientation.Horizontal);
    }
    
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<Control>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new ContentPresenter();
    }

    protected override void ContainerForItemPreparedOverride(Control container, object? item, int index)
    {
        base.ContainerForItemPreparedOverride(container, item, index);
        if (item is Control s)
        {
            container[!ToolBar.OverflowModeProperty] = s[!ToolBar.OverflowModeProperty];
        }
        else
        {
            if (container is ContentPresenter p)
            {
                p.ApplyTemplate();
                var c = p.Child;
                if (c != null)
                {
                    // container[ToolBar.OverflowModeProperty] = c[ToolBar.OverflowModeProperty];
                    container[!ToolBar.OverflowModeProperty] = c[!ToolBar.OverflowModeProperty];
                }
            }
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        OverflowPanel = e.NameScope.Find<Panel>(PART_OverflowPanel);
    }
}