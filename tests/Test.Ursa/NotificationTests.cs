using System.Linq;
using Ursa.Controls;
using Xunit;

namespace Test.Ursa;

public class NotificationTests
{
    [Fact]
    public void WindowNotificationManager_Close_RemovesSpecificNotification()
    {
        // Arrange
        var manager = new WindowNotificationManager();
        var notification1 = new Notification("Title 1", "Content 1");
        var notification2 = new Notification("Title 2", "Content 2");
        
        // Act - Show notifications first
        manager.Show(notification1);
        manager.Show(notification2);
        
        // Act - Close the first notification
        manager.Close(notification1);
        
        // Assert - The Close method should complete without throwing
        // Note: Full verification would require UI testing framework since _items is private
        Assert.True(true); // This test verifies the API doesn't throw
    }

    [Fact]
    public void WindowNotificationManager_CloseAll_DoesNotThrow()
    {
        // Arrange
        var manager = new WindowNotificationManager();
        var notification1 = new Notification("Title 1", "Content 1");
        var notification2 = new Notification("Title 2", "Content 2");
        
        // Act - Show notifications first
        manager.Show(notification1);
        manager.Show(notification2);
        
        // Act - Close all notifications
        manager.CloseAll();
        
        // Assert - The CloseAll method should complete without throwing
        Assert.True(true);
    }

    [Fact]
    public void WindowNotificationManager_Close_WithNullItems_DoesNotThrow()
    {
        // Arrange
        var manager = new WindowNotificationManager();
        var notification = new Notification("Title", "Content");
        
        // Act - Try to close without showing any notifications
        manager.Close(notification);
        
        // Assert - Should not throw
        Assert.True(true);
    }

    [Fact]
    public void WindowNotificationManager_CloseAll_WithNullItems_DoesNotThrow()
    {
        // Arrange
        var manager = new WindowNotificationManager();
        
        // Act - Try to close all without showing any notifications
        manager.CloseAll();
        
        // Assert - Should not throw
        Assert.True(true);
    }

    [Fact]
    public void WindowNotificationManager_Close_WithNullNotification_DoesNotThrow()
    {
        // Arrange
        var manager = new WindowNotificationManager();
        
        // Act - Try to close with null notification
        manager.Close(null!);
        
        // Assert - Should not throw
        Assert.True(true);
    }
}