using Avalonia.Input;

namespace Ursa.Controls;

public class NavigationMenuSeparator: NavigationMenuItem
{
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        e.Handled = true;
    }
}