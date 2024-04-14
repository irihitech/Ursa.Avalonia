using System.Data;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.OpenGL.Controls;
using Irihi.Avalonia.Shared.Common;


namespace Ursa.Controls;

[TemplatePart(PartNames.PART_Popup, typeof(Popup))]
public class TreeComboBox: SelectingItemsControl
{
    private Popup? _popup;
    
    private static readonly FuncTemplate<Panel?> DefaultPanel =
        new FuncTemplate<Panel?>(() => new VirtualizingStackPanel());

    public static readonly StyledProperty<double> MaxDropDownHeightProperty =
        ComboBox.MaxDropDownHeightProperty.AddOwner<TreeComboBox>();

    public double MaxDropDownHeight
    {
        get => GetValue(MaxDropDownHeightProperty);
        set => SetValue(MaxDropDownHeightProperty, value);
    }

    public static readonly StyledProperty<string?> WatermarkProperty =
        TextBox.WatermarkProperty.AddOwner<TreeComboBox>();

    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }
    
    public static readonly StyledProperty<bool> IsDropDownOpenProperty =
        ComboBox.IsDropDownOpenProperty.AddOwner<TreeComboBox>();
    
    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
        ContentControl.HorizontalContentAlignmentProperty.AddOwner<TreeComboBox>();

    public HorizontalAlignment HorizontalContentAlignment
    {
        get => GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
    }
    
    public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
        ContentControl.VerticalContentAlignmentProperty.AddOwner<TreeComboBox>();
    
    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> SelectedItemTemplateProperty =
        AvaloniaProperty.Register<TreeComboBox, IDataTemplate?>(nameof(SelectedItemTemplate));

    public IDataTemplate? SelectedItemTemplate
    {
        get => GetValue(SelectedItemTemplateProperty);
        set => SetValue(SelectedItemTemplateProperty, value);
    }

    public static readonly DirectProperty<TreeComboBox, object?> SelectionBoxItemProperty = AvaloniaProperty.RegisterDirect<TreeComboBox, object?>(
        nameof(SelectionBoxItem), o => o.SelectionBoxItem);
    private object? _selectionBoxItem;
    public object? SelectionBoxItem
    {
        get => _selectionBoxItem;
        protected set => SetAndRaise(SelectionBoxItemProperty, ref _selectionBoxItem, value);
    }
    
    static TreeComboBox()
    {
        ItemsPanelProperty.OverrideDefaultValue<TreeComboBox>(DefaultPanel);
        FocusableProperty.OverrideDefaultValue<TreeComboBox>(true);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _popup = e.NameScope.Find<Popup>(PartNames.PART_Popup);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<TreeComboBoxItem>(item, out recycleKey);
    }

    internal bool NeedsContainerInternal(object? item, int index, out object? recycleKey)
    {
        return NeedsContainerOverride(item, index, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new TreeComboBoxItem();
    }

    internal Control CreateContainerForItemInternal(object? item, int index, object? recycleKey)
    {
        return CreateContainerForItemOverride(item, index, recycleKey);
    }

    internal void ContainerForItemPreparedInternal(Control container, object? item, int index)
    {
        ContainerForItemPreparedOverride(container, item, index);
    }
    
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (e.InitialPressMouseButton == MouseButton.Left)
        {
            if (_popup is not null && _popup.IsOpen && e.Source is Visual v && _popup.IsInsidePopup(v))
            {
                var container = v.FindLogicalAncestorOfType<TreeComboBoxItem>();
                container?.SetValue(IsSelectedProperty, true);
                if (container?.Header is Control control)
                {
                    SelectionBoxItem = control.DataContext ?? control.ToString();
                    // UpdateSelectionFromEventSource(e.Source);
                    
                }
                else
                {
                    SelectionBoxItem = container?.Header; 
                    // UpdateSelectionFromEventSource(e.Source);
                }
                SetCurrentValue(IsDropDownOpenProperty, false);
            }
            else
            {
                IsDropDownOpen = !IsDropDownOpen;
            }
            
        }
    }
} 