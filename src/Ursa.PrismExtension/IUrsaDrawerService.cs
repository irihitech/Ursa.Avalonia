using Ursa.Controls;
using Ursa.Controls.Options;

namespace Ursa.PrismExtension;

public interface IUrsaDrawerService
{
    public void Show(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null);
    public void ShowCustom<TResult>(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null);
    public Task<DialogResult> ShowModal(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null);
    public Task<TResult?> ShowCustomModal<TResult>(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null);
}