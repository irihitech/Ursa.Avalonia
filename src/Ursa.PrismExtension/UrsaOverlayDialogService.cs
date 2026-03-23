using Avalonia.Controls;
using Prism.Ioc;
using Ursa.Controls;

namespace Ursa.PrismExtension;

internal class UrsaOverlayDialogService(IContainerExtension container) : IUrsaOverlayDialogService
{
    [Obsolete("Use ShowStandard instead")]
    public void Show(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        ShowStandard(viewName, vm, hostId, options);
    }

    public void ShowCustom(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        OverlayDialog.ShowCustom(v, vm, hostId, options);
    }

    [Obsolete("Use ShowStandardAsync instead")]
    public Task<DialogResult> ShowModal(string viewName, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null)
    {
        return ShowStandardAsync(viewName, vm, hostId, options);
    }

    [Obsolete("Use ShowCustomAsync instead")]
    public Task<TResult?> ShowCustomModal<TResult>(string viewName, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null)
    {
        return ShowCustomAsync<TResult>(viewName, vm, hostId, options);
    }

    public void ShowStandard(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        OverlayDialog.ShowStandard(v, vm, hostId, options);
    }

    public Task<DialogResult> ShowStandardAsync(string viewName, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        return OverlayDialog.ShowStandardAsync(v, vm, hostId, options);
    }

    public Task<TResult?> ShowCustomAsync<TResult>(string viewName, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        return OverlayDialog.ShowCustomAsync<TResult>(v, vm, hostId, options);
    }
}