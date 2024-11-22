using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Ursa.Common;

namespace Ursa.Controls;

public static class Dialog
{
    /// <summary>
    ///     Show a Window Dialog that with all content fully customized. And the owner of the dialog is specified.
    /// </summary>
    /// <param name="vm">Dialog ViewModel instance</param>
    /// <param name="owner"></param>
    /// <param name="options"></param>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    /// <returns></returns>
    public static void ShowCustom<TView, TViewModel>(TViewModel? vm, Window? owner = null,
        DialogOptions? options = null)
        where TView : Control, new()
    {
        var window = new DialogWindow
        {
            Content = new TView(),
            DataContext = vm
        };
        ConfigureDialogWindow(window, options);
        owner ??= GetMainWindow();
        if (owner is null)
        {
            window.Show();
        }
        else
        {
            window.Icon = owner.Icon;
            window.Show(owner);
        }
    }

    /// <summary>
    ///     Show a Window Dialog that with all content fully customized. And the owner of the dialog is specified.
    /// </summary>
    /// <param name="view">View to show in Dialog Window</param>
    /// <param name="vm">ViewModel</param>
    /// <param name="owner">Owner Window</param>
    /// <param name="options">Dialog options to configure the window. </param>
    public static void ShowCustom(Control view, object? vm, Window? owner = null, DialogOptions? options = null)
    {
        var window = new DialogWindow
        {
            Content = view,
            DataContext = vm
        };
        ConfigureDialogWindow(window, options);
        owner ??= GetMainWindow();
        if (owner is null)
        {
            window.Show();
        }
        else
        {
            window.Icon = owner.Icon;
            window.Show(owner);
        }
    }

    /// <summary>
    ///     Show a Modal Dialog Window with default style.
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="owner"></param>
    /// <param name="options"></param>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    /// <returns></returns>
    public static Task<DialogResult> ShowModal<TView, TViewModel>(TViewModel vm, Window? owner = null,
        DialogOptions? options = null)
        where TView : Control, new()
    {
        var window = new DefaultDialogWindow
        {
            Content = new TView(),
            DataContext = vm
        };
        ConfigureDefaultDialogWindow(window, options);
        owner ??= GetMainWindow();
        if (owner is null)
        {
            window.Show();
            return Task.FromResult(DialogResult.None);
        }

        window.Icon = owner.Icon;
        return window.ShowDialog<DialogResult>(owner);
    }

    /// <summary>
    ///     Show a Modal Dialog Window with default style.
    /// </summary>
    /// <param name="view"></param>
    /// <param name="vm"></param>
    /// <param name="owner"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static Task<DialogResult> ShowModal(Control view, object? vm, Window? owner = null,
        DialogOptions? options = null)
    {
        var window = new DefaultDialogWindow
        {
            Content = view,
            DataContext = vm
        };
        ConfigureDefaultDialogWindow(window, options);
        owner ??= GetMainWindow();
        if (owner is null)
        {
            window.Show();
            return Task.FromResult(DialogResult.None);
        }

        window.Icon = owner.Icon;
        return window.ShowDialog<DialogResult>(owner);
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
    public static Task<TResult?> ShowCustomModal<TView, TViewModel, TResult>(TViewModel vm, Window? owner = null,
        DialogOptions? options = null)
        where TView : Control, new()
    {
        var window = new DialogWindow
        {
            Content = new TView(),
            DataContext = vm
        };
        ConfigureDialogWindow(window, options);
        owner ??= GetMainWindow();
        if (owner is null)
        {
            window.Show();
            return Task.FromResult(default(TResult));
        }

        window.Icon = owner.Icon;
        return window.ShowDialog<TResult?>(owner);
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
    public static Task<TResult?> ShowCustomModal<TResult>(Control view, object? vm, Window? owner = null,
        DialogOptions? options = null)
    {
        var window = new DialogWindow
        {
            Content = view,
            DataContext = vm
        };
        ConfigureDialogWindow(window, options);
        owner ??= GetMainWindow();
        if (owner is null)
        {
            window.Show();
            return Task.FromResult(default(TResult));
        }

        window.Icon = owner.Icon;
        return window.ShowDialog<TResult?>(owner);
    }

    /// <summary>
    ///     Get the main window of the application as default owner of the dialog.
    /// </summary>
    /// <returns></returns>
    private static Window? GetMainWindow()
    {
        var lifetime = Application.Current?.ApplicationLifetime;
        return lifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } w } ? w : null;
    }

    /// <summary>
    ///     Attach options to dialog window.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="options"></param>
    private static void ConfigureDialogWindow(DialogWindow window, DialogOptions? options)
    {
        if (options is null) options = new DialogOptions();
        window.WindowStartupLocation = options.StartupLocation;
        window.Title = options.Title;
        window.IsCloseButtonVisible = options.IsCloseButtonVisible;
        window.ShowInTaskbar = options.ShowInTaskBar;
        window.CanDragMove = options.CanDragMove;
        window.CanResize = options.CanResize;
        window.IsManagedResizerVisible = options.CanResize;
        if (options.StartupLocation == WindowStartupLocation.Manual)
        {
            if (options.Position is not null)
                window.Position = options.Position.Value;
            else
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
        if (!string.IsNullOrWhiteSpace(options.StyleClass))
        {
            var styles = options.StyleClass!.Split(Constants.SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
            window.Classes.AddRange(styles);
        }
    }

    /// <summary>
    ///     Attach options to default dialog window.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="options"></param>
    private static void ConfigureDefaultDialogWindow(DefaultDialogWindow window, DialogOptions? options)
    {
        options ??= DialogOptions.Default;
        window.WindowStartupLocation = options.StartupLocation;
        window.Title = options.Title;
        window.Buttons = options.Button;
        window.Mode = options.Mode;
        window.ShowInTaskbar = options.ShowInTaskBar;
        window.IsCloseButtonVisible = options.IsCloseButtonVisible;
        window.CanDragMove = options.CanDragMove;
        window.IsManagedResizerVisible = options.CanResize;
        window.CanResize = options.CanResize;
        if (options.StartupLocation == WindowStartupLocation.Manual)
        {
            if (options.Position is not null)
                window.Position = options.Position.Value;
            else
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
        if (!string.IsNullOrWhiteSpace(options.StyleClass))
        {
            var styles = options.StyleClass!.Split(Constants.SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
            window.Classes.AddRange(styles);
        }
    }
}