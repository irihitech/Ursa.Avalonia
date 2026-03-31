using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Ursa.Common;
using Ursa.Controls.Options;

namespace Ursa.Controls;

public static class OverlayDrawer
{
    public static void ShowStandard<TView, TViewModel>(TViewModel vm, string? hostId = null, DrawerOptions? options = null)
        where TView : Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var drawer = new StandardDrawerControl
        {
            Content = new TView(),
            DataContext = vm
        };
        ConfigureStandardDrawer(drawer, options);
        host.AddDrawer(drawer);
    }

    public static void ShowStandard(Control control, object? vm, string? hostId = null,
        DrawerOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var drawer = new StandardDrawerControl
        {
            Content = control,
            DataContext = vm
        };
        ConfigureStandardDrawer(drawer, options);
        host.AddDrawer(drawer);
    }

    public static void ShowStandard(object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var view = host.GetDataTemplate(vm)?.Build(vm);
        if (view is null) view = new ContentControl { Padding = new Thickness(24) };
        view.DataContext = vm;
        var drawer = new StandardDrawerControl
        {
            Content = view,
            DataContext = vm
        };
        ConfigureStandardDrawer(drawer, options);
        host.AddDrawer(drawer);
    }

    public static Task<DialogResult> ShowStandardAsync<TView, TViewModel>(TViewModel vm, string? hostId = null,
        DrawerOptions? options = null)
        where TView : Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(DialogResult.None);
        var drawer = new StandardDrawerControl
        {
            Content = new TView(),
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureStandardDrawer(drawer, options);
        host.AddModalDrawer(drawer);
        return drawer.ShowAsync<DialogResult>();
    }

    public static Task<DialogResult> ShowStandardAsync(Control control, object? vm, string? hostId = null,
        DrawerOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(DialogResult.None);
        var drawer = new StandardDrawerControl
        {
            Content = control,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureStandardDrawer(drawer, options);
        host.AddModalDrawer(drawer);
        return drawer.ShowAsync<DialogResult>();
    }

    public static Task<DialogResult> ShowStandardAsync(object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(DialogResult.None);
        var view = host.GetDataTemplate(vm)?.Build(vm);
        if (view is null) view = new ContentControl { Padding = new Thickness(24) };
        view.DataContext = vm;
        var drawer = new StandardDrawerControl
        {
            Content = view,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureStandardDrawer(drawer, options);
        host.AddModalDrawer(drawer);
        return drawer.ShowAsync<DialogResult>();
    }

    public static void ShowCustom<TView, TViewModel>(TViewModel vm, string? hostId = null,
        DrawerOptions? options = null)
        where TView : Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var dialog = new CustomDrawerControl
        {
            Content = new TView(),
            DataContext = vm
        };
        ConfigureCustomDrawer(dialog, options);
        host.AddDrawer(dialog);
    }

    public static void ShowCustom(Control control, object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var dialog = new CustomDrawerControl
        {
            Content = control,
            DataContext = vm
        };
        ConfigureCustomDrawer(dialog, options);
        host.AddDrawer(dialog);
    }

    public static void ShowCustom(object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return;
        var view = host.GetDataTemplate(vm)?.Build(vm);
        if (view is null) view = new ContentControl { Padding = new Thickness(24) };
        view.DataContext = vm;
        var dialog = new CustomDrawerControl
        {
            Content = view,
            DataContext = vm
        };
        ConfigureCustomDrawer(dialog, options);
        host.AddDrawer(dialog);
    }

    public static Task<TResult?> ShowCustomAsync<TView, TViewModel, TResult>(TViewModel vm, string? hostId = null,
        DrawerOptions? options = null)
        where TView : Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult<TResult?>(default);
        var dialog = new CustomDrawerControl
        {
            Content = new TView(),
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureCustomDrawer(dialog, options);
        host.AddModalDrawer(dialog);
        return dialog.ShowAsync<TResult?>();
    }

    public static Task<TResult?> ShowCustomAsync<TResult>(Control control, object? vm, string? hostId = null,
        DrawerOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult<TResult?>(default);
        var dialog = new CustomDrawerControl
        {
            Content = control,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureCustomDrawer(dialog, options);
        host.AddModalDrawer(dialog);
        return dialog.ShowAsync<TResult?>();
    }

    public static Task<TResult?> ShowCustomAsync<TResult>(object? vm, string? hostId = null,
        DrawerOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult<TResult?>(default);
        var view = host.GetDataTemplate(vm)?.Build(vm);
        if (view is null) view = new ContentControl { Padding = new Thickness(24) };
        view.DataContext = vm;
        var dialog = new CustomDrawerControl
        {
            Content = view,
            DataContext = vm,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };
        ConfigureCustomDrawer(dialog, options);
        host.AddModalDrawer(dialog);
        return dialog.ShowAsync<TResult?>();
    }

    private static void ConfigureCustomDrawer(CustomDrawerControl drawer, DrawerOptions? options)
    {
        options ??= DrawerOptions.Default;
        drawer.Position = options.Position;
        drawer.IsCloseButtonVisible = options.IsCloseButtonVisible;
        drawer.CanLightDismiss = options.CanLightDismiss;
        drawer.CanResize = options.CanResize;
        if (options.Position == Position.Left || options.Position == Position.Right)
        {
            if(options.MinWidth is not null) drawer.MinWidth = options.MinWidth.Value;
            if(options.MaxWidth is not null) drawer.MaxWidth = options.MaxWidth.Value;
        }

        if (options.Position is Position.Top or Position.Bottom)
        {
            if(options.MinHeight is not null) drawer.MinHeight = options.MinHeight.Value;
            if(options.MaxHeight is not null) drawer.MaxHeight = options.MaxHeight.Value;
        }

        if (!string.IsNullOrWhiteSpace(options.StyleClass))
        {
            var styles = options.StyleClass!.Split(Constants.SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
            drawer.Classes.AddRange(styles);
        }
    }

    private static void ConfigureStandardDrawer(StandardDrawerControl drawer, DrawerOptions? options)
    {
        options ??= DrawerOptions.Default;
        drawer.Position = options.Position;
        drawer.IsCloseButtonVisible = options.IsCloseButtonVisible;
        drawer.CanLightDismiss = options.CanLightDismiss;
        drawer.Buttons = options.Buttons;
        drawer.Title = options.Title;
        drawer.CanResize = options.CanResize;
        if (options.Position == Position.Left || options.Position == Position.Right)
        {
            if(options.MinWidth is not null) drawer.MinWidth = options.MinWidth.Value;
            if(options.MaxWidth is not null) drawer.MaxWidth = options.MaxWidth.Value;
        }

        if (options.Position is Position.Top or Position.Bottom)
        {
            if(options.MinHeight is not null) drawer.MinHeight = options.MinHeight.Value;
            if(options.MaxHeight is not null) drawer.MaxHeight = options.MaxHeight.Value;
        }
        if (!string.IsNullOrWhiteSpace(options.StyleClass))
        {
            var styles = options.StyleClass!.Split(Constants.SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
            drawer.Classes.AddRange(styles);
        }
    }
}