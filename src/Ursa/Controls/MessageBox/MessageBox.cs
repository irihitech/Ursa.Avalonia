using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace Ursa.Controls.MessageBox;

public static class MessageBox
{
    public static async Task ShowAsync(string message)
    {
        var messageWindow = new MessageBoxWindow();
    }
}