using Avalonia.Controls;

namespace Ursa.Controls;

public static partial class OverlayDialog
{
    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDialog.ShowStandard instead.")]
    public static void Show<TView, TViewModel>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null)
        where TView : Control, new()
    {
        ShowStandard<TView, TViewModel>(vm, hostId, options);
    }


    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDialog.ShowStandard instead.")]
    public static void Show(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null)
    {
        ShowStandard(control, vm, hostId, options);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDialog.ShowCustomAsync instead.")]
    public static Task<TResult?> ShowCustomModal<TResult>(object? vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = null)
    {
        return ShowCustomAsync<TResult>(vm, hostId, options, token);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDialog.ShowStandard instead.")]
    public static void Show(object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        ShowStandard(vm, hostId, options);
    }


    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDialog.ShowStandardAsync instead.")]
    public static Task<DialogResult> ShowModal<TView, TViewModel>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = null)
        where TView : Control, new()
    {
        return ShowStandardAsync<TView, TViewModel>(vm, hostId, options, token);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDialog.ShowStandardAsync instead.")]
    public static Task<DialogResult> ShowModal(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = null)
    {
        return ShowStandardAsync(control, vm, hostId, options, token);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDialog.ShowCustomAsync instead.")]
    public static Task<TResult?> ShowCustomModal<TView, TViewModel, TResult>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = null)
        where TView : Control, new()
    {
        return ShowCustomAsync<TView, TViewModel, TResult>(vm, hostId, options, token);
    }

    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use OverlayDialog.ShowCustomAsync instead.")]
    public static Task<TResult?> ShowCustomModal<TResult>(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = null)
    {
        return ShowCustomAsync<TResult>(control, vm, hostId, options, token);
    }
}