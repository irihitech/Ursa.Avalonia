using Avalonia.Controls;
using Avalonia.Platform;

namespace Ursa.Controls.MessageBox;

public class MessageBoxWindow: Window
{
    protected override Type StyleKeyOverride => typeof(MessageBoxWindow);

    static MessageBoxWindow()
    {
        ExtendClientAreaChromeHintsProperty.OverrideDefaultValue<MessageBoxWindow>(ExtendClientAreaChromeHints
            .NoChrome);
    }
}