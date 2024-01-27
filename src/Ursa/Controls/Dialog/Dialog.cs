using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Ursa.Common;

namespace Ursa.Controls;

public static class Dialog
{
    /// <summary>
    /// Show a Window Modal Dialog that with all content fully customized. 
    /// </summary>
    /// <param name="vm"></param>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static async Task<TResult?> ShowCustomModalAsync<TView, TViewModel, TResult>(TViewModel vm) 
        where TView : Control, new()
    {
        var mainWindow = GetMainWindow();
        return await ShowCustomModalAsync<TView, TViewModel, TResult>(mainWindow, vm);
    }
    
    
    /// <summary>
    /// Show a Window Modal Dialog that with all content fully customized. And the owner of the dialog is specified.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="vm"></param>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static async Task<TResult> ShowCustomModalAsync<TView, TViewModel, TResult>(Window? owner, TViewModel? vm) 
        where TView: Control, new()
    {
        var window = new DialogWindow
        {
            Content = new TView { DataContext = vm },
            DataContext = vm,
        };
        if (owner is null)
        {
            window.Show();
            return default;
        }
        else
        {
            var result = await window.ShowDialog<TResult>(owner);
            return result;
        }
    }
    
    public static async Task<DialogResult> ShowModalAsync<TView, TViewModel>(
        Window? owner, 
        TViewModel vm, 
        string? title = null, 
        DialogMode mode = DialogMode.None, 
        DialogButton buttons = DialogButton.OKCancel)
        where TView : Control, new()
    {
        var window = new DefaultDialogWindow()
        {
            Content = new TView() { DataContext = vm },
            DataContext = vm,
            Buttons = buttons,
            Title = title,
            Mode = mode,
        };
        if (owner is null)
        {
            window.Show();
            return DialogResult.None;
        }
        else
        {
            var result = await window.ShowDialog<DialogResult>(owner);
            return result;
        }
    }
    
    public static async Task<DialogResult> ShowModalAsync<TView, TViewModel>(
        TViewModel vm, 
        string? title = null, 
        DialogMode mode = DialogMode.None, 
        DialogButton buttons = DialogButton.OKCancel)
        where TView: Control, new()
    {
        var mainWindow = GetMainWindow();
        return await ShowModalAsync<TView, TViewModel>(mainWindow, vm, title, mode, buttons);
    }
    
    private static Window? GetMainWindow()
    {
        var lifetime = Application.Current?.ApplicationLifetime;
        return lifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } w } ? w : null;
    }
}

public static class OverlayDialog
{
    public static Task<DialogResult> ShowModalAsync<TView, TViewModel>(
        TViewModel vm, 
        string? hostId = null, 
        string? title = null,
        DialogMode mode = DialogMode.None,
        DialogButton buttons = DialogButton.OKCancel)
        where TView : Control, new()
    {
        var t = new DefaultDialogControl()
        {
            Content = new TView(){ DataContext = vm },
            DataContext = vm,
            Buttons = buttons,
            Title = title,
            Mode = mode,
        };
        var host = OverlayDialogManager.GetHost(hostId);
        host?.AddModalDialog(t);
        return t.ShowAsync<DialogResult>();
    }

    public static Task<TResult> ShowCustomModalAsync<TView, TViewModel, TResult>(
        TViewModel vm, 
        string? hostId = null)
        where TView: Control, new()
    {
        var t = new DialogControl()
        {
            Content = new TView() { DataContext = vm },
            DataContext = vm,
        };
        var host = OverlayDialogManager.GetHost(hostId);
        host?.AddModalDialog(t);
        return t.ShowAsync<TResult>();
    }

    public static void Show<TView, TViewModel>(
        TViewModel vm, 
        string? hostId = null, 
        string? title = null, 
        DialogMode mode = DialogMode.None, 
        DialogButton buttons = DialogButton.OKCancel)
        where TView: Control, new()
    {
        var t = new DefaultDialogControl()
        {
            Content = new TView() { DataContext = vm },
            DataContext = vm,
            Buttons = buttons,
            Title = title,
            Mode = mode,
        };
        var host = OverlayDialogManager.GetHost(hostId);
        host?.AddDialog(t);
    }

    public static void ShowCustom<TView, TViewModel>(TViewModel vm, string? hostId = null)
        where TView: Control, new()
    {
        var t = new DialogControl()
        {
            Content = new TView() { DataContext = vm },
            DataContext = vm,
        };
        var host = OverlayDialogManager.GetHost(hostId);
        host?.AddDialog(t);
    }
    
}