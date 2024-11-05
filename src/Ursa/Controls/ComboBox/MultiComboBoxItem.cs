using Avalonia;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class MultiComboBoxItem: ContentControl
{
    private MultiComboBox? _parent;
    private static readonly Point s_invalidPoint = new (double.NaN, double.NaN);
    private Point _pointerDownPoint = s_invalidPoint;
    private bool _updateInternal;
    
    public static readonly StyledProperty<bool> IsSelectedProperty = AvaloniaProperty.Register<MultiComboBoxItem, bool>(
        nameof(IsSelected));

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }
    
    static MultiComboBoxItem()
    {
        IsSelectedProperty.AffectsPseudoClass<MultiComboBoxItem>(PseudoClassName.PC_Selected);
        PressedMixin.Attach<MultiComboBoxItem>();
        FocusableProperty.OverrideDefaultValue<MultiComboBoxItem>(true);
        IsSelectedProperty.Changed.AddClassHandler<MultiComboBoxItem, bool>((item, args) =>
            item.OnSelectionChanged(args));
    }

    private void OnSelectionChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (_updateInternal) return;
        var parent = this.FindLogicalAncestorOfType<MultiComboBox>();
        if (args.NewValue.Value)
        {
            parent?.SelectedItems?.Add(this.DataContext);
        }
        else
        {
            parent?.SelectedItems?.Remove(this.DataContext);
        }
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);
        _parent = this.FindLogicalAncestorOfType<MultiComboBox>();
        if(this.IsSelected)
            _parent?.SelectedItems?.Add(this.DataContext);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        _pointerDownPoint = e.GetPosition(this);
        if (e.Handled)
        {
            return;
        }
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            var p = e.GetCurrentPoint(this);
            if (p.Properties.PointerUpdateKind is PointerUpdateKind.LeftButtonPressed
                or PointerUpdateKind.RightButtonPressed)
            {
                if (p.Pointer.Type == PointerType.Mouse)
                {
                    this.IsSelected = !this.IsSelected;
                    e.Handled = true;
                }
                else
                {
                    _pointerDownPoint = p.Position;
                }
            }
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (!e.Handled && !double.IsNaN(_pointerDownPoint.X) &&
            e.InitialPressMouseButton is MouseButton.Left or MouseButton.Right)
        {
            var point = e.GetCurrentPoint(this);
            if (new Rect(Bounds.Size).ContainsExclusive(point.Position) && e.Pointer.Type == PointerType.Touch)
            {
                this.IsSelected = !this.IsSelected;
                e.Handled = true;
            }
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateSelection();
    }
    
    internal void UpdateSelection()
    {
        _updateInternal = true;
        if (_parent?.ItemsPanelRoot is VirtualizingPanel)
        {
            IsSelected =  _parent?.SelectedItems?.Contains(DataContext) ?? false;
        }
        _updateInternal = false;
    }
    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new ListItemAutomationPeer(this);
    }
}