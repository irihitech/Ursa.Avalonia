using Avalonia.Interactivity;

namespace Ursa.Controls;

/// <summary>
/// Event args for <see cref="KeyGestureInput.Cleared"/>, carrying the origin of the clear.
/// </summary>
public class KeyGestureInputClearedEventArgs(KeyGestureInputClearReason reason) : RoutedEventArgs
{
    /// <summary>
    /// Gets the origin of the clear.
    /// </summary>
    public KeyGestureInputClearReason Reason { get; } = reason;
}
