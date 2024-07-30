using Avalonia.Interactivity;

namespace Ursa.Controls;

public class VerificationCodeCompleteEventArgs(IList<string> code, RoutedEvent? @event) : RoutedEventArgs(@event)
{
    public IList<string> Code { get; } = code;
}