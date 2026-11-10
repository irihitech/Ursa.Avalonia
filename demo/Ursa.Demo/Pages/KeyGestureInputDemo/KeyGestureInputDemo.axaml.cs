using Avalonia.Controls;
using Ursa.Controls;

namespace Ursa.Demo.Pages.KeyGestureInputDemo;

public partial class KeyGestureInputDemo : UserControl
{
    public KeyGestureInputDemo()
    {
        InitializeComponent();
    }

    private void OnGestureCleared(object? sender, KeyGestureInputClearedEventArgs e)
    {
        ClearedReasonText.Text = $"Cleared: {e.Reason}";
    }
}
