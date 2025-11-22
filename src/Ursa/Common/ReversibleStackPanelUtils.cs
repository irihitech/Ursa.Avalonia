using Avalonia.Controls;
using Avalonia.Layout;

namespace Ursa.Common;

/// <summary>
/// Workaround for <see cref="ReversibleStackPanel"/> lacking <see cref="Layoutable.AffectsArrange"/> call
/// on <see cref="ReversibleStackPanel.ReverseOrderProperty"/> property.
/// Remove this workaround when the bug in Avalonia is fixed.
/// </summary>
internal class ReversibleStackPanelUtils : Layoutable
{
    private static bool _isBugFixed = false;

    public static void EnsureBugFixed()
    {
        if (_isBugFixed)
            return;
        AffectsArrange<ReversibleStackPanel>(ReversibleStackPanel.ReverseOrderProperty);
        _isBugFixed = true;
    }
}