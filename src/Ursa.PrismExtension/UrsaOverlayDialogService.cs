using Avalonia.Controls;
using Prism.Ioc;
using Ursa.Controls;

namespace Ursa.PrismExtension;

public class UrsaOverlayDialogService(IContainerExtension container): IUrsaOverlayDialogService
{
    public void Show(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        OverlayDialog.Show(v, vm, hostId, options);
    }

    public void ShowCustom(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        OverlayDialog.ShowCustom(v, vm, hostId, options);
    }
    
    public Task<DialogResult> ShowModal(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        return OverlayDialog.ShowModal(v, vm, hostId, options);
    }
    
    public Task<TResult?> ShowCustomModal<TResult>(string viewName, object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        return OverlayDialog.ShowCustomModal<TResult>(v, vm, hostId, options);
    }
}