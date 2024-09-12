namespace Ursa.Controls;

/// <summary>
/// Represents a notification that can be shown in a window or by the host operating system.
/// </summary>
public interface INotification : IMessage
{
    /// <summary>
    /// Gets the Title of the notification.
    /// </summary>
    string? Title { get; }

    /// <summary>
    /// Gets the Content of the notification.
    /// </summary>
    string? Content { get; }
}