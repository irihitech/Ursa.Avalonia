using Avalonia.Controls;
using Avalonia.Platform;

namespace Ursa.Controls;

public class MessageBoxWindow: Window
{
    protected override Type StyleKeyOverride => typeof(MessageBoxWindow);

    static MessageBoxWindow()
    {
        
    }
}