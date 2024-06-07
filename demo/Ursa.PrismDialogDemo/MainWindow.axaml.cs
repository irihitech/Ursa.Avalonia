using Avalonia.Controls;
using Avalonia.Interactivity;
using DryIoc;
using Ursa.PrismExtension;

namespace Ursa.PrismDialogDemo;

public partial class MainWindow : Window
{
    private IUrsaOverlayDialogService _dialogService;
    private IUrsaDrawerService _drawerService;
    public MainWindow(IUrsaOverlayDialogService dialogService, IUrsaDrawerService drawerService)
    {
        InitializeComponent();
        _dialogService = dialogService;
        _drawerService = drawerService;
    }

    private void DialogButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _dialogService.ShowModal("Default", null, null, null);
    }

    private void DrawerButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _drawerService.ShowModal("Default", null, null, null);
    }
}