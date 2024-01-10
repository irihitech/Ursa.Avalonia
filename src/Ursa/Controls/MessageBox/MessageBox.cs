using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace Ursa.Controls;

public static class MessageBox
{
    public static async Task<MessageBoxResult> ShowAsync(string message)
    {
        var messageWindow = new MessageBoxWindow()
        {
            Content = message
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
}