using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace Ursa.Controls;

public static class DialogBox
{
    public static async Task ShowAsync()
    {
        return;
    }

    public static async Task ShowAsync<TView, TViewModel>(TViewModel vm) 
        where TView: Control, new()
        where TViewModel: new()
    {
        TView t = new TView();
        t.DataContext = vm;
    }

    public static async Task<object?> ShowOverlayAsync<TView, TViewModel>(TViewModel vm, string hostId)
        where TView : Control, new()
        where TViewModel: new()
    {
        var t = new DialogControl()
        {
            Content = new TView(){ DataContext = vm },
            DataContext = vm,
        };
        t.DataContext = vm;
        var host = OverlayDialogManager.GetOverlayDialogHost(hostId);
        host?.Children.Add(t);
        return null;
    }
}