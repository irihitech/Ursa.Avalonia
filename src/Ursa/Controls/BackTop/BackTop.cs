using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Ursa.Controls.BackTop;

public class BackTop : Control
{
    public static readonly AttachedProperty<bool> AttachProperty =
        AvaloniaProperty.RegisterAttached<BackTop, Control, bool>("Attach");

    public static void SetAttach(Control obj, bool value) => obj.SetValue(AttachProperty, value);
    public static bool GetAttach(Control obj) => obj.GetValue(AttachProperty);
}