using Ursa.Controls;
using Ursa.Controls.Options;

namespace Ursa.PrismExtension;

public interface IUrsaDrawerService
{
    public Task<DialogResult> ShowDrawer(string viewName, object? vm, string? hostId = null, DefaultDrawerOptions? options = null);
    public Task<TResult?> ShowCustomDrawer<TResult>(string viewName, object? vm, string? hostId = null, CustomDrawerOptions? options = null);
}