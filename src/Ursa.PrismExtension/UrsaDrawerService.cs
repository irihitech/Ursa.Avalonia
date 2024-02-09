using Avalonia.Controls;
using Prism.Ioc;
using Ursa.Controls;
using Ursa.Controls.Options;

namespace Ursa.PrismExtension;

public class UrsaDrawerService(IContainerExtension container): IUrsaDrawerService 
{
    public void Show(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        Drawer.Show(v, vm, hostId, options);
    }

    public void ShowCustom<TResult>(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        Drawer.ShowCustom(v, vm, hostId, options);
    }

    public Task<DialogResult> ShowModal(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        return Drawer.ShowModal(v, vm, hostId, options);
    }

    public Task<TResult?> ShowCustomModal<TResult>(string viewName, object? vm, string? hostId = null, DrawerOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        return Drawer.ShowCustomModal<TResult?>(v, vm, hostId, options);
    }
}