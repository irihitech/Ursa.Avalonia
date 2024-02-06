using Avalonia.Controls;
using Prism.Ioc;
using Ursa.Controls;
using Ursa.Controls.Options;

namespace Ursa.PrismExtension;

public class UrsaDrawerService(IContainerExtension container): IUrsaDrawerService 
{
    public Task<DialogResult> ShowDrawer(string viewName, object? vm, string? hostId = null, DefaultDrawerOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        return Drawer.Show(v, vm, hostId, options);
    }
    
    public Task<TResult?> ShowCustomDrawer<TResult>(string viewName, object? vm, string? hostId = null,
        CustomDrawerOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        return Drawer.ShowCustom<TResult?>(v, vm, hostId, options);
    }
}