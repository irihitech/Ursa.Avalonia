using Avalonia.Input;
using Ursa.Common;

namespace Ursa.Controls;

public static class OverlayMessageBox
{
    public static async Task<MessageBoxResult> ShowAsync(
        string message,
        string? title = null,
        string? hostId = null,
        MessageBoxIcon icon = MessageBoxIcon.None,
        MessageBoxButton button = MessageBoxButton.OK,
        int? toplevelHashCode = null,
        string? styleClass = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, toplevelHashCode);
        if (host is null) return MessageBoxResult.None;
        var messageControl = new MessageBoxControl
        {
            Content = message,
            Title = title,
            Buttons = button,
            MessageIcon = icon,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        if (!string.IsNullOrWhiteSpace(styleClass))
        {
            var styles = styleClass!.Split(Constants.SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
            messageControl.Classes.AddRange(styles!);
        }

        host.AddModalDialog(messageControl);
        var result = await messageControl.ShowAsync<MessageBoxResult>();
        return result;
    }
}