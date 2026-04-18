namespace Ursa.Controls;

/// <summary>
/// Represents a notification manager that can be used to show notifications in a window or using
/// the host operating system.
/// </summary>
public interface INotificationManager
{
    /// <summary>
    /// Show a notification.
    /// </summary>
    /// <param name="notification">The notification to be displayed.</param>
    void Show(INotification notification);

    /// <summary>
    /// Closes a specific notification.
    /// </summary>
    /// <param name="notification">The notification to close.</param>
    void Close(INotification notification);

    /// <summary>
    /// Closes all currently visible notifications.
    /// </summary>
    void CloseAll();
}