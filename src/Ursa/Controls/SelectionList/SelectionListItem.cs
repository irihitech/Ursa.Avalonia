using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Ursa.Controls;

public class SelectionListItem: ContentControl, ISelectable
{
    static SelectionListItem()
    {
        SelectableMixin.Attach<SelectionListItem>(IsSelectedProperty);
        PressedMixin.Attach<SelectionListItem>();
        FocusableProperty.OverrideDefaultValue<SelectionListItem>(true);
    }

    public static readonly StyledProperty<bool> IsSelectedProperty = SelectingItemsControl.IsSelectedProperty.AddOwner<ListBoxItem>();

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (ItemsControl.ItemsControlFromItemContainer(this) is SelectionList list)
        {
            int index = list.IndexFromContainer(this);
            list.SelectByIndex(index);
        }
    }
}