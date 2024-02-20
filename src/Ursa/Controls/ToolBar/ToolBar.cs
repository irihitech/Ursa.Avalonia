using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;

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

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        OverflowPanel = e.NameScope.Find<Panel>(PART_OverflowPanel);
    }
}