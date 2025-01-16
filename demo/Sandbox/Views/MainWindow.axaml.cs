using Avalonia.Controls;
using Avalonia.Interactivity;
using Ursa.Controls;

namespace Sandbox.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var res = await OverlayDialog.ShowModal(new TextBlock() { Text = "sdfksjdl" }, "root");
    }
}