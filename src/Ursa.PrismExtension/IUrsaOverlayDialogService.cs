using Ursa.Controls;

namespace Ursa.PrismExtension;

public interface IUrsaOverlayDialogService
{
    public void Show(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null);

    public void ShowCustom(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null);

    public Task<DialogResult> ShowModal(string viewName, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null);

    public Task<TResult?> ShowCustomModal<TResult>(string viewName, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null);
}