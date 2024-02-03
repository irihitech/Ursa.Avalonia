using Avalonia.Controls;
using Ursa.Controls;

namespace Ursa.PrismExtension;

public interface IUrsaDialogService
{
    public void ShowCustom(string viewName, object? vm, Window? owner = null, DialogOptions? options = null);

    public Task<DialogResult> ShowModal(string viewName, object? vm, Window? owner = null,
        DialogOptions? options = null);

    public Task<TResult?> ShowCustomModal<TResult>(string viewName, object? vm, Window? owner = null,
        DialogOptions? options = null);
}