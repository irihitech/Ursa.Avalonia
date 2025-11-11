namespace Ursa.Controls;

/// <summary>
/// Specifies the reason why a message was closed.
/// </summary>
public enum MessageCloseReason
{
    /// <summary>
    /// The message closed because its display duration expired.
    /// </summary>
    Timeout,
    
    /// <summary>
    /// The message was closed by an explicit user action (e.g., clicking the close button).
    /// </summary>
    UserAction,
    
    /// <summary>
    /// The message was closed because a newer message arrived, displacing it due to the MaxItems limit.
    /// </summary>
    Displaced
}
