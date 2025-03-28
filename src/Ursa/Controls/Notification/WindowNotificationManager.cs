using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace Ursa.Controls;

/// <summary>
/// An <see cref="INotificationManager"/> that displays notifications in a <see cref="Window"/>.
/// </summary>
[PseudoClasses(PC_TopLeft, PC_TopRight, PC_BottomLeft, PC_BottomRight, PC_TopCenter, PC_BottomCenter)]
public class WindowNotificationManager : WindowMessageManager, INotificationManager
{
    public const string PC_TopLeft = ":topleft";
    public const string PC_TopRight = ":topright";
    public const string PC_BottomLeft = ":bottomleft";
    public const string PC_BottomRight = ":bottomright";
    public const string PC_TopCenter = ":topcenter";
    public const string PC_BottomCenter = ":bottomcenter";

    /// <summary>
    /// Defines the <see cref="Position"/> property.
    /// </summary>
    public static readonly StyledProperty<NotificationPosition> PositionProperty =
        AvaloniaProperty.Register<WindowNotificationManager, NotificationPosition>(nameof(Position),
            NotificationPosition.TopRight);

    /// <summary>
    /// Defines which corner of the screen notifications can be displayed in.
    /// </summary>
    /// <seealso cref="NotificationPosition"/>
    public NotificationPosition Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowNotificationManager"/> class.
    /// </summary>
    public WindowNotificationManager()
    {
        UpdatePseudoClasses(Position);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowNotificationManager"/> class.
    /// </summary>
    /// <param name="host">The TopLevel that will host the control.</param>
    public WindowNotificationManager(TopLevel? host) : this()
    {
        if (host is not null)
        {
            InstallFromTopLevel(host);
        }
    }

    public WindowNotificationManager(VisualLayerManager? visualLayerManager) : base(visualLayerManager)
    {
        UpdatePseudoClasses(Position);
    }

    static WindowNotificationManager()
    {
        HorizontalAlignmentProperty.OverrideDefaultValue<WindowNotificationManager>(HorizontalAlignment.Stretch);
        VerticalAlignmentProperty.OverrideDefaultValue<WindowNotificationManager>(VerticalAlignment.Stretch);
    }

    /// <summary>
    /// Tries to get the <see cref="WindowNotificationManager"/> from a <see cref="Visual"/>.
    /// </summary>
    /// <param name="visual">A <see cref="Visual"/> that is either a <see cref="Window"/> or a <see cref="VisualLayerManager"/>.</param>
    /// <param name="manager">The existing <see cref="WindowNotificationManager"/> if found, or null if not found.</param>
    /// <returns>True if a <see cref="WindowNotificationManager"/> is found; otherwise, false.</returns>
    public static bool TryGetNotificationManager(Visual? visual, out WindowNotificationManager? manager)
    {
        manager = visual?.FindDescendantOfType<WindowNotificationManager>();
        return manager is not null;
    }

    /// <inheritdoc/>
    public void Show(INotification content)
    {
        Show(content, content.Type, content.Expiration,
            content.ShowIcon, content.ShowClose,
            content.OnClick, content.OnClose);
    }

    /// <inheritdoc/>
    public override void Show(object content)
    {
        if (content is INotification notification)
        {
            Show(notification, notification.Type, notification.Expiration,
                notification.ShowIcon, notification.ShowClose,
                notification.OnClick, notification.OnClose);
        }
        else
        {
            Show(content, NotificationType.Information);
        }
    }

    /// <summary>
    /// Shows a Notification
    /// </summary>
    /// <param name="content">the content of the notification</param>
    /// <param name="type">the type of the notification</param>
    /// <param name="expiration">the expiration time of the notification after which it will automatically close. If the value is Zero then the notification will remain open until the user closes it</param>
    /// <param name="showIcon">whether to show the icon</param>
    /// <param name="showClose">whether to show the close button</param>
    /// <param name="onClick">an Action to be run when the notification is clicked</param>
    /// <param name="onClose">an Action to be run when the notification is closed</param>
    /// <param name="classes">style classes to apply</param>
    public async void Show(
        object content,
        NotificationType type,
        TimeSpan? expiration = null,
        bool showIcon = true,
        bool showClose = true,
        Action? onClick = null,
        Action? onClose = null,
        string[]? classes = null)
    {
        Dispatcher.UIThread.VerifyAccess();

        var notificationControl = new NotificationCard
        {
            Content = content,
            NotificationType = type,
            ShowIcon = showIcon,
            ShowClose = showClose,
            [!NotificationCard.PositionProperty] = this[!PositionProperty]
        };

        // Add style classes if any
        if (classes is not null)
        {
            foreach (var @class in classes)
            {
                notificationControl.Classes.Add(@class);
            }
        }

        notificationControl.MessageClosed += (sender, _) =>
        {
            onClose?.Invoke();

            _items?.Remove(sender);
        };

        notificationControl.PointerPressed += (_, _) => { onClick?.Invoke(); };

        Dispatcher.UIThread.Post(() =>
        {
            _items?.Add(notificationControl);

            if (_items?.OfType<NotificationCard>().Count(i => !i.IsClosing) > MaxItems)
            {
                _items.OfType<NotificationCard>().First(i => !i.IsClosing).Close();
            }
        });

        if (expiration == TimeSpan.Zero)
        {
            return;
        }

        await Task.Delay(expiration ?? TimeSpan.FromSeconds(3));

        notificationControl.Close();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == PositionProperty)
        {
            UpdatePseudoClasses(change.GetNewValue<NotificationPosition>());
        }
    }

    private void UpdatePseudoClasses(NotificationPosition position)
    {
        PseudoClasses.Set(PC_TopLeft, position == NotificationPosition.TopLeft);
        PseudoClasses.Set(PC_TopRight, position == NotificationPosition.TopRight);
        PseudoClasses.Set(PC_BottomLeft, position == NotificationPosition.BottomLeft);
        PseudoClasses.Set(PC_BottomRight, position == NotificationPosition.BottomRight);
        PseudoClasses.Set(PC_TopCenter, position == NotificationPosition.TopCenter);
        PseudoClasses.Set(PC_BottomCenter, position == NotificationPosition.BottomCenter);
    }
}