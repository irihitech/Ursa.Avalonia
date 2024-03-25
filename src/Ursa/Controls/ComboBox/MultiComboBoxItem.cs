using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Input;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class MultiComboBoxItem: ContentControl
{
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