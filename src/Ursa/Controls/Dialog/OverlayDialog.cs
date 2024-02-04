using Avalonia;
using Avalonia.Controls;
using Ursa.Common;

namespace Ursa.Controls;

public static class OverlayDialog
{
    public static void Show<TView, TViewModel>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null)
        where TView : Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return;
        var t = new DefaultDialogControl()
        {
            Content = new TView(){ DataContext = vm },
            DataContext = vm,
        };
        ConfigureDefaultDialogControl(t, options);
        host?.AddDialog(t);
    }
    
    public static void Show(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return;
        var t = new DefaultDialogControl()
        {
            Content = control,
            DataContext = vm,
        };
        ConfigureDefaultDialogControl(t, options);
        host?.AddDialog(t);
        
    }
    
    public static void Show(object? vm, string? hostId = null, OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return;
        var view = host.GetDataTemplate(vm)?.Build(vm);
        if (view is null) view = new ContentControl();
        view.DataContext = vm;
        var t = new DefaultDialogControl()
        {
            Content = view,
            DataContext = vm,
        };
        ConfigureDefaultDialogControl(t, options);
        host.AddDialog(t);
    }
    
    public static void ShowCustom<TView, TViewModel>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null)
        where TView: Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return;
        var t = new CustomDialogControl()
        {
            Content = new TView(),
            DataContext = vm,
        };
        ConfigureDialogControl(t, options);
        host?.AddDialog(t);
    }
    
    public static void ShowCustom(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return;
        var t = new CustomDialogControl()
        {
            Content = control,
            DataContext = vm,
        };
        ConfigureDialogControl(t, options);
        host?.AddDialog(t);
    }
    
    public static void ShowCustom(object? vm, string? hostId = null,
        OverlayDialogOptions? options = null)
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return;
        var view = host.GetDataTemplate(vm)?.Build(vm);
        if (view is null) view = new ContentControl() { Padding = new Thickness(24) };
        view.DataContext = vm;
        var t = new CustomDialogControl()
        {
            Content = view,
            DataContext = vm,
        };
        ConfigureDialogControl(t, options);
        host.AddDialog(t);
    }
    
    public static Task<DialogResult> ShowModal<TView, TViewModel>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = default)
        where TView: Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return Task.FromResult(DialogResult.None);
        var t = new DefaultDialogControl()
        {
            Content = new TView(),
            DataContext = vm,
        };
        ConfigureDefaultDialogControl(t, options);
        host?.AddModalDialog(t);
        return t.ShowAsync<DialogResult>(token);
    }
    
    public static Task<DialogResult> ShowModal(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = default)
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return Task.FromResult(DialogResult.None);
        var t = new DefaultDialogControl()
        {
            Content = control,
            DataContext = vm,
        };
        ConfigureDefaultDialogControl(t, options);
        host?.AddModalDialog(t);
        return t.ShowAsync<DialogResult>(token);
    }
    
    public static Task<TResult?> ShowCustomModal<TView, TViewModel, TResult>(TViewModel vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = default)
        where TView: Control, new()
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return Task.FromResult(default(TResult));
        var t = new CustomDialogControl()
        {
            Content = new TView(),
            DataContext = vm,
        };
        ConfigureDialogControl(t, options);
        host?.AddModalDialog(t);
        return t.ShowAsync<TResult?>(token);
    }
    
    public static Task<TResult?> ShowCustomModal<TResult>(Control control, object? vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = default)
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return Task.FromResult(default(TResult));
        var t = new CustomDialogControl()
        {
            Content = control,
            DataContext = vm,
        };
        ConfigureDialogControl(t, options);
        host?.AddModalDialog(t);
        return t.ShowAsync<TResult?>(token);
    }
    
    public static Task<TResult?> ShowCustomModal<TResult>(object? vm, string? hostId = null,
        OverlayDialogOptions? options = null, CancellationToken? token = default)
    {
        var host = OverlayDialogManager.GetHost(hostId);
        if (host is null) return Task.FromResult(default(TResult));
        var view = host.GetDataTemplate(vm)?.Build(vm);
        if (view is null) view = new ContentControl() { Padding = new Thickness(24) };
        view.DataContext = vm;
        var t = new CustomDialogControl()
        {
            Content = view,
            DataContext = vm,
        };
        ConfigureDialogControl(t, options);
        host?.AddModalDialog(t);
        return t.ShowAsync<TResult?>(token);
    }
    
    private static void ConfigureDialogControl(CustomDialogControl control, OverlayDialogOptions? options)
    {
        options ??= OverlayDialogOptions.Default;
        control.HorizontalAnchor = options.HorizontalAnchor;
        control.VerticalAnchor = options.VerticalAnchor;
        control.ActualHorizontalAnchor = options.HorizontalAnchor;
        control.ActualVerticalAnchor = options.VerticalAnchor;
        control.HorizontalOffset =
            control.HorizontalAnchor == HorizontalPosition.Center ? null : options.HorizontalOffset;
        control.VerticalOffset =
            options.VerticalAnchor == VerticalPosition.Center ? null : options.VerticalOffset;
        control.CanClickOnMaskToClose = options.CanClickOnMaskToClose;
        control.IsCloseButtonVisible = options.IsCloseButtonVisible;
    }
    
    private static void ConfigureDefaultDialogControl(DefaultDialogControl control, OverlayDialogOptions? options)
    {
        if (options is null) options = new OverlayDialogOptions();
        control.HorizontalAnchor = options.HorizontalAnchor;
        control.VerticalAnchor = options.VerticalAnchor;
        control.ActualHorizontalAnchor = options.HorizontalAnchor;
        control.ActualVerticalAnchor = options.VerticalAnchor;
        control.HorizontalOffset =
            control.HorizontalAnchor == HorizontalPosition.Center ? null : options.HorizontalOffset;
        control.VerticalOffset =
            options.VerticalAnchor == VerticalPosition.Center ? null : options.VerticalOffset;
        control.CanClickOnMaskToClose = options.CanClickOnMaskToClose;
        control.Mode = options.Mode;
        control.Buttons = options.Buttons;
        control.Title = options.Title;
    }
    
    
}