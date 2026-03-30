using Avalonia.Controls;

namespace Ursa.Controls;

public static partial class Dialog
{
    /// <summary>
    ///     Show a Modal Dialog Window with default style.
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="owner"></param>
    /// <param name="options"></param>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    /// <returns></returns>
    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use Dialog.ShowStandardAsync instead.")]
    public static Task<DialogResult> ShowModal<TView, TViewModel>(TViewModel vm, Window? owner = null,
        DialogOptions? options = null)
        where TView : Control, new()
    {
        return ShowStandardAsync<TView, TViewModel>(vm, owner, options);
    }
    
    /// <summary>
    ///     Show a Modal Dialog Window with default style.
    /// </summary>
    /// <param name="view"></param>
    /// <param name="vm"></param>
    /// <param name="owner"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use Dialog.ShowStandardAsync instead.")]
    public static Task<DialogResult> ShowModal(Control view, object? vm, Window? owner = null,
        DialogOptions? options = null)
    {
        return ShowStandardAsync(view, vm, owner, options);
    }
    
    /// <summary>
    ///     Show a Modal Dialog Window with all content fully customized.
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="owner"></param>
    /// <param name="options"></param>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use Dialog.ShowCustomAsync instead.")]
    public static Task<TResult?> ShowCustomModal<TView, TViewModel, TResult>(TViewModel vm, Window? owner = null,
        DialogOptions? options = null)
        where TView : Control, new()
    {
        return ShowCustomAsync<TView, TViewModel, TResult>(vm, owner, options);
    }
    
    /// <summary>
    ///     Show a Modal Dialog Window with all content fully customized.
    /// </summary>
    /// <param name="view"></param>
    /// <param name="vm"></param>
    /// <param name="owner"></param>
    /// <param name="options"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    [Obsolete("This will be removed in Ursa2.0 lifecycle. Please Use Dialog.ShowCustomAsync instead.")]
    public static Task<TResult?> ShowCustomModal<TResult>(Control view, object? vm, Window? owner = null,
        DialogOptions? options = null)
    {
        return ShowCustomAsync<TResult>(view, vm, owner, options);
    }
}