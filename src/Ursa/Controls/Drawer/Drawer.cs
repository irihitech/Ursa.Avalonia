namespace Ursa.Controls;

public static class Drawer
{
    public static Task<TResult?> ShowDialogAsync<TView, TViewModel, TResult>(TViewModel viewModel)
    {
        var host = OverlayDialogManager.GetHost(null);
        if (host is null) return Task.FromResult(default(TResult));
        var dialog = new DefaultDrawerControl();
        host.AddDrawer(dialog);
        return dialog.ShowAsync<TResult>();
    }
}