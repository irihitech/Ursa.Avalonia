using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Avalonia.VisualTree;
using UrsaNotification = Ursa.Controls.Notification;
using UrsaNotificationCard = Ursa.Controls.NotificationCard;
using UrsaWindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace HeadlessTest.Ursa.Controls.NotificationTests;

public class Tests
{
    [AvaloniaFact]
    public void CloseAll_Should_Mark_All_Notifications_As_Closing()
    {
        var window = new Window();
        var manager = new UrsaWindowNotificationManager(window);
        window.Show();

        var notification1 = new UrsaNotification("Title1", "Content1", expiration: TimeSpan.Zero);
        var notification2 = new UrsaNotification("Title2", "Content2", expiration: TimeSpan.Zero);

        manager.Show(notification1);
        manager.Show(notification2);

        Dispatcher.UIThread.RunJobs();

        var cards = window.GetVisualDescendants().OfType<UrsaNotificationCard>().ToList();
        Assert.Equal(2, cards.Count);
        Assert.All(cards, c => Assert.False(c.IsClosing));

        manager.CloseAll();

        Assert.All(cards, c => Assert.True(c.IsClosing));
    }

    [AvaloniaFact]
    public void Close_Should_Mark_Specific_Notification_As_Closing()
    {
        var window = new Window();
        var manager = new UrsaWindowNotificationManager(window);
        window.Show();

        var notification1 = new UrsaNotification("Title1", "Content1", expiration: TimeSpan.Zero);
        var notification2 = new UrsaNotification("Title2", "Content2", expiration: TimeSpan.Zero);

        manager.Show(notification1);
        manager.Show(notification2);

        Dispatcher.UIThread.RunJobs();

        var cards = window.GetVisualDescendants().OfType<UrsaNotificationCard>().ToList();
        Assert.Equal(2, cards.Count);

        manager.Close(notification1);

        var card1 = cards.FirstOrDefault(c => c.Content == notification1);
        var card2 = cards.FirstOrDefault(c => c.Content == notification2);

        Assert.NotNull(card1);
        Assert.NotNull(card2);
        Assert.True(card1.IsClosing);
        Assert.False(card2.IsClosing);
    }

    [AvaloniaFact]
    public void Close_With_Nonexistent_Notification_Should_Not_Throw()
    {
        var window = new Window();
        var manager = new UrsaWindowNotificationManager(window);
        window.Show();

        var notification = new UrsaNotification("Title", "Content", expiration: TimeSpan.Zero);

        // Should not throw when notification was never shown
        manager.Close(notification);

        window.Close();
    }

    [AvaloniaFact]
    public void CloseAll_With_No_Notifications_Should_Not_Throw()
    {
        var window = new Window();
        var manager = new UrsaWindowNotificationManager(window);
        window.Show();

        // Should not throw when no notifications are shown
        manager.CloseAll();

        window.Close();
    }
}

