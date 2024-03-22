using Avalonia;
using Avalonia.Controls;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class MultiComboBoxItem: ListBoxItem
{
    public MultiComboBoxItem()
    {
        this.GetObservable(IsFocusedProperty).Subscribe(a=> {
            if (a)
            {
                (Parent as MultiComboBox)?.ItemFocused(this);
            }
        });
    }
}