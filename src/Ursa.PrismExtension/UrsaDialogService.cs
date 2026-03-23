using Avalonia.Controls;
using Prism.Ioc;
using Ursa.Controls;

namespace Ursa.PrismExtension;

internal class UrsaDialogService(IContainerExtension container) : IUrsaDialogService
{
    public void ShowStandard(string viewName, object? vm, Window? owner = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        Dialog.ShowStandard(v, vm, owner);
    }

    public void ShowCustom(string viewName, object? vm, Window? owner = null, DialogOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        Dialog.ShowCustom(v, vm, owner, options);
    }

    public Task<DialogResult> ShowModal(string viewName, object? vm, Window? owner = null,
        DialogOptions? options = null)
    {
        return ShowStandardAsync(viewName, vm, owner, options);
    }

    public Task<TResult?> ShowCustomModal<TResult>(string viewName, object? vm, Window? owner = null,
        DialogOptions? options = null)
    {
        return ShowCustomAsync<TResult>(viewName, vm, owner, options);
    }

    public Task<DialogResult> ShowStandardAsync(string viewName, object? vm, Window? owner = null,
        DialogOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        return Dialog.ShowStandardAsync(v, vm, owner, options);
    }

    public Task<TResult?> ShowCustomAsync<TResult>(string viewName, object? vm, Window? owner = null,
        DialogOptions? options = null)
    {
        var v = container.Resolve<Control>(UrsaDialogServiceExtension.UrsaDialogViewPrefix + viewName);
        return Dialog.ShowCustomAsync<TResult>(v, vm, owner, options);
    }
}