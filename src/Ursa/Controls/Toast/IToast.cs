namespace Ursa.Controls;

/// <summary>
/// Represents a toast that can be shown in a window or by the host operating system.
/// </summary>
public interface IToast : IMessage
{
    /// <summary>
    /// Gets the toast message.
    /// </summary>
    string? Content { get; }
}