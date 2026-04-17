using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

/// <summary>
/// Control that represents and displays a message.
/// </summary>
[PseudoClasses(PC_Information, PC_Success, PC_Warning, PC_Error)]
public abstract class MessageCard : ContentControl
{
    public const string PC_Information = ":information";
    public const string PC_Success = ":success";
    public const string PC_Warning = ":warning";
    public const string PC_Error = ":error";

    private bool _isClosing;

    static MessageCard()
    {
        CloseOnClickProperty.Changed.AddClassHandler<Button, bool>(OnCloseOnClickPropertyChanged);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageCard"/> class.
    /// </summary>
    public MessageCard()
    {
        UpdateNotificationType();
    }

    /// <summary>
    /// Gets the reason the message was closed.
    /// </summary>
    private MessageCloseReason _closeReason = MessageCloseReason.UserAction;

    /// <summary>
    /// Determines if the message is already closing.
    /// </summary>
    public bool IsClosing
    {
        get => _isClosing;
        private set => SetAndRaise(IsClosingProperty, ref _isClosing, value);
    }

    /// <summary>
    /// Defines the <see cref="IsClosing"/> property.
    /// </summary>
    public static readonly DirectProperty<MessageCard, bool> IsClosingProperty =
        AvaloniaProperty.RegisterDirect<MessageCard, bool>(nameof(IsClosing), o => o.IsClosing);

    /// <summary>
    /// Determines if the message is closed.
    /// </summary>
    public bool IsClosed
    {
        get => GetValue(IsClosedProperty);
        set => SetValue(IsClosedProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="IsClosed"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsClosedProperty =
        AvaloniaProperty.Register<MessageCard, bool>(nameof(IsClosed));

    /// <summary>
    /// Gets or sets the type of the message
    /// </summary>
    public NotificationType NotificationType
    {
        get => GetValue(NotificationTypeProperty);
        set => SetValue(NotificationTypeProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="NotificationType" /> property
    /// </summary>
    public static readonly StyledProperty<NotificationType> NotificationTypeProperty =
        AvaloniaProperty.Register<MessageCard, NotificationType>(nameof(NotificationType));

    public bool ShowIcon
    {
        get => GetValue(ShowIconProperty);
        set => SetValue(ShowIconProperty, value);
    }

    public static readonly StyledProperty<bool> ShowIconProperty =
        AvaloniaProperty.Register<MessageCard, bool>(nameof(ShowIcon), true);

    public bool ShowClose
    {
        get => GetValue(ShowCloseProperty);
        set => SetValue(ShowCloseProperty, value);
    }

    public static readonly StyledProperty<bool> ShowCloseProperty =
        AvaloniaProperty.Register<MessageCard, bool>(nameof(ShowClose), true);

    /// <summary>
    /// Defines the <see cref="MessageClosed"/> event.
    /// </summary>
    public static readonly RoutedEvent<MessageClosedEventArgs> MessageClosedEvent =
        RoutedEvent.Register<MessageCard, MessageClosedEventArgs>(nameof(MessageClosed), RoutingStrategies.Bubble);


    /// <summary>
    /// Raised when the <see cref="MessageCard"/> has closed.
    /// </summary>
    public event EventHandler<MessageClosedEventArgs>? MessageClosed
    {
        add => AddHandler(MessageClosedEvent, value);
        remove => RemoveHandler(MessageClosedEvent, value);
    }

    public static bool GetCloseOnClick(Button obj)
    {
        _ = obj ?? throw new ArgumentNullException(nameof(obj));
        return obj.GetValue(CloseOnClickProperty);
    }

    public static void SetCloseOnClick(Button obj, bool value)
    {
        _ = obj ?? throw new ArgumentNullException(nameof(obj));
        obj.SetValue(CloseOnClickProperty, value);
    }

    /// <summary>
    /// Defines the CloseOnClick property.
    /// </summary>
    public static readonly AttachedProperty<bool> CloseOnClickProperty =
        AvaloniaProperty.RegisterAttached<MessageCard, Button, bool>("CloseOnClick", defaultValue: false);

    private static void OnCloseOnClickPropertyChanged(Button button, AvaloniaPropertyChangedEventArgs<bool> e)
    {
        var value = e.GetNewValue<bool>();
        if (value)
        {
            button.Click += Button_Click;
        }
        else
        {
            button.Click -= Button_Click;
        }
    }

    /// <summary>
    /// Called when a button inside the Message is clicked.
    /// </summary>
    private static void Button_Click(object? sender, RoutedEventArgs e)
    {
        var btn = sender as ILogical;
        var message = btn?.GetLogicalAncestors().OfType<MessageCard>().FirstOrDefault();
        message?.Close();
    }

    /// <summary>
    /// Closes the <see cref="MessageCard"/> with the specified <see cref="MessageCloseReason"/>.
    /// </summary>
    /// <param name="reason">The reason the message is being closed.</param>
    public void Close(MessageCloseReason reason = MessageCloseReason.UserAction)
    {
        if (IsClosing)
        {
            return;
        }

        _closeReason = reason;
        IsClosing = true;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == ContentProperty && e.NewValue is IMessage message)
        {
            SetValue(NotificationTypeProperty, message.Type);
        }

        if (e.Property == NotificationTypeProperty)
        {
            UpdateNotificationType();
        }

        if (e.Property == IsClosedProperty)
        {
            // Notice that we are not setting IsClosed directly, we set IsClosing and then move to IsClosed with Animation. 
            // So this logic is not visible in csharp part. 
            if (!IsClosing && !IsClosed)
            {
                return;
            }

            RaiseEvent(new MessageClosedEventArgs(_closeReason) { RoutedEvent = MessageClosedEvent });
        }
    }

    private void UpdateNotificationType()
    {
        switch (NotificationType)
        {
            case NotificationType.Error:
                PseudoClasses.Add(PC_Error);
                break;

            case NotificationType.Information:
                PseudoClasses.Add(PC_Information);
                break;

            case NotificationType.Success:
                PseudoClasses.Add(PC_Success);
                break;

            case NotificationType.Warning:
                PseudoClasses.Add(PC_Warning);
                break;
        }
    }
}
