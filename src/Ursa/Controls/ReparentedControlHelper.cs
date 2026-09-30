using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace Ursa.Controls;

internal static class ReparentedControlHelper
{
    internal static void InvalidateVisuals(Control control)
    {
        // A closed Popup has no visual root, so refresh its children when it loads.
        if (control.IsAttachedToVisualTree())
        {
            Refresh();
        }
        else
        {
            control.Loaded += OnLoaded;
        }

        void OnLoaded(object? sender, RoutedEventArgs e)
        {
            control.Loaded -= OnLoaded;
            Refresh();
        }

        void Refresh()
        {
            // Reparenting can leave a descendant's cached drawing unchanged.
            control.InvalidateVisual();
            foreach (var visual in control.GetVisualDescendants())
                visual.InvalidateVisual();
        }
    }
}
