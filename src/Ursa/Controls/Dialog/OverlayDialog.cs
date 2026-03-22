using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Ursa.Common;
using Ursa.Controls.OverlayShared;

namespace Ursa.Controls;

public static partial class OverlayDialog
{
    public static void ShowStandard<TView, TViewModel>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null)
        where TView : Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var t = new StandardDialogControl
        {
            Content = new TView(),
            DataContext = vm
        };
        ConfigureStandardDialogControl(t, options);
        host.AddDialog(t);
    }
    
    public static void ShowStandard(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var t = new StandardDialogControl
        {
            Content = control,
            DataContext = vm
        };
        ConfigureStandardDialogControl(t, options);
        host.AddDialog(t);
    }

    public static void ShowStandard(object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var view = host.GetDataTemplate(vm)?.Build(vm);
        if (view is null) view = new ContentControl();
        view.DataContext = vm;
        var t = new StandardDialogControl
        {
            Content = view,
            DataContext = vm
        };
        ConfigureStandardDialogControl(t, options);
        host.AddDialog(t);
    }

    public static void ShowCustom<TView, TViewModel>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null)
        where TView : Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var t = new CustomDialogControl
        {
            Content = new TView(),
            DataContext = vm
        };
        ConfigureCustomDialogControl(t, options);
        host.AddDialog(t);
    }

    public static void ShowCustom(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var t = new CustomDialogControl
        {
            Content = control,
            DataContext = vm
        };
        ConfigureCustomDialogControl(t, options);
        host.AddDialog(t);
    }

    public static void ShowCustom(object? vm, string? hostId = null,
        OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var view = host.GetDataTemplate(vm)?.Build(vm);
        if (view is null) view = new ContentControl { Padding = new Thickness(24) };
        view.DataContext = vm;
        var t = new CustomDialogControl
        {
            Content = view,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureCustomDialogControl(t, options);
        host.AddDialog(t);
    }


    public static Task<DialogResult> ShowStandardAsync<TView, TViewModel>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = null)
        where TView : Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(DialogResult.None);
        var t = new StandardDialogControl
        {
            Content = new TView(),
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureStandardDialogControl(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<DialogResult>(token);
    }


    public static Task<DialogResult> ShowStandardAsync(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(DialogResult.None);
        var t = new StandardDialogControl
        {
            Content = control,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureStandardDialogControl(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<DialogResult>(token);
    }


    public static Task<TResult?> ShowCustomAsync<TView, TViewModel, TResult>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = null)
        where TView : Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(default(TResult));
        var t = new CustomDialogControl
        {
            Content = new TView(),
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureCustomDialogControl(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<TResult?>(token);
    }


    public static Task<TResult?> ShowCustomAsync<TResult>(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(default(TResult));
        var t = new CustomDialogControl
        {
            Content = control,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureCustomDialogControl(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<TResult?>(token);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use Dialog.ShowCustomAsync instead.")]
    public static Task<TResult?> ShowCustomModal<TResult>(object? vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = null)
    {
        return ShowCustomAsync<TResult>(vm, hostId, options, token);
    }

    public static Task<TResult?> ShowCustomAsync<TResult>(object? vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(default(TResult));
        var view = host.GetDataTemplate(vm)?.Build(vm);
        if (view is null) view = new ContentControl { Padding = new Thickness(24) };
        view.DataContext = vm;
        var t = new CustomDialogControl
        {
            Content = view,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureCustomDialogControl(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<TResult?>(token);
    }

    private static void ConfigureCustomDialogControl(CustomDialogControl control, OverlayDialogOptions? options)
    {
        options ??= OverlayDialogOptions.Default;
        control.IsFullScreen = options.FullScreen;
        if (options.FullScreen)
        {
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            control.VerticalAlignment = VerticalAlignment.Stretch;
        }

        control.HorizontalAnchor = options.HorizontalAnchor;
        control.VerticalAnchor = options.VerticalAnchor;
        control.ActualHorizontalAnchor = options.HorizontalAnchor;
        control.ActualVerticalAnchor = options.VerticalAnchor;
        control.HorizontalOffset =
            control.HorizontalAnchor == HorizontalPosition.Center ? null : options.HorizontalOffset;
        control.VerticalOffset =
            options.VerticalAnchor == VerticalPosition.Center ? null : options.VerticalOffset;
        control.IsCloseButtonVisible = options.IsCloseButtonVisible;
        control.CanLightDismiss = options.CanLightDismiss;
        control.CanResize = options.CanResize;
        if (!string.IsNullOrWhiteSpace(options.StyleClass))
        {
            var styles = options.StyleClass!.Split(Constants.SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
            control.Classes.AddRange(styles);
        }

        DialogControlBase.SetCanDragMove(control, options.CanDragMove);
    }

    private static void ConfigureStandardDialogControl(StandardDialogControl control, OverlayDialogOptions? options)
    {
        options ??= new OverlayDialogOptions();
        control.IsFullScreen = options.FullScreen;
        if (options.FullScreen)
        {
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            control.VerticalAlignment = VerticalAlignment.Stretch;
        }
        control.HorizontalAnchor = options.HorizontalAnchor;
        control.VerticalAnchor = options.VerticalAnchor;
        control.ActualHorizontalAnchor = options.HorizontalAnchor;
        control.ActualVerticalAnchor = options.VerticalAnchor;
        control.HorizontalOffset =
            control.HorizontalAnchor == HorizontalPosition.Center ? null : options.HorizontalOffset;
        control.VerticalOffset =
            options.VerticalAnchor == VerticalPosition.Center ? null : options.VerticalOffset;
        control.Mode = options.Mode;
        control.Buttons = options.Buttons;
        control.Title = options.Title;
        control.CanLightDismiss = options.CanLightDismiss;
        control.IsCloseButtonVisible = options.IsCloseButtonVisible;
        control.CanResize = options.CanResize;
        if (options.OnDialogControlClosed != null)
        {
            control.AddHandler(OverlayFeedbackElement.ClosedEvent, options.OnDialogControlClosed);
            control.AddHandler(OverlayFeedbackElement.ClosedEvent, (s, _) =>
            {
                if (s is not DialogControlBase dc) return;
                dc.RemoveHandler(OverlayFeedbackElement.ClosedEvent, options.OnDialogControlClosed);
            });
        }

        if (!string.IsNullOrWhiteSpace(options.StyleClass))
        {
            var styles = options.StyleClass!.Split(Constants.SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
            control.Classes.AddRange(styles);
        }

        DialogControlBase.SetCanDragMove(control, options.CanDragMove);
    }

    internal static T? Recall<T>(string? hostId) where T : Control
    {
        var host = OverlayDialogManager.GetHost(hostId, null);
        if (host is null) return null;
        var item = host.Recall<T>();
        return item;
    }
}