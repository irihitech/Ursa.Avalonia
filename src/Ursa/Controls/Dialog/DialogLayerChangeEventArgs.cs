using Avalonia.Interactivity;

namespace Ursa.Controls;

public class DialogLayerChangeEventArgs: RoutedEventArgs
{
    public DialogLayerChangeType ChangeType { get; }
    
    public DialogLayerChangeEventArgs(DialogLayerChangeType type)
    {
        ChangeType = type;
    }
    public DialogLayerChangeEventArgs(RoutedEvent routedEvent, DialogLayerChangeType type): base(routedEvent)
    {
        ChangeType = type;
    }
}

public enum DialogLayerChangeType
{
    BringForward,
    SendBackward,
    BringToFront,
    SendToBack
}