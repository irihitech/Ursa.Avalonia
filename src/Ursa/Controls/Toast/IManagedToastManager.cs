using Avalonia.Metadata;

namespace Ursa.Controls;

/// <summary>
/// Represents a toast manager that can show arbitrary content.
/// Managed toast managers can show any content.
/// </summary>
/// <remarks>
/// Because toast managers of this type are implemented purely in managed code, they
/// can display arbitrary content, as opposed to toast managers which display toasts
/// using the host operating system's toast mechanism.
/// </remarks>
[NotClientImplementable]
public interface IManagedToastManager : IToastManager
{
    /// <summary>
    /// Shows a toast.
    /// </summary>
    /// <param name="content">The content to be displayed.</param>
    void Show(object content);
}