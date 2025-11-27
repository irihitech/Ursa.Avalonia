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
    private static int _isBugFixed = 0;

    public static void EnsureBugFixed()
    {
        if (Interlocked.CompareExchange(ref _isBugFixed, 1, 0) != 0)
            return;
        AffectsArrange<ReversibleStackPanel>(ReversibleStackPanel.ReverseOrderProperty);
    }
}