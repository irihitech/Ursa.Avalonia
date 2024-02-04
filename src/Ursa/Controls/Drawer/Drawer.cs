using Avalonia.Controls;
using Ursa.Common;

namespace Ursa.Controls;

public static class Drawer
{
    public static Task<TResult?> Show<TView, TViewModel, TResult>(TViewModel vm, Position position = Position.Right)
        where TView: Control, new()
    {
        var host = OverlayDialogManager.GetHost(null);
        if (host is null) return Task.FromResult(default(TResult));
        var dialog = new CustomDrawerControl()
        {
            Content = new TView(),
            DataContext = vm,
            Position = position,
        };
        host.AddDrawer(dialog);
        return dialog.ShowAsync<TResult>();
    }
}