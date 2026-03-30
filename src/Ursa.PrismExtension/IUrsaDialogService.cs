using Avalonia.Controls;
using Ursa.Controls;

namespace Ursa.PrismExtension;

public interface IUrsaDialogService
{
    public void ShowStandard(string viewName, object? vm, Window? owner = null);
    public void ShowCustom(string viewName, object? vm, Window? owner = null, DialogOptions? options = null);

    [Obsolete("Use ShowStandardAsync instead")]
    public Task<DialogResult> ShowModal(string viewName, object? vm, Window? owner = null,
        DialogOptions? options = null);

    [Obsolete("Use ShowCustomAsync instead")]
    public Task<TResult?> ShowCustomModal<TResult>(string viewName, object? vm, Window? owner = null,
        DialogOptions? options = null);

    public Task<DialogResult> ShowStandardAsync(string viewName, object? vm, Window? owner = null,
        DialogOptions? options = null);

    public Task<TResult?> ShowCustomAsync<TResult>(string viewName, object? vm, Window? owner = null,
        DialogOptions? options = null);
}