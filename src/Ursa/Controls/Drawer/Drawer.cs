using Avalonia.Controls;
using Ursa.Controls.Options;

// ReSharper disable RedundantExplicitArrayCreation

namespace Ursa.Controls;

[Obsolete("Use OverlayDrawer instead. This will be removed in Ursa2.0 lifecycle.")]
public static class Drawer
{
    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDrawer.ShowDefault instead. ")]
    public static void Show<TView, TViewModel>(TViewModel vm, string? hostId = null, DrawerOptions? options = null)
        where TView : Control, new()
    {
        OverlayDrawer.ShowDefault<TView, TViewModel>(vm, hostId, options);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDrawer.ShowDefault instead. ")]
    public static void Show(Control control, object? vm, string? hostId = null,
        DrawerOptions? options = null)
    {
        OverlayDrawer.ShowDefault(control, vm, hostId, options);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDrawer.ShowDefault instead. ")]
    public static void Show(object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        OverlayDrawer.ShowDefault(vm, hostId, options);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDrawer.ShowDefaultAsync instead. ")]
    public static Task<DialogResult> ShowModal<TView, TViewModel>(TViewModel vm, string? hostId = null,
        DrawerOptions? options = null)
        where TView : Control, new()
    {
        return OverlayDrawer.ShowDefaultAsync<TView, TViewModel>(vm, hostId, options);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDrawer.ShowDefaultAsync instead. ")]
    public static Task<DialogResult> ShowModal(Control control, object? vm, string? hostId = null,
        DrawerOptions? options = null)
    {
        return OverlayDrawer.ShowDefaultAsync(control, vm, hostId, options);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDrawer.ShowDefaultAsync instead. ")]
    public static Task<DialogResult> ShowModal(object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        return OverlayDrawer.ShowDefaultAsync(vm, hostId, options);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDrawer.ShowCustom instead. ")]
    public static void ShowCustom<TView, TViewModel>(TViewModel vm, string? hostId = null,
        DrawerOptions? options = null)
        where TView : Control, new()
    {
        OverlayDrawer.ShowCustom<TView, TViewModel>(vm, hostId, options);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDrawer.ShowCustom instead. ")]
    public static void ShowCustom(Control control, object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        OverlayDrawer.ShowCustom(control, vm, hostId, options);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDrawer.ShowCustom instead. ")]
    public static void ShowCustom(object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        OverlayDrawer.ShowCustom(vm, hostId, options);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDrawer.ShowCustomAsync instead. ")]
    public static Task<TResult?> ShowCustomModal<TView, TViewModel, TResult>(TViewModel vm, string? hostId = null,
        DrawerOptions? options = null)
        where TView : Control, new()
    {
        return OverlayDrawer.ShowCustomAsync<TView, TViewModel, TResult>(vm, hostId, options);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDrawer.ShowCustomAsync instead. ")]
    public static Task<TResult?> ShowCustomModal<TResult>(Control control, object? vm, string? hostId = null,
        DrawerOptions? options = null)
    {
        return OverlayDrawer.ShowCustomAsync<TResult>(control, vm, hostId, options);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDrawer.ShowCustomAsync instead. ")]
    public static Task<TResult?> ShowCustomModal<TResult>(object? vm, string? hostId = null,
        DrawerOptions? options = null)
    {
        return OverlayDrawer.ShowCustomAsync<TResult>(vm, hostId, options);
    }
}