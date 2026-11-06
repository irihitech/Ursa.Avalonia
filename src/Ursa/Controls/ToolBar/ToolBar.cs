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
[TemplatePart(PART_OverflowPopup, typeof(Popup))]
public class ToolBar: HeaderedItemsControl
{
    public const string PART_OverflowPanel = "PART_OverflowPanel";
    public const string PART_OverflowPopup = "PART_OverflowPopup";
    public const string PC_Overflow = ":overflow";
    
    internal Panel? OverflowPanel { get; private set; }

    /// <summary>
    /// Whether the overflow popup currently displays the items that do not fit. The panel only
    /// borrows the items while this is true; otherwise they stay in the toolbar panel.
    /// </summary>
    internal bool IsOverflowPopupOpen { get; private set; }

    /// <summary>Raised when <see cref="IsOverflowPopupOpen"/> changes, so the panel can re-layout.</summary>
    internal event EventHandler? OverflowPopupStateChanged;

    private void SetOverflowPopupOpen(bool value)
    {
        if (IsOverflowPopupOpen == value) return;
        IsOverflowPopupOpen = value;
        OverflowPopupStateChanged?.Invoke(this, System.EventArgs.Empty);
    }

    private Popup? _overflowPopup;
    
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
            PseudoClassesExtensions.Set(o.Classes, PC_Overflow, e.NewValue.Value);
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

        if (_overflowPopup is not null)
        {
            _overflowPopup.PropertyChanged -= OnOverflowPopupPropertyChanged;
        }

        _overflowPopup = e.NameScope.Find<Popup>(PART_OverflowPopup)
            ?? this.GetTemplateDescendants().OfType<Popup>().FirstOrDefault();
        if (_overflowPopup is not null)
        {
            // Observing IsOpen instead of Opened/Closed keeps the state correct even when the
            // platform cannot actually present the popup (headless or embedded hosts).
            _overflowPopup.PropertyChanged += OnOverflowPopupPropertyChanged;
            SetOverflowPopupOpen(_overflowPopup.IsOpen);
        }
        else
        {
            SetOverflowPopupOpen(false);
        }
    }

    private void OnOverflowPopupPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property != Popup.IsOpenProperty) return;
        SetOverflowPopupOpen(_overflowPopup?.IsOpen == true);
        // The panel moves the overflow items in or out of the popup and re-arranges the rest.
        OverflowPanel?.InvalidateMeasure();
        InvalidateMeasure();
        InvalidateArrange();
    }
}