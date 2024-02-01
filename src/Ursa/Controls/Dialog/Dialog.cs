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
    /// Show a Window Dialog that with all content fully customized. And the owner of the dialog is specified.
    /// </summary>
    /// <param name="vm">Dialog ViewModel instance</param>
    /// <param name="owner"></param>
    /// <param name="options"></param>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    /// <returns></returns>
    public static void ShowCustom<TView, TViewModel>(TViewModel? vm, Window? owner = null, DialogOptions? options = null) 
        where TView: Control, new()
    {
        var window = new DialogWindow
        {
            Content = new TView(),
            DataContext = vm,
        };
        AssignOptionsToDialogWindow(window, options);
        owner ??= GetMainWindow();
        if (owner is null)
        {
            window.Show();
        }
        else
        {
            window.Show(owner);
        }
    }
    
    public static void ShowCustom(Control view, object? vm, Window? owner = null, DialogOptions? options = null)
    { 
        var window = new DialogWindow
        {
            Content = view,
            DataContext = vm,
        };
        AssignOptionsToDialogWindow(window, options);
        owner ??= GetMainWindow();
        if (owner is null)
        {
            window.Show();
        }
        else
        {
            window.Show(owner);
        }
    }

    public static Task<DialogResult> ShowModal<TView, TViewModel>(TViewModel vm, Window? owner = null,
        DialogOptions? options = null)
        where TView: Control, new()
    {
        var window = new DefaultDialogWindow
        {
            Content = new TView(),
            DataContext = vm,
        };
        AssignOptionsToDefaultDialogWindow(window, options);
        owner ??= GetMainWindow();
        if (owner is null)
        {
            window.Show();
            return Task.FromResult(DialogResult.None);
        }
        return window.ShowDialog<DialogResult>(owner);
    }
    
    public static Task<DialogResult> ShowModal(Control view, object? vm, Window? owner = null, DialogOptions? options = null)
    {
        var window = new DefaultDialogWindow
        {
            Content = view,
            DataContext = vm,
        };
        AssignOptionsToDefaultDialogWindow(window, options);
        owner ??= GetMainWindow();
        if (owner is null)
        {
            window.Show();
            return Task.FromResult(DialogResult.None);
        }
        return window.ShowDialog<DialogResult>(owner);
    }

    public static Task<TResult?> ShowCustomModal<TView, TViewModel, TResult>(TViewModel vm, Window? owner = null,
        DialogOptions? options = null)
        where TView: Control, new()
    {
        var window = new DialogWindow
        {
            Content = new TView(),
            DataContext = vm,
        };
        AssignOptionsToDialogWindow(window, options);
        owner ??= GetMainWindow();
        if (owner is null)
        {
            window.Show();
            return Task.FromResult(default(TResult));
        }
        return window.ShowDialog<TResult?>(owner);
    }

    public static Task<TResult?> ShowCustomModal<TResult>(Control view, object? vm, Window? owner = null,
        DialogOptions? options = null)
    {
        var window = new DialogWindow
        {
            Content = view,
            DataContext = vm,
        };
        AssignOptionsToDialogWindow(window, options);
        owner ??= GetMainWindow();
        if (owner is null)
        {
            window.Show();
            return Task.FromResult(default(TResult));
        }
        return window.ShowDialog<TResult?>(owner);
    }
    
    private static Window? GetMainWindow()
    {
        var lifetime = Application.Current?.ApplicationLifetime;
        return lifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } w } ? w : null;
    }

    private static void AssignOptionsToDialogWindow(DialogWindow window, DialogOptions? options)
    {
        if (options is null)
        {
            options = new DialogOptions();
        }
        window.WindowStartupLocation = options.StartupLocation;
        window.Title = options.Title;
        if (options.StartupLocation == WindowStartupLocation.Manual)
        {
            if (options.Position is not null)
            {
                window.Position = options.Position.Value;
            }
            else
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
        }
    }
    
    private static void AssignOptionsToDefaultDialogWindow(DefaultDialogWindow window, DialogOptions? options)
    {
        if (options is null)
        {
            options = new DialogOptions();
        }
        window.WindowStartupLocation = options.StartupLocation;
        window.Title = options.Title;
        window.Buttons = options.Button;
        window.Mode = options.Mode;
        if (options.StartupLocation == WindowStartupLocation.Manual)
        {
            if (options.Position is not null)
            {
                window.Position = options.Position.Value;
            }
            else
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
        }
    }
    
    
}