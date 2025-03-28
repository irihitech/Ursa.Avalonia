using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace Ursa.Controls;

/// <summary>
/// An <see cref="IToastManager"/> that displays toasts in a <see cref="Window"/>.
/// </summary>
public class WindowToastManager : WindowMessageManager, IToastManager
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WindowToastManager"/> class.
    /// </summary>
    public WindowToastManager()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowToastManager"/> class.
    /// </summary>
    /// <param name="host">The TopLevel that will host the control.</param>
    public WindowToastManager(TopLevel? host) : this()
    {
        if (host is not null)
        {
            InstallFromTopLevel(host);
        }
    }

    public WindowToastManager(VisualLayerManager? visualLayerManager) : base(visualLayerManager)
    {
    }

    /// <summary>
    /// Tries to get the <see cref="WindowToastManager"/> from a <see cref="Window"/> or <see cref="VisualLayerManager"/>.
    /// </summary>
    /// <param name="visual">A <see cref="Visual"/> that is either a <see cref="Window"/> or a <see cref="VisualLayerManager"/>.</param>
    /// <param name="manager">The existing <see cref="WindowToastManager"/> if found, or null if not found.</param>
    /// <returns>True if a <see cref="WindowToastManager"/> is found; otherwise, false.</returns>
    public static bool TryGetToastManager(Visual? visual, out WindowToastManager? manager)
    {
        manager = visual?.FindDescendantOfType<WindowToastManager>();
        return manager is not null;
    }

    /// <inheritdoc/>
    public void Show(IToast content)
    {
        Show(content, content.Type, content.Expiration,
            content.ShowIcon, content.ShowClose,
            content.OnClick, content.OnClose);
    }

    /// <inheritdoc/>
    public override void Show(object content)
    {
        if (content is IToast toast)
        {
            Show(toast, toast.Type, toast.Expiration,
                toast.ShowIcon, toast.ShowClose,
                toast.OnClick, toast.OnClose);
        }
        else
        {
            Show(content, NotificationType.Information);
        }
    }

    /// <summary>
    /// Shows a Toast
    /// </summary>
    /// <param name="content">the content of the toast</param>
    /// <param name="type">the type of the toast</param>
    /// <param name="expiration">the expiration time of the toast after which it will automatically close. If the value is Zero then the toast will remain open until the user closes it</param>
    /// <param name="showIcon">whether to show the icon</param>
    /// <param name="showClose">whether to show the close button</param>
    /// <param name="onClick">an Action to be run when the toast is clicked</param>
    /// <param name="onClose">an Action to be run when the toast is closed</param>
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

        var toastControl = new ToastCard
        {
            Content = content,
            NotificationType = type,
            ShowIcon = showIcon,
            ShowClose = showClose
        };

        // Add style classes if any
        if (classes is not null)
        {
            foreach (var @class in classes)
            {
                toastControl.Classes.Add(@class);
            }
        }

        toastControl.MessageClosed += (sender, _) =>
        {
            onClose?.Invoke();

            _items?.Remove(sender);
        };

        toastControl.PointerPressed += (_, _) => { onClick?.Invoke(); };

        Dispatcher.UIThread.Post(() =>
        {
            _items?.Add(toastControl);

            if (_items?.OfType<ToastCard>().Count(i => !i.IsClosing) > MaxItems)
            {
                _items.OfType<ToastCard>().First(i => !i.IsClosing).Close();
            }
        });

        if (expiration == TimeSpan.Zero)
        {
            return;
        }

        await Task.Delay(expiration ?? TimeSpan.FromSeconds(3));

        toastControl.Close();
    }
}