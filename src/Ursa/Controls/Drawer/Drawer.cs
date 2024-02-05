using Avalonia;
using Avalonia.Controls;
using Ursa.Common;
using Ursa.Controls.Options;

namespace Ursa.Controls;

public static class Drawer
{
    public static Task<DialogResult> Show<TView, TViewModel>(TViewModel vm, string? hostId = null, DefaultDrawerOptions? options = null)
        where TView: Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return Task.FromResult(default(DialogResult));
        var drawer = new DefaultDrawerControl()
        {
            Content = new TView(),
            DataContext = vm,
        };
        ConfigureDefaultDrawer(drawer, options);
        host.AddDrawer(drawer);
        return drawer.ShowAsync<DialogResult>();
    }

    public static Task<DialogResult> Show(Control control, object? vm, string? hostId = null,
        DefaultDrawerOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return Task.FromResult(default(DialogResult));
        var drawer = new DefaultDrawerControl()
        {
            Content = control,
            DataContext = vm,
        };
        ConfigureDefaultDrawer(drawer, options);
        host.AddDrawer(drawer);
        return drawer.ShowAsync<DialogResult>();
    }

    public static Task<DialogResult> Show(object? vm, string? hostId = null, DefaultDrawerOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return Task.FromResult(default(DialogResult));
        var view = host.GetDataTemplate(vm)?.Build(vm);
        if (view is null) view = new ContentControl() { Padding = new Thickness(24) };
        view.DataContext = vm;
        var drawer = new DefaultDrawerControl()
        {
            Content = view,
            DataContext = vm,
        };
        ConfigureDefaultDrawer(drawer, options);
        host.AddDrawer(drawer);
        return drawer.ShowAsync<DialogResult>();
    }
    
    public static Task<TResult?> ShowCustom<TView, TViewModel, TResult>(TViewModel vm, string? hostId = null, CustomDrawerOptions? options = null)
        where TView: Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return Task.FromResult(default(TResult));
        var dialog = new CustomDrawerControl()
        {
            Content = new TView(),
            DataContext = vm,
        };
        ConfigureCustomDrawer(dialog, options);
        host.AddDrawer(dialog);
        return dialog.ShowAsync<TResult>();
    }
    
    public static Task<TResult?> ShowCustom<TResult>(Control control, object? vm, string? hostId = null, CustomDrawerOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return Task.FromResult(default(TResult));
        var dialog = new CustomDrawerControl()
        {
            Content = control,
            DataContext = vm,
        };
        ConfigureCustomDrawer(dialog, options);
        host.AddDrawer(dialog);
        return dialog.ShowAsync<TResult>();
    }
    
    public static Task<TResult?> ShowCustom<TResult>(object? vm, string? hostId = null, CustomDrawerOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return Task.FromResult(default(TResult));
        var view = host.GetDataTemplate(vm)?.Build(vm);
        if (view is null) view = new ContentControl() { Padding = new Thickness(24) };
        view.DataContext = vm;
        var dialog = new CustomDrawerControl()
        {
            Content = view,
            DataContext = vm,
        };
        ConfigureCustomDrawer(dialog, options);
        host.AddDrawer(dialog);
        return dialog.ShowAsync<TResult>();
    }
    
    
    private static void ConfigureCustomDrawer(CustomDrawerControl drawer, CustomDrawerOptions? options)
    {
        options ??= CustomDrawerOptions.Default;
        drawer.Position = options.Position;
        drawer.CanClickOnMaskToClose = options.CanClickOnMaskToClose;
        drawer.IsCloseButtonVisible = options.IsCloseButtonVisible;
        drawer.ShowMask = options.ShowMask;
        drawer.CanLightDismiss = options.CanLightDismiss;
        if (options.Position == Position.Left || options.Position == Position.Right)
        {
            drawer.MinWidth = options.MinWidth ?? 0.0;
            drawer.MaxWidth = options.MaxWidth ?? double.PositiveInfinity;
        }
        if (options.Position is Position.Top or Position.Bottom)
        {
            drawer.MinHeight = options.MinHeight ?? 0.0;
            drawer.MaxHeight = options.MaxHeight ?? double.PositiveInfinity;
        }
    }
    
    private static void ConfigureDefaultDrawer(DefaultDrawerControl drawer, DefaultDrawerOptions? options)
    {
        options ??= DefaultDrawerOptions.Default;
        drawer.Position = options.Position;
        drawer.CanClickOnMaskToClose = options.CanClickOnMaskToClose;
        drawer.IsCloseButtonVisible = options.IsCloseButtonVisible;
        drawer.Buttons = options.Buttons;
        drawer.Title = options.Title;
        drawer.ShowMask = options.ShowMask;
        drawer.CanLightDismiss = options.CanLightDismiss;
        if (options.Position == Position.Left || options.Position == Position.Right)
        {
            drawer.MinWidth = options.MinWidth ?? 0.0;
            drawer.MaxWidth = options.MaxWidth ?? double.PositiveInfinity;
        }
        if (options.Position is Position.Top or Position.Bottom)
        {
            drawer.MinHeight = options.MinHeight ?? 0.0;
            drawer.MaxHeight = options.MaxHeight ?? double.PositiveInfinity;
        }
    }
}