using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class MultiComboBoxItem: ContentControl
{
    private MultiComboBox? _parent;
    
    public static readonly StyledProperty<bool> IsSelectedProperty = AvaloniaProperty.Register<MultiComboBoxItem, bool>(
        nameof(IsSelected));

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }
    
    static MultiComboBoxItem()
    {
        IsSelectedProperty.AffectsPseudoClass<MultiComboBoxItem>(":selected");
        PressedMixin.Attach<MultiComboBoxItem>();
        FocusableProperty.OverrideDefaultValue<MultiComboBoxItem>(true);
        IsSelectedProperty.Changed.AddClassHandler<MultiComboBoxItem, bool>((item, args) =>
            item.OnSelectionChanged(args));
    }

    private void OnSelectionChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
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

    public MultiComboBoxItem()
    {
        this.GetObservable(IsFocusedProperty).Subscribe(a=> {
            if (a)
            {
                (Parent as MultiComboBox)?.ItemFocused(this);
            }
        });
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
        if (e.Handled)
        {
            return;
        }
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            this.IsSelected = !this.IsSelected;
            e.Handled = true;
        }
    }
}