using Avalonia;
using Avalonia.Input;

namespace Ursa.Helpers;

public class FocusHelper
{
    public static readonly AttachedProperty<bool> DialogFocusHintProperty =
        AvaloniaProperty.RegisterAttached<FocusHelper, InputElement, bool>("DialogFocusHint");

    public static void SetDialogFocusHint(InputElement obj, bool value) => obj.SetValue(DialogFocusHintProperty, value);
    public static bool GetDialogFocusHint(InputElement obj) => obj.GetValue(DialogFocusHintProperty);
}