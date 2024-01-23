using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace Ursa.Controls;

public static class Dialog
{
    public static async Task<TResult?> ShowModalAsync<TView, TViewModel, TResult>(TViewModel vm) 
        where TView : Control, new()
    {

        var lifetime = Application.Current?.ApplicationLifetime;
        if (lifetime is IClassicDesktopStyleApplicationLifetime classLifetime)
        {
            var window = new DialogWindow
            {
                Content = new TView { DataContext = vm },
                DataContext = vm,
            };
            if (classLifetime.MainWindow is not { } main)
            {
                window.Show();
                return default;
            }
            var result = await window.ShowDialog<TResult>(main);
            return result;
        }

        return default(TResult);
    }
    
    public static async Task<TResult> ShowModalAsync<TView, TViewModel, TResult>(Window owner, TViewModel? vm) 
        where TView: Control, new()
    {
        var window = new DialogWindow
        {
            Content = new TView() { DataContext = vm },
            DataContext = vm
        };
        return await window.ShowDialog<TResult>(owner);
    }
}

public static class OverlayDialog
{
    public static Task<TResult> ShowModalAsync<TView, TViewModel, TResult>(TViewModel vm, string? hostId = null)
        where TView : Control, new()
    {
        var t = new DialogControl()
        {
            Content = new TView(){ DataContext = vm },
            DataContext = vm,
        };
        var host = OverlayDialogManager.GetHost(hostId);
        host?.AddModalDialog(t);
        return t.ShowAsync<TResult>();
    }

    public static Task<TResult> ShowModalAsync<TView, TViewModel, TResult>(TViewModel vm, DialogOptions options, string? hostId = null)
        where TView : Control, new()
    {
        var t = new DialogControl()
        {
            Content = new TView() { DataContext = vm },
            DataContext = vm,
            ExtendToClientArea = options.ExtendToClientArea,
            Title = options.Title,
        };
        var host = OverlayDialogManager.GetHost(hostId);
        host?.AddModalDialog(t);
        return t.ShowAsync<TResult>();
    }

    public static void Show<TView, TViewModel>(TViewModel vm, string? hostId = null)
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

    public static void Show<TView, TViewModel>(TViewModel vm, DialogOptions options, string? hostId)
        where TView: Control, new()
    {
        var t = new DialogControl()
        {
            Content = new TView() { DataContext = vm },
            DataContext = vm,
            ExtendToClientArea = options.ExtendToClientArea,
            Title = options.Title,
        };
        var host = OverlayDialogManager.GetHost(hostId);
        host?.AddModalDialog(t);
    }
}