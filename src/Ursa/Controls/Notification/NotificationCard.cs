using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Notifications;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

/// <summary>
/// Control that represents and displays a notification.
/// </summary>
[PseudoClasses(
    WindowNotificationManager.PC_TopLeft,
    WindowNotificationManager.PC_TopRight,
    WindowNotificationManager.PC_BottomLeft,
    WindowNotificationManager.PC_BottomRight,
    WindowNotificationManager.PC_TopCenter,
    WindowNotificationManager.PC_BottomCenter
)]
public class NotificationCard : MessageCard
{
    private NotificationPosition _position;

    public NotificationPosition Position
    {
        get => _position;
        set => SetAndRaise(PositionProperty, ref _position, value);
    }

    public static readonly DirectProperty<NotificationCard, NotificationPosition> PositionProperty =
        AvaloniaProperty.RegisterDirect<NotificationCard, NotificationPosition>(nameof(Position),
            o => o.Position, (o, v) => o.Position = v);

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);
        UpdatePseudoClasses(Position);
    }

    private void UpdatePseudoClasses(NotificationPosition position)
    {
        PseudoClasses.Set(WindowNotificationManager.PC_TopLeft, position == NotificationPosition.TopLeft);
        PseudoClasses.Set(WindowNotificationManager.PC_TopRight, position == NotificationPosition.TopRight);
        PseudoClasses.Set(WindowNotificationManager.PC_BottomLeft, position == NotificationPosition.BottomLeft);
        PseudoClasses.Set(WindowNotificationManager.PC_BottomRight, position == NotificationPosition.BottomRight);
        PseudoClasses.Set(WindowNotificationManager.PC_TopCenter, position == NotificationPosition.TopCenter);
        PseudoClasses.Set(WindowNotificationManager.PC_BottomCenter, position == NotificationPosition.BottomCenter);
    }
}