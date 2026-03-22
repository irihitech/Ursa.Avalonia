using Avalonia.Controls;
using Prism.Ioc;
using Ursa.Controls;

namespace Ursa.PrismExtension;

public class UrsaOverlayDialogService(IContainerExtension container): IUrsaOverlayDialogService
{
    public void Show(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        OverlayDialog.ShowStandard(v, vm, hostId, options);
    }

    public void ShowCustom(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        OverlayDialog.ShowCustom(v, vm, hostId, options);
    }
    
    public Task<DialogResult> ShowModal(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        return OverlayDialog.ShowStandardAsync(v, vm, hostId, options);
    }
    
    public Task<TResult?> ShowCustomModal<TResult>(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        return OverlayDialog.ShowCustomAsync<TResult>(v, vm, hostId, options);
    }
}