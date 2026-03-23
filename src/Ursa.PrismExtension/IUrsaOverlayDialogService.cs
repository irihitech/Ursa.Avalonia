using Ursa.Controls;

namespace Ursa.PrismExtension;

public interface IUrsaOverlayDialogService
{
    [Obsolete("Use ShowStandard instead")]
    public void Show(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null);
    
    public void ShowCustom(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null);

    [Obsolete("Use ShowStandardAsync instead")]
    public Task<DialogResult> ShowModal(string viewName, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null);

    [Obsolete("Use ShowCustomAsync instead")]
    public Task<TResult?> ShowCustomModal<TResult>(string viewName, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null);
    
    public void ShowStandard(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null);

    public Task<DialogResult> ShowStandardAsync(string viewName, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null);

    public Task<TResult?> ShowCustomAsync<TResult>(string viewName, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null);
}