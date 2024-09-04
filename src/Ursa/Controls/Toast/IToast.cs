using Avalonia.Controls.Notifications;
using Avalonia.Metadata;

namespace Ursa.Controls;

/// <summary>
/// Represents a toast that can be shown in a window or by the host operating system.
/// </summary>
[NotClientImplementable]
public interface IToast
{

    /// <summary>
    /// Gets the toast message.
    /// </summary>
    string? Content { get; }

    /// <summary>
    /// Gets the <see cref="NotificationType"/> of the toast.
    /// </summary>
    NotificationType Type { get; }

    /// <summary>
    /// Gets the expiration time of the toast after which it will automatically close.
    /// If the value is <see cref="TimeSpan.Zero"/> then the toast will remain open until the user closes it.
    /// </summary>
    TimeSpan Expiration { get; }

    /// <summary>
    /// Gets an Action to be run when the toast is clicked.
    /// </summary>
    Action? OnClick { get; }

    /// <summary>
    /// Gets an Action to be run when the toast is closed.
    /// </summary>
    Action? OnClose { get; }
}