using Avalonia.Controls;

namespace Ursa.Controls;

public static partial class OverlayDialog
{
    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use Dialog.ShowDefault instead.")]
    public static void Show<TView, TViewModel>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null)
        where TView : Control, new()
    {
        ShowDefault<TView, TViewModel>(vm, hostId, options);
    }


    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use Dialog.ShowDefault instead.")]
    public static void Show(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null)
    {
        ShowDefault(control, vm, hostId, options);
    }


    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use Dialog.ShowDefault instead.")]
    public static void Show(object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        ShowDefault(vm, hostId, options);
    }


    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use Dialog.ShowDefaultAsync instead.")]
    public static Task<DialogResult> ShowModal<TView, TViewModel>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = null)
        where TView : Control, new()
    {
        return ShowDefaultAsync<TView, TViewModel>(vm, hostId, options, token);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use Dialog.ShowDefaultAsync instead.")]
    public static Task<DialogResult> ShowModal(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = null)
    {
        return ShowDefaultAsync(control, vm, hostId, options, token);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use Dialog.ShowCustomAsync instead.")]
    public static Task<TResult?> ShowCustomModal<TView, TViewModel, TResult>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = null)
        where TView : Control, new()
    {
        return ShowCustomAsync<TView, TViewModel, TResult>(vm, hostId, options, token);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use Dialog.ShowCustomAsync instead.")]
    public static Task<TResult?> ShowCustomModal<TResult>(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = null)
    {
        return ShowCustomAsync<TResult>(control, vm, hostId, options, token);
    }
}