using Avalonia.Metadata;

namespace Ursa.Controls;

/// <summary>
/// Represents a toast manager that can be used to show toasts in a window or using
/// the host operating system.
/// </summary>
[NotClientImplementable]
public interface IToastManager
{
    /// <summary>
    /// Show a toast.
    /// </summary>
    /// <param name="toast">The toast to be displayed.</param>
    void Show(IToast toast);

    /// <summary>
    /// Closes a specific toast.
    /// </summary>
    /// <param name="toast">The toast to close.</param>
    void Close(IToast toast);

    /// <summary>
    /// Closes all currently visible toasts.
    /// </summary>
    void CloseAll();
}