using Avalonia.Interactivity;

namespace Ursa.Controls;

public class MessageClosedEventArgs(MessageCloseReason reason) : RoutedEventArgs
{
    public MessageCloseReason Reason { get; } = reason;
}
