using Avalonia.Controls;
using Ursa.Common;

namespace Ursa.Controls;

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