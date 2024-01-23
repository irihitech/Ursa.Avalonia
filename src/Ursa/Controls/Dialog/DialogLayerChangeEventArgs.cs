namespace Ursa.Controls;

public class DialogLayerChangeEventArgs
{
    public DialogLayerChangeType ChangeType { get; }
    public DialogLayerChangeEventArgs(DialogLayerChangeType type)
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