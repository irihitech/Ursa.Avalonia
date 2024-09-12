using Avalonia.Controls.Notifications;

namespace Ursa.Controls;

/// <summary>
/// Represents a message that can be shown in a window or by the host operating system.
/// </summary>
public interface IMessage
{
    /// <summary>
    /// Gets the <see cref="NotificationType"/> of the message.
    /// </summary>
    NotificationType Type { get; }

    /// <summary>
    /// Gets a value indicating whether the message should show an icon.
    /// </summary>
    bool ShowIcon { get; }

    /// <summary>
    /// Gets a value indicating whether the message should show a close button.
    /// </summary>
    bool ShowClose { get; }

    /// <summary>
    /// Gets the expiration time of the message after which it will automatically close.
    /// If the value is <see cref="TimeSpan.Zero"/> then the message will remain open until the user closes it.
    /// </summary>
    TimeSpan Expiration { get; }

    /// <summary>
    /// Gets an Action to be run when the message is clicked.
    /// </summary>
    Action? OnClick { get; }

    /// <summary>
    /// Gets an Action to be run when the message is closed.
    /// </summary>
    Action? OnClose { get; }
}