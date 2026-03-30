using Ursa.Controls;
using Ursa.Controls.Options;

namespace Ursa.PrismExtension;

public interface IUrsaDrawerService
{
    [Obsolete("Use ShowStandard instead")]
    public void Show(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null);
    public void ShowCustom<TResult>(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null);
    [Obsolete("Use ShowStandardAsync instead")]
    public Task<DialogResult> ShowModal(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null);
    [Obsolete("Use ShowCustomAsync instead")]
    public Task<TResult?> ShowCustomModal<TResult>(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null);
    
    public void ShowStandard(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null);
    public Task<DialogResult> ShowStandardAsync(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null);
    public Task<TResult?> ShowCustomAsync<TResult>(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null);
}