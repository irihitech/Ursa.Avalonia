using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Ursa.Common;

namespace Ursa.Controls;

public static class MessageBox
{
    public static async Task<MessageBoxResult> ShowAsync(
        string message,
        string? title = null,
        MessageBoxIcon icon = MessageBoxIcon.None,
        MessageBoxButton button = MessageBoxButton.OK,
        string? styleClass = null)
    {
        var messageWindow = new MessageBoxWindow(button)
        {
            Content = message,
            Title = title,
            MessageIcon = icon
        };
        if (!string.IsNullOrWhiteSpace(styleClass))
        {
            var styles = styleClass!.Split(Constants.SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
            messageWindow.Classes.AddRange(styles);
        }
        var lifetime = Application.Current?.ApplicationLifetime;
        if (lifetime is not IClassicDesktopStyleApplicationLifetime classLifetime) return MessageBoxResult.None;
        var main = classLifetime.MainWindow;
        if (main is null)
        {
            messageWindow.Show();
            return MessageBoxResult.None;
        }

        var result = await messageWindow.ShowDialog<MessageBoxResult>(main);
        return result;
    }

    public static async Task<MessageBoxResult> ShowAsync(
        Window owner,
        string message,
        string title,
        MessageBoxIcon icon = MessageBoxIcon.None,
        MessageBoxButton button = MessageBoxButton.OK,
        string? styleClass = null)
    {
        var messageWindow = new MessageBoxWindow(button)
        {
            Content = message,
            Title = title,
            MessageIcon = icon
        };
        if (!string.IsNullOrWhiteSpace(styleClass))
        {
            var styles = styleClass!.Split(Constants.SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
            messageWindow.Classes.AddRange(styles!);
        }
        var result = await messageWindow.ShowDialog<MessageBoxResult>(owner);
        return result;
    }

    public static async Task<MessageBoxResult> ShowOverlayAsync(
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