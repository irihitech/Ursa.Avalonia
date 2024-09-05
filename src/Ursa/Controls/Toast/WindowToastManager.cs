using System.Collections;
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
/// An <see cref="IToastManager"/> that displays toasts in a <see cref="Window"/>.
/// </summary>
[TemplatePart("PART_Items", typeof(Panel))]
public class WindowToastManager : TemplatedControl, IManagedToastManager
{
    private IList? _items;

    /// <summary>
    /// Defines the <see cref="MaxItems"/> property.
    /// </summary>
    public static readonly StyledProperty<int> MaxItemsProperty =
        AvaloniaProperty.Register<WindowToastManager, int>(nameof(MaxItems), 5);

    /// <summary>
    /// Defines the maximum number of toasts visible at once.
    /// </summary>
    public int MaxItems
    {
        get => GetValue(MaxItemsProperty);
        set => SetValue(MaxItemsProperty, value);
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

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowToastManager"/> class.
    /// </summary>
    public WindowToastManager()
    {
    }

    static WindowToastManager()
    {
        HorizontalAlignmentProperty.OverrideDefaultValue<WindowToastManager>(HorizontalAlignment.Stretch);
        VerticalAlignmentProperty.OverrideDefaultValue<WindowToastManager>(VerticalAlignment.Stretch);
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var itemsControl = e.NameScope.Find<Panel>("PART_Items");
        _items = itemsControl?.Children;
    }

    /// <inheritdoc/>
    public void Show(IToast content)
    {
        Show(content, content.Type, content.Expiration, content.ShowClose, content.OnClick, content.OnClose);
    }

    /// <inheritdoc/>
    public void Show(object content)
    {
        if (content is IToast toast)
        {
            Show(toast, toast.Type, toast.Expiration, toast.ShowClose, toast.OnClick, toast.OnClose);
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
    /// <param name="showClose">whether to show the close button</param>
    /// <param name="onClick">an Action to be run when the toast is clicked</param>
    /// <param name="onClose">an Action to be run when the toast is closed</param>
    /// <param name="classes">style classes to apply</param>
    public async void Show(object content,
        NotificationType type,
        TimeSpan? expiration = null,
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

        toastControl.ToastClosed += (sender, _) =>
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

    /// <summary>
    /// Installs the <see cref="WindowToastManager"/> within the <see cref="AdornerLayer"/>
    /// </summary>
    private void InstallFromTopLevel(TopLevel topLevel)
    {
        topLevel.TemplateApplied += TopLevelOnTemplateApplied;
        var adorner = topLevel.FindDescendantOfType<VisualLayerManager>()?.AdornerLayer;
        if (adorner is not null)
        {
            adorner.Children.Add(this);
            AdornerLayer.SetAdornedElement(this, adorner);
        }
    }

    private void TopLevelOnTemplateApplied(object? sender, TemplateAppliedEventArgs e)
    {
        if (Parent is AdornerLayer adornerLayer)
        {
            adornerLayer.Children.Remove(this);
            AdornerLayer.SetAdornedElement(this, null);
        }

        // Reinstall toast manager on template reapplied.
        var topLevel = (TopLevel)sender!;
        topLevel.TemplateApplied -= TopLevelOnTemplateApplied;
        InstallFromTopLevel(topLevel);
    }
}