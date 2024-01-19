using Avalonia.Controls;

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
        return;
    }
}