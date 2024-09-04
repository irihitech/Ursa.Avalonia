using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

/// <summary>
/// Control that represents and displays a toast.
/// </summary>
[PseudoClasses(PC_Information, PC_Success, PC_Warning, PC_Error)]
public class ToastCard : ContentControl
{
    public const string PC_Information = ":information";
    public const string PC_Success = ":success";
    public const string PC_Warning = ":warning";
    public const string PC_Error = ":error";

    private bool _isClosing;

    static ToastCard()
    {
        CloseOnClickProperty.Changed.AddClassHandler<Button>(OnCloseOnClickPropertyChanged);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ToastCard"/> class.
    /// </summary>
    public ToastCard()
    {
        UpdateNotificationType();
    }

    /// <summary>
    /// Determines if the toast is already closing.
    /// </summary>
    public bool IsClosing
    {
        get => _isClosing;
        private set => SetAndRaise(IsClosingProperty, ref _isClosing, value);
    }

    /// <summary>
    /// Defines the <see cref="IsClosing"/> property.
    /// </summary>
    public static readonly DirectProperty<ToastCard, bool> IsClosingProperty =
        AvaloniaProperty.RegisterDirect<ToastCard, bool>(nameof(IsClosing), o => o.IsClosing);

    /// <summary>
    /// Determines if the toast is closed.
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
        AvaloniaProperty.Register<ToastCard, bool>(nameof(IsClosed));

    /// <summary>
    /// Gets or sets the type of the toast
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
        AvaloniaProperty.Register<ToastCard, NotificationType>(nameof(NotificationType));
    public bool ShowClose
    {
        get => GetValue(ShowCloseProperty);
        set => SetValue(ShowCloseProperty, value);
    }

    public static readonly StyledProperty<bool> ShowCloseProperty =
        AvaloniaProperty.Register<ToastCard, bool>(nameof(ShowClose), true);

    /// <summary>
    /// Defines the <see cref="ToastClosed"/> event.
    /// </summary>
    public static readonly RoutedEvent<RoutedEventArgs> ToastClosedEvent =
        RoutedEvent.Register<ToastCard, RoutedEventArgs>(nameof(ToastClosed), RoutingStrategies.Bubble);


    /// <summary>
    /// Raised when the <see cref="ToastCard"/> has closed.
    /// </summary>
    public event EventHandler<RoutedEventArgs>? ToastClosed
    {
        add => AddHandler(ToastClosedEvent, value);
        remove => RemoveHandler(ToastClosedEvent, value);
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
        AvaloniaProperty.RegisterAttached<ToastCard, Button, bool>("CloseOnClick", defaultValue: false);

    private static void OnCloseOnClickPropertyChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
    {
        var button = (Button)d;
        var value = (bool)e.NewValue!;
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
    /// Called when a button inside the Toast is clicked.
    /// </summary>
    private static void Button_Click(object? sender, RoutedEventArgs e)
    {
        var btn = sender as ILogical;
        var toast = btn?.GetLogicalAncestors().OfType<ToastCard>().FirstOrDefault();
        toast?.Close();
    }

    /// <summary>
    /// Closes the <see cref="ToastCard"/>.
    /// </summary>
    public void Close()
    {
        if (IsClosing)
        {
            return;
        }

        IsClosing = true;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == ContentProperty && e.NewValue is IToast toast)
        {
            SetValue(NotificationTypeProperty, toast.Type);
        }

        if (e.Property == NotificationTypeProperty)
        {
            UpdateNotificationType();
        }

        if (e.Property == IsClosedProperty)
        {
            if (!IsClosing && !IsClosed)
            {
                return;
            }

            RaiseEvent(new RoutedEventArgs(ToastClosedEvent));
        }
    }

    private void UpdateNotificationType()
    {
        switch (NotificationType)
        {
            case NotificationType.Error:
                PseudoClasses.Add(":error");
                break;

            case NotificationType.Information:
                PseudoClasses.Add(":information");
                break;

            case NotificationType.Success:
                PseudoClasses.Add(":success");
                break;

            case NotificationType.Warning:
                PseudoClasses.Add(":warning");
                break;
        }
    }
}