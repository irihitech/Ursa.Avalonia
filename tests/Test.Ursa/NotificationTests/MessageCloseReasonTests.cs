using Avalonia.Controls.Notifications;
using Ursa.Controls;

namespace Test.Ursa.NotificationTests;

public class MessageCloseReasonTests
{
    [Fact]
    public void Notification_SupportsOnCloseWithReason()
    {
        MessageCloseReason? capturedReason = null;
        var notification = new global::Ursa.Controls.Notification(
            "Test Title",
            "Test Content",
            NotificationType.Information,
            TimeSpan.FromSeconds(1),
            true,
            null,
            null,
            reason => capturedReason = reason
        );

        Assert.NotNull(notification.OnCloseWithReason);
        notification.OnCloseWithReason?.Invoke(MessageCloseReason.UserAction);
        Assert.Equal(MessageCloseReason.UserAction, capturedReason);
    }

    [Fact]
    public void Toast_SupportsOnCloseWithReason()
    {
        MessageCloseReason? capturedReason = null;
        var toast = new Toast(
            "Test Content",
            NotificationType.Information,
            TimeSpan.FromSeconds(1),
            true,
            null,
            null,
            reason => capturedReason = reason
        );

        Assert.NotNull(toast.OnCloseWithReason);
        toast.OnCloseWithReason?.Invoke(MessageCloseReason.Timeout);
        Assert.Equal(MessageCloseReason.Timeout, capturedReason);
    }

    [Fact]
    public void Notification_BackwardCompatibility_OnCloseStillWorks()
    {
        bool closeCalled = false;
        var notification = new global::Ursa.Controls.Notification(
            "Test Title",
            "Test Content",
            NotificationType.Information,
            TimeSpan.FromSeconds(1),
            true,
            null,
            () => closeCalled = true
        );

        Assert.NotNull(notification.OnClose);
        notification.OnClose?.Invoke();
        Assert.True(closeCalled);
    }

    [Fact]
    public void Toast_BackwardCompatibility_OnCloseStillWorks()
    {
        bool closeCalled = false;
        var toast = new Toast(
            "Test Content",
            NotificationType.Information,
            TimeSpan.FromSeconds(1),
            true,
            null,
            () => closeCalled = true
        );

        Assert.NotNull(toast.OnClose);
        toast.OnClose?.Invoke();
        Assert.True(closeCalled);
    }

    [Fact]
    public void MessageCloseReason_HasExpectedValues()
    {
        // Verify enum has the expected values
        Assert.True(Enum.IsDefined(typeof(MessageCloseReason), MessageCloseReason.Timeout));
        Assert.True(Enum.IsDefined(typeof(MessageCloseReason), MessageCloseReason.UserAction));
        Assert.True(Enum.IsDefined(typeof(MessageCloseReason), MessageCloseReason.Displaced));
    }
}
