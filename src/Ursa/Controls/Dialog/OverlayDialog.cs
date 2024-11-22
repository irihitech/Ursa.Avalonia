using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Ursa.Common;

namespace Ursa.Controls;

public static class OverlayDialog
{
    public static void Show<TView, TViewModel>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null)
        where TView : Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var t = new DefaultDialogControl()
        {
            Content = new TView(),
            DataContext = vm,
        };
        ConfigureDefaultDialogControl(t, options);
        host.AddDialog(t);
    }
    
    public static void Show(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var t = new DefaultDialogControl()
        {
            Content = control,
            DataContext = vm,
        };
        ConfigureDefaultDialogControl(t, options);
        host.AddDialog(t);
        
    }
    
    public static void Show(object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var view = host.GetDataTemplate(vm)?.Build(vm);
        if (view is null) view = new ContentControl();
        view.DataContext = vm;
        var t = new DefaultDialogControl()
        {
            Content = view,
            DataContext = vm,
        };
        ConfigureDefaultDialogControl(t, options);
        host.AddDialog(t);
    }
    
    public static void ShowCustom<TView, TViewModel>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null)
        where TView: Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var t = new CustomDialogControl()
        {
            Content = new TView(),
            DataContext = vm,
        };
        ConfigureCustomDialogControl(t, options);
        host.AddDialog(t);
    }
    
    public static void ShowCustom(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var t = new CustomDialogControl()
        {
            Content = control,
            DataContext = vm,
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
        if (view is null) view = new ContentControl() { Padding = new Thickness(24) };
        view.DataContext = vm;
        var t = new CustomDialogControl()
        {
            Content = view,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureCustomDialogControl(t, options);
        host.AddDialog(t);
    }
    
    public static Task<DialogResult> ShowModal<TView, TViewModel>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = default)
        where TView: Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(DialogResult.None);
        var t = new DefaultDialogControl()
        {
            Content = new TView(),
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureDefaultDialogControl(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<DialogResult>(token);
    }
    
    public static Task<DialogResult> ShowModal(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = default)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(DialogResult.None);
        var t = new DefaultDialogControl()
        {
            Content = control,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureDefaultDialogControl(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<DialogResult>(token);
    }
    
    public static Task<TResult?> ShowCustomModal<TView, TViewModel, TResult>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = default)
        where TView: Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(default(TResult));
        var t = new CustomDialogControl()
        {
            Content = new TView(),
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureCustomDialogControl(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<TResult?>(token);
    }
    
    public static Task<TResult?> ShowCustomModal<TResult>(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = default)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(default(TResult));
        var t = new CustomDialogControl()
        {
            Content = control,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureCustomDialogControl(t, options);
        host.AddModalDialog(t);
        return t.ShowAsync<TResult?>(token);
    }
    
    public static Task<TResult?> ShowCustomModal<TResult>(object? vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = default)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(default(TResult));
        var view = host.GetDataTemplate(vm)?.Build(vm);
        if (view is null) view = new ContentControl() { Padding = new Thickness(24) };
        view.DataContext = vm;
        var t = new CustomDialogControl()
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
    
    private static void ConfigureDefaultDialogControl(DefaultDialogControl control, OverlayDialogOptions? options)
    {
        if (options is null) options = new OverlayDialogOptions();
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
        if (!string.IsNullOrWhiteSpace(options.StyleClass))
        {
            var styles = options.StyleClass!.Split(Constants.SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
            control.Classes.AddRange(styles);
        }
        DialogControlBase.SetCanDragMove(control, options.CanDragMove);
    }

    internal static T? Recall<T>(string? hostId) where T: Control
    {
        var host = OverlayDialogManager.GetHost(hostId, null);
        if (host is null) return null;
        var item = host.Recall<T>();
        return item;
    }
}
