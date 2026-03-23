using Avalonia.Controls;
using Prism.Ioc;
using Ursa.Controls;
using Ursa.Controls.Options;

namespace Ursa.PrismExtension;

internal class UrsaDrawerService(IContainerExtension container) : IUrsaDrawerService
{
    [Obsolete("Use ShowStandard instead")]
    public void Show(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        ShowStandard(viewName, vm, hostId, options);
    }

    public void ShowCustom<TResult>(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        OverlayDrawer.ShowCustom(v, vm, hostId, options);
    }

    [Obsolete("Use ShowStandardAsync instead")]
    public Task<DialogResult> ShowModal(string viewName, object? vm, string? hostId = null,
        DrawerOptions? options = null)
    {
        return ShowStandardAsync(viewName, vm, hostId, options);
    }

    [Obsolete("Use ShowCustomAsync instead")]
    public Task<TResult?> ShowCustomModal<TResult>(string viewName, object? vm, string? hostId = null,
        DrawerOptions? options = null)
    {
        return ShowCustomAsync<TResult>(viewName, vm, hostId, options);
    }

    public void ShowStandard(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        OverlayDrawer.ShowStandard(v, vm, hostId, options);
    }

    public Task<DialogResult> ShowStandardAsync(string viewName, object? vm, string? hostId = null,
        DrawerOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        return OverlayDrawer.ShowStandardAsync(v, vm, hostId, options);
    }

    public Task<TResult?> ShowCustomAsync<TResult>(string viewName, object? vm, string? hostId = null,
        DrawerOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        return OverlayDrawer.ShowCustomAsync<TResult?>(v, vm, hostId, options);
    }
}