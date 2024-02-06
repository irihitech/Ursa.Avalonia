using Avalonia.Controls;
using Avalonia.Interactivity;
using DryIoc;
using Ursa.PrismExtension;

namespace Ursa.PrismDialogDemo;

public partial class MainWindow : Window
{
    private IUrsaOverlayDialogService _ursa;
    public MainWindow(IUrsaOverlayDialogService ursa)
    {
        InitializeComponent();
        _ursa = ursa;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        _ursa.ShowModal("Default", null, null, null);
    }
}