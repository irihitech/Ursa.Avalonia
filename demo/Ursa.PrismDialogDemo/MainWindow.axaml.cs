using Avalonia.Controls;
using Avalonia.Interactivity;
using DryIoc;
using Ursa.Controls;
using Ursa.Controls.Options;
using Ursa.PrismExtension;

namespace Ursa.PrismDialogDemo;

public partial class MainWindow : Window
{
    private readonly IUrsaOverlayDialogService _overlayDialogService;
    private readonly IUrsaDialogService _aloneDialogService;
    private readonly IUrsaDrawerService _drawerService;

    public MainWindow(IUrsaOverlayDialogService overlayDialogService, IUrsaDialogService aloneDialogService,
        IUrsaDrawerService drawerService)
    {
        InitializeComponent();
        _overlayDialogService = overlayDialogService;
        _aloneDialogService = aloneDialogService;
        _drawerService = drawerService;
    }

    private void OverlayDialogButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _overlayDialogService.ShowModal("Default", null, null, new OverlayDialogOptions()
        {
            Title = "This is dialog title"
        });
    }

    private void AloneDialogButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _aloneDialogService.ShowModal("Default", null, null, new DialogOptions()
        {
            Title = "This is dialog title"
        });
    }

    private void DrawerButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _drawerService.ShowModal("Default", null, null, new DrawerOptions()
        {
            Title = "This is dialog title"
        });
    }
}