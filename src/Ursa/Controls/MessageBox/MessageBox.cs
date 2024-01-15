using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;

namespace Ursa.Controls;

public static class MessageBox
{
    public static async Task<MessageBoxResult> ShowAsync(
        string message, 
        string? title = null,
        MessageBoxIcon icon = MessageBoxIcon.None,
        MessageBoxButton button = MessageBoxButton.OKCancel)
    {
        var messageWindow = new MessageBoxWindow(button)
        {
            Content = message,
            Title = title,
            MessageIcon = icon,
        };
        var lifetime = Application.Current?.ApplicationLifetime;
        if (lifetime is IClassicDesktopStyleApplicationLifetime classLifetime)
        {
            var main = classLifetime.MainWindow;
            if (main is null)
            {
                messageWindow.Show();
                return MessageBoxResult.None;
            }
            else
            {
                var result = await messageWindow.ShowDialog<MessageBoxResult>(main);
                return result;
            }
        }
        else
        {
            return MessageBoxResult.None;
        }
    }
    
    public static async Task<MessageBoxResult> ShowAsync(
        Window owner,
        string message, 
        string title, 
        MessageBoxIcon icon = MessageBoxIcon.None,
        MessageBoxButton button = MessageBoxButton.OKCancel)
    {
        var messageWindow = new MessageBoxWindow(button)
        {
            Content = message,
            Title = title,
            MessageIcon = icon,
        };
        var result = await messageWindow.ShowDialog<MessageBoxResult>(owner);
        return result;
    }
}